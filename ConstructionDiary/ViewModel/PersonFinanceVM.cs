using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ConstructionDiary
{
    public class PersonFinanceVM
    {

        [Required(ErrorMessage ="This field is required")]
        public string SelectedDate { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public Guid GivenAmountBy { get; set; }

        public List<SelectListItem> UsersList { get; set; }
        
        [Required(ErrorMessage = "This field is required")]
        public string PaymentType { get; set; }

        public string CreditOrDebit { get; set; }

        public string ChequeNo { get; set; }

        public string BankName { get; set; }

        public string ChequeFor { get; set; }

        public string Remarks { get; set; }

        public Guid PersonId { get; set; }

        public Guid? SiteId { get; set; }
        public List<SelectListItem> SitesList { get; set; }

    }
}


