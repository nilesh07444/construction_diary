using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ConstructionDiary
{
    public class PersonVM
    {        
        [Required]
        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        //[Required(ErrorMessage = "Please Upload Photo")]
        //[ImageValidate]
        public HttpPostedFileBase PostedFile { get; set; }

        public string strPersonPhoto { get; set; }
        public Guid PersonId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
    
}