using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPE.Domain.Model;
using CPE.Domain.Services;
using CPE.PartPhotoManager.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace CPE.PartPhotoManager.ViewModels
{
    public sealed class PartSearchViewModel : ViewModelBase
    {
        private readonly IPartService _parts;
        private readonly IPartVersionService _partVersions;
        private readonly IPhotoService _photos;
        private readonly ITricornService _tricorn;

        public ObservableCollection<PartSearchResult> Results { get; private set; } = new ObservableCollection<PartSearchResult>();

        public PartSearchViewModel(IPartService parts, IPartVersionService partVersions, IPhotoService photos, ITricornService tricorn)
        {
            if (IsInDesignMode)
            {
                return;
            }

            _parts = parts;
            _partVersions = partVersions;
            _photos = photos;
            _tricorn = tricorn;
        }
        
        public async Task PerformSearchAsync(string userReference)
        {
            Results.Clear();

            var worksOrder = await _tricorn.GetWorksOrderByUserReferenceAsync(userReference);

            var existingPart = await _parts.GetWhereDrawingNumberEqualsAsync(worksOrder.DrawingNumber);

            if (existingPart == null)
            {

            }
            else
            {
                var lastVersion = await _partVersions.GetLatestVersionAsync(existingPart.Id);

                var photo = await _photos.GetPhotoByPartAsync(existingPart);

                var result = new PartSearchResult
                {
                    DrawingNumber = existingPart.DrawingNumber,
                    LatestVersion = lastVersion.VersionNumber,
                    Name = existingPart.Name,
                    Photo = photo
                };

                Results.Add(result);
            }
        }
    }
}
