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
    
    public partial class DItemMatTrace
    {
        public int MatTrace_Reference { get; set; }
        public Nullable<int> DItem_Reference { get; set; }
        public Nullable<int> MWithdrawal_Reference { get; set; }
        public Nullable<int> WOMaterial_Reference { get; set; }
        public Nullable<int> Material_Reference { get; set; }
        public bool Use_Internal_Batch_No { get; set; }
    
        public virtual DItem DItem { get; set; }
    }
}
