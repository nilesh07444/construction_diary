using ConstructionDiary.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary
{
    public class PeticashVM
    {
        public Guid PeticashId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Staff", ResourceType = typeof(Resource))]
        public Guid UserId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Date", ResourceType = typeof(Resource))]
        public string SelectedDate { get; set; }
        [Display(Name = "CreditDebit", ResourceType = typeof(Resource))]
        public string CreditDebit { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public decimal Amount { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Remarks", ResourceType = typeof(Resource))]
        public string Remarks { get; set; }
        public Guid? TableId { get; set; }
        public int? AmountWhereUsed { get; set; }        
        public Guid ClientId { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        // Additional fields
        public DateTime dtSelectedDate { get; set; }
        public string StaffName { get; set; }
        public List<SelectListItem> StaffList { get; set; } 
    }
}