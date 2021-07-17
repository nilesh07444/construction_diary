using ConstructionDiary.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public class PartyVM
    {
        public Guid PartyId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        [Display(Name = "PartyName", ResourceType = typeof(Resource))]
        public string PartyName { get; set; }

        [Display(Name = "Remarks", ResourceType = typeof(Resource))]
        public string Remarks { get; set; }

        public Guid ClientId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }        
    }
}