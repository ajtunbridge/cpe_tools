using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using CPE.Domain.Services;

namespace CPE.Sales
{
    public class PhotoCache
    {
        private readonly MemoryCache _memCache = new MemoryCache("PhotoCache");
        private readonly CacheItemPolicy _policy = new CacheItemPolicy {SlidingExpiration = new TimeSpan(0, 0, 5, 0)};
        private readonly IPhotoService _photoService;

        public PhotoCache(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        public async Task<byte[]> GetPhotoAsync(string drawingNumber)
        {
            if (_memCache.Contains(drawingNumber))
            {
                return (byte[])_memCache[drawingNumber];
            }
            
            throw new NotImplementedException();
        }
        
    }
}
