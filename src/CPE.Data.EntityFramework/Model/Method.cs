//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CPE.Data.EntityFramework.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Method
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Method()
        {
            this.Operations = new HashSet<Operation>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsPreferred { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    
        public virtual PartVersion PartVersion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Operation> Operations { get; set; }
    }
}
