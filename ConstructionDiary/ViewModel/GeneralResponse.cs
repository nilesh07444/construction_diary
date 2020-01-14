using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public class GeneralResponse
    {
        public bool IsError { get; set; } = false;
        public string ErrorMessage = "";
        public string RedirectUrl = "";
    }
}