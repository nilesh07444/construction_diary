using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public class FinanceList
    {
        public Guid FinanceId { get; set; }
        public Guid PersonId { get; set; }
        public DateTime SelectedDate { get; set; }
        public decimal Amount { get; set; }
        public Guid? SiteId { get; set; }
        public string SiteName { get; set; }
        public string CreditOrDebit { get; set; }
        public Guid GivenAmountBy { get; set; }
        public string PaymentType { get; set; }
        public string ChequeNo { get; set; }
        public string BankName { get; set; }
        public string ChequeFor { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string PersonName { get; set; }
        public string FirstName { get; set; }
    }

    public class MyFinanceList
    {
        public Guid ContractorFinanceId { get; set; }
        public Guid? SiteId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? SelectedDate { get; set; }
        public decimal? Amount { get; set; }
        public string CreditOrDebit { get; set; }
        public string SiteName { get; set; }
        public string PaymentType { get; set; }
        public string ChequeNo { get; set; }
        public string BankName { get; set; }
        public string ChequeFor { get; set; }
        public string Remarks { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string FirstName { get; set; }
    }
    public class DashboardCount
    {
        public long TotalClients { get; set; }
        public long TotalPersons { get; set; }
        public long TotalSites { get; set; }
        public string TotalGivenAmountToPersons { get; set; }
        public string TotalTakenAmountFromSites { get; set; }
        public string TotalBalanceAmount { get; set; }

    }
}