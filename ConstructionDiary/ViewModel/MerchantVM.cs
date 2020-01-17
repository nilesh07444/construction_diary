using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ConstructionDiary.ResourceFiles;

namespace ConstructionDiary
{
    public class MerchantVM
    {
        public Guid MerchantId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "MerchantName", ResourceType = typeof(Resource))]
        public string MerchantName { get; set; }

        [Display(Name = "FirmName", ResourceType = typeof(Resource))]
        public string FirmName { get; set; }

        [Display(Name = "EmailId", ResourceType = typeof(Resource))]
        public string EmailId { get; set; }

        [MaxLength(15)]
        [Display(Name = "MobileNo", ResourceType = typeof(Resource))]
        public string MobileNo { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resource))]
        public string Address { get; set; } 
        public bool IsActive { get; set; }
    }
}