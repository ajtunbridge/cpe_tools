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
    
    public partial class ToolGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ToolGroup()
        {
            this.ToolGroups1 = new HashSet<ToolGroup>();
            this.Tools = new HashSet<Tool>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentGroupId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ToolGroup> ToolGroups1 { get; set; }
        public virtual ToolGroup ToolGroup1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tool> Tools { get; set; }
    }
}
