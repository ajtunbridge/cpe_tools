using System;
using CPE.Domain.Model;

namespace CPE.Data.EntityFramework.Model
{
    public partial class Customer : ICustomer, IEquatable<Customer>
    {
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