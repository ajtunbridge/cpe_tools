﻿using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using CPE.Sales.Messages;
using CPE.Sales.Models;
using CPE.Sales.Properties;
using GalaSoft.MvvmLight.Messaging;

namespace CPE.Sales.Services
{
    public sealed class DrawingFileService
    {
        private readonly Dictionary<string, BitmapSource> _iconCache = new Dictionary<string, BitmapSource>();

        private readonly Dictionary<string, List<DrawingFile>> _searchCache =
            new Dictionary<string, List<DrawingFile>>();

        private readonly string[] _validFileExtensions = {".pdf", ".tif", ".tiff"};
        private string _lastSearch;

        private bool _searchInProgress;

        public async Task FindDrawingFilesAsync(string drawingNumber)
        {
            if (_searchInProgress)
            {
                return;
            }

            _searchInProgress = true;

            // do a full search if the same drawing number is searched twice in a row
            if (drawingNumber == _lastSearch && _searchCache.ContainsKey(drawingNumber))
            {
                _searchCache.Remove(drawingNumber);
            }

            _lastSearch = drawingNumber;

            if (_searchCache.ContainsKey(drawingNumber))
            {
                foreach (var result in _searchCache[drawingNumber])
                {
                    Messenger.Default.Send(new DrawingFileFoundMessage(result));
                }
            }
            else
            {
                await Task.Factory.StartNew(() =>
                {
                    var rootFolder = Settings.Default.DrawingFileFolderName;
                    var searchPattern = "*" + drawingNumber + "*";

                    var results = new List<DrawingFile>();

                    var pending = new Queue<string>();
                    pending.Enqueue(rootFolder);
                    while (pending.Count > 0)
                    {
                        rootFolder = pending.Dequeue();

                        var tmp = Directory.GetFiles(rootFolder, searchPattern);

                        for (var i = 0; i < tmp.Length; i++)
                        {
                            var ext = Path.GetExtension(tmp[i]).ToLower();

                            if (_validFileExtensions.All(validExt => ext != validExt))
                            {
                                continue;
                            }

                            BitmapSource icon = null;

                            Application.Current.Dispatcher.Invoke(() => icon = GetIcon(tmp[i]));

                            var file = new DrawingFile();
                            file.FileName = Path.GetFileNameWithoutExtension(tmp[i]);
                            file.FullPath = tmp[i];
                            file.CreatedAt = File.GetLastWriteTime(tmp[i]);
                            file.Icon = icon;
                            results.Add(file);

                            Messenger.Default.Send(new DrawingFileFoundMessage(file));
                        }

                        tmp = Directory.GetDirectories(rootFolder);

                        for (var i = 0; i < tmp.Length; i++)
                        {
                            pending.Enqueue(tmp[i]);
                        }
                    }

                    if (results.Count > 0)
                    {
                        _searchCache.Add(drawingNumber, results);
                    }
                });
            }

            _searchInProgress = false;
        }

        private BitmapSource GetIcon(string fileName)
        {
            var key = Path.GetExtension(fileName).ToLower();

            if (_iconCache.ContainsKey(key))
            {
                return _iconCache[key];
            }

            var sysIcon = Icon.ExtractAssociatedIcon(fileName);

            var bmpSrc = Imaging.CreateBitmapSourceFromHIcon(
                sysIcon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            sysIcon.Dispose();

            _iconCache.Add(key, bmpSrc);

            return bmpSrc;
        }
    }
}