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
using Newtonsoft.Json;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class MaterialController : MyBaseController
    {
        ConstructionDiaryEntities _db;
        public MaterialController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index(string duration, string start, string end)
        {
            List<MaterialPurchaseVM> lstPurchase = new List<MaterialPurchaseVM>();
            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                if (string.IsNullOrEmpty(duration))
                    duration = "month";


                //ViewBag.PersonName = GetPersonName(id);
                //ViewBag.PersonId = id;
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

                ViewBag.reportStartDate = startDate.ToString("dd/MM/yyyy");
                ViewBag.reportEndDate = endDate.ToString("dd/MM/yyyy");

                lstPurchase = GetMaterialListByFilter("", startDate, endDate);
                  
            }
            catch (Exception ex)
            { 
            }
             
            return View(lstPurchase);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SavePurchase(string purchase)
        {
            GeneralResponse response = new GeneralResponse();

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Guid ClientId = new Guid(clsSession.ClientID.ToString());

                    MaterialPurchaseVM purchaseData = JsonConvert.DeserializeObject<MaterialPurchaseVM>(purchase);
                    DateTime sale_date = DateTime.ParseExact(purchaseData.PurchaseDate, "dd/MM/yyyy", null);

                    // Save Sale
                    decimal subTotal = 0;
                    decimal cgstAmount = 0;
                    decimal sgstAmount = 0;
                    decimal grandAmount = 0;
                    decimal gstAmt = 0;

                    purchaseData.PurchaseItem.ForEach(x =>
                    {
                        subTotal += (x.Weight * x.Rate);
                    });

                    if (purchaseData.GST_Per != null)
                    {
                        decimal perGst = Convert.ToDecimal(purchaseData.GST_Per) / 2;
                        cgstAmount = (subTotal * perGst) / 100;
                        sgstAmount = (subTotal * perGst) / 100;
                        gstAmt = cgstAmount + sgstAmount;
                    }
                    grandAmount = subTotal + gstAmt;

                    if (purchaseData.AdjustmentAmount != null)
                    {
                        grandAmount += Convert.ToDecimal(purchaseData.AdjustmentAmount);
                    }

                    tbl_MaterialPurchase objPurchase = new tbl_MaterialPurchase();
                    objPurchase.MaterialPurchaseId = Guid.NewGuid();
                    objPurchase.PurchaseDate = sale_date;
                    objPurchase.SiteId = purchaseData.SiteId;
                    objPurchase.MerchantId = purchaseData.MerchantId;
                    objPurchase.ClientId = ClientId;
                    objPurchase.Remarks = purchaseData.Remarks;
                    objPurchase.GST_Per = Convert.ToInt32(purchaseData.GST_Per);
                    objPurchase.AdjustmentAmount = purchaseData.AdjustmentAmount;
                    objPurchase.CGST_Amount = cgstAmount;
                    objPurchase.SGST_Amount = sgstAmount;
                    objPurchase.SubTotal = subTotal;
                    objPurchase.Total = grandAmount;
                    objPurchase.IsActive = true;
                    objPurchase.IsDeleted = false;
                    objPurchase.CreatedBy = clsSession.UserID;
                    objPurchase.CreatedDate = DateTime.UtcNow;
                    objPurchase.ModifiedBy = clsSession.UserID;
                    objPurchase.ModifiedDate = DateTime.UtcNow;
                    _db.tbl_MaterialPurchase.Add(objPurchase);
                    _db.SaveChanges();

                    // Save PurchaseItem  
                    purchaseData.PurchaseItem.ForEach(x =>
                    {
                        tbl_MaterialPurchaseItems objPurchaseItem = new tbl_MaterialPurchaseItems();
                        objPurchaseItem.MaterialPurchaseItemId = Guid.NewGuid();
                        objPurchaseItem.MaterialPurchaseId = objPurchase.MaterialPurchaseId;
                        objPurchaseItem.MaterialTypeId = x.MaterialTypeId;
                        objPurchaseItem.Weight = x.Weight;
                        objPurchaseItem.Rate = x.Rate;
                        objPurchaseItem.Amount = (x.Weight * x.Rate);
                        _db.tbl_MaterialPurchaseItems.Add(objPurchaseItem);
                        _db.SaveChanges();
                    });

                    response.IsError = false;
                    response.RedirectUrl = Url.Action("Index", "Material");
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

        public ActionResult Edit(Guid Id)
        {
            MaterialPurchaseVM objPurchase = (from s in _db.tbl_MaterialPurchase
                                      where s.MaterialPurchaseId == Id
                                      select new MaterialPurchaseVM
                                      {
                                          MaterialPurchaseId = s.MaterialPurchaseId,
                                          dtPurchaseDate = s.PurchaseDate,
                                          SiteId = s.SiteId,
                                          MerchantId = s.MerchantId,
                                          SubTotal = s.SubTotal,
                                          GST_Per = s.GST_Per,
                                          CGST_Amount = s.CGST_Amount,
                                          SGST_Amount = s.SGST_Amount,
                                          Remarks = s.Remarks,
                                          AdjustmentAmount = s.AdjustmentAmount,
                                          Total = s.Total
                                      }).FirstOrDefault();

            objPurchase.PurchaseDate = objPurchase.dtPurchaseDate.ToString("dd/MM/yyyy");

            return View(objPurchase);
        }

        [HttpPost]
        public ActionResult EditPurchase(string purchase)
        {
            GeneralResponse response = new GeneralResponse();

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    MaterialPurchaseVM saleData = JsonConvert.DeserializeObject<MaterialPurchaseVM>(purchase);

                    tbl_MaterialPurchase objPurchase = _db.tbl_MaterialPurchase.Where(x => x.MaterialPurchaseId == saleData.MaterialPurchaseId).FirstOrDefault();
                    if (objPurchase != null)
                    {

                        DateTime sale_date = DateTime.ParseExact(saleData.PurchaseDate, "dd/MM/yyyy", null);

                        // Save Sale
                        decimal subTotal = 0;
                        decimal cgstAmount = 0;
                        decimal sgstAmount = 0;
                        decimal grandAmount = 0;
                        decimal gstAmt = 0;

                        saleData.PurchaseItem.ForEach(x =>
                        {
                            if (!x.IsDeleteItem)
                            {
                                subTotal += (x.Weight * x.Rate);
                            }
                        });

                        if (saleData.GST_Per != null)
                        {
                            decimal perGst = Convert.ToDecimal(saleData.GST_Per) / 2;
                            cgstAmount = (subTotal * perGst) / 100;
                            sgstAmount = (subTotal * perGst) / 100;
                            gstAmt = cgstAmount + sgstAmount;
                        }
                        grandAmount = subTotal + gstAmt;

                        if (saleData.AdjustmentAmount != null)
                        {
                            grandAmount += Convert.ToDecimal(saleData.AdjustmentAmount);
                        }

                        objPurchase.PurchaseDate = sale_date;
                        objPurchase.SiteId = saleData.SiteId;
                        objPurchase.MerchantId = saleData.MerchantId;
                        objPurchase.Remarks = saleData.Remarks;
                        objPurchase.GST_Per = Convert.ToInt32(saleData.GST_Per);
                        objPurchase.AdjustmentAmount = saleData.AdjustmentAmount;
                        objPurchase.CGST_Amount = cgstAmount;
                        objPurchase.SGST_Amount = sgstAmount;
                        objPurchase.SubTotal = subTotal;
                        objPurchase.Total = grandAmount;
                        objPurchase.ModifiedBy = clsSession.UserID;
                        objPurchase.ModifiedDate = DateTime.UtcNow;
                        _db.SaveChanges();

                        // Save SaleItem  
                        saleData.PurchaseItem.ForEach(x =>
                        {
                            if (x.IsDeleteItem && x.MaterialPurchaseItemId != null)
                            {
                                // delete 
                                tbl_MaterialPurchaseItems objDeleteItem = _db.tbl_MaterialPurchaseItems.Where(p => p.MaterialPurchaseItemId == x.MaterialPurchaseItemId).FirstOrDefault();
                                if (objDeleteItem != null)
                                {
                                    _db.tbl_MaterialPurchaseItems.Remove(objDeleteItem);
                                    _db.SaveChanges();
                                }
                            }
                            else
                            {
                                tbl_MaterialPurchaseItems objPurchaseItem = new tbl_MaterialPurchaseItems();
                                if (x.MaterialPurchaseItemId == null)
                                {
                                    objPurchaseItem.MaterialPurchaseItemId = Guid.NewGuid();
                                    objPurchaseItem.MaterialPurchaseId = objPurchase.MaterialPurchaseId;
                                    _db.tbl_MaterialPurchaseItems.Add(objPurchaseItem);
                                }
                                else
                                {
                                    objPurchaseItem = _db.tbl_MaterialPurchaseItems.FirstOrDefault(p => p.MaterialPurchaseItemId == x.MaterialPurchaseItemId);
                                }

                                objPurchaseItem.MaterialTypeId = x.MaterialTypeId;
                                objPurchaseItem.Weight = x.Weight;
                                objPurchaseItem.Rate = x.Rate;
                                objPurchaseItem.Amount = (x.Weight * x.Rate);
                                _db.SaveChanges();
                            }
                        });

                        response.IsError = false;
                        response.RedirectUrl = Url.Action("Index", "Material");
                    }

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
        public string DeleteMaterial(Guid MaterialId)
        {
            string ReturnMessage = "";

            try
            {
                tbl_MaterialPurchase objPurchase = _db.tbl_MaterialPurchase.Where(x => x.MaterialPurchaseId == MaterialId && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objPurchase == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    //objPurchase.IsDeleted = true;
                    //objPurchase.ModifiedDate = DateTime.UtcNow;

                    List<tbl_MaterialPurchaseItems> lstItems = _db.tbl_MaterialPurchaseItems.Where(x => x.MaterialPurchaseId == objPurchase.MaterialPurchaseId).ToList();
                    if(lstItems.Count > 0)
                    {
                        _db.tbl_MaterialPurchaseItems.RemoveRange(lstItems);
                    } 
                    _db.tbl_MaterialPurchase.Remove(objPurchase);
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
        
        public string ExportPDFOfMaterial(string start, string end)
        {

            string Result = "";
            try
            {
                DateTime start_date = DateTime.ParseExact(start, "dd/MM/yyyy", null);
                DateTime end_date = DateTime.ParseExact(end, "dd/MM/yyyy", null);

                List<MaterialPurchaseVM> lstMaterial = GetMaterialListByFilter("", start_date, end_date);

                decimal? TotalMaterialAmount = lstMaterial.Select(x => x.Total).Sum();
                string strTotalMaterialAmount = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalMaterialAmount));

                string[] strColumns = new string[5] { "Date", "Merchant Name", "Site Name", "GST", "Total" };
                if (lstMaterial != null && lstMaterial.Count() > 0)
                {

                    List<DateTime> lstDateTemp = new List<DateTime>();
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<!DOCTYPE html>");
                    strHTML.Append("<style>");
                    strHTML.Append("@page {@bottom-center {content: \"Page \" counter(page) \" of \" counter(pages);}}");
                    strHTML.Append("</style>");

                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<thead style=\"display:table-header-group;\">");
                    string Title = "Material List";
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
                    foreach (var obj in lstMaterial)
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
                                            strcolval = Convert.ToDateTime(obj.dtPurchaseDate).ToString("dd/MM/yyyy");
                                            break;
                                        }
                                    case "Merchant Name":
                                        {
                                            strcolval = obj.MerchantName;
                                            break;
                                        }
                                    case "Site Name":
                                        {
                                            strcolval = obj.SiteName;
                                            break;
                                        }
                                    case "GST":
                                        {
                                            strcolval = obj.GST_Per.ToString() + "%";
                                            break;
                                        }
                                    case "Total":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.Total);
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
                    strHTML.Append("<th colspan='4' style='text-align:right; border: 1px solid #ccc;'>Total</th>");
                    strHTML.Append("<th style='border: 1px solid #ccc;'> " + strTotalMaterialAmount + " </th>");                    
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
                    Response.AddHeader("content-disposition", "download;filename=Material List" + ".pdf");
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
        
        public List<MaterialPurchaseVM> GetMaterialListByFilter(string duration, DateTime startDate, DateTime endDate)
        {
            List<MaterialPurchaseVM> lstMaterial = new List<MaterialPurchaseVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString()); 

            lstMaterial = (from mat in _db.tbl_MaterialPurchase
                           join site in _db.tbl_Sites on mat.SiteId equals site.SiteId
                           join marchant in _db.tbl_Merchant on mat.MerchantId equals marchant.MerchantId into outerJoinMerchant
                           from marchant in outerJoinMerchant.DefaultIfEmpty()
                           where !mat.IsDeleted && mat.IsActive && mat.ClientId == ClientId
                                 && mat.PurchaseDate >= startDate && mat.PurchaseDate <= endDate
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

            return lstMaterial;
        }
        
    }
}