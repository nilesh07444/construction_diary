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
    public class BillDebitVM
    {
        public Guid BillId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "BillDate", ResourceType = typeof(Resource))]
        public string BillDate { get; set; }

        [Display(Name = "BillNo", ResourceType = typeof(Resource))]
        public string BillNo { get;
            set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "BillType", ResourceType = typeof(Resource))]
        public string BillType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "SiteName", ResourceType = typeof(Resource))]
        public Guid SiteId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public Guid PersonId { get; set; }
        [Display(Name = "SquareFeet", ResourceType = typeof(Resource))]
        public decimal? SquareFeet { get; set; }

        [Display(Name = "Rate", ResourceType = typeof(Resource))]
        public decimal? Rate { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "Remarks", ResourceType = typeof(Resource))]
        public string Remarks { get; set; }
         
        public bool IsActive { get; set; }
        public string SiteName { get; set; }
        public string PersonName { get; set; }
        public DateTime dBillDate { get; set; }
        public List<SelectListItem> SiteList { get; set; }
    }

    public class BillDebitNewVM
    {
        public Guid BillId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "BillDate", ResourceType = typeof(Resource))]
        public string BillDate { get; set; }

        [Display(Name = "BillNo", ResourceType = typeof(Resource))]
        public string BillNo { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "SiteName", ResourceType = typeof(Resource))]
        public Guid SiteId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public Guid PersonId { get; set; }
         
        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "Remarks", ResourceType = typeof(Resource))]
        public string Remarks { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "BillFile", ResourceType = typeof(Resource))]
        public HttpPostedFileBase BillFile { get; set; }
        public bool IsActive { get; set; }
        public string SiteName { get; set; }
        public string PersonName { get; set; }
        public DateTime dBillDate { get; set; }
        public List<SelectListItem> SiteList { get; set; } 
        public tbl_Files ObjFile { get; set; }
        public string BillType { get; set; }
    }

}