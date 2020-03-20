using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary
{
    public class BillItemVM
    {
        public Guid BillItemId { get; set; }
        public string BillItemName { get; set; }
        public int BillItemTypeId { get; set; }
        public string Remarks { get; set; }        
        public bool IsActive { get; set; }

        public List<SelectListItem> BillItemTypeList { get; set; }
        public string BillItemType { get; set; }
    }
}