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
    
    public partial class tbl_VehicleRent
    {
        public System.Guid VehicleRentId { get; set; }
        public System.Guid VehicleOwnerId { get; set; }
        public System.DateTime VehicleRentDate { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public string VehicleNumber { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public bool IsPaid { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public System.Guid ClientId { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
