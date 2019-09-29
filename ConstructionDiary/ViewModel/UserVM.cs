using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary
{
      
    public class UserVM
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
         
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Firstname { get; set; }

        [Display(Name = "Email Id")]
        public string EmailId { get; set; }

        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        public string UserPhoto { get; set; }

        //[ImageValidate]
        public HttpPostedFileBase PostedFile { get; set; }

        public Guid UserId { get; set; }
        public Guid? ClientId { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }

        public string RoleName { get; set; }
        public List<SelectListItem> UserRoleList { get; set; }
    }
     

}