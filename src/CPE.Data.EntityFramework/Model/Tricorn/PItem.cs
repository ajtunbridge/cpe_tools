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
    
    public partial class PItem
    {
        public int PItem_Reference { get; set; }
        public Nullable<double> Quantity { get; set; }
        public Nullable<double> Quantity_For_Stock { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public string Form { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public string Drawing_Number { get; set; }
        public string Drawing_Issue { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> Required_Date { get; set; }
        public Nullable<System.DateTime> Delivery_Date { get; set; }
        public Nullable<double> Quantity_Delivered { get; set; }
        public string Notes { get; set; }
        public int POrder_Reference { get; set; }
        public Nullable<int> WOrder_Reference { get; set; }
        public Nullable<int> Material_Reference { get; set; }
        public Nullable<int> Part_Reference { get; set; }
        public Nullable<int> WOMaterial_Reference { get; set; }
        public Nullable<int> WOPart_Reference { get; set; }
        public Nullable<int> WOSubContract_Reference { get; set; }
        public Nullable<int> WOTool_Reference { get; set; }
        public int Client_Reference { get; set; }
        public System.DateTime Date_Created { get; set; }
        public System.DateTime Date_Last_Modified { get; set; }
        public byte[] Private_Notes { get; set; }
        public Nullable<int> DItem_Reference { get; set; }
        public string User_Reference { get; set; }
        public string PriceType { get; set; }
        public Nullable<int> NominalCode_Reference { get; set; }
        public Nullable<int> CostCentre_Reference { get; set; }
        public Nullable<int> VAT_Reference { get; set; }
        public bool Exported { get; set; }
        public Nullable<double> Local_Cost { get; set; }
        public Nullable<System.DateTime> Promise_Date { get; set; }
        public string Unit_Of_Measurement { get; set; }
        public string Part_Number { get; set; }
        public string Part_Issue { get; set; }
    
        public virtual POrder POrder { get; set; }
    }
}
