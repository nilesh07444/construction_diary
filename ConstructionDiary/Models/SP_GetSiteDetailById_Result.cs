//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConstructionDiary.Models
{
    using System;
    
    public partial class SP_GetSiteDetailById_Result
    {
        public Nullable<System.Guid> ContractorFinanceId { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> SelectedDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> UpdatedAmount { get; set; }
        public string CreditOrDebit { get; set; }
        public Nullable<System.Guid> SiteId { get; set; }
        public string PaymentType { get; set; }
        public string ChequeNo { get; set; }
        public string BankName { get; set; }
        public string ChequeFor { get; set; }
        public string Remarks { get; set; }
    }
}
