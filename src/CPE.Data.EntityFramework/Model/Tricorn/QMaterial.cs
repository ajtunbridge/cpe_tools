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
    
    public partial class QMaterial
    {
        public int QMaterial_Reference { get; set; }
        public string Name { get; set; }
        public Nullable<double> Items { get; set; }
        public double Quantity { get; set; }
        public Nullable<double> Needed { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<int> Cost_Quantity { get; set; }
        public Nullable<decimal> Total_Cost { get; set; }
        public Nullable<decimal> Unit_Cost { get; set; }
        public string Form { get; set; }
        public string Notes { get; set; }
        public Nullable<int> Supplier_Reference { get; set; }
        public int Quote_Reference { get; set; }
        public int Material_Reference { get; set; }
        public int Client_Reference { get; set; }
        public System.DateTime Date_Created { get; set; }
        public System.DateTime Date_Last_Modified { get; set; }
        public Nullable<double> Uplift_Percentage { get; set; }
        public Nullable<decimal> Surcharge { get; set; }
        public Nullable<int> Delivery_Lead_Time { get; set; }
        public Nullable<decimal> Adjustment { get; set; }
        public Nullable<int> NumFileAttachments { get; set; }
        public Nullable<int> NumBrokenLinks { get; set; }
    
        public virtual Quote Quote { get; set; }
    }
}
