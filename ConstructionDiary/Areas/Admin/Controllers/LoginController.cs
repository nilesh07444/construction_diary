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

namespace ConstructionDiary.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        ConstructionDiaryEntities _db;
        public LoginController()
        {
            _db = new ConstructionDiaryEntities();
        }
        // GET: Admin/Login
        public ActionResult Index()
        {
            //if (clsSession.SessionID != null)
            //    return RedirectToAction("Index", "Dashboard");

            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginVM userLogin)
        {
            try
            {
                string EncyptedPassword = userLogin.Password; // Encrypt(userLogin.Password);
                var data = _db.tbl_Users.Where(x => (x.UserName == userLogin.UserName || x.EmailId == userLogin.UserName)
                && x.Password == EncyptedPassword && !x.IsDeleted).FirstOrDefault();

                if (data != null)
                {
                    // Get Client Details
                    if (data.ClientId != null)
                    {
                        Guid clientId = new Guid(data.ClientId.ToString());
                        var clientData = _db.tbl_Clients.Where(x => x.ClientId == clientId).FirstOrDefault();
                        DateTime todayDate = DateTime.UtcNow;

                        if (clientData.IsDeleted)
                        {
                            TempData["LoginError"] = "Your Firm is deleted. Please contact administrator.";
                            return View();
                        }
                        else if (!clientData.IsActive)
                        {
                            TempData["LoginError"] = "Your Firm is not active. Please contact administrator.";
                            return View();
                        }
                        else if (!data.IsActive)
                        {
                            TempData["LoginError"] = "Your Account is not active. Please contact administrator.";
                            return View();
                        }
                        else if (
                                    (clientData.PackageTypeId < (int)PackageTypes.OneTimePackage) &&
                                    (clientData.ExpireDate != null) &&
                                    (clientData.ExpireDate.Value.Date < todayDate.Date)
                                )
                        {
                            TempData["LoginError"] = "Your Trial Period Expired. Please contact administrator.";
                            return View();
                        }
                        else
                        {
                            clsSession.FirmName = clientData.FirmName;
                        }
                    }

                    clsSession.SessionID = Session.SessionID;
                    clsSession.UserID = data.UserId;
                    clsSession.RoleID = data.RoleId;
                    clsSession.UserName = data.FirstName;
                    clsSession.ClientID = data.ClientId;
                    clsSession.ImagePath = data.UserPhoto;
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    TempData["LoginError"] = "Invalid username or password";
                    return View();
                }
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        public ActionResult Signout()
        {
            clsSession.SessionID = "";
            return RedirectToAction("Index");
        }

        /* For testing */
        public string Encrypt(string text)
        {
            string value = CoreHelper.Encrypt(text);
            return value;
        }
        public string Decrypt(string text)
        {
            string value = CoreHelper.Decrypt(text);
            return value;
        }

        // pdf 
        public string ExportPDF()
        {
             
            string Result = "";

            try
            {                

                List<MyFinanceList> list = (from finance in _db.tbl_ContractorFinance
                                                   join user in _db.tbl_Users on finance.UserId equals user.UserId
                                                   join site in _db.tbl_Sites on finance.SiteId equals site.SiteId
                                                   //where site.ClientId == ClientId
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

                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 20f, 20f, 20f, 20f); 
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    writer.PageEvent = new PDFGeneratePageEventHelper();
                    pdfDoc.Open();

                    XMLWorkerHelper objHelp = XMLWorkerHelper.GetInstance();
                    objHelp.ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "download;filename=Test PDF Name.pdf");
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