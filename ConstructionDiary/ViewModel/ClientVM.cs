using ConstructionDiary.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary
{
    public class ClientVM
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "ClientName", ResourceType = typeof(Resource))]
        public string ClientName { get; set; }

        [Display(Name = "FirmName", ResourceType = typeof(Resource))]
        public string FirmName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "PackageType", ResourceType = typeof(Resource))]
        public int PackageTypeId { get; set; }

        public string PackageType { get; set; }

        [Display(Name = "ExpiryDate", ResourceType = typeof(Resource))]
        public string ExpireDate { get; set; }
         
        [Display(Name = "Address", ResourceType = typeof(Resource))]
        public string Address { get; set; }

        [Display(Name = "Remarks", ResourceType = typeof(Resource))]
        public string Remarks { get; set; }
        public Guid ClientId { get; set; }
        public List<SelectListItem> PackageTypeList { get; set; }
        public int TotalUsers { get; set; }

        //
        public DateTime? dtExpireDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}