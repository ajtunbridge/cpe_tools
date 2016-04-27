using System;
using System.Collections.Generic;

namespace MSOutlookProvider.Model
{
    public sealed class MailFolder : IEquatable<MailFolder>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<MailFolder> Children { get; } = new List<MailFolder>();

        public bool Equals(MailFolder other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is MailFolder && Equals((MailFolder) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(MailFolder left, MailFolder right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MailFolder left, MailFolder right)
        {
            return !Equals(left, right);
        }
    }
}