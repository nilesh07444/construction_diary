using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ConstructionDiary.Models;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class MyAccountController : Controller
    {
        ConstructionDiaryEntities _db;
        public MyAccountController()
        {
            _db = new ConstructionDiaryEntities();
        }

        // GET: MyAccount
        public ActionResult Index()
        {
            MyFinanceVM objFinance = new MyFinanceVM();
            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                List<MyFinanceList> financeList = (from finance in _db.tbl_ContractorFinance
                                                   join user in _db.tbl_Users on finance.UserId equals user.UserId
                                                   join site in _db.tbl_Sites on finance.SiteId equals site.SiteId
                                                   where site.ClientId == ClientId
                                                   select new MyFinanceList
                                                   {
                                                       ContractorFinanceId = finance.ContractorFinanceId,
                                                       SiteId = finance.SiteId,
                                                       SelectedDate = finance.SelectedDate,
                                                       Amount = finance.Amount,
                                                       CreditOrDebit = finance.CreditOrDebit,
                                                       SiteName = site.SiteName,
                                                       UserId = finance.UserId,
                                                       
                                                       PaymentType = finance.PaymentType,
                                                       ChequeNo = finance.ChequeNo,
                                                       BankName = finance.BankName,
                                                       ChequeFor = finance.ChequeFor,
                                                       Remarks = finance.Remarks,
                                                       IsActive = finance.IsActive,
                                                       IsDeleted = finance.IsDeleted,
                                                       CreatedBy = finance.CreatedBy,
                                                       UpdatedBy = finance.UpdatedBy,
                                                       CreatedDate = finance.CreatedDate,
                                                       ModifiedDate = finance.ModifiedDate,
                                                       FirstName = user.FirstName
                                                   }).Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.SelectedDate).ToList();
                ViewData["FinanceList"] = financeList;

                objFinance.UsersList = _db.tbl_Users.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                         .ToList();

                objFinance.SitesList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName })
                         .ToList();

            }
            catch (Exception ex)
            {
            }
            return View(objFinance);
        }

        [HttpPost]
        public string DeleteContractorFinance(string FinanceId)
        {
            string ReturnMessage = "";

            try
            {
                Guid FinanceIdToDelete = new Guid(FinanceId);
                tbl_ContractorFinance objFinance = _db.tbl_ContractorFinance.Where(x => x.ContractorFinanceId == FinanceIdToDelete && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objFinance == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objFinance.IsDeleted = true;
                    objFinance.UpdatedBy = new Guid(clsSession.UserID.ToString());
                    objFinance.ModifiedDate = DateTime.UtcNow;
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

        [Route("account")]
        public ActionResult Account()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            var lstFinance = (from p in _db.SP_GetTodayPartyFinance(ClientId)
                              select p).ToList();

            return View(lstFinance);
        }

        public ActionResult Add(Guid? id) // id == SiteId
        {
            MyFinanceVM objFinance = new MyFinanceVM();
            try
            {
                TempData["siteId"] = id;
                if (id != null)
                {
                    objFinance.SiteId = id;
                }

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                objFinance.UsersList = _db.tbl_Users.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                         .ToList();

                objFinance.SitesList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName })
                         .ToList();

            }
            catch (Exception ex)
            {
            }
            return View(objFinance);
        }

        [HttpPost]
        public ActionResult Add(MyFinanceVM objFinance)
        {
            try
            {
                string qSiteId = Convert.ToString(TempData["siteId"]);

                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                // Get Users List
                objFinance.UsersList = _db.tbl_Users.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                         .ToList();

                // Get Sites List
                objFinance.SitesList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName })
                         .ToList();

                if (ModelState.IsValid)
                {
                    DateTime date = DateTime.ParseExact(objFinance.SelectedDate, "dd/MM/yyyy", null);

                    tbl_ContractorFinance finance = new tbl_ContractorFinance();
                    finance.ContractorFinanceId = Guid.NewGuid();

                    finance.SiteId = objFinance.SiteId;
                    finance.UserId = objFinance.UserId;
                    finance.SelectedDate = date;
                    finance.Amount = objFinance.Amount;
                    finance.CreditOrDebit = objFinance.CreditOrDebit;
                    finance.PaymentType = objFinance.PaymentType;

                    if (objFinance.PaymentType == "Cheque")
                    {
                        finance.ChequeNo = objFinance.ChequeNo;
                        finance.BankName = objFinance.BankName;
                        finance.ChequeFor = objFinance.ChequeFor;
                    }
                    else
                    {
                        finance.ChequeNo = "";
                        finance.BankName = "";
                        finance.ChequeFor = "";
                    }

                    finance.Remarks = objFinance.Remarks;
                    finance.IsActive = true;
                    finance.IsDeleted = false;
                    finance.CreatedBy = clsSession.UserID;
                    finance.CreatedDate = DateTime.UtcNow;
                    _db.tbl_ContractorFinance.Add(finance);
                    _db.SaveChanges();

                    if (!string.IsNullOrEmpty(qSiteId))
                    {
                        return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Site", action = "Detail", Id = qSiteId }));
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                     
                }
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        public ActionResult Edit(Guid Id, Guid? site)
        {
            MyFinanceVM objFinance = new MyFinanceVM();
            try
            {
                TempData["siteId"] = site;

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                tbl_ContractorFinance objContractorFinance = _db.tbl_ContractorFinance.Where(x => x.ContractorFinanceId == Id).FirstOrDefault();

                if (objContractorFinance != null)
                {
                    objFinance.ContractorFinanceId = objContractorFinance.ContractorFinanceId;
                    objFinance.SiteId = objContractorFinance.SiteId;
                    objFinance.UserId = objContractorFinance.UserId;
                    objFinance.SelectedDate = Convert.ToDateTime(objContractorFinance.SelectedDate).ToString("dd/MM/yyyy");
                    objFinance.Amount = objContractorFinance.Amount;
                    objFinance.CreditOrDebit = objContractorFinance.CreditOrDebit;
                    objFinance.PaymentType = objContractorFinance.PaymentType;
                    objFinance.ChequeNo = objContractorFinance.ChequeNo;
                    objFinance.BankName = objContractorFinance.BankName;
                    objFinance.ChequeFor = objContractorFinance.ChequeFor;
                    objFinance.Remarks = objContractorFinance.Remarks;
                }

                objFinance.UsersList = _db.tbl_Users.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                         .ToList();

                objFinance.SitesList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName })
                         .ToList();

            }
            catch (Exception ex)
            {
            }
            return View(objFinance);
        }

        [HttpPost]
        public ActionResult Edit(MyFinanceVM objFinance)
        {
            try
            {
                string qSiteId = Convert.ToString(TempData["siteId"]);

                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                // Get Users List
                objFinance.UsersList = _db.tbl_Users.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                         .ToList();

                // Get Sites List
                objFinance.SitesList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName })
                         .ToList();

                if (ModelState.IsValid)
                {
                    DateTime date = DateTime.ParseExact(objFinance.SelectedDate, "dd/MM/yyyy", null);

                    tbl_ContractorFinance finance = _db.tbl_ContractorFinance.Where(x => x.ContractorFinanceId == objFinance.ContractorFinanceId).FirstOrDefault();

                    if (finance != null)
                    {
                        finance.SiteId = objFinance.SiteId;
                        finance.UserId = objFinance.UserId;
                        finance.SelectedDate = date;
                        finance.Amount = objFinance.Amount;
                        finance.CreditOrDebit = objFinance.CreditOrDebit;
                        finance.PaymentType = objFinance.PaymentType;

                        if (objFinance.PaymentType == "Cheque")
                        {
                            finance.ChequeNo = objFinance.ChequeNo;
                            finance.BankName = objFinance.BankName;
                            finance.ChequeFor = objFinance.ChequeFor;
                        }
                        else
                        {
                            finance.ChequeNo = "";
                            finance.BankName = "";
                            finance.ChequeFor = "";
                        }

                        finance.Remarks = objFinance.Remarks;
                        finance.UpdatedBy = clsSession.UserID;
                        finance.ModifiedDate = DateTime.UtcNow;
                        _db.SaveChanges();
                    }

                    if (!string.IsNullOrEmpty(qSiteId))
                    {
                        return RedirectToAction("Detail", new RouteValueDictionary(new { controller = "Site", action = "Detail", Id = qSiteId }));
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        public ActionResult Report()
        {
            MyFinanceVM objFinance = new MyFinanceVM();
            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                List<MyFinanceList> financeList = (from finance in _db.tbl_ContractorFinance
                                                   join user in _db.tbl_Users on finance.UserId equals user.UserId
                                                   join site in _db.tbl_Sites on finance.SiteId equals site.SiteId
                                                   where site.ClientId == ClientId
                                                   select new MyFinanceList
                                                   {
                                                       ContractorFinanceId = finance.ContractorFinanceId,
                                                       SiteId = finance.SiteId,
                                                       SelectedDate = finance.SelectedDate,
                                                       Amount = finance.Amount,
                                                       CreditOrDebit = finance.CreditOrDebit,
                                                       SiteName = site.SiteName,
                                                       UserId = finance.UserId,
                                                       PaymentType = finance.PaymentType,
                                                       ChequeNo = finance.ChequeNo,
                                                       BankName = finance.BankName,
                                                       ChequeFor = finance.ChequeFor,
                                                       Remarks = finance.Remarks,
                                                       IsActive = finance.IsActive,
                                                       IsDeleted = finance.IsDeleted,
                                                       CreatedBy = finance.CreatedBy,
                                                       UpdatedBy = finance.UpdatedBy,
                                                       CreatedDate = finance.CreatedDate,
                                                       ModifiedDate = finance.ModifiedDate,
                                                       FirstName = user.FirstName
                                                   }).Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.SelectedDate).ToList();
                ViewData["FinanceList"] = financeList;

                objFinance.UsersList = _db.tbl_Users.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                         .ToList();

                objFinance.SitesList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName })
                         .ToList();

            }
            catch (Exception ex)
            {
            }
            return View(objFinance);
        }

    }
}