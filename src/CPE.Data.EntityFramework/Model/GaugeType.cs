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
    
    public partial class GaugeType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GaugeType()
        {
            this.Gauges = new HashSet<Gauge>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> CalibrationMethodId { get; set; }
        public byte DefaultCalibrationPeriod { get; set; }
    
        public virtual CalibrationMethod CalibrationMethod { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Gauge> Gauges { get; set; }
    }
}
