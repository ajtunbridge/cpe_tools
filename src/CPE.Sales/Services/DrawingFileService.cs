using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using CPE.Domain.IO;
using CPE.Sales.Messages;
using CPE.Sales.Models;
using GalaSoft.MvvmLight.Messaging;

namespace CPE.Sales.Services
{
    public sealed class DrawingFileService
    {
        private readonly Dictionary<string, List<DrawingFile>> _searchCache = new Dictionary<string, List<DrawingFile>>();
        private readonly Dictionary<string, BitmapSource> _iconCache = new Dictionary<string, BitmapSource>();
        private readonly string[] _validFileExtensions = {".pdf", ".tif", ".tiff"};

        private bool _searchInProgress;

        public async Task FindDrawingFilesAsync(string drawingNumber)
        {
            if (_searchInProgress)
            {
                return;
            }

            _searchInProgress = true;

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
                    var rootFolder = Properties.Settings.Default.DrawingFileFolderName;
                    var searchPattern = "*" + drawingNumber + "*";

                    var results = new List<DrawingFile>();

                    Queue<string> pending = new Queue<string>();
                    pending.Enqueue(rootFolder);
                    while (pending.Count > 0)
                    {
                        rootFolder = pending.Dequeue();

                        var tmp = Directory.GetFiles(rootFolder, searchPattern);

                        for (int i = 0; i < tmp.Length; i++)
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

                        for (int i = 0; i < tmp.Length; i++)
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
