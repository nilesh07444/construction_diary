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
        [Required]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Display(Name = "Firm Name")]
        public string FirmName { get; set; }

        [Required]
        [Display(Name = "Package Type")]
        public int PackageTypeId { get; set; }

        public string PackageType { get; set; }

        [Display(Name = "Expiry Date")]
        public string ExpireDate { get; set; }
        public DateTime? dtExpireDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [Display(Name = "Remarks")]
        public string Remarks { get; set; }
        public Guid ClientId { get; set; }
        public List<SelectListItem> PackageTypeList { get; set; }
        public int TotalUsers { get; set; }

    }
}