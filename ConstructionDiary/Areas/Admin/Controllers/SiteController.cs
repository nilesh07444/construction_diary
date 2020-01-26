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
using ConstructionDiary.ViewModel;

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
            var lstSites = (from p in _db.SP_GetSitesList(ClientId)
                            select p).ToList();

            //List<tbl_Sites> lstSites = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == clsSession.ClientID).ToList();
            return View(lstSites);
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

                string[] strColumns = new string[6] { "Date", "Amount", "Type", "Payment Type", "Bank Name", "By Amount" };
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
                                            strcolval = obj.BankName;
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
                objSiteInfo.TotalExpenseAmount = _db.tbl_Expenses.Where(x => x.SiteId == id).ToList().Select(x => x.Amount).Sum();

                // Get Material Amount
                objSiteInfo.TotalMaterialAmount = _db.tbl_MaterialPurchase.Where(x => x.SiteId == id).ToList().Select(x => x.Total).Sum();

                // Get Attendance Amount
                List<tbl_PersonAttendance> personAttendance = _db.tbl_PersonAttendance.Where(x => x.SiteId == id).ToList();
                decimal? totalPayableAmount = personAttendance.Select(x => x.PayableAmount).Sum();
                decimal? totalWithdrawAmount = personAttendance.Select(x => x.WithdrawAmount).Sum();
                decimal? totalOvertimeAmount = personAttendance.Select(x => x.OvertimeAmount).Sum();

                objSiteInfo.TotalPersonAttendanceAmount = Convert.ToDecimal(totalPayableAmount);

                // Get Credit
                List<tbl_ContractorFinance> lstCredit = _db.tbl_ContractorFinance.Where(x => x.SiteId == id).ToList();
                decimal? totalCreditReceivedAmount = lstCredit.Where(x => x.CreditOrDebit == "Credit").Select(x => x.Amount).Sum();
                decimal? totalCreditGivenAmount = lstCredit.Where(x => x.CreditOrDebit == "Debit").Select(x => x.Amount).Sum(); 
                decimal totalCreditBalanceAmount = Convert.ToDecimal(totalCreditReceivedAmount) - Convert.ToDecimal(totalCreditGivenAmount);

                objSiteInfo.TotalCreditAmount = totalCreditBalanceAmount;

                // Get Debit
                List<tbl_Finance> lstDebit = _db.tbl_Finance.Where(x => x.SiteId == id).ToList();
                decimal? totalDebitReceivedAmount = lstDebit.Where(x => x.CreditOrDebit == "Credit").Select(x => x.Amount).Sum();
                decimal? totalDebitGivenAmount = lstDebit.Where(x => x.CreditOrDebit == "Debit").Select(x => x.Amount).Sum();
                decimal totalDebitBalanceAmount = Convert.ToDecimal(totalDebitGivenAmount) - Convert.ToDecimal(totalDebitReceivedAmount);

                objSiteInfo.TotalDebitAmount = totalDebitBalanceAmount;

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

    }
}