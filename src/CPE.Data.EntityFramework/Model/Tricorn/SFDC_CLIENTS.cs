//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CPE.Data.EntityFramework.Model.Tricorn
{
    using System;
    using System.Collections.Generic;
    
    public partial class SFDC_CLIENTS
    {
        public int SFDCClientRef { get; set; }
        public string ClientIdentifier { get; set; }
        public bool IsProvisioned { get; set; }
        public bool ActiveMode { get; set; }
        public int CurrentWorkCentreCount { get; set; }
        public bool ShowJobPacks { get; set; }
        public bool EditTimes { get; set; }
        public bool ShowEstimates { get; set; }
        public Nullable<bool> SwapperEnabled { get; set; }
        public bool ShowExternalQueue { get; set; }
    }
}
