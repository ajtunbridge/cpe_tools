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
    
    public partial class Batch
    {
        public int Batch_Reference { get; set; }
        public double Quantity { get; set; }
        public Nullable<System.DateTime> Workshop_Date { get; set; }
        public string User_Reference { get; set; }
        public bool Delivery_Required { get; set; }
        public Nullable<int> DItem_Reference { get; set; }
        public bool Invoice_Required { get; set; }
        public Nullable<int> InvItem_Reference { get; set; }
        public Nullable<decimal> Total_Cost { get; set; }
        public string Notes { get; set; }
        public Nullable<int> WOrder_Reference { get; set; }
        public int Client_Reference { get; set; }
        public System.DateTime Date_Created { get; set; }
        public System.DateTime Date_Last_Modified { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public string DNote_User_Reference { get; set; }
        public string Invoice_User_Reference { get; set; }
        public bool Indicated_Last_Batch { get; set; }
        public bool Added_To_Stock { get; set; }
        public bool Scrapped { get; set; }
        public bool Hold { get; set; }
        public Nullable<int> NumFileAttachments { get; set; }
        public Nullable<int> NumBrokenLinks { get; set; }
        public Nullable<int> Reserve_WOPart_Reference { get; set; }
        public Nullable<int> Reserve_WOrder_Reference { get; set; }
    
        public virtual WOrder WOrder { get; set; }
    }
}
