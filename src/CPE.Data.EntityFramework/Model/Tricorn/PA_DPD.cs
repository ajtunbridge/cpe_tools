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
    
    public partial class PA_DPD
    {
        public decimal NO { get; set; }
        public decimal PANO { get; set; }
        public decimal WEIGHT { get; set; }
        public string PACKETNO { get; set; }
        public string PACKETNOCHECKSUM { get; set; }
        public Nullable<decimal> BARCODEID { get; set; }
        public string BARCODE { get; set; }
        public string BARCODECHECKSUM { get; set; }
        public string DSORT { get; set; }
        public string OSORT { get; set; }
        public string DEPOT { get; set; }
        public string DPDCNTRYSIGN { get; set; }
        public Nullable<decimal> FRPOSTCODE { get; set; }
        public string INTCNTRYSIGN { get; set; }
        public string SERVICE { get; set; }
        public Nullable<decimal> TOPOSTCODE { get; set; }
        public string CNAME { get; set; }
        public string CHNAME { get; set; }
        public Nullable<System.DateTime> CDATE { get; set; }
        public Nullable<System.DateTime> CHDATE { get; set; }
        public Nullable<decimal> MANDANTNO { get; set; }
    }
}