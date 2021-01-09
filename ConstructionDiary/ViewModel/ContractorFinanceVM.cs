using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public class ContractorFinanceVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime? SelectedDate { get; set; }
        public decimal? Amount { get; set; }
        public string CreditOrDebit { get; set; }
        public Guid? SiteId { get; set; }
        public string PaymentType { get; set; }
        public string ChequeNo { get; set; }
        public string BankName { get; set; } 
        public string ChequeFor { get; set; }
        public string Remarks { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}