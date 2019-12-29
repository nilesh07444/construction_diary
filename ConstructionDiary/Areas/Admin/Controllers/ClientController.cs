using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class ClientController : MyBaseController
    {
        ConstructionDiaryEntities _db;
        public ClientController()
        {
            _db = new ConstructionDiaryEntities();
        }
        public ActionResult Index()
        {

            List<ClientVM> lstClients = (from client in _db.tbl_Clients
                                         join pkg_type in _db.tbl_PackageType on client.PackageTypeId equals pkg_type.PackageTypeId
                                         where client.IsActive && !client.IsDeleted
                                         select new ClientVM
                                         {
                                             ClientId = client.ClientId,
                                             ClientName = client.ClientName,
                                             FirmName = client.FirmName,
                                             PackageTypeId = client.PackageTypeId,
                                             dtExpireDate = client.ExpireDate,
                                             IsActive = client.IsActive,
                                             PackageType = pkg_type.PackageType,
                                             //TotalUsers = getTotalUsers(client.ClientId)
                                         }).ToList();

            lstClients.ForEach(x =>
            {
                x.TotalUsers = _db.tbl_Users.Where(p => p.ClientId == x.ClientId && !p.IsDeleted).ToList().Count();
            });

            return View(lstClients);
        }

        public ActionResult Add()
        {
            ClientVM client = new ClientVM();

            client.PackageTypeList = _db.tbl_PackageType
                         .Select(o => new SelectListItem { Value = o.PackageTypeId.ToString(), Text = o.PackageType })
                         .ToList();

            return View(client);
        }

        [HttpPost]
        public ActionResult Add(ClientVM client)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {

                try
                {

                    DateTime? exp_date = null;
                    if (client.PackageTypeId <= 5)
                    {
                        if (client.ExpireDate != null)
                            exp_date = DateTime.ParseExact(Convert.ToString(client.ExpireDate), "dd/MM/yyyy", null);
                    }

                    tbl_Clients objClient = new tbl_Clients();
                    objClient.ClientId = Guid.NewGuid();
                    objClient.ClientName = client.ClientName;
                    objClient.FirmName = client.FirmName;
                    objClient.PackageTypeId = client.PackageTypeId;
                    objClient.ExpireDate = exp_date;
                    objClient.IsActive = true;
                    objClient.IsDeleted = false;
                    objClient.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Clients.Add(objClient);
                    _db.SaveChanges();

                }
                catch (Exception ex)
                {

                }
                return RedirectToAction("Index");
            }

            client.PackageTypeList = _db.tbl_PackageType
                         .Select(o => new SelectListItem { Value = o.PackageTypeId.ToString(), Text = o.PackageType })
                         .ToList();

            return View(client);
        }

        public ActionResult Edit(Guid id)
        {
            ClientVM client = new ClientVM();

            tbl_Clients objClient = _db.tbl_Clients.Where(x => x.ClientId == id).FirstOrDefault();

            if (objClient != null)
            {
                client.ClientId = objClient.ClientId;
                client.ClientName = objClient.ClientName;
                client.FirmName = objClient.FirmName;
                client.PackageTypeId = objClient.PackageTypeId;
                client.dtExpireDate = objClient.ExpireDate;
                if (objClient.ExpireDate != null)
                    client.ExpireDate = Convert.ToDateTime(objClient.ExpireDate).ToString("dd/MM/yyyy");
                client.Remarks = objClient.Remarks;
            }

            client.PackageTypeList = _db.tbl_PackageType
                         .Select(o => new SelectListItem { Value = o.PackageTypeId.ToString(), Text = o.PackageType })
                         .ToList();

            return View(client);
        }

        [HttpPost]
        public ActionResult Edit(ClientVM client)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {

                try
                {
                    tbl_Clients objClient = _db.tbl_Clients.Where(x => x.ClientId == client.ClientId).FirstOrDefault();

                    if (objClient != null)
                    {
                        DateTime? exp_date = null;
                        if (client.PackageTypeId <= 5)
                        {
                            if (client.ExpireDate != null)
                                exp_date = DateTime.ParseExact(Convert.ToString(client.ExpireDate), "dd/MM/yyyy", null);
                        }

                        objClient.ClientName = client.ClientName;
                        objClient.FirmName = client.FirmName;
                        objClient.PackageTypeId = client.PackageTypeId;
                        objClient.ExpireDate = exp_date;
                        objClient.Remarks = client.Remarks;
                        objClient.ModifiedDate = DateTime.UtcNow;
                        _db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                }
                return RedirectToAction("Index");
            }

            client.PackageTypeList = _db.tbl_PackageType
                         .Select(o => new SelectListItem { Value = o.PackageTypeId.ToString(), Text = o.PackageType })
                         .ToList();

            return View(client);
        }

        [HttpPost]
        public string DeleteClient(Guid ClientId)
        {
            string ReturnMessage = "";

            try
            {
                tbl_Clients objClient = _db.tbl_Clients.Where(x => x.ClientId == ClientId && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objClient == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objClient.IsDeleted = true;
                    objClient.ModifiedDate = DateTime.UtcNow;
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

        public ActionResult Users(Guid id)
        {
            Guid ClientId = id;
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

            ViewBag.ClientId = ClientId;
            return View(lstUsers);
        }

        public ActionResult AddUser(Guid id)
        {
            UserVM user = new UserVM();
            user.ClientId = id;

            user.UserRoleList = _db.tbl_Role.Where(x => x.RoleId > 1)
                         .Select(o => new SelectListItem { Value = o.RoleId.ToString(), Text = o.RoleName })
                         .ToList();

            return View(user);
        }

        [HttpPost]
        public ActionResult AddUser(UserVM user)
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
                                                       (
                                                           x.UserName.ToLower() == user.UserName.ToLower() || 
                                                           (!string.IsNullOrEmpty(user.EmailId) && x.EmailId.ToLower() == user.EmailId.ToLower())
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
                return RedirectToAction("Users", new { id = user.ClientId });
            }

            return View(user);
        }

        public ActionResult EditUser(Guid id)
        {
            UserVM user = new UserVM();

            try
            {
                user.UserRoleList = _db.tbl_Role.Where(x => x.RoleId > 1)
                         .Select(o => new SelectListItem { Value = o.RoleId.ToString(), Text = o.RoleName })
                         .ToList();
                 
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
        public ActionResult EditUser(UserVM user)
        {

            user.UserRoleList = _db.tbl_Role.Where(x => x.RoleId > 1)
                         .Select(o => new SelectListItem { Value = o.RoleId.ToString(), Text = o.RoleName })
                         .ToList();

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
                }
                catch (Exception ex)
                {
                }
                return RedirectToAction("Users", new { id = user.ClientId });
            }

            return View(user);
        }

    }
}