using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    public class SettingController : MyBaseController
    {
        // GET: Admin/Setting
        public ActionResult Index()
        {
            ViewBag.SelectedLanguage = Request.Cookies["culture"].Value;
            return View();
        }
        public ActionResult ChangeLanguage(string lang)
        {
            new LanguageMang().SetLanguage(lang);
            return RedirectToAction("Index");
        }
    }
}