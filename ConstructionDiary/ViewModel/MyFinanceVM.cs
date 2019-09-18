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
        [Required(ErrorMessage = "This field is required")]
        public string SelectedDate { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public Guid? UserId { get; set; }
        
        public List<SelectListItem> UsersList { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public Guid? SiteId { get; set; }

        public List<SelectListItem> SitesList { get; set; }
         
        [Required(ErrorMessage = "This field is required")]
        public string ReasonFor { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string PaymentType { get; set; }

        public string CreditOrDebit { get; set; }

        public string ChequeNo { get; set; }

        public string BankName { get; set; }

        public string BranchName { get; set; }

        public string Remarks { get; set; }

    }
}