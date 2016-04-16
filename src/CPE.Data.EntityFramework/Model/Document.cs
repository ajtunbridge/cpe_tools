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
    
    public partial class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public bool IsLocked { get; set; }
        public bool IsApproved { get; set; }
        public Nullable<int> PartId { get; set; }
        public Nullable<int> PartVersionId { get; set; }
        public Nullable<int> OperationId { get; set; }
        public Nullable<int> ExternalCalibrationRecordId { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    
        public virtual CalibrationResult CalibrationResult { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual Part Part { get; set; }
        public virtual PartVersion PartVersion { get; set; }
    }
}
