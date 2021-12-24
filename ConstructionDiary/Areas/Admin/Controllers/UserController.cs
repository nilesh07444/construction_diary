using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class UserController : MyBaseController
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

            if (lstUsers != null && lstUsers.Count > 0)
            {
                lstUsers.ForEach(usr =>
                {
                    List<tbl_Peticase> lstPeticash = _db.tbl_Peticase.Where(x => x.UserId == usr.UserId).ToList();
                    if (lstPeticash != null && lstPeticash.Count > 0)
                    {
                        decimal TotalCredit = lstPeticash.Where(x => x.CreditDebit == "Credit").ToList().Sum(x => x.Amount);
                        decimal TotalDebit = lstPeticash.Where(x => x.CreditDebit == "Debit").ToList().Sum(x => x.Amount);
                        decimal Balance = TotalCredit - TotalDebit;

                        usr.PeticashBalance = Balance;

                    }

                });
            }

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

            List<UserPageModuleAccessVM> UserPageModuleAccessList = CoreHelper.GetAssignedUserPageAccessList(Guid.Empty);
            ViewData["UserPageModuleAccessList"] = UserPageModuleAccessList;

            return View(user);
        }

        [HttpPost]
        public ActionResult Add(UserVM user)
        {

            user.UserRoleList = _db.tbl_Role.Where(x => x.RoleId > 1)
                         .Select(o => new SelectListItem { Value = o.RoleId.ToString(), Text = o.RoleName })
                         .ToList();

            List<UserPageModuleAccessVM> UserPageModuleAccessList = CoreHelper.GetAssignedUserPageAccessList(Guid.Empty);
            ViewData["UserPageModuleAccessList"] = UserPageModuleAccessList;

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                try
                {
                    var existData = _db.tbl_Users.Where(x => !x.IsDeleted &&
                                                       (x.UserName.ToLower() == user.UserName.ToLower() || (!string.IsNullOrEmpty(user.EmailId) && x.EmailId.ToLower() == user.EmailId.ToLower()))
                                                       ).FirstOrDefault();

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

                    #region Assign selected page to user

                    if (user.RoleId == (int)UserRoles.Staff && !string.IsNullOrEmpty(user.strSelectedPageModuleAccess))
                    {
                        List<string> lstSelectedPageModules = user.strSelectedPageModuleAccess.Split(',').ToList<string>();

                        lstSelectedPageModules.ForEach(strPageModuleId =>
                        {
                            long pageModuleId = Convert.ToInt64(strPageModuleId);

                            tbl_UserPageModuleAccess userPageModuleAccessVM = new tbl_UserPageModuleAccess();
                            userPageModuleAccessVM.UserId = objUser.UserId;
                            userPageModuleAccessVM.PageModuleId = pageModuleId;
                            userPageModuleAccessVM.CreatedBy = clsSession.UserID;
                            userPageModuleAccessVM.CreatedDate = DateTime.UtcNow;
                            _db.tbl_UserPageModuleAccess.Add(userPageModuleAccessVM);
                            _db.SaveChanges();

                        });
                    }

                    #endregion

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
                                                       (x.UserName.ToLower() == user.UserName.ToLower() || (!string.IsNullOrEmpty(user.EmailId) && x.EmailId.ToLower() == user.EmailId.ToLower()))).FirstOrDefault();

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


                List<UserPageModuleAccessVM> UserPageModuleAccessList = CoreHelper.GetAssignedUserPageAccessList(id);
                ViewData["UserPageModuleAccessList"] = UserPageModuleAccessList;

            }
            catch (Exception ex)
            {

            }

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(UserVM user)
        {

            user.UserRoleList = _db.tbl_Role.Where(x => x.RoleId > 1)
                         .Select(o => new SelectListItem { Value = o.RoleId.ToString(), Text = o.RoleName })
                         .ToList();

            List<UserPageModuleAccessVM> UserPageModuleAccessList = CoreHelper.GetAssignedUserPageAccessList(user.UserId);
            ViewData["UserPageModuleAccessList"] = UserPageModuleAccessList;

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                try
                {
                    var existData = _db.tbl_Users.Where(x => !x.IsDeleted && x.UserId != user.UserId &&
                                                       (
                                                        x.UserName.ToLower() == user.UserName.ToLower() || (!string.IsNullOrEmpty(user.EmailId) && x.EmailId.ToLower() == user.EmailId.ToLower())
                                                        )).FirstOrDefault();

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

                    tbl_Users objUser = _db.tbl_Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
                    objUser.UserName = user.UserName;
                    objUser.Password = user.Password;
                    objUser.RoleId = user.RoleId;
                    objUser.FirstName = user.Firstname;
                    objUser.EmailId = user.EmailId;
                    objUser.MobileNo = user.MobileNo;
                    objUser.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();

                    #region Assign selected office locations

                    if (user.RoleId == (int)UserRoles.Staff && !string.IsNullOrEmpty(user.strSelectedPageModuleAccess))
                    {
                        List<string> lstSelectedPageModules = user.strSelectedPageModuleAccess.Split(',').ToList<string>();

                        long x = 0;
                        var longSelectedPageModuleList = lstSelectedPageModules.Where(str => long.TryParse(str, out x)).Select(str => x).ToList();

                        // delete unselected locations
                        List<tbl_UserPageModuleAccess> lstUnSelectedEmployeeOfficeLocations = _db.tbl_UserPageModuleAccess.Where(p => p.UserId == user.UserId && !longSelectedPageModuleList.Contains(p.PageModuleId)).ToList();
                        if (lstUnSelectedEmployeeOfficeLocations != null && lstUnSelectedEmployeeOfficeLocations.Count > 0)
                        {
                            _db.tbl_UserPageModuleAccess.RemoveRange(lstUnSelectedEmployeeOfficeLocations);
                            _db.SaveChanges();
                        }

                        // Save missing locations
                        lstSelectedPageModules.ForEach(strPageModuleId =>
                        {
                            long pageModuleId = Convert.ToInt64(strPageModuleId);

                            tbl_UserPageModuleAccess objDuplicateLocation = _db.tbl_UserPageModuleAccess.Where(l => l.UserId == objUser.UserId && l.PageModuleId == pageModuleId).FirstOrDefault();

                            if (objDuplicateLocation == null)
                            {
                                tbl_UserPageModuleAccess userPageModuleAccessVM = new tbl_UserPageModuleAccess();
                                userPageModuleAccessVM.UserId = objUser.UserId;
                                userPageModuleAccessVM.PageModuleId = pageModuleId;
                                userPageModuleAccessVM.CreatedBy = clsSession.UserID;
                                userPageModuleAccessVM.CreatedDate = DateTime.UtcNow;
                                _db.tbl_UserPageModuleAccess.Add(userPageModuleAccessVM);
                                _db.SaveChanges();
                            }
                        });

                    }
                    else
                    {
                        // Remove old locations, if exists
                        List<tbl_UserPageModuleAccess> lstDeleteUserPageModuleAccess = _db.tbl_UserPageModuleAccess.Where(x => x.UserId == user.UserId).ToList();
                        if (lstDeleteUserPageModuleAccess != null && lstDeleteUserPageModuleAccess.Count > 0)
                        {
                            _db.tbl_UserPageModuleAccess.RemoveRange(lstDeleteUserPageModuleAccess);
                            _db.SaveChanges();
                        }
                    }

                    #endregion

                }
                catch (Exception ex)
                {
                }
                return RedirectToAction("Index");
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

        [HttpPost]
        public string ChangeUserStatus(Guid UserId, bool Status)
        {
            string ReturnMessage = "";

            try
            {
                tbl_Users objSite = _db.tbl_Users.Where(x => x.UserId == UserId && x.IsDeleted == false).FirstOrDefault();

                if (objSite == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objSite.IsActive = Status;
                    objSite.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();
                    ReturnMessage = "success";
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                ReturnMessage = "exception";
            }

            return ReturnMessage;
        }

    }
}