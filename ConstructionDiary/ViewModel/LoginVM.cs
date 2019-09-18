using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConstructionDiary
{
    public class LoginVM
    {
        [Required(ErrorMessage= "UserName is required.")] 
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")] 
        public string Password { get; set; }
    }
}