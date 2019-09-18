using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    public class PersonController : Controller
    {
        // GET: Admin/Person
        public ActionResult Index()
        {
            return View();
        }
    }
}