using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public class ChangePasswordVM
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirmation Password must match.")]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }

    }
}