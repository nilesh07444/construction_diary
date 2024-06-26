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
    
    public partial class tbl_Expenses
    {
        public System.Guid ExpenseId { get; set; }
        public System.DateTime ExpenseDate { get; set; }
        public int ExpenseTypeId { get; set; }
        public System.Guid ClientId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Nullable<System.Guid> SiteId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual tbl_ExpenseType tbl_ExpenseType { get; set; }
        public virtual tbl_Sites tbl_Sites { get; set; }
    }
}
