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
    public class ChallanController : MyBaseController
    {
        ConstructionDiaryEntities _db;
        public ChallanController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index(string duration, string start, string end, string site)
        {
            List<ChallanVM> lstChallan = new List<ChallanVM>();

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

                //if (RoleID != (int)UserRoles.Staff)
                //{
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
                //}

                ViewBag.reportStartDate = startDate.ToString("dd/MM/yyyy");
                ViewBag.reportEndDate = endDate.ToString("dd/MM/yyyy");

                lstChallan = GetChallanListByFilter(startDate, endDate, site);

                List<SelectListItem> lstSites = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();
                ViewBag.SiteList = lstSites;

            }
            catch (Exception ex)
            { 
            }

            return View(lstChallan);
        }

        public List<ChallanVM> GetChallanListByFilter(DateTime startDate, DateTime endDate, string site)
        {
            List<ChallanVM> lstChallan = new List<ChallanVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            int RoleID = clsSession.RoleID;

            Guid selectedSite = (site != null && !string.IsNullOrEmpty(site) ? new Guid(site) : Guid.Empty);

            lstChallan = (from c in _db.tbl_Challan
                           join sites in _db.tbl_Sites on c.SiteId equals sites.SiteId
                           join marchant in _db.tbl_Merchant on c.MerchantId equals marchant.MerchantId into outerJoinMerchant
                           from marchant in outerJoinMerchant.DefaultIfEmpty()
                           where !c.IsDeleted && c.ClientId == ClientId
                                 //&& (RoleID != (int)UserRoles.Staff || c.CreatedBy == clsSession.UserID)
                                 && (selectedSite == Guid.Empty || c.SiteId == selectedSite)
                                 && c.ChallanDate >= startDate && c.ChallanDate <= endDate
                           select new ChallanVM
                           {
                               ChallanId = c.ChallanId,
                               dtChallanDate = c.ChallanDate,
                               ChallanNo = c.ChallanNo,
                               CarNo = c.CarNo,
                               SiteId = c.SiteId,
                               SiteName = sites.SiteName,
                               MerchantName = (marchant != null ? marchant.FirmName : ""),
                               ChallanPhoto = c.ChallanPhoto,
                               Remarks = c.Remarks,
                               ObjFile = _db.tbl_Files.Where(x => x.ParentId == c.ChallanId && x.FileCategory == (int)FileType.Challan).FirstOrDefault()
                           }).OrderByDescending(x => x.dtChallanDate).ToList();

            return lstChallan;
        }

        public ActionResult Add()
        {
            ChallanVM objChallan = new ChallanVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            objChallan.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            objChallan.MerchantList = _db.tbl_Merchant.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.MerchantId.ToString(), Text = o.FirmName })
                         .OrderBy(x => x.Text).ToList();

            return View(objChallan);
        }

        [HttpPost]
        public ActionResult Add(ChallanVM challan, HttpPostedFileBase ChallanPhotoFile)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            challan.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            challan.MerchantList = _db.tbl_Merchant.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.MerchantId.ToString(), Text = o.FirmName })
                         .OrderBy(x => x.Text).ToList();

            if (ModelState.IsValid)
            {
                try
                {

                    DateTime exp_date = DateTime.ParseExact(challan.ChallanDate, "dd/MM/yyyy", null);

                    tbl_Challan objChallan = new tbl_Challan();
                    objChallan.ChallanId = Guid.NewGuid();
                    objChallan.ChallanDate = exp_date;
                    objChallan.ChallanNo = challan.ChallanNo;
                    objChallan.SiteId = challan.SiteId;
                    objChallan.MerchantId = challan.MerchantId;
                    objChallan.CarNo = challan.CarNo;
                    objChallan.Remarks = challan.Remarks;

                    objChallan.ClientId = ClientId;
                    objChallan.IsActive = true;
                    objChallan.IsDeleted = false;
                    objChallan.CreatedBy = clsSession.UserID;
                    objChallan.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Challan.Add(objChallan);
                    _db.SaveChanges();

                    // Save + Upload Expense File
                    string fileName = string.Empty;
                    string path = Server.MapPath("~/DataFiles/ChallanFile/");
                    if (ChallanPhotoFile != null)
                    {
                        fileName = Guid.NewGuid() + "-" + Path.GetFileName(ChallanPhotoFile.FileName);

                        string full_path = Path.Combine(path, fileName);
                        ChallanPhotoFile.SaveAs(full_path);

                        tbl_Files objFile = new tbl_Files();
                        objFile.FileId = Guid.NewGuid();
                        objFile.ParentId = objChallan.ChallanId;
                        objFile.FileCategory = (int)FileType.Challan;
                        objFile.OriginalFileName = ChallanPhotoFile.FileName;
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

            return View(challan);
        }

        public ActionResult Edit(Guid Id)
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            ChallanVM objChallan = (from c in _db.tbl_Challan
                                    where c.ChallanId == Id
                                    select new ChallanVM
                                    {
                                        ChallanId = c.ChallanId,
                                        ChallanNo = c.ChallanNo,
                                        dtChallanDate = c.ChallanDate,
                                        CarNo = c.CarNo,
                                        MerchantId = c.MerchantId,
                                        SiteId = c.SiteId,
                                        Remarks = c.Remarks
                                    }).FirstOrDefault();

            objChallan.ChallanDate = Convert.ToDateTime(objChallan.dtChallanDate).ToString("dd/MM/yyyy");

            objChallan.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            objChallan.MerchantList = _db.tbl_Merchant.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.MerchantId.ToString(), Text = o.FirmName })
                         .OrderBy(x => x.Text).ToList();

            return View(objChallan);
        }

        [HttpPost]
        public ActionResult Edit(ChallanVM challan, HttpPostedFileBase ChallanPhotoFile)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            challan.SiteList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            challan.MerchantList = _db.tbl_Merchant.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.MerchantId.ToString(), Text = o.FirmName })
                         .OrderBy(x => x.Text).ToList();

            if (ModelState.IsValid)
            {
                try
                {

                    DateTime exp_date = DateTime.ParseExact(challan.ChallanDate, "dd/MM/yyyy", null);

                    tbl_Challan objChallan = _db.tbl_Challan.Where(x => x.ChallanId == challan.ChallanId).FirstOrDefault();

                    objChallan.ChallanDate = exp_date;
                    objChallan.ChallanNo = challan.ChallanNo;
                    objChallan.SiteId = challan.SiteId;
                    objChallan.MerchantId = challan.MerchantId;
                    objChallan.CarNo = challan.CarNo;
                    objChallan.Remarks = challan.Remarks;
                      
                    objChallan.ModifiedBy = clsSession.UserID;
                    objChallan.ModifiedDate = DateTime.UtcNow; 
                    _db.SaveChanges();

                    // Save + Upload File
                    string fileName = string.Empty;
                    string path = Server.MapPath("~/DataFiles/ChallanFile/");
                    if (ChallanPhotoFile != null)
                    {
                        fileName = Guid.NewGuid() + "-" + Path.GetFileName(ChallanPhotoFile.FileName);

                        string full_path = Path.Combine(path, fileName);
                        ChallanPhotoFile.SaveAs(full_path);

                        tbl_Files objFile = _db.tbl_Files.Where(x => x.ParentId == objChallan.ChallanId).FirstOrDefault();
                        if (objFile != null)
                        { 
                            objFile.OriginalFileName = ChallanPhotoFile.FileName;
                            objFile.EncryptFileName = fileName;                         
                            _db.SaveChanges();
                        }
                    }

                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
                }
                return RedirectToAction("Index");
            }

            return View(challan);
        }

        [HttpPost]
        public string DeleteChallan(Guid ChallanId)
        {
            string ReturnMessage = "";

            try
            {
                tbl_Challan objChallan = _db.tbl_Challan.Where(x => x.ChallanId == ChallanId && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objChallan == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objChallan.IsDeleted = true;
                    objChallan.ModifiedDate = DateTime.UtcNow;
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

        public string ExportPDFOfChallan(string start, string end, string site)
        {

            string Result = "";
            try
            {
                DateTime start_date = DateTime.ParseExact(start, "dd/MM/yyyy", null);
                DateTime end_date = DateTime.ParseExact(end, "dd/MM/yyyy", null);

                List<ChallanVM> lstChallan = GetChallanListByFilter(start_date, end_date, site);

                string[] strColumns = new string[6] { "Challan Date", "Challan No", "Site Name", "Merchant Name", "Car No" , "Remarks" };
                if (lstChallan != null && lstChallan.Count() > 0)
                {

                    List<DateTime> lstDateTemp = new List<DateTime>();
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<!DOCTYPE html>");
                    strHTML.Append("<style>");
                    strHTML.Append("@page {@bottom-center {content: \"Page \" counter(page) \" of \" counter(pages);}}");
                    strHTML.Append("</style>");

                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<thead style=\"display:table-header-group;\">");
                    string Title = "Challan List";
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
                    foreach (var obj in lstChallan)
                    {

                        if (obj != null)
                        {

                            strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                            for (int Col = 0; Col < strColumns.Length; Col++)
                            {
                                string strcolval = "";
                                switch (strColumns[Col])
                                {

                                    case "Challan Date":
                                        {
                                            strcolval = Convert.ToDateTime(obj.dtChallanDate).ToString("dd/MM/yyyy");
                                            break;
                                        }
                                    case "Challan No":
                                        {
                                            strcolval = obj.ChallanNo;
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
                                    case "Car No":
                                        {
                                            strcolval = obj.CarNo;
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
                    strHTML.Append("<th style='text-align:right; border: 1px solid #ccc;'></th>");
                    strHTML.Append("<th style='border: 1px solid #ccc;'> </th>");
                    strHTML.Append("<th colspan='4' style='border: 1px solid #ccc;'></th>");
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
                    Response.AddHeader("content-disposition", "download;filename=Challan List" + ".pdf");
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