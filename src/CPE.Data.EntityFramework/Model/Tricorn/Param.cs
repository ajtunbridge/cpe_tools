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
    
    public partial class Param
    {
        public int Record_Reference { get; set; }
        public string ParamType { get; set; }
        public string MachineID { get; set; }
        public string LoginID { get; set; }
        public string ParamName { get; set; }
        public string DataType { get; set; }
        public Nullable<int> ValueInt { get; set; }
        public Nullable<double> ValueFloat { get; set; }
        public Nullable<bool> ValueBool { get; set; }
        public string ValueString { get; set; }
    }
}
