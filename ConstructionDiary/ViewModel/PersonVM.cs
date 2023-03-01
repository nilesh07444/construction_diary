using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ConstructionDiary.ResourceFiles;

namespace ConstructionDiary
{
    public class PersonVM
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "Name", ResourceType = typeof(Resource))]
        public string Firstname { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Display(Name = "MobileNo", ResourceType = typeof(Resource))]
        public string MobileNo { get; set; }

        [Display(Name = "DailyRate", ResourceType = typeof(Resource))]
        public int? DailyRate { get; set; }

        [Display(Name = "PersonType", ResourceType = typeof(Resource))]
        public int? PersonTypeId { get; set; }

        public List<SelectListItem> PersonTypeList { get; set; }

        //[Required(ErrorMessage = "Please Upload Photo")]
        //[ImageValidate]
        public HttpPostedFileBase PostedFile { get; set; }

        public string strPersonPhoto { get; set; }
        public Guid PersonId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class AttendancePersonVM
    {
        public Guid PersonId { get; set; }
        public string PersonFirstName { get; set; }
        public bool IsGroupPerson { get; set; }
        public Guid? PersonGroupId { get; set; }
        public bool? IsActive { get; set; }

        //
        public int? PersonTypeId { get; set; }
        public string PersonTypeName { get; set; }
        public int? OrderNo { get; set; } 
        public decimal RemainingAmount { get; set; }
    }

}