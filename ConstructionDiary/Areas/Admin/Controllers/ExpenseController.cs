using ConstructionDiary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.ResourceFiles;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using System.Text;
using ConstructionDiary.Helper;
using System.IO;

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

        public ActionResult Index(string duration, string start, string end, string type, string site)
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            int RoleID = clsSession.RoleID;

            List<ExpenseVM> lstExpense = new List<ExpenseVM>();

            try
            {
                if (string.IsNullOrEmpty(duration))
                    duration = "month";


                //ViewBag.PersonName = GetPersonName(id);
                //ViewBag.PersonId = id;
                ViewBag.Duration = duration;
                ViewBag.StartDate = start;
                ViewBag.EndDate = end;

                ViewBag.ExpenseType = type;
                ViewBag.Site = site;

                DateTime startDate = DateTime.Today;
                DateTime endDate = DateTime.Today;

                if (RoleID != (int)UserRoles.Staff)
                {
                    if (duration == "month")
                    {
                        var myDate = DateTime.Now;
                        startDate = new DateTime(myDate.Year, myDate.Month, 1);

                        DateTime lastDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);
                        endDate = lastDay;
                    }
                    else if (duration == "custom")
                    {
                        if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
                        {
                            startDate = DateTime.ParseExact(start, "dd/MM/yyyy", null);
                            endDate = DateTime.ParseExact(end, "dd/MM/yyyy", null);
                        }
                    }
                }

                ViewBag.reportStartDate = startDate.ToString("dd/MM/yyyy");
                ViewBag.reportEndDate = endDate.ToString("dd/MM/yyyy");

                lstExpense = GetExpenseListByFilter("", startDate, endDate, type, site);

                List<SelectListItem> lstSites = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();
                ViewBag.SiteList = lstSites;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message.ToString();
            }

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
        public ActionResult Add(ExpenseVM expense, HttpPostedFileBase ExpenseFile)
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

                    // Save + Upload Expense File
                    string fileName = string.Empty;
                    string path = Server.MapPath("~/DataFiles/ExpenseFile/");
                    if (ExpenseFile != null)
                    {
                        fileName = Guid.NewGuid() + "-" + Path.GetFileName(ExpenseFile.FileName);

                        string full_path = Path.Combine(path, fileName);
                        ExpenseFile.SaveAs(full_path);

                        tbl_Files objFile = new tbl_Files();
                        objFile.FileId = Guid.NewGuid();
                        objFile.ParentId = objExpense.ExpenseId;
                        objFile.FileCategory = (int)FileType.Expense;
                        objFile.OriginalFileName = ExpenseFile.FileName;
                        objFile.EncryptFileName = fileName;
                        _db.tbl_Files.Add(objFile);
                        _db.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
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
        public ActionResult Edit(ExpenseVM expense, HttpPostedFileBase ExpenseFile)
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

                    // Save + Upload Expense File
                    string fileName = string.Empty;
                    string path = Server.MapPath("~/DataFiles/ExpenseFile/");
                    if (ExpenseFile != null)
                    {
                        fileName = Guid.NewGuid() + "-" + Path.GetFileName(ExpenseFile.FileName);

                        string full_path = Path.Combine(path, fileName);
                        ExpenseFile.SaveAs(full_path);

                        tbl_Files objFile = _db.tbl_Files.Where(x => x.ParentId == objExpense.ExpenseId).FirstOrDefault();

                        if (objFile == null)
                        {
                            objFile = new tbl_Files();
                            objFile.FileId = Guid.NewGuid();
                            _db.tbl_Files.Add(objFile);
                        }

                        objFile.ParentId = objExpense.ExpenseId;
                        objFile.FileCategory = (int)FileType.Expense;
                        objFile.OriginalFileName = ExpenseFile.FileName;
                        objFile.EncryptFileName = fileName;

                        _db.SaveChanges();
                    }

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

        public string ExportPDFOfExpense(string start, string end, string type, string site)
        {

            string Result = "";
            try
            {
                DateTime start_date = DateTime.ParseExact(start, "dd/MM/yyyy", null);
                DateTime end_date = DateTime.ParseExact(end, "dd/MM/yyyy", null);

                List<ExpenseVM> lstExpense = GetExpenseListByFilter("", start_date, end_date, type, site);

                decimal? TotalExpenseAmount = lstExpense.Select(x => x.Amount).Sum();
                string strTotalExpenseAmount = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalExpenseAmount));

                string[] strColumns = new string[5] { "Date", "Amount", "Expense Type", "Site Name", "Description" };
                if (lstExpense != null && lstExpense.Count() > 0)
                {

                    List<DateTime> lstDateTemp = new List<DateTime>();
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<!DOCTYPE html>");
                    strHTML.Append("<style>");
                    strHTML.Append("@page {@bottom-center {content: \"Page \" counter(page) \" of \" counter(pages);}}");
                    strHTML.Append("</style>");

                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<thead style=\"display:table-header-group;\">");
                    string Title = "Expense List";
                    strHTML.Append("<tr>");
                    strHTML.Append("<th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #ccc\">");
                    strHTML.Append(Title);
                    strHTML.Append("</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("<tr><th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #ccc\">From " + start_date.ToString("dd/MM/yyyy") + " To " + end_date.ToString("dd/MM/yyyy") + " </th></tr>");
                    strHTML.Append("<tr>");
                    for (int idx = 0; idx < strColumns.Length; idx++)
                    {
                        strHTML.Append("<th style=\"border: 1px solid #ccc\">");
                        strHTML.Append(strColumns[idx]);
                        strHTML.Append("</th>");
                    }
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody>");
                    foreach (var obj in lstExpense)
                    {

                        if (obj != null)
                        {

                            strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                            for (int Col = 0; Col < strColumns.Length; Col++)
                            {
                                string strcolval = "";
                                switch (strColumns[Col])
                                {

                                    case "Date":
                                        {
                                            strcolval = Convert.ToDateTime(obj.dtExpenseDate).ToString("dd/MM/yyyy");
                                            break;
                                        }
                                    case "Amount":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.Amount);
                                            break;
                                        }
                                    case "Expense Type":
                                        {
                                            strcolval = obj.ExpenseType;
                                            break;
                                        }
                                    case "Site Name":
                                        {
                                            strcolval = obj.SiteName;
                                            break;
                                        }
                                    case "Description":
                                        {
                                            strcolval = obj.Description;
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }

                                }
                                strHTML.Append("<td style=\"width: auto; border: 1px solid #ccc\">");
                                strHTML.Append(strcolval);
                                strHTML.Append("</td>");
                            }
                            strHTML.Append("</tr>");
                        }
                    }

                    // Total
                    strHTML.Append("<tr>");
                    strHTML.Append("<th style='text-align:right; border: 1px solid #ccc;'>Total</th>");
                    strHTML.Append("<th style='border: 1px solid #ccc;'> " + strTotalExpenseAmount + " </th>");
                    strHTML.Append("<th colspan='3' style='border: 1px solid #ccc;'></th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("</tbody>");
                    strHTML.Append("</table>");

                    StringReader sr = new StringReader(strHTML.ToString());

                    var myString = strHTML.ToString();
                    var myByteArray = System.Text.Encoding.UTF8.GetBytes(myString);
                    var ms = new MemoryStream(myByteArray);

                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 20f, 20f, 20f, 20f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    writer.PageEvent = new PDFGeneratePageEventHelper();
                    pdfDoc.Open();

                    XMLWorkerHelper objHelp = XMLWorkerHelper.GetInstance();
                    objHelp.ParseXHtml(writer, pdfDoc, ms, null, Encoding.UTF8, new UnicodeFontFactory());

                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "download;filename=Expense List" + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }

                return Result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
            }

        }

        public List<ExpenseVM> GetExpenseListByFilter(string expensetype, DateTime startDate, DateTime endDate, string type, string site)
        {
            List<ExpenseVM> lstExpenses = new List<ExpenseVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            int RoleID = clsSession.RoleID;

            int selectedExpenseType = (type != null && !string.IsNullOrEmpty(type) ? Int32.Parse(type) : 0);
            Guid selectedSite = (site != null && !string.IsNullOrEmpty(site) ? new Guid(site) : Guid.Empty);

            lstExpenses = (from c in _db.tbl_Expenses
                           join exp in _db.tbl_ExpenseType on c.ExpenseTypeId equals exp.ExpenseTypeId
                           join st in _db.tbl_Sites on c.SiteId equals st.SiteId into outerJoinSite
                           from st in outerJoinSite.DefaultIfEmpty()
                           where !c.IsDeleted && c.ClientId == ClientId
                                 && (RoleID != (int)UserRoles.Staff || c.CreatedBy == clsSession.UserID)
                                 && (selectedExpenseType == 0 || c.ExpenseTypeId == selectedExpenseType)
                                 && (selectedSite == Guid.Empty || c.SiteId == selectedSite)
                                 && c.ExpenseDate >= startDate && c.ExpenseDate <= endDate
                           select new ExpenseVM
                           {
                               ExpenseId = c.ExpenseId,
                               dtExpenseDate = c.ExpenseDate,
                               Amount = c.Amount,
                               Description = c.Description,
                               SiteId = c.SiteId,
                               SiteName = st.SiteName,
                               ExpenseTypeId = c.ExpenseTypeId,
                               ExpenseType = exp.ExpenseType,
                               IsActive = c.IsActive,
                               ObjExpenseFile = _db.tbl_Files.Where(x => x.ParentId == c.ExpenseId && x.FileCategory == (int)FileType.Expense).FirstOrDefault()
                           }).OrderByDescending(x => x.dtExpenseDate).ToList();

            return lstExpenses;
        }

    }

}