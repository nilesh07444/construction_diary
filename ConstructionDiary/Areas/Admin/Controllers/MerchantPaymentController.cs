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
    [filters]
    public class MerchantPaymentController : MyBaseController
    {
        ConstructionDiaryEntities _db;
        public MerchantPaymentController()
        {
            _db = new ConstructionDiaryEntities();
        }
        public ActionResult Index(string duration, string start, string end, string site)
        {
            List<MerchantPaymentVM> lstMerchantPayment = new List<MerchantPaymentVM>();
            try
            {
                if (string.IsNullOrEmpty(duration))
                    duration = "month";

                ViewBag.Duration = duration;
                ViewBag.StartDate = start;
                ViewBag.EndDate = end;

                ViewBag.Site = site;

                DateTime startDate = DateTime.Today;
                DateTime endDate = DateTime.Today;

                Guid ClientId = new Guid(clsSession.ClientID.ToString());
                int RoleID = clsSession.RoleID;

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

                lstMerchantPayment = GetMerchantPaymentListByFilter(startDate, endDate, site);

                List<SelectListItem> lstSites = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();
                ViewBag.SiteList = lstSites;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(lstMerchantPayment);
        }

        public List<MerchantPaymentVM> GetMerchantPaymentListByFilter(DateTime startDate, DateTime endDate, string site)
        {
            List<MerchantPaymentVM> lstMerchantPayment = new List<MerchantPaymentVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            int RoleID = clsSession.RoleID;

            Guid selectedSite = (site != null && !string.IsNullOrEmpty(site) ? new Guid(site) : Guid.Empty);

            lstMerchantPayment = (from c in _db.tbl_MerchantPayment
                                  join sites in _db.tbl_Sites on c.SiteId equals sites.SiteId
                                  join marchant in _db.tbl_Merchant on c.MerchantId equals marchant.MerchantId into outerJoinMerchant
                                  from marchant in outerJoinMerchant.DefaultIfEmpty()
                                  where c.ClientId == ClientId
                                        && (RoleID != (int)UserRoles.Staff || c.CreatedBy == clsSession.UserID)
                                        && (selectedSite == Guid.Empty || c.SiteId == selectedSite)
                                        && c.PaymentDate >= startDate && c.PaymentDate <= endDate
                                  select new MerchantPaymentVM
                                  {
                                      MerchantPaymentId = c.MerchantPaymentId,
                                      dtPaymentDate = c.PaymentDate,
                                      ChequeFor = c.ChequeFor,
                                      ChequeNo = c.ChequeNo,
                                      BankName = c.BankName,
                                      Amount = c.Amount,
                                      PaymentType = c.PaymentType,
                                      SiteId = c.SiteId,
                                      SiteName = sites.SiteName,
                                      MerchantName = (marchant != null ? marchant.FirmName : ""),
                                      Remarks = c.Remarks
                                  }).OrderByDescending(x => x.dtPaymentDate).ToList();

            return lstMerchantPayment;
        }

        public ActionResult Add()
        {
            MerchantPaymentVM objMerchantPayment = new MerchantPaymentVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            objMerchantPayment.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            objMerchantPayment.MerchantList = _db.tbl_Merchant.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.MerchantId.ToString(), Text = o.FirmName })
                         .OrderBy(x => x.Text).ToList();

            return View(objMerchantPayment);
        }

        [HttpPost]
        public ActionResult Add(MerchantPaymentVM merchantPayment)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            merchantPayment.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            merchantPayment.MerchantList = _db.tbl_Merchant.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.MerchantId.ToString(), Text = o.FirmName })
                         .OrderBy(x => x.Text).ToList();

            if (ModelState.IsValid)
            {
                try
                {

                    DateTime exp_date = DateTime.ParseExact(merchantPayment.PaymentDate, "dd/MM/yyyy", null);

                    tbl_MerchantPayment objMerchantPayment = new tbl_MerchantPayment();
                    objMerchantPayment.MerchantPaymentId = Guid.NewGuid();
                    objMerchantPayment.PaymentDate = exp_date;
                    objMerchantPayment.Amount = Convert.ToDecimal(merchantPayment.Amount);

                    if (merchantPayment.PaymentType == "Cheque")
                    {
                        objMerchantPayment.ChequeNo = merchantPayment.ChequeNo;
                        objMerchantPayment.BankName = merchantPayment.BankName;
                        objMerchantPayment.ChequeFor = merchantPayment.ChequeFor;
                    }
                    else
                    {
                        objMerchantPayment.ChequeNo = "";
                        objMerchantPayment.BankName = "";
                        objMerchantPayment.ChequeFor = "";
                    }

                    objMerchantPayment.SiteId = merchantPayment.SiteId;
                    objMerchantPayment.MerchantId = merchantPayment.MerchantId;
                    objMerchantPayment.PaymentType = merchantPayment.PaymentType;
                    objMerchantPayment.Remarks = merchantPayment.Remarks;
                    objMerchantPayment.ClientId = ClientId;
                    objMerchantPayment.CreatedBy = clsSession.UserID;
                    objMerchantPayment.CreatedDate = DateTime.UtcNow;
                    _db.tbl_MerchantPayment.Add(objMerchantPayment);
                    _db.SaveChanges();

                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
                }
                return RedirectToAction("Index");
            }

            return View(merchantPayment);
        }

        public ActionResult Edit(Guid Id)
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            MerchantPaymentVM objMerchantPayment = (from c in _db.tbl_MerchantPayment
                                                    where c.MerchantPaymentId == Id
                                                    select new MerchantPaymentVM
                                                    {
                                                        MerchantPaymentId = c.MerchantPaymentId,
                                                        dtPaymentDate = c.PaymentDate,
                                                        ChequeFor = c.ChequeFor,
                                                        ChequeNo = c.ChequeNo,
                                                        BankName = c.BankName,
                                                        Amount = c.Amount,
                                                        PaymentType = c.PaymentType,
                                                        MerchantId = c.MerchantId,
                                                        SiteId = c.SiteId,
                                                        Remarks = c.Remarks
                                                    }).FirstOrDefault();

            objMerchantPayment.PaymentDate = Convert.ToDateTime(objMerchantPayment.dtPaymentDate).ToString("dd/MM/yyyy");

            objMerchantPayment.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            objMerchantPayment.MerchantList = _db.tbl_Merchant.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.MerchantId.ToString(), Text = o.FirmName })
                         .OrderBy(x => x.Text).ToList();

            return View(objMerchantPayment);
        }

        [HttpPost]
        public ActionResult Edit(MerchantPaymentVM merchantPayment)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            merchantPayment.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            merchantPayment.MerchantList = _db.tbl_Merchant.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.MerchantId.ToString(), Text = o.FirmName })
                         .OrderBy(x => x.Text).ToList();

            if (ModelState.IsValid)
            {
                try
                {

                    DateTime exp_date = DateTime.ParseExact(merchantPayment.PaymentDate, "dd/MM/yyyy", null);

                    tbl_MerchantPayment objMerchantPayment = _db.tbl_MerchantPayment.Where(x => x.MerchantPaymentId == merchantPayment.MerchantPaymentId).FirstOrDefault();

                    objMerchantPayment.PaymentDate = exp_date;
                    objMerchantPayment.Amount = Convert.ToDecimal(merchantPayment.Amount);

                    if (merchantPayment.PaymentType == "Cheque")
                    {
                        objMerchantPayment.ChequeNo = merchantPayment.ChequeNo;
                        objMerchantPayment.BankName = merchantPayment.BankName;
                        objMerchantPayment.ChequeFor = merchantPayment.ChequeFor;
                    }
                    else
                    {
                        objMerchantPayment.ChequeNo = "";
                        objMerchantPayment.BankName = "";
                        objMerchantPayment.ChequeFor = "";
                    }

                    objMerchantPayment.SiteId = merchantPayment.SiteId;
                    objMerchantPayment.MerchantId = merchantPayment.MerchantId;
                    objMerchantPayment.PaymentType = merchantPayment.PaymentType;
                    objMerchantPayment.Remarks = merchantPayment.Remarks;
                    objMerchantPayment.ModifiedBy = clsSession.UserID;
                    objMerchantPayment.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();

                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
                }
                return RedirectToAction("Index");
            }

            return View(merchantPayment);
        }

        [HttpPost]
        public string DeleteMerchantPayment(Guid MerchantPaymentId)
        {
            string ReturnMessage = "";

            try
            {
                tbl_MerchantPayment objMerchantPayment = _db.tbl_MerchantPayment.Where(x => x.MerchantPaymentId == MerchantPaymentId).FirstOrDefault();

                if (objMerchantPayment == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    _db.tbl_MerchantPayment.Remove(objMerchantPayment);
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

        public string ExportPDFOfMerchantPayment(string start, string end, string site)
        {

            string Result = "";
            try
            {
                DateTime start_date = DateTime.ParseExact(start, "dd/MM/yyyy", null);
                DateTime end_date = DateTime.ParseExact(end, "dd/MM/yyyy", null);

                List<MerchantPaymentVM> lstMerchantPayment = GetMerchantPaymentListByFilter(start_date, end_date, site);


                string[] strColumns = new string[9] { "Payment Date", "Merchant Name", "Site Name", "Amount", "Payment Type", "Cheque No", "Bank Name", "Cheque For", "Remarks" };
                if (lstMerchantPayment != null && lstMerchantPayment.Count() > 0)
                {
                    decimal totalAmount = lstMerchantPayment.Sum(x => x.Amount.Value);

                    List<DateTime> lstDateTemp = new List<DateTime>();
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<!DOCTYPE html>");
                    strHTML.Append("<style>");
                    strHTML.Append("@page {@bottom-center {content: \"Page \" counter(page) \" of \" counter(pages);}}");
                    strHTML.Append("</style>");

                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<thead style=\"display:table-header-group;\">");
                    string Title = "Merchant Payment List";
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
                    foreach (var obj in lstMerchantPayment)
                    {

                        if (obj != null)
                        {

                            strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                            for (int Col = 0; Col < strColumns.Length; Col++)
                            {
                                string strcolval = "";
                                switch (strColumns[Col])
                                {

                                    case "Payment Date":
                                        {
                                            strcolval = Convert.ToDateTime(obj.dtPaymentDate).ToString("dd/MM/yyyy");
                                            break;
                                        }
                                    case "Amount":
                                        {
                                            strcolval = obj.Amount.ToString();
                                            break;
                                        }
                                    case "Site Name":
                                        {
                                            strcolval = obj.SiteName;
                                            break;
                                        }
                                    case "Merchant Name":
                                        {
                                            strcolval = obj.MerchantName;
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
                    strHTML.Append("<th colspan='3' style='text-align:right; border: 1px solid #ccc;'>Total:</th>");
                    strHTML.Append("<th colspan='6' style='border: 1px solid #ccc;'>" + totalAmount + "</th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("</tbody>");
                    strHTML.Append("</table>");
                     
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
                    Response.AddHeader("content-disposition", "download;filename=Merchant Payment List" + ".pdf");
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