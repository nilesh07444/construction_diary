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
    using System.Collections.Generic;
    
    public partial class tbl_ContractorFinance
    {
        public System.Guid ContractorFinanceId { get; set; }
        public Nullable<System.Guid> SiteId { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public Nullable<System.DateTime> SelectedDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string CreditOrDebit { get; set; }
        public string PaymentType { get; set; }
        public string ChequeNo { get; set; }
        public string BankName { get; set; }
        public string ChequeFor { get; set; }
        public string Remarks { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.Guid> UpdatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual tbl_Sites tbl_Sites { get; set; }
    }
}
