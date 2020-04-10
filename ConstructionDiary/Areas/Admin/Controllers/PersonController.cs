using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Helper;
using ConstructionDiary.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using ConstructionDiary.ResourceFiles;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class PersonController : MyBaseController
    {
        ConstructionDiaryEntities _db;
        public PersonController()
        {
            _db = new ConstructionDiaryEntities();
        }
        public ActionResult Index()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<PersonDetailVM> personDetail = (from person in _db.tbl_Persons
                                                 where person.ClientId == ClientId && person.IsDeleted == false && person.IsAttendancePerson == false
                                                 select new PersonDetailVM
                                                 {
                                                     PersonId = person.PersonId,
                                                     PersonName = person.PersonFirstName,
                                                     TotalBillAmount = _db.tbl_BillDebit.Where(x => x.PersonId == person.PersonId).ToList().Select(x => x.TotalAmount).Sum(),
                                                     TotalDebitAmount = _db.tbl_Finance.Where(x => x.PersonId == person.PersonId && x.CreditOrDebit == "Debit" && !x.IsDeleted).ToList().Select(x => x.Amount).Sum()
                                                 }).ToList();

            //var lstPersons = (from p in _db.SP_GetPersonsList(ClientId)
            //                  select p).ToList();

            return View(personDetail);
        }

        public ActionResult Add()
        {
            PersonVM person = new PersonVM();
            person.PersonTypeList = _db.tbl_PersonType
                         .Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.PersonType })
                         .ToList();

            return View(person);
        }

        [HttpPost]
        public ActionResult Add(PersonVM person, HttpPostedFileBase postedFile)
        {
            person.PersonTypeList = _db.tbl_PersonType
                         .Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.PersonType })
                         .ToList();

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                /*
                string fileName = string.Empty;
                string path = Server.MapPath("~/Images/PersonPhoto/");
                if (postedFile != null)
                {
                    fileName = Guid.NewGuid() + Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(path + fileName);
                }
                */

                try
                {
                    tbl_Persons objPerson = new tbl_Persons();
                    objPerson.PersonId = Guid.NewGuid();
                    objPerson.PersonFirstName = person.Firstname;
                    objPerson.PersonAddress = person.Address;
                    //objPerson.MobileNo = person.MobileNo;
                    //objPerson.DailyRate = person.DailyRate;
                    //objPerson.PersonTypeId = person.PersonTypeId;
                    //objPerson.PersonPhoto = fileName;
                    objPerson.IsAttendancePerson = false;
                    objPerson.IsActive = true;
                    objPerson.IsDeleted = false;
                    objPerson.ClientId = clsSession.ClientID;
                    objPerson.CreatedBy = new Guid(clsSession.UserID.ToString());
                    objPerson.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Persons.Add(objPerson);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {

                }
                return RedirectToAction("Index");
            }

            return View(person);
        }

        public ActionResult Edit(Guid id)
        {
            PersonVM person = new PersonVM();

            person.PersonTypeList = _db.tbl_PersonType
                         .Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.PersonType })
                         .ToList();

            tbl_Persons obj = _db.tbl_Persons.Where(x => x.PersonId == id).FirstOrDefault();
            if (obj != null)
            {
                person.PersonId = obj.PersonId;
                person.Firstname = obj.PersonFirstName;
                person.Address = obj.PersonAddress;
                person.MobileNo = obj.MobileNo;
                //person.DailyRate = obj.DailyRate;
                //person.PersonTypeId = obj.PersonTypeId;
                person.strPersonPhoto = obj.PersonPhoto;
            }

            return View(person);
        }

        [HttpPost]
        public ActionResult Edit(PersonVM person, HttpPostedFileBase postedFile)
        {

            person.PersonTypeList = _db.tbl_PersonType
                         .Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.PersonType })
                         .ToList();

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                try
                {
                    tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == person.PersonId).FirstOrDefault();

                    if (objPerson != null)
                    {

                        string fileName = string.Empty;
                        string path = Server.MapPath("~/Images/PersonPhoto/");
                        if (postedFile != null)
                        {
                            fileName = Guid.NewGuid() + Path.GetFileName(postedFile.FileName);
                            postedFile.SaveAs(path + fileName);
                        }
                        else
                        {
                            fileName = person.strPersonPhoto;
                        }

                        objPerson.PersonFirstName = person.Firstname;
                        objPerson.PersonAddress = person.Address;
                        objPerson.PersonPhoto = fileName;
                        //objPerson.DailyRate = person.DailyRate;
                        //objPerson.PersonTypeId = person.PersonTypeId;
                        objPerson.MobileNo = person.MobileNo;
                        objPerson.ClientId = clsSession.ClientID;
                        objPerson.UpdatedBy = new Guid(clsSession.UserID.ToString());
                        objPerson.ModifiedDate = DateTime.UtcNow;
                        _db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                }
                return RedirectToAction("Index");
            }

            return View(person);
        }

        [HttpPost]
        public string DeletePerson(string PersonId)
        {
            string ReturnMessage = "";

            try
            {
                Guid PersonIdToDelete = new Guid(PersonId);

                tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == PersonIdToDelete && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objPerson == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objPerson.IsDeleted = true;
                    objPerson.UpdatedBy = new Guid(clsSession.UserID.ToString());
                    objPerson.ModifiedDate = DateTime.UtcNow;
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

        public ActionResult View(Guid id)
        {
            PersonVM person = new PersonVM();
            try
            {
                tbl_Persons obj = _db.tbl_Persons.Where(x => x.PersonId == id).FirstOrDefault();
                if (obj != null)
                {
                    person.PersonId = obj.PersonId;
                    person.Firstname = obj.PersonFirstName;
                    person.MobileNo = obj.MobileNo;
                    person.Address = obj.PersonAddress;
                    person.strPersonPhoto = obj.PersonPhoto;
                    person.CreatedBy = GetUserName(obj.CreatedBy);
                    person.UpdatedBy = GetUserName(obj.UpdatedBy);
                }
            }
            catch (Exception ex)
            {
            }
            return View(person);
        }

        public ActionResult Finance(Guid id)
        {
            PersonFinanceVM objFinance = new PersonFinanceVM();
            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                List<FinanceList> financeList = (from finance in _db.tbl_Finance
                                                 join user in _db.tbl_Users on finance.GivenAmountBy equals user.UserId
                                                 join site in _db.tbl_Sites on finance.SiteId equals site.SiteId into outerJoinSite
                                                 from site in outerJoinSite.DefaultIfEmpty()
                                                 where finance.PersonId == id
                                                 select new FinanceList
                                                 {
                                                     FinanceId = finance.FinanceId,
                                                     PersonId = finance.PersonId,
                                                     SelectedDate = finance.SelectedDate,
                                                     Amount = finance.Amount,
                                                     SiteId = finance.SiteId,
                                                     SiteName = (site != null) ? site.SiteName : "",
                                                     CreditOrDebit = finance.CreditOrDebit,
                                                     GivenAmountBy = finance.GivenAmountBy,
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
                                                     FirstName = user.FirstName,
                                                 }).Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.SelectedDate).ToList();
                ViewData["FinanceList"] = financeList;

                objFinance.PersonId = id;
                objFinance.UsersList = _db.tbl_Users.Where(x => x.ClientId == ClientId && x.IsActive == true && x.IsDeleted == false)
                         .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                         .OrderBy(x => x.Text).ToList();

                objFinance.SitesList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

                ViewBag.PersonName = GetPersonName(id);

            }
            catch (Exception ex)
            {
            }
            return View(objFinance);
        }

        [HttpPost]
        public ActionResult Finance(PersonFinanceVM finance)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                finance.UsersList = _db.tbl_Users.Where(x => x.ClientId == ClientId)
                     .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                     .OrderBy(x => x.Text).ToList();

                if (ModelState.IsValid)
                {
                    DateTime date = DateTime.ParseExact(finance.SelectedDate, "dd/MM/yyyy", null);

                    tbl_Finance objFinance = new tbl_Finance();
                    objFinance.FinanceId = Guid.NewGuid();
                    objFinance.PersonId = finance.PersonId;
                    objFinance.SelectedDate = date;
                    objFinance.GivenAmountBy = finance.GivenAmountBy;
                    objFinance.Amount = Convert.ToDecimal(finance.Amount);
                    objFinance.SiteId = finance.SiteId;
                    objFinance.CreditOrDebit = "Debit"; // finance.CreditOrDebit;
                    objFinance.PaymentType = finance.PaymentType;
                    objFinance.ChequeNo = finance.ChequeNo;
                    objFinance.BankName = finance.BankName;
                    objFinance.ChequeFor = finance.ChequeFor;
                    objFinance.Remarks = finance.Remarks;
                    objFinance.IsActive = true;
                    objFinance.IsDeleted = false;
                    objFinance.CreatedBy = clsSession.UserID;
                    objFinance.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Finance.Add(objFinance);
                    _db.SaveChanges();

                    return RedirectToAction("Finance", new { id = finance.PersonId });

                }
            }
            catch (Exception ex)
            {

            }

            return View(finance);
        }

        [HttpPost]
        public string DeleteFinance(string FinanceId)
        {
            string ReturnMessage = "";

            try
            {
                Guid FinanceIdToDelete = new Guid(FinanceId);
                tbl_Finance objFinance = _db.tbl_Finance.Where(x => x.FinanceId == FinanceIdToDelete && x.IsActive == true
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

        public ActionResult Attandance(Guid id, string duration, string start, string end)
        {
            List<ReportPersonAttendanceVM> lstPersonAttendanceVM = new List<ReportPersonAttendanceVM>();
            try
            {
                if (string.IsNullOrEmpty(duration))
                    duration = "month";

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == id).FirstOrDefault();

                ViewBag.PersonName = objPerson.PersonFirstName;
                ViewBag.PersonId = id;
                ViewBag.PersonTypeId = objPerson.PersonTypeId;
                ViewBag.Duration = duration;
                ViewBag.StartDate = start;
                ViewBag.EndDate = end;

                DateTime startDate = DateTime.Today;
                DateTime endDate = DateTime.Today;

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

                lstPersonAttendanceVM = getAttendanceByFilter(id, startDate, endDate);

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message.ToString();
            }
            return View(lstPersonAttendanceVM);
        }

        public List<ReportPersonAttendanceVM> getAttendanceByFilter(Guid PersonId, DateTime startDate, DateTime endDate)
        {

            List<ReportPersonAttendanceVM> lst = (
                    from attper in _db.tbl_PersonAttendance
                    join att in _db.tbl_Attendance on attper.AttendanceId equals att.AttendaceId into outerJoinAttendance
                    from att in outerJoinAttendance.DefaultIfEmpty()
                    join site in _db.tbl_Sites on attper.SiteId equals site.SiteId into outerJoinSite
                    from site in outerJoinSite.DefaultIfEmpty()
                    where attper.PersonId == PersonId && att.AttendanceDate >= startDate && att.AttendanceDate <= endDate
                    select new ReportPersonAttendanceVM
                    {
                        PersonAttendanceId = attper.PersonAttendanceId,
                        AttendanceDate = att.AttendanceDate,
                        PersonId = attper.PersonId,
                        PersonTypeId = attper.PersonTypeId,
                        TotalRokadiya = attper.TotalRokadiya,
                        AttendanceStatus = attper.AttendanceStatus,
                        OvertimeAmount = attper.OvertimeAmount,
                        PersonDailyRate = attper.PersonDailyRate,
                        WithdrawAmount = attper.WithdrawAmount,
                        SiteId = attper.SiteId,
                        SiteName = site.SiteName,
                        PayableAmount = attper.PayableAmount,
                        Remarks = attper.Remarks
                    }).OrderBy(x => x.AttendanceDate).ToList();

            return lst;
        }

        public string GetUserName(Guid? Id)
        {
            string userName = "";

            if (Id != null)
            {
                tbl_Users objUser = _db.tbl_Users.Where(x => x.UserId == Id).FirstOrDefault();
                if (objUser != null)
                    userName = objUser.FirstName;
            }
            return userName;
        }

        public string GetPersonName(Guid Id)
        {
            string personName = "";

            tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == Id).FirstOrDefault();
            if (objPerson != null)
                personName = objPerson.PersonFirstName;

            return personName;
        }

        public string ExportPDFOfPersonAttendance(Guid id, string duration, string start, string end)
        {

            string Result = "";

            try
            {
                List<ReportPersonAttendanceVM> lstPersonAttendanceVM = new List<ReportPersonAttendanceVM>();
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                if (string.IsNullOrEmpty(duration))
                    duration = "month";

                DateTime startDate = DateTime.Today;
                DateTime endDate = DateTime.Today;

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

                lstPersonAttendanceVM = getAttendanceByFilter(id, startDate, endDate);

                string PersonName = GetPersonName(id);

                decimal? TotalPayableAmount = lstPersonAttendanceVM.Select(x => x.PayableAmount).Sum();
                string strTotalPayableAmount = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalPayableAmount));

                decimal? TotalWithdrawAmount = lstPersonAttendanceVM.Select(x => x.WithdrawAmount).Sum();
                string strTotalWithdrawAmount = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalWithdrawAmount));

                decimal? TotalOvertimeAmount = lstPersonAttendanceVM.Select(x => x.OvertimeAmount).Sum();
                string strTotalOvertimeAmount = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalOvertimeAmount));

                decimal? RemainingAmt = (TotalPayableAmount + TotalOvertimeAmount) - TotalWithdrawAmount;
                string strRemainingAmt = CoreHelper.GetFormatterAmount(Convert.ToDecimal(RemainingAmt));

                string[] strColumns = new string[7] { "Date", "Status", "Amount", "Overtime", "Withdraw", "Site Name", "Remarks" };
                if (lstPersonAttendanceVM != null && lstPersonAttendanceVM.Count() > 0)
                {

                    List<DateTime> lstDateTemp = new List<DateTime>();
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<!DOCTYPE html>");
                    strHTML.Append("<style>");
                    strHTML.Append("@page {@bottom-center {content: \"Page \" counter(page) \" of \" counter(pages);}}");
                    strHTML.Append("</style>");


                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<thead style=\"display:table-header-group;\">");
                    string Title = "Attendance List Of " + PersonName;
                    strHTML.Append("<tr>");
                    strHTML.Append("<th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #ccc\">");
                    strHTML.Append(Title);
                    strHTML.Append("</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("<tr><th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #ccc\">From " + startDate.ToString("dd/MM/yyyy") + " To " + endDate.ToString("dd/MM/yyyy") + " </th></tr>");
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
                    foreach (var obj in lstPersonAttendanceVM)
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
                                            strcolval = Convert.ToDateTime(obj.AttendanceDate).ToString("dd/MM/yyyy");
                                            break;
                                        }
                                    case "Status":
                                        {
                                            strcolval = obj.AttendanceStatus.ToString();
                                            break;
                                        }
                                    case "Amount":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.PayableAmount);
                                            break;
                                        }
                                    case "Overtime":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.OvertimeAmount);
                                            break;
                                        }
                                    case "Withdraw":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.WithdrawAmount);
                                            break;
                                        }
                                    case "Site Name":
                                        {
                                            strcolval = obj.SiteName;
                                            break;
                                        }
                                    case "Remarks":
                                        {
                                            strcolval = obj.Remarks;
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
                    strHTML.Append("<th colspan='2' style='text-align:right; border: 1px solid #ccc;'>Total</th>");
                    strHTML.Append("<th style='border: 1px solid #ccc;'> " + strTotalPayableAmount + " </th>");
                    strHTML.Append("<th style='border: 1px solid #ccc;'> " + strTotalOvertimeAmount + " </th>");
                    strHTML.Append("<th style='border: 1px solid #ccc;'> " + strTotalWithdrawAmount + " </th>");
                    strHTML.Append("<th colspan='3' style='border: 1px solid #ccc;'></th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("</tbody>");
                    strHTML.Append("</table>");

                    // Calculation table

                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='margin-top:20px; width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<tbody>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #ccc\">Total Days</td>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #ccc\">= " + lstPersonAttendanceVM.Sum(x => x.AttendanceStatus).ToString("0.##") + " Days</td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #ccc\">Remaining Amount</td>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #ccc\">= (Amount + Overtime) - Withdraw </td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #ccc\"></td>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #ccc\">= (" + strTotalPayableAmount + " + " + strTotalOvertimeAmount + ") - " + strTotalWithdrawAmount + "  </td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #ccc\"></td>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #ccc\">= " + strRemainingAmt + "</td>");
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
                    Response.AddHeader("content-disposition", "download;filename=Attendance List Of " + PersonName + ".pdf");
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

        public ActionResult Bill(Guid Id)
        {
            List<BillDebitVM> lstBill = new List<BillDebitVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            ViewBag.PersonId = Id;
            ViewBag.PersonName = GetPersonName(Id);

            lstBill = (from dbill in _db.tbl_BillDebit
                       join site in _db.tbl_Sites on dbill.SiteId equals site.SiteId
                       where dbill.ClientId == ClientId && dbill.PersonId == Id && !dbill.IsDeleted
                       select new BillDebitVM
                       {
                           BillId = dbill.BillId,
                           dBillDate = dbill.BillDate,
                           BillNo = dbill.BillNo,
                           BillType = dbill.BillType,
                           SiteId = dbill.SiteId,
                           SiteName = site.SiteName,
                           PersonId = dbill.PersonId,
                           SquareFeet = dbill.SquareFeet,
                           Rate = dbill.Rate,
                           TotalAmount = dbill.TotalAmount,
                           Remarks = dbill.Remarks
                       }).OrderByDescending(x => x.dBillDate).ToList();

            return View(lstBill);
        }

        public ActionResult AddBill(Guid Id)
        {
            BillDebitVM objBill = new BillDebitVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            objBill.PersonId = Id;

            objBill.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            return View(objBill);
        }

        [HttpPost]
        public ActionResult AddBill(BillDebitVM billVM)
        {

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                billVM.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    // Check validation
                    if (billVM.BillType == "Other")
                    {
                        if (billVM.TotalAmount == null || billVM.TotalAmount <= 0)
                        {
                            ModelState.AddModelError("TotalAmount", Resource.Required);
                            return View(billVM);
                        }
                    }
                    else
                    {
                        bool IsFieldNull = false;
                        if (billVM.SquareFeet == null || billVM.SquareFeet <= 0)
                        {
                            ModelState.AddModelError("SquareFeet", Resource.Required);
                            IsFieldNull = true;
                        }
                        if (billVM.Rate == null || billVM.Rate <= 0)
                        {
                            ModelState.AddModelError("Rate", Resource.Required);
                            IsFieldNull = true;
                        }

                        if (IsFieldNull)
                        {
                            return View(billVM);
                        }
                        else
                        {
                            billVM.TotalAmount = billVM.SquareFeet * billVM.Rate;
                        }
                    }

                    DateTime bill_date = DateTime.ParseExact(billVM.BillDate, "dd/MM/yyyy", null);

                    tbl_BillDebit objBill = new tbl_BillDebit();
                    objBill.BillId = Guid.NewGuid();
                    objBill.BillDate = bill_date;
                    objBill.BillNo = billVM.BillNo;
                    objBill.BillType = billVM.BillType;
                    objBill.SiteId = billVM.SiteId;
                    objBill.PersonId = billVM.PersonId;
                    objBill.SquareFeet = billVM.SquareFeet;
                    objBill.Rate = billVM.Rate;
                    objBill.TotalAmount = Convert.ToDecimal(billVM.TotalAmount);
                    objBill.Remarks = billVM.Remarks;
                    objBill.ClientId = ClientId;
                    objBill.IsActive = true;
                    objBill.IsDeleted = false;
                    objBill.CreatedBy = clsSession.UserID;
                    objBill.CreatedDate = DateTime.UtcNow;
                    _db.tbl_BillDebit.Add(objBill);
                    _db.SaveChanges();

                    return RedirectToAction("Bill", new { id = billVM.PersonId });

                }
            }
            catch (Exception ex)
            {
            }

            return View(billVM);
        }

        [HttpPost]
        public string DeleteBill(string BillId)
        {
            string ReturnMessage = "";

            try
            {
                Guid BillIdToDelete = new Guid(BillId);
                tbl_BillDebit objBill = _db.tbl_BillDebit.Where(x => x.BillId == BillIdToDelete && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objBill == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    // Soft delete
                    //objBill.IsDeleted = true;
                    //objBill.ModifiedBy = new Guid(clsSession.UserID.ToString());
                    //objBill.ModifiedDate = DateTime.UtcNow;

                    // hard delete
                    _db.tbl_BillDebit.Remove(objBill);
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

        public ActionResult EditBill(Guid Id) // id= BillId
        {
            BillDebitVM objBill = new BillDebitVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            objBill.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            tbl_BillDebit bill = _db.tbl_BillDebit.Where(x => x.BillId == Id).FirstOrDefault();
            if (objBill != null)
            {
                objBill.BillId = bill.BillId;
                objBill.BillDate = Convert.ToDateTime(bill.BillDate).ToString("dd/MM/yyyy");
                objBill.BillNo = bill.BillNo;
                objBill.BillType = bill.BillType;
                objBill.SiteId = bill.SiteId;
                objBill.PersonId = bill.PersonId;
                objBill.SquareFeet = bill.SquareFeet;
                objBill.Rate = bill.Rate;
                objBill.TotalAmount = Convert.ToDecimal(bill.TotalAmount);
                objBill.Remarks = bill.Remarks;
            }

            return View(objBill);
        }

        [HttpPost]
        public ActionResult EditBill(BillDebitVM billVM)
        {

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                billVM.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    // Check validation
                    if (billVM.BillType == "Other")
                    {
                        if (billVM.TotalAmount == null || billVM.TotalAmount <= 0)
                        {
                            ModelState.AddModelError("TotalAmount", Resource.Required);
                            return View(billVM);
                        }
                    }
                    else
                    {
                        bool IsFieldNull = false;
                        if (billVM.SquareFeet == null || billVM.SquareFeet <= 0)
                        {
                            ModelState.AddModelError("SquareFeet", Resource.Required);
                            IsFieldNull = true;
                        }
                        if (billVM.Rate == null || billVM.Rate <= 0)
                        {
                            ModelState.AddModelError("Rate", Resource.Required);
                            IsFieldNull = true;
                        }

                        if (IsFieldNull)
                        {
                            return View(billVM);
                        }
                        else
                        {
                            billVM.TotalAmount = billVM.SquareFeet * billVM.Rate;
                        }
                    }

                    DateTime bill_date = DateTime.ParseExact(billVM.BillDate, "dd/MM/yyyy", null);

                    tbl_BillDebit objBill = _db.tbl_BillDebit.FirstOrDefault(x => x.BillId == billVM.BillId);
                    if (objBill != null)
                    {
                        objBill.BillDate = bill_date;
                        objBill.BillNo = billVM.BillNo;
                        objBill.BillType = billVM.BillType;
                        objBill.SiteId = billVM.SiteId;
                        objBill.SquareFeet = billVM.SquareFeet;
                        objBill.Rate = billVM.Rate;
                        objBill.TotalAmount = Convert.ToDecimal(billVM.TotalAmount);
                        objBill.Remarks = billVM.Remarks;
                        objBill.ModifiedBy = clsSession.UserID;
                        objBill.ModifiedDate = DateTime.UtcNow;
                        _db.SaveChanges();
                    }

                    return RedirectToAction("Bill", new { id = billVM.PersonId });

                }
            }
            catch (Exception ex)
            {
            }

            return View(billVM);
        }

        public string ExportPDFOfSelectedPerson(Guid id)
        {

            string Result = "";

            try
            {

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                List<FinanceList> financeList = (from finance in _db.tbl_Finance
                                                 join user in _db.tbl_Users on finance.GivenAmountBy equals user.UserId
                                                 join site in _db.tbl_Sites on finance.SiteId equals site.SiteId into outerJoinSite
                                                 from site in outerJoinSite.DefaultIfEmpty()
                                                 where finance.PersonId == id
                                                 select new FinanceList
                                                 {
                                                     FinanceId = finance.FinanceId,
                                                     PersonId = finance.PersonId,
                                                     SelectedDate = finance.SelectedDate,
                                                     Amount = finance.Amount,
                                                     SiteId = finance.SiteId,
                                                     SiteName = (site != null) ? site.SiteName : "",
                                                     CreditOrDebit = finance.CreditOrDebit,
                                                     GivenAmountBy = finance.GivenAmountBy,
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
                                                     FirstName = user.FirstName,
                                                 }).Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.SelectedDate).ToList();

                string PersonName = GetPersonName(id);

                decimal? TotalAmount = financeList.Select(x => x.Amount).Sum();
                string strTotalAmount = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalAmount));

                string[] strColumns = new string[6] { "Date", "Amount", "Site Name", "Remarks", "Payment Type", "Bank Name" };
                if (financeList != null && financeList.Count() > 0)
                {

                    List<DateTime> lstDateTemp = new List<DateTime>();
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<!DOCTYPE html>");
                    strHTML.Append("<style>");
                    strHTML.Append("@page {@bottom-center {content: \"Page \" counter(page) \" of \" counter(pages);}}");
                    strHTML.Append("</style>");


                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<thead style=\"display:table-header-group;\">");
                    string Title = "Debit List Of " + PersonName;
                    strHTML.Append("<tr>");
                    strHTML.Append("<th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #ccc\">");
                    strHTML.Append(Title);
                    strHTML.Append("</th>");
                    strHTML.Append("</tr>");
                    //strHTML.Append("<tr><th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #ccc\">From " + startDate.ToString("dd/MM/yyyy") + " To " + endDate.ToString("dd/MM/yyyy") + " </th></tr>");
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
                    foreach (var obj in financeList)
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
                                            strcolval = Convert.ToDateTime(obj.SelectedDate).ToString("dd/MM/yyyy");
                                            break;
                                        }
                                    case "Amount":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.Amount);
                                            break;
                                        }
                                    case "Site Name":
                                        {
                                            strcolval = obj.SiteName;
                                            break;
                                        }
                                    case "Remarks":
                                        {
                                            strcolval = obj.Remarks;
                                            break;
                                        }
                                    case "Payment Type":
                                        {
                                            strcolval = obj.PaymentType;
                                            break;
                                        }
                                    case "Bank Name":
                                        {
                                            strcolval = obj.BankName;
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
                    strHTML.Append("<th style='border: 1px solid #ccc;'> " + strTotalAmount + " </th>");
                    strHTML.Append("<th colspan='5' style='border: 1px solid #ccc;'></th>");
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
                    Response.AddHeader("content-disposition", "download;filename=Debit List Of " + PersonName + ".pdf");
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

        public ActionResult AddDebitEntry(Guid Id) // id=PersonId
        {
            PersonFinanceVM objFinance = new PersonFinanceVM();

            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            objFinance.PersonId = Id;
            objFinance.UsersList = _db.tbl_Users.Where(x => x.ClientId == ClientId && x.IsActive == true && x.IsDeleted == false)
                     .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                     .OrderBy(x => x.Text).ToList();

            objFinance.SitesList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                     .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                     .ToList();

            ViewBag.PersonName = GetPersonName(Id);

            return View(objFinance);
        }

        [HttpPost]
        public ActionResult AddDebitEntry(PersonFinanceVM finance)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                finance.UsersList = _db.tbl_Users.Where(x => x.ClientId == ClientId)
                     .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                     .OrderBy(x => x.Text).ToList();

                if (ModelState.IsValid)
                {
                    DateTime date = DateTime.ParseExact(finance.SelectedDate, "dd/MM/yyyy", null);

                    tbl_Finance objFinance = new tbl_Finance();
                    objFinance.FinanceId = Guid.NewGuid();
                    objFinance.PersonId = finance.PersonId;
                    objFinance.SelectedDate = date;
                    objFinance.GivenAmountBy = finance.GivenAmountBy;
                    objFinance.Amount = Convert.ToDecimal(finance.Amount);
                    objFinance.SiteId = finance.SiteId;
                    objFinance.CreditOrDebit = "Debit"; // finance.CreditOrDebit;
                    objFinance.PaymentType = finance.PaymentType;
                    if (finance.PaymentType == "Cheque")
                    {
                        objFinance.ChequeNo = finance.ChequeNo;
                        objFinance.BankName = finance.BankName;
                        objFinance.ChequeFor = finance.ChequeFor;
                    }
                    else
                    {
                        objFinance.ChequeNo = "";
                        objFinance.BankName = "";
                        objFinance.ChequeFor = "";
                    }
                    objFinance.Remarks = finance.Remarks;
                    objFinance.IsActive = true;
                    objFinance.IsDeleted = false;
                    objFinance.CreatedBy = clsSession.UserID;
                    objFinance.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Finance.Add(objFinance);
                    _db.SaveChanges();

                    return RedirectToAction("Finance", new { id = finance.PersonId });

                }
            }
            catch (Exception ex)
            {

            }

            return View(finance);
        }

        public ActionResult EditDebitEntry(Guid Id) // id=FinanceId
        {
            PersonFinanceVM objFinance = (from f in _db.tbl_Finance
                                          where f.FinanceId == Id
                                          select new PersonFinanceVM
                                          {
                                              FinanceId = f.FinanceId,
                                              PersonId = f.PersonId,
                                              dtSelectedDate = f.SelectedDate,
                                              Amount = f.Amount,
                                              SiteId = f.SiteId,
                                              GivenAmountBy = f.GivenAmountBy,
                                              PaymentType = f.PaymentType,
                                              ChequeNo = f.ChequeNo,
                                              BankName = f.BankName,
                                              ChequeFor = f.ChequeFor,
                                              Remarks = f.Remarks
                                          }).FirstOrDefault();

            objFinance.SelectedDate = Convert.ToDateTime(objFinance.dtSelectedDate).ToString("dd/MM/yyyy");

            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            objFinance.UsersList = _db.tbl_Users.Where(x => x.ClientId == ClientId && x.IsActive == true && x.IsDeleted == false)
                     .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                     .OrderBy(x => x.Text).ToList();

            objFinance.SitesList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                     .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                     .ToList();

            ViewBag.PersonName = GetPersonName(Id);

            return View(objFinance);
        }

        [HttpPost]
        public ActionResult EditDebitEntry(PersonFinanceVM finance)
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            try
            {

                if (ModelState.IsValid)
                {
                    DateTime date = DateTime.ParseExact(finance.SelectedDate, "dd/MM/yyyy", null);

                    tbl_Finance objFinance = _db.tbl_Finance.Where(x => x.FinanceId == finance.FinanceId).FirstOrDefault();
                    objFinance.SelectedDate = date;
                    objFinance.GivenAmountBy = finance.GivenAmountBy;
                    objFinance.Amount = Convert.ToDecimal(finance.Amount);
                    objFinance.SiteId = finance.SiteId;
                    objFinance.PaymentType = finance.PaymentType;

                    if (finance.PaymentType == "Cheque")
                    {
                        objFinance.ChequeNo = finance.ChequeNo;
                        objFinance.BankName = finance.BankName;
                        objFinance.ChequeFor = finance.ChequeFor;
                    }
                    else
                    {
                        objFinance.ChequeNo = "";
                        objFinance.BankName = "";
                        objFinance.ChequeFor = "";
                    }

                    objFinance.Remarks = finance.Remarks;
                    objFinance.UpdatedBy = clsSession.UserID;
                    objFinance.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();

                    return RedirectToAction("Finance", new { id = finance.PersonId });

                }
            }
            catch (Exception ex)
            {

            }

            finance.UsersList = _db.tbl_Users.Where(x => x.ClientId == ClientId)
                     .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                     .OrderBy(x => x.Text).ToList();

            finance.SitesList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                     .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                     .ToList();

            return View(finance);
        }
         
        public ActionResult NewBill(Guid Id)
        {
            List<BillDebitNewVM> lstBill = new List<BillDebitNewVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            ViewBag.PersonId = Id;
            ViewBag.PersonName = GetPersonName(Id);

            lstBill = (from dbill in _db.tbl_BillDebitNew
                       join site in _db.tbl_Sites on dbill.SiteId equals site.SiteId
                       where dbill.ClientId == ClientId && dbill.PersonId == Id && !dbill.IsDeleted
                       select new BillDebitNewVM
                       {
                           BillId = dbill.BillId,
                           dBillDate = dbill.BillDate,
                           BillNo = dbill.BillNo,
                           BillType = dbill.BillType,
                           SiteId = dbill.SiteId,
                           SiteName = site.SiteName,
                           PersonId = dbill.PersonId, 
                           TotalAmount = dbill.TotalAmount,
                           Remarks = dbill.Remarks,
                           ObjFile = _db.tbl_Files.Where(x => x.ParentId == dbill.BillId && x.FileCategory == (int)FileType.Debit).FirstOrDefault()
                       }).OrderByDescending(x => x.dBillDate).ToList();

            return View(lstBill);
        }

        public ActionResult AddBillByFile(Guid Id)
        {
            BillDebitNewVM objBill = new BillDebitNewVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            objBill.PersonId = Id;

            objBill.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            ViewBag.PersonName = GetPersonName(Id);

            return View(objBill);
        }

        [HttpPost]
        public ActionResult AddBillByFile(BillDebitNewVM billVM, HttpPostedFileBase BillFile)
        {

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                billVM.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                     

                    DateTime bill_date = DateTime.ParseExact(billVM.BillDate, "dd/MM/yyyy", null);

                    tbl_BillDebitNew objBill = new tbl_BillDebitNew();
                    objBill.BillId = Guid.NewGuid();
                    objBill.BillDate = bill_date;
                    objBill.BillNo = billVM.BillNo;
                    objBill.BillType = "File";
                    objBill.SiteId = billVM.SiteId;
                    objBill.PersonId = billVM.PersonId; 
                    objBill.TotalAmount = Convert.ToDecimal(billVM.TotalAmount);
                    objBill.Remarks = billVM.Remarks;
                    objBill.ClientId = ClientId;
                    objBill.IsActive = true;
                    objBill.IsDeleted = false;
                    objBill.CreatedBy = clsSession.UserID;
                    objBill.CreatedDate = DateTime.UtcNow;
                    _db.tbl_BillDebitNew.Add(objBill);
                    _db.SaveChanges();

                    // Save + Upload Expense File
                    string fileName = string.Empty;
                    string path = Server.MapPath("~/DataFiles/DebitBillFile/");
                    if (BillFile != null)
                    {
                        fileName = Guid.NewGuid() + "-" + Path.GetFileName(BillFile.FileName);

                        string full_path = Path.Combine(path, fileName);
                        BillFile.SaveAs(full_path);

                        tbl_Files objFile = new tbl_Files();
                        objFile.FileId = Guid.NewGuid();
                        objFile.ParentId = objBill.BillId;
                        objFile.FileCategory = (int)FileType.Debit;
                        objFile.OriginalFileName = BillFile.FileName;
                        objFile.EncryptFileName = fileName;
                        _db.tbl_Files.Add(objFile);
                        _db.SaveChanges();
                    }

                    return RedirectToAction("NewBill", new { id = billVM.PersonId });

                }
            }
            catch (Exception ex)
            {
            }

            return View(billVM);
        }

        public ActionResult EditBillByFile(Guid Id) // id= BillId
        {
            BillDebitNewVM objBill = new BillDebitNewVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            objBill.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            tbl_BillDebitNew bill = _db.tbl_BillDebitNew.Where(x => x.BillId == Id).FirstOrDefault();
            if (objBill != null)
            {
                objBill.BillId = bill.BillId;
                objBill.BillDate = Convert.ToDateTime(bill.BillDate).ToString("dd/MM/yyyy");
                objBill.BillNo = bill.BillNo;
                objBill.BillType = bill.BillType;
                objBill.SiteId = bill.SiteId;
                objBill.PersonId = bill.PersonId; 
                objBill.TotalAmount = Convert.ToDecimal(bill.TotalAmount);
                objBill.Remarks = bill.Remarks;
            }

            ViewBag.PersonName = GetPersonName(objBill.PersonId);

            return View(objBill);
        }

        [HttpPost]
        public ActionResult EditBillByFile(BillDebitNewVM billVM, HttpPostedFileBase BillFile)
        {

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                billVM.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                     
                    DateTime bill_date = DateTime.ParseExact(billVM.BillDate, "dd/MM/yyyy", null);

                    tbl_BillDebitNew objBill = _db.tbl_BillDebitNew.FirstOrDefault(x => x.BillId == billVM.BillId);
                    if (objBill != null)
                    {
                        objBill.BillDate = bill_date;
                        objBill.BillNo = billVM.BillNo; 
                        objBill.SiteId = billVM.SiteId; 
                        objBill.TotalAmount = Convert.ToDecimal(billVM.TotalAmount);
                        objBill.Remarks = billVM.Remarks;
                        objBill.ModifiedBy = clsSession.UserID;
                        objBill.ModifiedDate = DateTime.UtcNow;
                        _db.SaveChanges();
                    }

                    // Save + Upload Debit File
                    string fileName = string.Empty;
                    string path = Server.MapPath("~/DataFiles/DebitBillFile/");
                    if (BillFile != null)
                    {
                        fileName = Guid.NewGuid() + "-" + Path.GetFileName(BillFile.FileName);

                        string full_path = Path.Combine(path, fileName);
                        BillFile.SaveAs(full_path);

                        tbl_Files objFile1 = _db.tbl_Files.Where(x => x.ParentId == objBill.BillId).FirstOrDefault();
                        if (objFile1 != null)
                        {
                            //Edit 
                            objFile1.OriginalFileName = BillFile.FileName;
                            objFile1.EncryptFileName = fileName;
                            _db.SaveChanges();
                        }
                        else
                        {
                            tbl_Files objFile = new tbl_Files();
                            objFile.FileId = Guid.NewGuid();
                            objFile.ParentId = objBill.BillId;
                            objFile.FileCategory = (int)FileType.Debit;
                            objFile.OriginalFileName = BillFile.FileName;
                            objFile.EncryptFileName = fileName;
                            _db.tbl_Files.Add(objFile);
                            _db.SaveChanges();
                        }

                    }

                    return RedirectToAction("NewBill", new { id = billVM.PersonId });

                }
            }
            catch (Exception ex)
            {
            }

            return View(billVM);
        }
         
    }

    public class UnicodeFontFactory : FontFactoryImp
    {
        private static readonly string fontpath = System.Web.HttpContext.Current.Server.MapPath("~/fonts/");
        private readonly BaseFont _baseFont;

        public UnicodeFontFactory()
        {
            _baseFont = BaseFont.CreateFont(fontpath + "ARIALUNI.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        }

        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
          bool cached)
        {
            return new Font(_baseFont, size, style, color);
        }
    }

}