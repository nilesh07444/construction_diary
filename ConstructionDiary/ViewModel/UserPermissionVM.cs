using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public class UserPermissionVM
    {        
        public bool Party { get; set; }
        public bool Site { get; set; }
        public bool Debit { get; set; }
        public bool Credit { get; set; }
        public bool Expense { get; set; }
        public bool Material { get; set; }
        public bool Estimate { get; set; }
        public bool Challan { get; set; }
        public bool Attendance { get; set; }
        public bool Pagar { get; set; }
        public bool Peticash { get; set; }
        public bool Merchant { get; set; }
    }
}