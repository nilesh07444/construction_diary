using ConstructionDiary.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary
{
    public class BillItemVM
    {

        public Guid BillItemId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "ItemName", ResourceType = typeof(Resource))]
        public string BillItemName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "ItemType", ResourceType = typeof(Resource))]
        public int BillItemTypeId { get; set; }
        [Display(Name = "Remarks", ResourceType = typeof(Resource))]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }

        public List<SelectListItem> BillItemTypeList { get; set; }
        public string BillItemType { get; set; }
    }
}