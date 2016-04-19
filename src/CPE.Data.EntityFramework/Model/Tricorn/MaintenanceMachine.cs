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
    
    public partial class MaintenanceMachine
    {
        public int Machine_Reference { get; set; }
        public string Prefix { get; set; }
        public Nullable<int> Number { get; set; }
        public string Suffix { get; set; }
        public Nullable<int> Procedure_Reference { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string SerialNumber { get; set; }
        public Nullable<System.DateTime> Date_Purchased { get; set; }
        public Nullable<System.DateTime> Date_First_Active { get; set; }
        public string Inhouse_Period { get; set; }
        public Nullable<int> Inhouse_Interval { get; set; }
        public string External_Period { get; set; }
        public Nullable<int> External_Interval { get; set; }
        public string Colour_Code { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> Date_Last_Inhouse { get; set; }
        public Nullable<System.DateTime> Date_Next_Inhouse { get; set; }
        public Nullable<System.DateTime> Date_Last_External { get; set; }
        public Nullable<System.DateTime> Date_Next_External { get; set; }
        public Nullable<System.DateTime> Date_Created { get; set; }
        public Nullable<System.DateTime> Date_Last_Modified { get; set; }
        public Nullable<int> Client_Reference { get; set; }
        public Nullable<System.DateTime> Date_Last_Active { get; set; }
    }
}