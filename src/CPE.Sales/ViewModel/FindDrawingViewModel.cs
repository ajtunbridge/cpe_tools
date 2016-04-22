using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CPE.Sales.ExtensionMethods;
using CPE.Sales.Messages;
using CPE.Sales.Models;
using CPE.Sales.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace CPE.Sales.ViewModel
{
    public sealed class FindDrawingViewModel : ViewModelBase
    {
        private readonly DrawingFileService _fileService;
        private DrawingFile _selectedFile;

        public ObservableCollection<DrawingFile> FoundDrawingFiles { get; } =
            new ObservableCollection<DrawingFile>();

        public DrawingFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                RaisePropertyChanged();
            }
        }
        public FindDrawingViewModel(DrawingFileService fileService)
        {
            if (IsInDesignMode)
            {
                return;
            }

            _fileService = fileService;

            Messenger.Default.Register<DrawingFileFoundMessage>(this, HandleDrawingFileFoundMessage);
        }

        public async Task FindDrawingFilesAsync(string drawingNumber)
        {
            FoundDrawingFiles.Clear();

            await _fileService.FindDrawingFilesAsync(drawingNumber);        
        }

        private void HandleDrawingFileFoundMessage(DrawingFileFoundMessage message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                FoundDrawingFiles.Add(message.FoundFile);
                FoundDrawingFiles.Sort();
            });
        }
    }
}
