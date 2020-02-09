using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConstructionDiary.ViewModel
{
    public class SiteInfoVM
    {
        public Guid SiteId { get; set; }
        public string SiteName { get; set; }
        public decimal TotalCreditAmount { get; set; }
        public decimal TotalDebitAmount { get; set; }
        public decimal TotalMaterialAmount { get; set; }
        public decimal TotalPersonAttendanceAmount { get; set; }
        public decimal TotalExpenseAmount { get; set; } 
        
        public decimal CalculatedCreditAmount { get; set; }
        public decimal CalculatedDebitAmount { get; set; }
        public decimal CalculatedBalanceAmount { get; set; }

    }
  
}