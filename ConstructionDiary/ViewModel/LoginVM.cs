using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ConstructionDiary.ResourceFiles;

namespace ConstructionDiary
{
    public class LoginVM
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string Password { get; set; }
    }

    public class ExportPdfVM
    { 
        public string Sr { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    }

}