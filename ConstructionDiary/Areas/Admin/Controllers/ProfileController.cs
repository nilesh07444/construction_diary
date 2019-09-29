using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class ProfileController : Controller
    {
        ConstructionDiaryEntities _db;
        public ProfileController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            UserVM objUser = new UserVM();
            try
            {
                tbl_Users obj = _db.tbl_Users.Where(x => x.UserId == clsSession.UserID).FirstOrDefault();
                if (obj != null)
                {
                    objUser.Firstname = obj.FirstName;
                    objUser.EmailId = obj.EmailId;
                    objUser.MobileNo = obj.MobileNo;
                    objUser.UserPhoto = obj.UserPhoto;
                    objUser.UserName = obj.UserName;
                    objUser.Password = obj.Password;
                }
            }
            catch (Exception ex)
            {

            }

            return View(objUser);
        }

        [HttpPost]
        public ActionResult Index(UserVM user, HttpPostedFileBase postedFile)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {

                tbl_Users objUser = _db.tbl_Users.Where(x => x.UserId == clsSession.UserID).FirstOrDefault();
                if (objUser != null)
                {
                    string fileName = string.Empty;
                    string path = Server.MapPath("~/Images/UserPhoto/");
                    if (postedFile != null)
                    {
                        fileName = Guid.NewGuid() + Path.GetFileName(postedFile.FileName);
                        postedFile.SaveAs(path + fileName);
                    }
                    else
                    {
                        fileName = user.UserPhoto;
                    }

                    clsSession.ImagePath = fileName;
                    clsSession.UserName = user.Firstname;

                    objUser.FirstName = user.Firstname;
                    objUser.EmailId = user.EmailId;
                    objUser.MobileNo = user.MobileNo;
                    objUser.UserPhoto = fileName;
                    objUser.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(user);

        }

        [HttpPost]
        public string ChangePassword(FormCollection frm)
        {
            string ReturnMessage = "";
            try
            {
                string CurrentPassword = frm["currentpwd"];
                string NewPassword = frm["newpwd"];

                Guid UserId = clsSession.UserID;
                tbl_Users objUser = _db.tbl_Users.Where(x => x.UserId == UserId).FirstOrDefault();

                if (objUser != null)
                {
                    string EncryptedCurrentPassword = CurrentPassword; // CoreHelper.Encrypt(CurrentPassword);
                    if (objUser.Password == EncryptedCurrentPassword)
                    {
                        objUser.Password = NewPassword; //CoreHelper.Encrypt(NewPassword);
                        _db.SaveChanges();

                        ReturnMessage = "SUCCESS";
                        Session.Clear();
                    }
                    else
                    {
                        ReturnMessage = "CP_NOT_MATCHED";
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnMessage = "ERROR";
            }

            return ReturnMessage;
        }

    }
}