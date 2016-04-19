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
    
    public partial class Supplier
    {
        public int Supplier_Reference { get; set; }
        public string External_Supplier { get; set; }
        public string Name { get; set; }
        public string Terms { get; set; }
        public string Quality_Program { get; set; }
        public bool Trace { get; set; }
        public Nullable<System.DateTime> Last_Audit { get; set; }
        public string Notes { get; set; }
        public Nullable<int> Contact_Reference { get; set; }
        public Nullable<int> Location_Reference { get; set; }
        public int Client_Reference { get; set; }
        public System.DateTime Date_Created { get; set; }
        public System.DateTime Date_Last_Modified { get; set; }
        public bool Approved { get; set; }
        public string Supplying { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<int> Owned_By { get; set; }
        public Nullable<int> Last_Modified { get; set; }
        public Nullable<int> VAT_Reference { get; set; }
        public string Supplier_Status { get; set; }
        public string Country_Code { get; set; }
        public Nullable<int> NumFileAttachments { get; set; }
        public Nullable<int> NumBrokenLinks { get; set; }
    }
}
