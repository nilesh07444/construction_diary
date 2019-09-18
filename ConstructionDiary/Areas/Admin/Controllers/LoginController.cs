using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        ConstructionDiaryEntities _db;
        public LoginController()
        {
            _db = new ConstructionDiaryEntities();
        }
        // GET: Admin/Login
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginVM userLogin)
        {
            try
            {
                string EncyptedPassword = userLogin.Password; // Encrypt(userLogin.Password);
                var data = _db.tbl_Users.Where(x => (x.UserName == userLogin.UserName || x.EmailId == userLogin.UserName)
                && x.Password == EncyptedPassword && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();

                if (data != null)
                {
                    clsSession.SessionID = Session.SessionID;
                    clsSession.UserID = data.UserId;
                    clsSession.UserName = data.FirstName + " " + data.LastName;
                    clsSession.ClientID = data.ClientId;
                    clsSession.ImagePath = data.UserPhoto;
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    TempData["LoginError"] = "Invalid username or password";
                    return View();
                }
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        public ActionResult Signout()
        {
            clsSession.SessionID = "";
            return RedirectToAction("Index");
        }

        /* For testing */
        public string Encrypt(string text)
        {
            string value = CoreHelper.Encrypt(text);
            return value;
        }
        public string Decrypt(string text)
        {
            string value = CoreHelper.Decrypt(text);
            return value;
        }


    }
}