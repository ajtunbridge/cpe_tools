//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CPE.Data.EntityFramework.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TricornTool
    {
        public int Id { get; set; }
        public int ToolId { get; set; }
        public int TricornReference { get; set; }
    
        public virtual Tool Tool { get; set; }
    }
}
