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
    
    public partial class tbl_Merchant
    {
        public System.Guid MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string FirmName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public System.Guid ClientId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual tbl_Clients tbl_Clients { get; set; }
    }
}