using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;
using Newtonsoft.Json;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    public class LoginController : MyBaseController
    {
        ConstructionDiaryEntities _db;
        public LoginController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            //if (clsSession.SessionID != null)
            //    return RedirectToAction("Index", "Dashboard");

            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginVM userLogin)
        {
            try
            {
                string EncyptedPassword = userLogin.Password; // Encrypt(userLogin.Password);
                var data = _db.tbl_Users.Where(x => (x.UserName == userLogin.UserName || x.EmailId == userLogin.UserName)
                && x.Password == EncyptedPassword && !x.IsDeleted).FirstOrDefault();

                if (data != null)
                {
                    // Get Client Details
                    if (data.ClientId != null)
                    {
                        Guid clientId = new Guid(data.ClientId.ToString());
                        var clientData = _db.tbl_Clients.Where(x => x.ClientId == clientId).FirstOrDefault();
                        DateTime todayDate = DateTime.UtcNow;

                        if (clientData.IsDeleted)
                        {
                            TempData["LoginError"] = "Your Firm is deleted. Please contact administrator.";
                            return View();
                        }
                        else if (!clientData.IsActive)
                        {
                            TempData["LoginError"] = "Your Firm is not active. Please contact administrator.";
                            return View();
                        }
                        else if (!data.IsActive)
                        {
                            TempData["LoginError"] = "Your Account is not active. Please contact administrator.";
                            return View();
                        }
                        else if (
                                    (clientData.PackageTypeId < (int)PackageTypes.OneTimePackage) &&
                                    (clientData.ExpireDate != null) &&
                                    (clientData.ExpireDate.Value.Date < todayDate.Date)
                                )
                        {
                            TempData["LoginError"] = "Your Trial Period Expired. Please contact administrator.";
                            return View();
                        }
                        else
                        {
                            clsSession.FirmName = clientData.FirmName;
                        }
                    }

                    clsSession.SessionID = Session.SessionID;
                    clsSession.UserID = data.UserId;
                    clsSession.RoleID = data.RoleId;
                    clsSession.UserName = data.FirstName;
                    clsSession.ClientID = data.ClientId;
                    clsSession.ImagePath = data.UserPhoto;

                    List<UserPageModuleAccessVM> lstPagePermissions = CoreHelper.GetAssignedUserPageAccessListWhileLogin(data.UserId, data.RoleId);

                    UserPermissionVM objUserPermission = new UserPermissionVM();
                    objUserPermission.Party = lstPagePermissions.Where(x => x.PageModuleId == (int)UserPermissionEnum.Party).First().IsAssigned;
                    objUserPermission.Site = lstPagePermissions.Where(x => x.PageModuleId == (int)UserPermissionEnum.Site).First().IsAssigned;
                    objUserPermission.Debit = lstPagePermissions.Where(x => x.PageModuleId == (int)UserPermissionEnum.Debit).First().IsAssigned;
                    objUserPermission.Credit = lstPagePermissions.Where(x => x.PageModuleId == (int)UserPermissionEnum.Credit).First().IsAssigned;
                    objUserPermission.Expense = lstPagePermissions.Where(x => x.PageModuleId == (int)UserPermissionEnum.Expense).First().IsAssigned;
                    objUserPermission.Material = lstPagePermissions.Where(x => x.PageModuleId == (int)UserPermissionEnum.Material).First().IsAssigned;
                    objUserPermission.Estimate = lstPagePermissions.Where(x => x.PageModuleId == (int)UserPermissionEnum.Estimate).First().IsAssigned;
                    objUserPermission.Challan = lstPagePermissions.Where(x => x.PageModuleId == (int)UserPermissionEnum.Challan).First().IsAssigned;
                    objUserPermission.Attendance = lstPagePermissions.Where(x => x.PageModuleId == (int)UserPermissionEnum.Attendance).First().IsAssigned;
                    objUserPermission.Pagar = lstPagePermissions.Where(x => x.PageModuleId == (int)UserPermissionEnum.Pagar).First().IsAssigned;
                    objUserPermission.Peticash = lstPagePermissions.Where(x => x.PageModuleId == (int)UserPermissionEnum.Peticash).First().IsAssigned;
                    objUserPermission.Merchant = lstPagePermissions.Where(x => x.PageModuleId == (int)UserPermissionEnum.Merchant).First().IsAssigned;

                    string jsonPermissionValues = JsonConvert.SerializeObject(objUserPermission, Formatting.Indented, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    clsSession.UserPermission = jsonPermissionValues;

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
                TempData["LoginError"] = "Error: " + ex.Message.ToString();
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