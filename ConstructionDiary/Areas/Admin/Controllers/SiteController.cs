using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using ConstructionDiary.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ConstructionDiary.ResourceFiles;
using Newtonsoft.Json;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class SiteController : MyBaseController
    {
        ConstructionDiaryEntities _db;
        public SiteController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<SiteDetailVM> siteDetail = (from site in _db.tbl_Sites
                                             where site.ClientId == ClientId && !site.IsDeleted
                                             select new SiteDetailVM
                                             {
                                                 SiteId = site.SiteId,
                                                 SiteName = site.SiteName,
                                                 SiteDescription = site.SiteDescription,
                                                 IsActive = site.IsActive,
                                                 TotalBillAmount = _db.tbl_BillSiteNew.Where(x => x.SiteId == site.SiteId).ToList().Select(x => x.TotalAmount).Sum(),
                                                 TotalCreditAmount = _db.tbl_ContractorFinance.Where(x => x.SiteId == site.SiteId && x.CreditOrDebit == "Credit" && x.IsDeleted == false).ToList().Select(x => x.Amount).Sum()
                                             }).ToList();

            //var lstSites = (from p in _db.SP_GetSitesList(ClientId)
            //                select p).ToList();

            return View(siteDetail);
        }

        [HttpPost]
        public string SaveSite(string site_id, string site_name, string site_desc)
        {
            string ReturnMessage = "";

            try
            {
                if (site_id == "0")
                {
                    // Add Site
                    tbl_Sites objOldSite = _db.tbl_Sites.Where(x => x.SiteName == site_name && x.IsActive == true
                                                                && x.IsDeleted == false).FirstOrDefault();
                    if (objOldSite != null)
                    {
                        ReturnMessage = "alreadyexist";
                    }
                    else
                    {
                        tbl_Sites objSite = new tbl_Sites();
                        objSite.SiteId = Guid.NewGuid();
                        objSite.SiteName = site_name;
                        objSite.SiteDescription = site_desc;
                        objSite.ClientId = new Guid(clsSession.ClientID.ToString());
                        objSite.IsActive = true;
                        objSite.IsDeleted = false;
                        objSite.CreatedBy = new Guid(clsSession.UserID.ToString());
                        objSite.CreatedDate = DateTime.UtcNow;
                        _db.tbl_Sites.Add(objSite);
                        _db.SaveChanges();
                        ReturnMessage = "created";
                    }
                }
                else
                {
                    // Edit Site
                    Guid SiteIdToEdit = new Guid(site_id);

                    // Check to already exist or not
                    tbl_Sites objSite = _db.tbl_Sites.Where(x => x.SiteId != SiteIdToEdit && x.SiteName.ToLower() == site_name.ToLower()
                                                                && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                    if (objSite != null)
                    {
                        // already exist 
                        ReturnMessage = "alreadyexist";
                    }
                    else
                    {
                        tbl_Sites objSiteToUpdate = _db.tbl_Sites.Where(x => x.SiteId == SiteIdToEdit
                                                        && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();

                        objSiteToUpdate.SiteName = site_name;
                        objSiteToUpdate.SiteDescription = site_desc;
                        objSiteToUpdate.UpdatedBy = new Guid(clsSession.UserID.ToString());
                        objSiteToUpdate.ModifiedDate = DateTime.UtcNow;
                        _db.SaveChanges();

                        ReturnMessage = "updated";
                    }

                }

            }
            catch (Exception ex)
            {
                ReturnMessage = "exception";
            }

            return ReturnMessage;
        }

        [HttpPost]
        public string DeleteSite(string SiteId)
        {
            string ReturnMessage = "";

            try
            {
                Guid SiteIdToDelete = new Guid(SiteId);

                tbl_Sites objSite = _db.tbl_Sites.Where(x => x.SiteId == SiteIdToDelete && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objSite == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objSite.IsDeleted = true;
                    objSite.UpdatedBy = new Guid(clsSession.UserID.ToString());
                    objSite.ModifiedDate = DateTime.UtcNow;
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

        public ActionResult Detail(Guid id)
        {

            tbl_Sites objSite = new tbl_Sites();

            try
            {
                objSite = _db.tbl_Sites.Where(x => x.SiteId == id).FirstOrDefault();

                ViewData["SiteFinanceList"] = (from p in _db.SP_GetSiteDetailById(id)
                                               select p).ToList();

            }
            catch (Exception ex)
            {

            }

            return View(objSite);
        }

        public string ExportPDFOfSelectedSite(Guid id)
        {
            tbl_Sites objSite = new tbl_Sites();
            string Result = "";

            try
            {
                objSite = _db.tbl_Sites.Where(x => x.SiteId == id).FirstOrDefault();

                var list = (from p in _db.SP_GetSiteDetailById(id)
                            select p).ToList();

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                decimal? TotalCreditAmount = list.Select(x => x.Amount).Sum();
                string strTotalCreditAmount = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalCreditAmount));

                string[] strColumns = new string[6] { "Date", "Amount", "Type", "Payment Type", "Bank Name", "Remarks" };
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
                    string Title = "Credit List Of " + objSite.SiteName;
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
                                            strcolval = obj.ChequeNo + " " + obj.BankName;
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
                    strHTML.Append("<th style='text-align:right; border: 1px solid #ccc;'>Total</th>");
                    strHTML.Append("<th style='border: 1px solid #ccc;'> " + strTotalCreditAmount + " </th>");
                    strHTML.Append("<th colspan='5' style='border: 1px solid #ccc;'></th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("</tbody>");

                    strHTML.Append("</table>");
                    StringReader sr = new StringReader(strHTML.ToString());

                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 20f, 20f, 20f, 20f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    writer.PageEvent = new PDFGeneratePageEventHelper();
                    pdfDoc.Open();

                    XMLWorkerHelper objHelp = XMLWorkerHelper.GetInstance();
                    objHelp.ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "download;filename=Credit List Of " + objSite.SiteName + ".pdf");
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

        public void ExportExcelOfSelectedSite(Guid id)
        {

            tbl_Sites objSite = new tbl_Sites();

            objSite = _db.tbl_Sites.Where(x => x.SiteId == id).FirstOrDefault();

            var dtList = (from p in _db.SP_GetSiteDetailById(id)
                          select p).ToList();

            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Credit List");
            /*
            workSheet.Cells[1, 1].Style.Font.Bold = true;
            workSheet.Cells[1, 1].Style.Font.Size = 20;
            workSheet.Cells[1, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            workSheet.Cells[1, 1].Value = "Credit List"; 
            */

            string[] strColumns = new string[8] { "Date", "Amount", "Type", "Payment Type", "Cheque No", "Bank Name", "Cheque For", "By Amount" };

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
                                    strcolval = obj.UserName;
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
                Response.AddHeader("content-disposition", "attachment;  filename=Credit List Of " + objSite.SiteName + ".xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }

        public ActionResult View(Guid id)
        {
            SiteInfoVM objSiteInfo = new SiteInfoVM();
            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                objSiteInfo.SiteId = id;
                objSiteInfo.SiteName = _db.tbl_Sites.First(x => x.SiteId == id).SiteName;

                // Get Expenses Amount
                objSiteInfo.TotalExpenseAmount = _db.tbl_Expenses.Where(x => x.SiteId == id && x.IsDeleted == false).ToList().Select(x => x.Amount).Sum();

                // Get Material Amount
                objSiteInfo.TotalMaterialAmount = _db.tbl_MaterialPurchase.Where(x => x.SiteId == id && !x.IsDeleted).ToList().Select(x => x.Total).Sum();

                // Get Attendance Amount
                List<tbl_PersonAttendance> personAttendance = _db.tbl_PersonAttendance.Where(x => x.SiteId == id).ToList();
                decimal? totalPayableAmount = personAttendance.Select(x => x.PayableAmount).Sum();
                decimal? totalWithdrawAmount = personAttendance.Select(x => x.WithdrawAmount).Sum();
                decimal? totalOvertimeAmount = personAttendance.Select(x => x.OvertimeAmount).Sum();

                objSiteInfo.TotalPersonAttendanceAmount = Convert.ToDecimal(totalPayableAmount);

                // Get Credit
                List<tbl_ContractorFinance> lstCredit = _db.tbl_ContractorFinance.Where(x => x.SiteId == id && x.IsDeleted == false).ToList();
                decimal? totalCreditReceivedAmount = lstCredit.Where(x => x.CreditOrDebit == "Credit").Select(x => x.Amount).Sum();
                //decimal? totalCreditGivenAmount = lstCredit.Where(x => x.CreditOrDebit == "Debit").Select(x => x.Amount).Sum();
                //decimal totalCreditBalanceAmount = Convert.ToDecimal(totalCreditReceivedAmount) - Convert.ToDecimal(totalCreditGivenAmount);

                objSiteInfo.TotalCreditAmount = Convert.ToDecimal(totalCreditReceivedAmount); //totalCreditBalanceAmount;

                // Get Debit
                List<tbl_Finance> lstDebit = _db.tbl_Finance.Where(x => x.SiteId == id && !x.IsDeleted).ToList();
                //decimal? totalDebitReceivedAmount = lstDebit.Where(x => x.CreditOrDebit == "Credit").Select(x => x.Amount).Sum();
                decimal? totalDebitGivenAmount = lstDebit.Where(x => x.CreditOrDebit == "Debit").Select(x => x.Amount).Sum();
                //decimal totalDebitBalanceAmount = Convert.ToDecimal(totalDebitGivenAmount) - Convert.ToDecimal(totalDebitReceivedAmount);

                objSiteInfo.TotalDebitAmount = Convert.ToDecimal(totalDebitGivenAmount); // totalDebitBalanceAmount;

                // Calculate Balance
                objSiteInfo.CalculatedCreditAmount = objSiteInfo.TotalCreditAmount;
                objSiteInfo.CalculatedDebitAmount = objSiteInfo.TotalDebitAmount + objSiteInfo.TotalMaterialAmount + objSiteInfo.TotalExpenseAmount + objSiteInfo.TotalPersonAttendanceAmount;
                objSiteInfo.CalculatedBalanceAmount = objSiteInfo.CalculatedCreditAmount - objSiteInfo.CalculatedDebitAmount;

            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message.ToString();
            }

            return View(objSiteInfo);
        }

        public ActionResult Bill(Guid Id) // Id = SiteId
        {
            List<BillSiteVM> lstBill = new List<BillSiteVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            tbl_Sites objSite = _db.tbl_Sites.Where(x => x.SiteId == Id).First();

            ViewBag.SiteId = Id;
            ViewBag.SiteName = objSite.SiteName;

            lstBill = (from dbill in _db.tbl_BillSite
                       join site in _db.tbl_Sites on dbill.SiteId equals site.SiteId
                       where dbill.ClientId == ClientId && dbill.SiteId == Id && !dbill.IsDeleted
                       select new BillSiteVM
                       {
                           BillId = dbill.BillId,
                           dBillDate = dbill.BillDate,
                           BillNo = dbill.BillNo,
                           BillType = dbill.BillType,
                           SiteId = dbill.SiteId,
                           SiteName = site.SiteName,
                           SquareFeet = dbill.SquareFeet,
                           Rate = dbill.Rate,
                           TotalAmount = dbill.TotalAmount,
                           Remarks = dbill.Remarks
                       }).OrderByDescending(x => x.dBillDate).ToList();

            return View(lstBill);
        }

        public ActionResult AddBill(Guid Id)
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            BillSiteVM objBill = new BillSiteVM();
            objBill.SiteId = Id;

            return View(objBill);
        }

        [HttpPost]
        public ActionResult AddBill(BillSiteVM billVM)
        {

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

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

                    tbl_BillSite objBill = new tbl_BillSite();
                    objBill.BillId = Guid.NewGuid();
                    objBill.BillDate = bill_date;
                    objBill.BillNo = billVM.BillNo;
                    objBill.BillType = billVM.BillType;
                    objBill.SiteId = billVM.SiteId;
                    objBill.SquareFeet = billVM.SquareFeet;
                    objBill.Rate = billVM.Rate;
                    objBill.TotalAmount = Convert.ToDecimal(billVM.TotalAmount);
                    objBill.Remarks = billVM.Remarks;
                    objBill.ClientId = ClientId;
                    objBill.IsActive = true;
                    objBill.IsDeleted = false;
                    objBill.CreatedBy = clsSession.UserID;
                    objBill.CreatedDate = DateTime.UtcNow;
                    _db.tbl_BillSite.Add(objBill);
                    _db.SaveChanges();

                    return RedirectToAction("Bill", new { id = billVM.SiteId });

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
                tbl_BillSite objBill = _db.tbl_BillSite.Where(x => x.BillId == BillIdToDelete && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objBill == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    _db.tbl_BillSite.Remove(objBill);
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
            BillSiteVM objBill = new BillSiteVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            tbl_BillSite bill = _db.tbl_BillSite.Where(x => x.BillId == Id).FirstOrDefault();
            if (objBill != null)
            {
                objBill.BillId = bill.BillId;
                objBill.BillDate = Convert.ToDateTime(bill.BillDate).ToString("dd/MM/yyyy");
                objBill.BillNo = bill.BillNo;
                objBill.BillType = bill.BillType;
                objBill.SiteId = bill.SiteId;
                objBill.SquareFeet = bill.SquareFeet;
                objBill.Rate = bill.Rate;
                objBill.TotalAmount = Convert.ToDecimal(bill.TotalAmount);
                objBill.Remarks = bill.Remarks;
            }

            return View(objBill);
        }

        [HttpPost]
        public ActionResult EditBill(BillSiteVM billVM)
        {

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

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

                    tbl_BillSite objBill = _db.tbl_BillSite.FirstOrDefault(x => x.BillId == billVM.BillId);
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

                    return RedirectToAction("Bill", new { id = billVM.SiteId });

                }
            }
            catch (Exception ex)
            {
            }

            return View(billVM);
        }

        public ActionResult AllDebitSiteData(Guid Id) // Id=SiteId
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            ViewBag.SiteId = Id;
            ViewBag.SiteName = _db.tbl_Sites.First(x => x.SiteId == Id).SiteName;

            List<FinanceList> financeList = (from finance in _db.tbl_Finance
                                             join user in _db.tbl_Users on finance.GivenAmountBy equals user.UserId
                                             join person in _db.tbl_Persons on finance.PersonId equals person.PersonId
                                             where finance.SiteId == Id
                                             select new FinanceList
                                             {
                                                 FinanceId = finance.FinanceId,
                                                 PersonId = finance.PersonId,
                                                 PersonName = person.PersonFirstName,
                                                 SelectedDate = finance.SelectedDate,
                                                 Amount = finance.Amount,
                                                 SiteId = finance.SiteId,
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

            return View(financeList);
        }

        public ActionResult AllMaterialSiteData(Guid Id) // Id=SiteId
        {
            List<MaterialPurchaseVM> lstMaterial = new List<MaterialPurchaseVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            ViewBag.SiteId = Id;
            ViewBag.SiteName = _db.tbl_Sites.First(x => x.SiteId == Id).SiteName;

            lstMaterial = (from mat in _db.tbl_MaterialPurchase
                           join site in _db.tbl_Sites on mat.SiteId equals site.SiteId
                           join marchant in _db.tbl_Merchant on mat.MerchantId equals marchant.MerchantId into outerJoinMerchant
                           from marchant in outerJoinMerchant.DefaultIfEmpty()
                           where !mat.IsDeleted && mat.IsActive && mat.ClientId == ClientId
                                 && mat.SiteId == Id
                           select new MaterialPurchaseVM
                           {
                               MaterialPurchaseId = mat.MaterialPurchaseId,
                               dtPurchaseDate = mat.PurchaseDate,
                               SiteId = mat.SiteId,
                               Total = mat.Total,
                               GST_Per = mat.GST_Per,
                               SiteName = site.SiteName,
                               MerchantName = marchant.FirmName
                           }).OrderByDescending(x => x.dtPurchaseDate).ToList();

            return View(lstMaterial);
        }

        public ActionResult AllExpenseSiteData(Guid Id) // Id=SiteId
        {
            List<ExpenseVM> lstExpenses = new List<ExpenseVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            ViewBag.SiteId = Id;
            ViewBag.SiteName = _db.tbl_Sites.First(x => x.SiteId == Id).SiteName;

            lstExpenses = (from c in _db.tbl_Expenses
                           join exp in _db.tbl_ExpenseType on c.ExpenseTypeId equals exp.ExpenseTypeId
                           join site in _db.tbl_Sites on c.SiteId equals site.SiteId into outerJoinSite
                           from site in outerJoinSite.DefaultIfEmpty()
                           where !c.IsDeleted && c.ClientId == ClientId
                           && c.SiteId == Id
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
                           }).OrderByDescending(x => x.dtExpenseDate).ToList();

            return View(lstExpenses);
        }

        public ActionResult AllAttendanceSiteData(Guid Id) // Id=SiteId
        {
            ViewBag.SiteId = Id;
            ViewBag.SiteName = _db.tbl_Sites.First(x => x.SiteId == Id).SiteName;

            List<SiteAttendanceVM> lstSiteAttendance = (from pa in _db.tbl_PersonAttendance
                                                        join p in _db.tbl_Persons on pa.PersonId equals p.PersonId
                                                        where pa.SiteId == Id
                                                        select new SiteAttendanceVM
                                                        {
                                                            PersonAttendanceId = pa.PersonAttendanceId,
                                                            AttendaceId = pa.AttendanceId,
                                                            AttendanceDate = pa.CreatedDate,
                                                            PersonId = pa.PersonId,
                                                            PersonTypeId = pa.PersonTypeId,
                                                            PersonName = p.PersonFirstName,
                                                            AttendanceStatus = pa.AttendanceStatus,
                                                            TotalRokadiya = pa.TotalRokadiya,
                                                            PersonDailyRate = pa.PersonDailyRate,
                                                            PayableAmount = pa.PayableAmount,
                                                            WithdrawAmount = pa.WithdrawAmount,
                                                            OvertimeAmount = pa.OvertimeAmount,
                                                            Remarks = pa.Remarks
                                                        }).OrderByDescending(x => x.AttendanceDate).ToList();

            return View(lstSiteAttendance);
        }

        [HttpPost]
        public string ChangeSiteStatus(Guid SiteId, bool Status)
        {
            string ReturnMessage = "";

            try
            {
                tbl_Sites objSite = _db.tbl_Sites.Where(x => x.SiteId == SiteId && x.IsDeleted == false).FirstOrDefault();

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

        public ActionResult NewBill(Guid Id) // Id = SiteId
        {
            List<BillSiteNewVM> lstBill = new List<BillSiteNewVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            tbl_Sites objSite = _db.tbl_Sites.Where(x => x.SiteId == Id).First();

            ViewBag.SiteId = Id;
            ViewBag.SiteName = objSite.SiteName;

            lstBill = (from dbill in _db.tbl_BillSiteNew
                       join site in _db.tbl_Sites on dbill.SiteId equals site.SiteId
                       where dbill.ClientId == ClientId && dbill.SiteId == Id && !dbill.IsDeleted
                       select new BillSiteNewVM
                       {
                           BillId = dbill.BillId,
                           dBillDate = dbill.BillDate,
                           BillNo = dbill.BillNo,
                           BillType = dbill.BillType,
                           SiteId = dbill.SiteId,
                           SiteName = site.SiteName,
                           TotalAmount = dbill.TotalAmount,
                           Remarks = dbill.Remarks,
                           ObjFile = _db.tbl_Files.Where(x => x.ParentId == dbill.BillId && x.FileCategory == (int)FileType.Credit).FirstOrDefault()
                       }).OrderByDescending(x => x.dBillDate).ToList();

            return View(lstBill);
        }

        public ActionResult AddBillByFile(Guid Id)
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            BillSiteNewVM objBill = new BillSiteNewVM();
            objBill.SiteId = Id;

            ViewBag.SiteName = _db.tbl_Sites.First(x => x.SiteId == Id).SiteName;

            return View(objBill);
        }

        [HttpPost]
        public ActionResult AddBillByFile(BillSiteNewVM billVM, HttpPostedFileBase BillFile)
        {

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    // Check validation

                    DateTime bill_date = DateTime.ParseExact(billVM.BillDate, "dd/MM/yyyy", null);

                    tbl_BillSiteNew objBill = new tbl_BillSiteNew();
                    objBill.BillId = Guid.NewGuid();
                    objBill.BillDate = bill_date;
                    objBill.BillNo = billVM.BillNo;
                    objBill.BillType = "File";
                    objBill.SiteId = billVM.SiteId;
                    objBill.TotalAmount = Convert.ToDecimal(billVM.TotalAmount);
                    objBill.Remarks = billVM.Remarks;
                    objBill.ClientId = ClientId;
                    objBill.IsActive = true;
                    objBill.IsDeleted = false;
                    objBill.CreatedBy = clsSession.UserID;
                    objBill.CreatedDate = DateTime.UtcNow;
                    _db.tbl_BillSiteNew.Add(objBill);
                    _db.SaveChanges();

                    // Save + Upload Expense File
                    string fileName = string.Empty;
                    string path = Server.MapPath("~/DataFiles/SiteBillFile/");
                    if (BillFile != null)
                    {
                        fileName = Guid.NewGuid() + "-" + Path.GetFileName(BillFile.FileName);

                        string full_path = Path.Combine(path, fileName);
                        BillFile.SaveAs(full_path);

                        tbl_Files objFile = new tbl_Files();
                        objFile.FileId = Guid.NewGuid();
                        objFile.ParentId = objBill.BillId;
                        objFile.FileCategory = (int)FileType.Credit;
                        objFile.OriginalFileName = BillFile.FileName;
                        objFile.EncryptFileName = fileName;
                        _db.tbl_Files.Add(objFile);
                        _db.SaveChanges();
                    }

                    return RedirectToAction("NewBill", new { id = billVM.SiteId });

                }
            }
            catch (Exception ex)
            {
            }

            return View(billVM);
        }

        public ActionResult EditBillByFile(Guid Id)
        {
            BillSiteNewVM objBill = new BillSiteNewVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            tbl_BillSiteNew bill = _db.tbl_BillSiteNew.Where(x => x.BillId == Id).FirstOrDefault();
            if (objBill != null)
            {
                objBill.BillId = bill.BillId;
                objBill.BillDate = Convert.ToDateTime(bill.BillDate).ToString("dd/MM/yyyy");
                objBill.BillNo = bill.BillNo;
                objBill.BillType = bill.BillType;
                objBill.SiteId = bill.SiteId;
                objBill.TotalAmount = Convert.ToDecimal(bill.TotalAmount);
                objBill.Remarks = bill.Remarks;
            }

            ViewBag.SiteName = _db.tbl_Sites.First(x => x.SiteId == objBill.SiteId).SiteName;

            return View(objBill);
        }

        [HttpPost]
        public ActionResult EditBillByFile(BillSiteNewVM billVM, HttpPostedFileBase BillFile)
        {

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    // Check validation

                    DateTime bill_date = DateTime.ParseExact(billVM.BillDate, "dd/MM/yyyy", null);

                    tbl_BillSiteNew objBill = _db.tbl_BillSiteNew.Where(x => x.BillId == billVM.BillId).FirstOrDefault();

                    objBill.BillDate = bill_date;
                    objBill.BillNo = billVM.BillNo;
                    objBill.TotalAmount = Convert.ToDecimal(billVM.TotalAmount);
                    objBill.Remarks = billVM.Remarks;
                    objBill.ClientId = ClientId;
                    objBill.ModifiedBy = clsSession.UserID;
                    objBill.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();

                    // Save + Upload Credit File
                    string fileName = string.Empty;
                    string path = Server.MapPath("~/DataFiles/SiteBillFile/");
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
                            objFile.FileCategory = (int)FileType.Credit;
                            objFile.OriginalFileName = BillFile.FileName;
                            objFile.EncryptFileName = fileName;
                            _db.tbl_Files.Add(objFile);
                            _db.SaveChanges();
                        }

                    }

                    return RedirectToAction("NewBill", new { id = billVM.SiteId });

                }
            }
            catch (Exception ex)
            {
            }

            return View(billVM);
        }

        public ActionResult AddBillByArea(Guid Id)
        {
            ViewBag.SiteId = Id;
            ViewBag.SiteName = _db.tbl_Sites.First(x => x.SiteId == Id).SiteName;
            return View();
        }

        [HttpPost]
        public ActionResult AddBillByArea(string BillData)
        {
            GeneralResponse response = new GeneralResponse();

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Guid ClientId = new Guid(clsSession.ClientID.ToString());

                    AreaSiteBillVM objBillInfo = JsonConvert.DeserializeObject<AreaSiteBillVM>(BillData);

                    DateTime bill_date = DateTime.ParseExact(objBillInfo.BillDate, "dd/MM/yyyy", null);

                    decimal GrandTotalAmt = CalculateGrandTotal(objBillInfo.BillSiteItem);

                    // Save data in SiteBill
                    tbl_BillSiteNew objBill = new tbl_BillSiteNew();
                    objBill.BillId = Guid.NewGuid();
                    objBill.BillDate = bill_date;
                    objBill.BillNo = objBillInfo.BillNo;
                    objBill.BillType = "Area";
                    objBill.SiteId = objBillInfo.SiteId;
                    objBill.TotalAmount = GrandTotalAmt;
                    objBill.Remarks = objBillInfo.Remarks;
                    objBill.ClientId = ClientId;
                    objBill.IsActive = true;
                    objBill.IsDeleted = false;
                    objBill.CreatedBy = clsSession.UserID;
                    objBill.CreatedDate = DateTime.UtcNow;
                    _db.tbl_BillSiteNew.Add(objBill);
                    _db.SaveChanges();

                    // Save data in SiteBillItem
                    objBillInfo.BillSiteItem.ForEach(item =>
                    {
                        decimal? TotalArea = CalculateBillArea(item);
                        decimal? TotalAmt = TotalArea * item.Rate;

                        tbl_BillSiteItem objItem = new tbl_BillSiteItem();
                        objItem.BillSiteItemId = Guid.NewGuid();
                        objItem.BillId = objBill.BillId;
                        objItem.ItemName = item.ItemName;
                        objItem.ItemCategory = item.ItemCategory;
                        objItem.ItemType = item.ItemType;
                        objItem.Length = item.Length;
                        objItem.Height = item.Height;
                        objItem.Width = item.Width;
                        objItem.Qty = item.Qty;
                        objItem.Rate = item.Rate;
                        objItem.Area = TotalArea;
                        objItem.Amount = Convert.ToDecimal(TotalAmt);
                        objItem.CreatedDate = DateTime.UtcNow;
                        _db.tbl_BillSiteItem.Add(objItem);
                        _db.SaveChanges();

                    });

                    response.IsError = false;
                    response.RedirectUrl = Url.Action("NewBill", "Site", new { id = objBillInfo.SiteId });
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsError = true;
                    response.ErrorMessage = ex.Message.ToString();
                }
            }

            return Json(response);
        }

        public ActionResult EditBillByArea(Guid Id)
        {
            AreaSiteBillVM objBillInfo = (from b in _db.tbl_BillSiteNew
                                          where b.BillId == Id
                                          select new AreaSiteBillVM
                                          {
                                              BillId = b.BillId,
                                              dtBillDate = b.BillDate,
                                              BillNo = b.BillNo,
                                              Remarks = b.Remarks,
                                              SiteId = b.SiteId,
                                              GrandTotal = b.TotalAmount,
                                              BillSiteItem =
                                                (from i in _db.tbl_BillSiteItem
                                                 where i.BillId == b.BillId
                                                 select new AreaSiteBillItemVM
                                                 {
                                                     BillSiteItemId = i.BillSiteItemId,
                                                     ItemCategory = i.ItemCategory,
                                                     ItemName = i.ItemName,
                                                     ItemType = i.ItemType,
                                                     Length = i.Length,
                                                     Width = i.Width,
                                                     Height = i.Height,
                                                     Qty = i.Qty,
                                                     Rate = i.Rate,
                                                     Area = i.Area,
                                                     Amount = i.Amount,
                                                     SeqNo = i.SeqNo
                                                 }).OrderBy(x => x.SeqNo).ToList()
                                          }).FirstOrDefault();

            objBillInfo.BillDate = Convert.ToDateTime(objBillInfo.dtBillDate).ToString("dd/MM/yyyy");

            ViewBag.SiteId = objBillInfo.SiteId;
            ViewBag.SiteName = _db.tbl_Sites.First(x => x.SiteId == objBillInfo.SiteId).SiteName;

            return View(objBillInfo);
        }

        [HttpPost]
        public ActionResult EditBillByArea(string BillData)
        {
            GeneralResponse response = new GeneralResponse();

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Guid ClientId = new Guid(clsSession.ClientID.ToString());

                    AreaSiteBillVM objBillInfo = JsonConvert.DeserializeObject<AreaSiteBillVM>(BillData);

                    DateTime bill_date = DateTime.ParseExact(objBillInfo.BillDate, "dd/MM/yyyy", null);

                    decimal GrandTotalAmt = CalculateGrandTotal(objBillInfo.BillSiteItem);

                    // Save data in SiteBill
                    tbl_BillSiteNew objBill = _db.tbl_BillSiteNew.Where(x => x.BillId == objBillInfo.BillId).FirstOrDefault();
                    objBill.BillDate = bill_date;
                    objBill.BillNo = objBillInfo.BillNo;
                    objBill.TotalAmount = GrandTotalAmt;
                    objBill.Remarks = objBillInfo.Remarks;
                    objBill.ClientId = ClientId;
                    objBill.ModifiedBy = clsSession.UserID;
                    objBill.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();

                    // Check unselected and exist in table
                    List<Guid> lstItemIds = objBillInfo.BillSiteItem.Select(x => Guid.Parse(x.BillSiteItemId.ToString())).ToList();

                    List<tbl_BillSiteItem> ToDeleteItems = _db.tbl_BillSiteItem.Where(x => x.BillId == objBillInfo.BillId &&
                                                                !lstItemIds.Contains(x.BillSiteItemId)
                                                                ).ToList();

                    if (ToDeleteItems.Count > 0)
                    {
                        _db.tbl_BillSiteItem.RemoveRange(ToDeleteItems);
                        _db.SaveChanges();
                    }

                    // Save data in SiteBillItem
                    objBillInfo.BillSiteItem.ForEach(item =>
                    {
                        decimal? TotalArea = CalculateBillArea(item);
                        decimal? TotalAmt = TotalArea * item.Rate;

                        tbl_BillSiteItem objItem = _db.tbl_BillSiteItem.Where(x => x.BillSiteItemId == item.BillSiteItemId).FirstOrDefault();

                        if (objItem == null)
                        {
                            tbl_BillSiteItem objItemNew = new tbl_BillSiteItem();
                            objItemNew.BillSiteItemId = Guid.NewGuid();
                            objItemNew.BillId = objBill.BillId;
                            objItemNew.ItemName = item.ItemName;
                            objItemNew.ItemCategory = item.ItemCategory;
                            objItemNew.ItemType = item.ItemType;
                            objItemNew.Length = item.Length;
                            objItemNew.Height = item.Height;
                            objItemNew.Width = item.Width;
                            objItemNew.Qty = item.Qty;
                            objItemNew.Rate = item.Rate;
                            objItemNew.Area = TotalArea;
                            objItemNew.Amount = Convert.ToDecimal(TotalAmt);
                            objItem.CreatedDate = DateTime.UtcNow;
                            _db.tbl_BillSiteItem.Add(objItemNew);
                            _db.SaveChanges();
                        }
                        else
                        {

                            objItem.ItemName = item.ItemName;
                            objItem.ItemCategory = item.ItemCategory;
                            objItem.ItemType = item.ItemType;
                            objItem.Length = item.Length;
                            objItem.Height = item.Height;
                            objItem.Width = item.Width;
                            objItem.Qty = item.Qty;
                            objItem.Rate = item.Rate;
                            objItem.Area = TotalArea;
                            objItem.Amount = Convert.ToDecimal(TotalAmt);
                            _db.SaveChanges();
                        }

                    });

                    response.IsError = false;
                    response.RedirectUrl = Url.Action("NewBill", "Site", new { id = objBillInfo.SiteId });
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsError = true;
                    response.ErrorMessage = ex.Message.ToString();
                }
            }

            return Json(response);
        }

        [HttpPost]
        public string DeleteNewBill(Guid BillId)
        {
            string ReturnMessage = "";

            try
            {

                tbl_BillSiteNew objBill = _db.tbl_BillSiteNew.Where(x => x.BillId == BillId && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objBill == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    if (objBill.BillType == "Area")
                    {
                        List<tbl_BillSiteItem> lstItems = _db.tbl_BillSiteItem.Where(x => x.BillId == BillId).ToList();
                        if (lstItems.Count > 0)
                        {
                            _db.tbl_BillSiteItem.RemoveRange(lstItems);
                            _db.SaveChanges();
                        }
                    }

                    _db.tbl_BillSiteNew.Remove(objBill);
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

        private decimal CalculateBillArea(AreaSiteBillItemVM item)
        {
            decimal areaTotal = 0;

            decimal Length = (item.Length == null ? 0 : Convert.ToDecimal(item.Length));
            decimal Height = (item.Height == null ? 0 : Convert.ToDecimal(item.Height));
            decimal Width = (item.Width == null ? 0 : Convert.ToDecimal(item.Width));
            decimal Qty = (item.Qty == null ? 0 : Convert.ToDecimal(item.Qty));

            if (item.ItemType == "cft")
            {
                areaTotal = Length * Height * Width * Qty;
            }
            else if (item.ItemType == "sft")
            {
                areaTotal = Length * Width * Qty;
            }
            else if (item.ItemType == "rft")
            {
                areaTotal = Length * Qty;
            }
            else if (item.ItemType == "nos")
            {
                areaTotal = Qty;
            }

            return areaTotal;
        }

        private decimal CalculateGrandTotal(List<AreaSiteBillItemVM> lstItem)
        {
            decimal grandTotal = 0;

            lstItem.ForEach(item =>
            {

                decimal areaTotal = CalculateBillArea(item);
                decimal totalAmt = areaTotal * Convert.ToDecimal(item.Rate);

                if (item.ItemCategory == "add")
                    grandTotal += totalAmt;
                else if (item.ItemCategory == "less")
                    grandTotal -= totalAmt;

            });

            return grandTotal;
        }

        public string ExportPDFOfSelectedSiteBill(Guid id)
        {
            tbl_Sites objSite = new tbl_Sites();
            string Result = "";

            try
            {
                tbl_BillSiteNew objSiteBill = _db.tbl_BillSiteNew.Where(x => x.BillId == id).FirstOrDefault();
                List<tbl_BillSiteItem> objSiteBillItems = _db.tbl_BillSiteItem.Where(x => x.BillId == id).OrderBy(x => x.SeqNo).ToList();

                objSite = _db.tbl_Sites.Where(x => x.SiteId == id).FirstOrDefault();

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                decimal? SubTotalAmount = objSiteBillItems.Select(x => x.Amount).Sum();
                string strSubTotalAmount = CoreHelper.GetFormatterAmount(Convert.ToDecimal(SubTotalAmount));

                string[] strColumns = new string[9] { "SNo", "Particulars", "Nos", "Length", "Width", "Height", "Area", "Rate", "Amount" };
                if (objSiteBillItems != null && objSiteBillItems.Count() > 0)
                {

                    List<DateTime> lstDateTemp = new List<DateTime>();
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<!DOCTYPE html>");
                    strHTML.Append("<style>");
                    strHTML.Append("@page {@bottom-center {content: \"Page \" counter(page) \" of \" counter(pages);}}");
                    strHTML.Append("</style>");

                    strHTML.Append("<table border='0' cellspacing='0' cellpadding='5' style='width:100%; text-align:center; margin-bottom: 5px;'><tr><td> <h3>Labour with Material Bill</h3> </td></tr></table>");

                    strHTML.Append("<table border='1' cellspacing='0' cellpadding='5' style='width:45%; text-align:center; margin-bottom: 5px; display: inline-block;'><tr><td> To, Divyaa Construnction </td></tr></table>");
                    strHTML.Append("<table border='1' cellspacing='0' cellpadding='5' style='width:45%; text-align:center; margin-bottom: 5px; display: inline-block;'><tr><td> Invoice No. : 12 </td></tr></table>");
                    
                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #000000;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<thead style=\"display:table-header-group;\">");
                    strHTML.Append("<tr>");

                    for (int idx = 0; idx < strColumns.Length; idx++)
                    {
                        strHTML.Append("<th style=\"border: 1px solid #000000; text-align:center; \">");
                        strHTML.Append(strColumns[idx]);
                        strHTML.Append("</th>");
                    }

                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody>");

                    int counter = 1;

                    foreach (var obj in objSiteBillItems)
                    {

                        if (obj != null)
                        {

                            strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                            for (int Col = 0; Col < strColumns.Length; Col++)
                            {

                                string strcolval = "";
                                switch (strColumns[Col])
                                {
                                    case "SNo":
                                        {
                                            strcolval = counter.ToString();
                                            break;
                                        }
                                    case "Particulars":
                                        {
                                            strcolval = obj.ItemName;
                                            break;
                                        }
                                    case "Nos":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.Qty);
                                            break;
                                        }
                                    case "Length":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.Length);
                                            break;
                                        }
                                    case "Width":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.Width);
                                            break;
                                        }
                                    case "Height":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.Height);
                                            break;
                                        }
                                    case "Area":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.Area) + " " + obj.ItemType;
                                            break;
                                        }
                                    case "Rate":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.Rate);
                                            break;
                                        }
                                    case "Amount":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.Amount);
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }

                                }
                                strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\">");
                                strHTML.Append(strcolval);
                                strHTML.Append("</td>");
                            }
                            strHTML.Append("</tr>");
                        }

                        counter++;
                    }

                    // Total
                    strHTML.Append("<tr>");
                    strHTML.Append("<th style='text-align:right; border: 1px solid #000000;'></th>");
                    strHTML.Append("<th style='border: 1px solid #000000;'>Sub Total</th>");
                    strHTML.Append("<th style='text-align:right; border: 1px solid #000000;'></th>");
                    strHTML.Append("<th style='text-align:right; border: 1px solid #000000;'></th>");
                    strHTML.Append("<th style='text-align:right; border: 1px solid #000000;'></th>");
                    strHTML.Append("<th style='text-align:right; border: 1px solid #000000;'></th>");
                    strHTML.Append("<th style='text-align:right; border: 1px solid #000000;'></th>");
                    strHTML.Append("<th style='text-align:right; border: 1px solid #000000;'></th>");
                    strHTML.Append("<th style='border: 1px solid #000000;'> " + strSubTotalAmount + " </th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("</tbody>");

                    strHTML.Append("</table>");
                    StringReader sr = new StringReader(strHTML.ToString());

                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 20f, 20f, 20f, 20f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    writer.PageEvent = new PDFGeneratePageEventHelper();
                    pdfDoc.Open();

                    XMLWorkerHelper objHelp = XMLWorkerHelper.GetInstance();
                    objHelp.ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "download;filename=Site Bill.pdf");
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


    }
}