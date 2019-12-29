using ConstructionDiary.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary
{
    public class MyFinanceVM
    {
        public Guid ContractorFinanceId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string SelectedDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public Guid? UserId { get; set; }
        
        public List<SelectListItem> UsersList { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public Guid? SiteId { get; set; }

        public List<SelectListItem> SitesList { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string PaymentType { get; set; }

        public string CreditOrDebit { get; set; }

        public string ChequeNo { get; set; }

        public string BankName { get; set; }

        public string ChequeFor { get; set; }

        public string Remarks { get; set; }

    }
}