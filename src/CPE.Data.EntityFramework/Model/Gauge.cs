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
    
    public partial class Gauge
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Gauge()
        {
            this.ExternalCalibrationRecords = new HashSet<ExternalCalibrationRecord>();
        }
    
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Name { get; set; }
        public int GaugeTypeId { get; set; }
        public int HeldBy { get; set; }
        public Nullable<double> SizeRangeMin { get; set; }
        public Nullable<double> SizeRangeMax { get; set; }
        public bool IsReferenceOnly { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExternalCalibrationRecord> ExternalCalibrationRecords { get; set; }
        public virtual GaugeType GaugeType { get; set; }
    }
}
