using System;

namespace SyncfusionPdfParser
{
    public sealed class CachedText : IEquatable<CachedText>
    {
        public string FileName { get; set; } = string.Empty;

        public string Text { get; set; }

        public bool Equals(CachedText other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(FileName, other.FileName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is CachedText && Equals((CachedText) obj);
        }

        public override int GetHashCode()
        {
            return FileName.GetHashCode();
        }

        public static bool operator ==(CachedText left, CachedText right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CachedText left, CachedText right)
        {
            return !Equals(left, right);
        }
    }
}