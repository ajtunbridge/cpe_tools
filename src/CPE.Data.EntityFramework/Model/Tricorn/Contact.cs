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
    
    public partial class Contact
    {
        public int Contact_Reference { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
        public string Notes { get; set; }
        public Nullable<int> Customer_Reference { get; set; }
        public Nullable<int> Supplier_Reference { get; set; }
        public int Client_Reference { get; set; }
        public System.DateTime Date_Created { get; set; }
        public System.DateTime Date_Last_Modified { get; set; }
    }
}
