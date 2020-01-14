using ConstructionDiary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.ResourceFiles;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class ExpenseController : MyBaseController
    {
        ConstructionDiaryEntities _db;
        public ExpenseController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<ExpenseVM> lstExpense = (from c in _db.tbl_Expenses
                                          join exp in _db.tbl_ExpenseType on c.ExpenseTypeId equals exp.ExpenseTypeId 
                                          join site in _db.tbl_Sites on c.SiteId equals site.SiteId into outerJoinSite
                                          from site in outerJoinSite.DefaultIfEmpty()
                                          where !c.IsDeleted && c.ClientId == ClientId
                                          select new ExpenseVM
                                          {
                                              ExpenseId = c.ExpenseId,
                                              dtExpenseDate = c.ExpenseDate,
                                              Amount = c.Amount,
                                              Description = c.Description,
                                              SiteId = c.SiteId,
                                              SiteName = site.SiteName,
                                              ExpenseTypeId = c.ExpenseTypeId,
                                              ExpenseType = exp.ExpenseType,
                                              IsActive = c.IsActive
                                          }).OrderByDescending(x=>x.dtExpenseDate).ToList();
            return View(lstExpense);
        }

        public ActionResult Add()
        {
            ExpenseVM objExpense = new ExpenseVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            objExpense.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            objExpense.ExpenseTypeList = _db.tbl_ExpenseType
                         .Select(o => new SelectListItem { Value = o.ExpenseTypeId.ToString(), Text = o.ExpenseType })
                         .ToList();

            return View(objExpense);
        }

        [HttpPost]
        public ActionResult Add(ExpenseVM expense)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            expense.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            expense.ExpenseTypeList = _db.tbl_ExpenseType
                         .Select(o => new SelectListItem { Value = o.ExpenseTypeId.ToString(), Text = o.ExpenseType })
                         .ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    if (expense.ExpenseTypeId == 1)
                    {
                        if (expense.SiteId == null)
                        {
                            ModelState.AddModelError("SiteId", Resource.Required);
                            return View(expense);
                        }
                    }

                    DateTime exp_date = DateTime.ParseExact(expense.ExpenseDate, "dd/MM/yyyy", null);

                    tbl_Expenses objExpense = new tbl_Expenses();
                    objExpense.ExpenseId = Guid.NewGuid();
                    objExpense.ExpenseDate = exp_date;
                    objExpense.Amount = Convert.ToDecimal(expense.Amount);
                    objExpense.ExpenseTypeId = expense.ExpenseTypeId;
                    objExpense.Description = expense.Description;

                    if (expense.ExpenseTypeId == 1)
                    {
                        objExpense.SiteId = expense.SiteId;
                    }
                    else
                    {
                        objExpense.SiteId = null;
                    }

                    objExpense.ClientId = ClientId;
                    objExpense.IsActive = true;
                    objExpense.IsDeleted = false;
                    objExpense.CreatedBy = clsSession.UserID;
                    objExpense.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Expenses.Add(objExpense);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                }
                return RedirectToAction("Index");
            }
              
            return View(expense);
        }

        public ActionResult Edit(Guid Id)
        {
            ExpenseVM objExpense = (from c in _db.tbl_Expenses
                                    where c.ExpenseId == Id
                                    select new ExpenseVM
                                    {
                                        ExpenseId = c.ExpenseId,
                                        dtExpenseDate = c.ExpenseDate,
                                        Description = c.Description,
                                        Amount = c.Amount,
                                        SiteId = c.SiteId,
                                        ExpenseTypeId = c.ExpenseTypeId,
                                        IsActive = c.IsActive
                                    }).FirstOrDefault();

            objExpense.ExpenseDate = Convert.ToDateTime(objExpense.dtExpenseDate).ToString("dd/MM/yyyy");

            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            objExpense.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            objExpense.ExpenseTypeList = _db.tbl_ExpenseType
                         .Select(o => new SelectListItem { Value = o.ExpenseTypeId.ToString(), Text = o.ExpenseType })
                         .ToList();

            return View(objExpense);
        }

        [HttpPost]
        public ActionResult Edit(ExpenseVM expense)
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            expense.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            expense.ExpenseTypeList = _db.tbl_ExpenseType
                         .Select(o => new SelectListItem { Value = o.ExpenseTypeId.ToString(), Text = o.ExpenseType })
                         .ToList();

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                try
                {
                    if (expense.ExpenseTypeId == 1)
                    {
                        if (expense.SiteId == null)
                        {
                            ModelState.AddModelError("SiteId", Resource.Required);
                            return View(expense);
                        }
                    }

                    tbl_Expenses objExpense = _db.tbl_Expenses.Where(x => x.ExpenseId == expense.ExpenseId).FirstOrDefault();

                    DateTime exp_date = DateTime.ParseExact(expense.ExpenseDate, "dd/MM/yyyy", null);

                    objExpense.ExpenseDate = exp_date;
                    objExpense.Amount = Convert.ToDecimal(expense.Amount);
                    objExpense.ExpenseTypeId = expense.ExpenseTypeId;
                    if (expense.ExpenseTypeId == 1)
                    {
                        objExpense.SiteId = expense.SiteId;
                    }
                    else
                    {
                        objExpense.SiteId = null;
                    }
                    objExpense.Description = expense.Description;
                    objExpense.ModifiedBy = clsSession.UserID;
                    objExpense.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                }
                return RedirectToAction("Index");
            }

            

            return View(expense);
        }

        [HttpPost]
        public string DeleteExpense(Guid ExpenseId)
        {
            string ReturnMessage = "";

            try
            {
                tbl_Expenses objExpense = _db.tbl_Expenses.Where(x => x.ExpenseId == ExpenseId && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objExpense == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objExpense.IsDeleted = true;
                    objExpense.ModifiedDate = DateTime.UtcNow;
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