using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public class MaterialTypeVM
    {
        public Guid MaterialTypeId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Material Type")]
        public string MaterialType { get; set; }

        public bool IsActive { get; set; }
    }
}