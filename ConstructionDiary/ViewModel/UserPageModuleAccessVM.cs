using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public class UserPageModuleAccessVM
    {
        public long UserPageModuleAccessId { get; set; }
        public long PageModuleId { get; set; }
        public Guid UserId { get; set; }
        public bool IsAssigned { get; set; }

        //
        public string PageModuleName { get; set; }
    }
}