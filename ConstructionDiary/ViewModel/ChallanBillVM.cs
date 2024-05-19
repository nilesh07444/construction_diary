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
    public class ChallanBillVM
    {
        public Guid ChallanBillId { get; set; }
        public DateTime dtChallanBillDate { get; set; }
        public Guid MerchantId { get; set; }
        public Guid? ChallanId { get; set; }
        public Guid? ChallanGroupId { get; set; }
        public decimal BillAmount { get; set; }
        public string BillPhoto { get; set; } 

        // Additional fields
        public string MerchantName { get; set; }
        public string ChallanNo { get; set; }
        public tbl_Files ObjFile { get; set; }
    }

    public class ChallanBillRequestVM
    {
        public Guid ChallanBillId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "ChallanBillDate", ResourceType = typeof(Resource))]
        public string ChallanBillDate { get; set; }

        [Display(Name = "ChallanNo", ResourceType = typeof(Resource))]
        public Guid ChallanId { get; set; }

        public List<SelectListItem> ChallanList { get; set; }
    }
}