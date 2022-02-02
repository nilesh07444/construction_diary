using ConstructionDiary.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary
{
    public class VehicleRentVM
    {
        public Guid VehicleRentId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        //[Display(Name = "VehicleRentDate", ResourceType = typeof(Resource))]
        public string VehicleRentDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        //[Display(Name = "VehicleOwnerName", ResourceType = typeof(Resource))]
        public Guid VehicleOwnerId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        //[Display(Name = "FromLocation", ResourceType = typeof(Resource))]
        public string FromLocation { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        //[Display(Name = "ToLocation", ResourceType = typeof(Resource))]
        public string ToLocation { get; set; }

        //[Display(Name = "VehicleNumber", ResourceType = typeof(Resource))]
        public string VehicleNumber { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resource))]
        public decimal? Amount { get; set; }

        //[Display(Name = "IsPaid", ResourceType = typeof(Resource))]
        public bool IsPaid { get; set; }

        [Display(Name = "Remarks", ResourceType = typeof(Resource))]
        public string Remarks { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        //
        public DateTime dtVehicleRentDate { get; set; }
        public string VehicleOwnerName { get; set; }

        public List<SelectListItem> VehicleOwnerList { get; set; }
    }

    public class VehicleOwnerVM
    {
        public Guid VehicleOwnerId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        //[Display(Name = "VehicleOwnerName", ResourceType = typeof(Resource))]
        public string VehicleOwnerName { get; set; }

        [Display(Name = "MobileNo", ResourceType = typeof(Resource))]
        public string MobileNo { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Remarks", ResourceType = typeof(Resource))]
        public string Remarks { get; set; }
    }
}