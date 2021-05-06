using ConstructionDiary.ResourceFiles;
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
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "UserName", ResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string Firstname { get; set; }

        [Display(Name = "EmailId", ResourceType = typeof(Resource))]
        public string EmailId { get; set; }

        [Display(Name = "MobileNo", ResourceType = typeof(Resource))]
        public string MobileNo { get; set; }

        public string UserPhoto { get; set; }

        //[ImageValidate]
        public HttpPostedFileBase PostedFile { get; set; }

        public Guid UserId { get; set; }
        public Guid? ClientId { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Role", ResourceType = typeof(Resource))]
        public int RoleId { get; set; }

        public string RoleName { get; set; }
        public List<SelectListItem> UserRoleList { get; set; }
        public decimal? PeticashBalance { get; set; }

    }
     
}