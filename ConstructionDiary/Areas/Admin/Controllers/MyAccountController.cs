using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ConstructionDiary.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using System.Text;
using ConstructionDiary.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class MyAccountController : MyBaseController
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
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
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
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
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
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
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
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
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
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
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
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            }
            catch (Exception ex)
            {
            }
            return View(objFinance);
        }

        public string ExportPDFOfAllCreditList()
        {

            string Result = "";

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                List<MyFinanceList> list = (from finance in _db.tbl_ContractorFinance
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


                string[] strColumns = new string[7] { "Date", "Amount", "Site Name", "Type", "Payment Type", "Bank Name", "By Amount" };
                if (list != null && list.Count() > 0)
                {

                    List<DateTime> lstDateTemp = new List<DateTime>();
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<!DOCTYPE html>");
                    strHTML.Append("<style>");
                    strHTML.Append("@page {@bottom-center {content: \"Page \" counter(page) \" of \" counter(pages);}}");
                    strHTML.Append("</style>");
                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<thead style=\"display:table-header-group;\">");
                    string Title = "Credit List";
                    strHTML.Append("<tr>");
                    strHTML.Append("<th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #ccc\">");
                    strHTML.Append(Title);
                    strHTML.Append("</th>");
                    strHTML.Append("</tr>");
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
                    foreach (var obj in list)
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
                                    case "Type":
                                        {
                                            strcolval = obj.CreditOrDebit;
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
                                    case "By Amount":
                                        {
                                            strcolval = obj.FirstName;
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
                    strHTML.Append("</tbody>");
                    //strHTML.Append("<tfoot>");
                    //strHTML.Append("<tr>");
                    //strHTML.Append("<td colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #ccc\">");
                    //strHTML.Append("Confidential: Personally Identifiable Materials");
                    //strHTML.Append("</td>");
                    //strHTML.Append("</tr>");
                    //strHTML.Append("</tfoot>");
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
                    //objHelp.ParseXHtml(writer, pdfDoc, sr);
                    objHelp.ParseXHtml(writer, pdfDoc, ms, null, Encoding.UTF8, new UnicodeFontFactory());

                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "download;filename=Credit List.pdf");
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

        public void ExportExcelOfAllCreditList()
        {

            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<MyFinanceList> dtList = (from finance in _db.tbl_ContractorFinance
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


            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Credit List");
            /*
            workSheet.Cells[1, 1].Style.Font.Bold = true;
            workSheet.Cells[1, 1].Style.Font.Size = 20;
            workSheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            workSheet.Cells[1, 1].Value = "Credit List"; 
            */

            string[] strColumns = new string[9] { "Date", "Amount", "Site Name", "Type", "Payment Type", "Cheque No", "Bank Name", "Cheque For", "By Amount" };

            for (var col = 1; col < strColumns.Length + 1; col++)
            {
                workSheet.Cells[1, col].Style.Font.Bold = true;
                workSheet.Cells[1, col].Style.Font.Size = 12;
                workSheet.Cells[1, col].Value = strColumns[col - 1];
                workSheet.Cells[1, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[1, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells[1, col].AutoFitColumns(30, 70);
                workSheet.Cells[1, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[1, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[1, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[1, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[1, col].Style.WrapText = true;
            }

            int row1 = 0;
            foreach (var obj in dtList)
            {
                if (obj != null)
                {

                    for (var col = 1; col < strColumns.Length + 1; col++)
                    {
                        string strcolval = "";
                        switch (strColumns[col - 1].Trim())
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
                            case "Type":
                                {
                                    strcolval = obj.CreditOrDebit;
                                    break;
                                }
                            case "Payment Type":
                                {
                                    strcolval = obj.PaymentType;
                                    break;
                                }
                            case "Cheque No":
                                {
                                    strcolval = obj.ChequeNo;
                                    break;
                                }
                            case "Bank Name":
                                {
                                    strcolval = obj.BankName;
                                    break;
                                }
                            case "Cheque For":
                                {
                                    strcolval = obj.ChequeFor;
                                    break;
                                }
                            case "By Amount":
                                {
                                    strcolval = obj.FirstName;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        workSheet.Cells[row1 + 2, col].Style.Font.Bold = false;
                        workSheet.Cells[row1 + 2, col].Style.Font.Size = 12;
                        workSheet.Cells[row1 + 2, col].Value = strcolval;
                        workSheet.Cells[row1 + 2, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[row1 + 2, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        workSheet.Cells[row1 + 2, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row1 + 2, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row1 + 2, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row1 + 2, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row1 + 2, col].Style.WrapText = true;
                        workSheet.Cells[row1 + 2, col].AutoFitColumns(30, 70);
                    }
                    row1 = row1 + 1;
                }
            }


            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Credit List.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }

    }
}