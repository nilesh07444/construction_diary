using ConstructionDiary.Models;
using ConstructionDiary.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary
{
    public class ChallanVM
    {
        public Guid ChallanId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "ChallanDate", ResourceType = typeof(Resource))]
        public string ChallanDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "ChallanNo", ResourceType = typeof(Resource))]
        public string ChallanNo { get; set; }
        
        [Display(Name = "SiteName", ResourceType = typeof(Resource))]
        public Guid SiteId { get; set; }

        [Display(Name = "MerchantName", ResourceType = typeof(Resource))]
        public Guid? MerchantId { get; set; }

        [Display(Name = "CarNo", ResourceType = typeof(Resource))]
        public string CarNo { get; set; }

        [Display(Name = "ChallanPhotoFile", ResourceType = typeof(Resource))]
        public HttpPostedFileBase ChallanPhotoFile { get; set; }

        [Display(Name = "Remarks", ResourceType = typeof(Resource))]
        public string Remarks { get; set; }

        public Guid ClientId { get; set; }
        public bool IsActive { get; set; }

        // Additional Fields
        public string ChallanPhoto { get; set; }
        public string SiteName { get; set; }
        public string MerchantName { get; set; }
        public DateTime dtChallanDate { get; set; }

        public tbl_Files ObjFile { get; set; }

        public List<SelectListItem> MerchantList { get; set; }
        public List<SelectListItem> SiteList { get; set; }

    }
}