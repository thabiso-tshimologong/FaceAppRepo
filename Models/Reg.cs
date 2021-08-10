using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Face_Recognition.Models
{
    public class Reg
    {
        public int user_id { get; set; }
        [Required(ErrorMessage = "Enter Your Email!")]
        [Display(Name = "Email:")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]

        public string username { get; set; }
        [Required(ErrorMessage = "Enter Password!")]
        [Display(Name = "Password")]
        [StringLength(maximumLength: 10, MinimumLength = 8, ErrorMessage = "Password Must be Between 8 - 10 CHaracters")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required(ErrorMessage = "Enter Confirm-Password!")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("password")]
        public string conf_pass { get; set; }

        [Required(ErrorMessage = "Upload Photo Please")]
        [Display(Name = "Photo")]
        public string photo { get; set; }
        [Required(ErrorMessage = "Choose Student role")]
        [Display(Name = "Role")]
        public int role_id { get; set; }
    }
}
