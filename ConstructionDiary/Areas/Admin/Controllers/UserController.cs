using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class UserController : Controller
    {
        ConstructionDiaryEntities _db;
        public UserController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<UserVM> lstUsers = (from user in _db.tbl_Users
                                     join role in _db.tbl_Role on user.RoleId equals role.RoleId
                                     where user.ClientId == ClientId && !user.IsDeleted
                                     select new UserVM
                                     {
                                         UserId = user.UserId,
                                         Firstname = user.FirstName,
                                         EmailId = user.EmailId,
                                         MobileNo = user.MobileNo,
                                         RoleId = user.RoleId,
                                         RoleName = role.RoleName,
                                         UserName = user.UserName,
                                         Password = user.Password,
                                         UserPhoto = user.UserPhoto,
                                         IsActive = user.IsActive
                                     }).ToList();

            return View(lstUsers);
        }

        public ActionResult Add()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            UserVM user = new UserVM();
            user.ClientId = ClientId;

            user.UserRoleList = _db.tbl_Role.Where(x => x.RoleId > 1)
                         .Select(o => new SelectListItem { Value = o.RoleId.ToString(), Text = o.RoleName })
                         .ToList();

            return View(user);
        }

        [HttpPost]
        public ActionResult Add(UserVM user)
        {

            user.UserRoleList = _db.tbl_Role.Where(x => x.RoleId > 1)
                         .Select(o => new SelectListItem { Value = o.RoleId.ToString(), Text = o.RoleName })
                         .ToList();

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                try
                {
                    var existData = _db.tbl_Users.Where(x => !x.IsDeleted &&
                                                       (x.UserName.ToLower() == user.UserName.ToLower() || x.EmailId.ToLower() == user.EmailId.ToLower())).FirstOrDefault();

                    if (existData != null)
                    {
                        if (existData.UserName.ToLower() == user.UserName.ToLower())
                        {
                            ModelState.AddModelError("Username", "UserName is already exist");
                        }
                        if (existData.EmailId.ToLower() == user.EmailId.ToLower())
                        {
                            ModelState.AddModelError("EmailId", "EmailId is already exist");
                        }

                        return View(user);
                    }

                    tbl_Users objUser = new tbl_Users();
                    objUser.UserId = Guid.NewGuid();
                    objUser.UserName = user.UserName;
                    objUser.Password = user.Password;
                    objUser.ClientId = user.ClientId;
                    objUser.RoleId = user.RoleId;
                    objUser.FirstName = user.Firstname;
                    objUser.EmailId = user.EmailId;
                    objUser.MobileNo = user.MobileNo;
                    objUser.IsActive = true;
                    objUser.IsDeleted = false;
                    objUser.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Users.Add(objUser);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                }
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Edit(Guid id)
        {
            UserVM user = new UserVM();

            try
            {
                user.UserRoleList = _db.tbl_Role.Where(x => x.RoleId > 1)
                             .Select(o => new SelectListItem { Value = o.RoleId.ToString(), Text = o.RoleName })
                             .ToList();

                var existData = _db.tbl_Users.Where(x => x.UserId != id && !x.IsDeleted &&
                                                       (x.UserName.ToLower() == user.UserName.ToLower() || x.EmailId.ToLower() == user.EmailId.ToLower())).FirstOrDefault();

                if (existData != null)
                {
                    if (existData.UserName.ToLower() == user.UserName.ToLower())
                    {
                        ModelState.AddModelError("Username", "UserName is already exist");
                    }
                    if (existData.EmailId.ToLower() == user.EmailId.ToLower())
                    {
                        ModelState.AddModelError("EmailId", "EmailId is already exist");
                    }

                    return View(user);
                }

                tbl_Users objUser = _db.tbl_Users.Where(x => x.UserId == id).FirstOrDefault();

                if (objUser != null)
                {
                    user.UserId = objUser.UserId;
                    user.Firstname = objUser.FirstName;
                    user.UserName = objUser.UserName;
                    user.Password = objUser.Password;
                    user.EmailId = objUser.EmailId;
                    user.RoleId = objUser.RoleId;
                    user.ClientId = objUser.ClientId;
                    user.EmailId = objUser.EmailId;
                    user.MobileNo = objUser.MobileNo;
                    user.UserPhoto = objUser.UserPhoto;
                }

                
            }
            catch (Exception ex)
            {

            }

            return View(user);
        }

        [HttpPost]
        public string DeleteUser(Guid UserId)
        {
            string ReturnMessage = "";

            try
            {
                tbl_Users objUser = _db.tbl_Users.Where(x => x.UserId == UserId && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objUser == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objUser.IsDeleted = true;
                    objUser.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();
                    ReturnMessage = "success";
                }
            }
            catch (Exception ex)
            {
                ReturnMessage = "exception";
            }

            return ReturnMessage;
        }

    }
}