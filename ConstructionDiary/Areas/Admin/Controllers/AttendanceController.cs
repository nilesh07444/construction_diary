using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    public class AttendanceController : Controller
    {
        // GET: Admin/Attendance
        public ActionResult Index()
        {
            return View();
        }
    }
}