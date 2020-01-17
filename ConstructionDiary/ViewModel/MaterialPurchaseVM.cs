using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public class MaterialPurchaseVM
    {
        public Guid? MaterialPurchaseId { get; set; }
        public string PurchaseDate { get; set; }
        public DateTime dtPurchaseDate { get; set; } 
        public Guid SiteId { get; set; }
        public Guid? MerchantId { get; set; }
        public string Remarks { get; set; }
        public decimal? GST_Per { get; set; }
        public decimal? CGST_Amount { get; set; }
        public decimal? SGST_Amount { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? AdjustmentAmount { get; set; }
        public decimal? Total { get; set; }
        public bool IsActive { get; set; }
        public List<MaterialPurchaseItemVM> PurchaseItem { get; set; }
        public string SiteName { get; set; }
        public string MerchantName { get; set; }
    }
}