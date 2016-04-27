using System;
using System.Runtime.Caching;
using CPE.Domain.IO;
using CPE.Domain.Model;

namespace CPE.Data.EntityFramework.Model
{
    public partial class Customer : ICustomer, IEquatable<Customer>
    {
        private readonly MemoryCache _cache = new MemoryCache("BlobCache");

        public SalesOrderParserSettingsBlob GetSalesOrderParserSettings()
        {
            if (SalesOrderParserSettings == null)
            {
                return null;
            }

            if (_cache.Contains("ParserBlob"))
            {
                return _cache["ParserBlob"] as SalesOrderParserSettingsBlob;
            }

            var settings = BlobHandler.Decompress<SalesOrderParserSettingsBlob>(SalesOrderParserSettings);

            _cache.Add("ParserBlob", settings, DateTime.Now.AddSeconds(30));

            return settings;
        }

        public bool HasSalesOrderParserSettings => SalesOrderParserSettings != null;

        public void SetSalesOrderParserSettings(SalesOrderParserSettingsBlob settings)
        {
            SalesOrderParserSettings = BlobHandler.Compress(settings);

            if (_cache.Contains("ParserBlob"))
            {
                _cache.Remove("ParserBlob");
            }
        }

        public bool Equals(Customer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Customer) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}