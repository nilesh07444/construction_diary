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
    public class ExpenseVM
    {
        public Guid ExpenseId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Date", ResourceType = typeof(Resource))]
        public string ExpenseDate { get; set; }

        public DateTime dtExpenseDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public decimal? Amount { get; set; } = null;

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Description", ResourceType = typeof(Resource))]
        public string Description { get; set; }

        [Display(Name = "SiteName", ResourceType = typeof(Resource))]
        public Guid? SiteId { get; set; }
        public string SiteName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "ExpenseType", ResourceType = typeof(Resource))]
        public int ExpenseTypeId { get; set; }
        public string ExpenseType { get; set; }
        public bool IsActive { get; set; }

        public string ExpenseFileName { get; set; }        
        [Display(Name = "ExpenseFile", ResourceType = typeof(Resource))]
        public HttpPostedFileBase ExpenseFile { get; set; }

        public List<SelectListItem> ExpenseTypeList { get; set; }
        public List<SelectListItem> SiteList { get; set; }

        public tbl_Files ObjExpenseFile { get; set; }
    }
}