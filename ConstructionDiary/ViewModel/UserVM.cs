using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public class UserVM
    {
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        [Required]
        [Display(Name = "Email Id")]
        public string EmailId { get; set; }

        public string UserPhoto { get; set; }

        //[ImageValidate]
        public HttpPostedFileBase PostedFile { get; set; }
    }
}