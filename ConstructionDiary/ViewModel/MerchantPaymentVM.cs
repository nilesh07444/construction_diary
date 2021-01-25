using ConstructionDiary.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary
{
    public class MerchantPaymentVM
    {
        public Guid MerchantPaymentId { get; set; }
        [Display(Name = "PaymentDate", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string PaymentDate { get; set; }
        [Display(Name = "SiteName", ResourceType = typeof(Resource))]
        public Guid? SiteId { get; set; }

        [Display(Name = "MerchantName", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public Guid MerchantId { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public decimal? Amount { get; set; }

        [Display(Name = "PaymentType", ResourceType = typeof(Resource))]
        public string PaymentType { get; set; }

        [Display(Name = "ChequeNo", ResourceType = typeof(Resource))]
        public string ChequeNo { get; set; }

        [Display(Name = "BankName", ResourceType = typeof(Resource))]
        public string BankName { get; set; }
        [Display(Name = "ChequeFor", ResourceType = typeof(Resource))]
        public string ChequeFor { get; set; }
        [Display(Name = "Remarks", ResourceType = typeof(Resource))]
        public string Remarks { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime dtPaymentDate { get; set; }
        public string MerchantName { get; set; }
        public string SiteName { get; set; }
        public List<SelectListItem> MerchantList { get; set; }
        public List<SelectListItem> SiteList { get; set; }
    }
}


