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
    public class BillSiteVM
    {
        public Guid BillId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "BillDate", ResourceType = typeof(Resource))]
        public string BillDate { get; set; }

        [Display(Name = "BillNo", ResourceType = typeof(Resource))]
        public string BillNo { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "BillType", ResourceType = typeof(Resource))]
        public string BillType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "SiteName", ResourceType = typeof(Resource))]
        public Guid SiteId { get; set; }

        [Display(Name = "SquareFeet", ResourceType = typeof(Resource))]
        public decimal? SquareFeet { get; set; }

        [Display(Name = "Rate", ResourceType = typeof(Resource))]
        public decimal? Rate { get; set; }

        [Display(Name = "Total", ResourceType = typeof(Resource))]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "Remarks", ResourceType = typeof(Resource))]
        public string Remarks { get; set; }

        public string BillFileName { get; set; }
        [Display(Name = "BillFile", ResourceType = typeof(Resource))]
        public HttpPostedFileBase BillFile { get; set; }

        public bool IsActive { get; set; }
        public string SiteName { get; set; }
        public DateTime dBillDate { get; set; }
    }

    public class BillSiteNewVM
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

        [Display(Name = "Total", ResourceType = typeof(Resource))]
        public decimal? TotalAmount { get; set; }

        [Display(Name = "Remarks", ResourceType = typeof(Resource))]
        public string Remarks { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "BillFile", ResourceType = typeof(Resource))]
        public HttpPostedFileBase BillFile { get; set; }

        public bool IsActive { get; set; }
        public string SiteName { get; set; }
        public DateTime dBillDate { get; set; }
        public string BillFileName { get; set; }
        public string BillType { get; set; }

        public tbl_Files ObjFile { get; set; }
    }

    public class AreaSiteBillVM
    {
        public Guid? BillId { get; set; }
        public Guid SiteId { get; set; }
        public DateTime dtBillDate { get; set; }
        public string BillDate { get; set; }
        public string BillNo { get; set; }
        public string Remarks { get; set; }
        public List<AreaSiteBillItemVM> BillSiteItem { get; set; }

        // Additional fields
        public decimal? GrandTotal { get; set; }
    }
    public class AreaSiteBillItemVM
    {
        public Guid? BillSiteItemId { get; set; }
        public string ItemCategory { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Area { get; set; }
        //public decimal? Rate { get; set; }
        //public decimal? Amount { get; set; }
        public int SeqNo { get; set; }
    }

    public class BillSiteAbstractVM
    {
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string ItemCategory { get; set; }
        public IEnumerable<AreaSiteBillItemVM> BillSiteItem { get; set; }
    }

    public class BillSiteFinalVM
    {
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string ItemCategory { get; set; }
        public decimal? Area { get; set; } = 0;
        public decimal? Rate { get; set; } = 0;
        public decimal? Amount { get; set; } = 0;
    }

    public class SaveFinalBillVM
    {
        public Guid BillId { get; set; }
        public List<SaveFinalBillItemVM> FinalBillItemVM { get; set; }
    }

    public class SaveFinalBillItemVM
    {
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public decimal? Area { get; set; } = 0;
        public decimal? Rate { get; set; } = 0;
        public decimal? Amount { get; set; } = 0;
    }

}