//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Face_Recognition.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class attendace
    {
        [Display(Name = "ID")]
        public int att_id { get; set; }
        [Required(ErrorMessage = "Enter date!")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public System.DateTime att_date { get; set; }
        public string username { get; set; }
    
        public virtual attendace attendace1 { get; set; }
        public virtual attendace attendace2 { get; set; }
    }
}
