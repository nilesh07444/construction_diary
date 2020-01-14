using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public class MaterialPurchaseItemVM
    {
        public Guid? MaterialPurchaseItemId { get; set; }
        public Guid? MaterialPurchasesId { get; set; }
        public Guid MaterialTypeId { get; set; }
        public decimal Weight { get; set; }
        public decimal Rate { get; set; }
        public decimal? Amount { get; set; }
        public bool IsDeleteItem { get; set; }
    }
}