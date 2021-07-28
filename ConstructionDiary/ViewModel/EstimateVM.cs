using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public class EstimateVM
    {
        public Guid? EstimateId { get; set; }
        public DateTime? EstimateDate { get; set; }
        public string PartyName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<EstimateItemVM> EstimateItem { get; set; }

        //
        public string strEstimateDate { get; set; }
    }

    public class EstimateItemVM
    {
        public Guid? EstimateItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Nos { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

}