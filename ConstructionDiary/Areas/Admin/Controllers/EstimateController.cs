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
    public class EstimateController : MyBaseController
    {
        ConstructionDiaryEntities _db;
        public EstimateController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            List<EstimateVM> lstEstimates = new List<EstimateVM>();

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                lstEstimates = (from e in _db.tbl_Estimate
                                where e.ClientId == ClientId && !e.IsDeleted
                                orderby e.EstimateId
                                select new EstimateVM
                                {
                                    EstimateId = e.EstimateId,
                                    EstimateDate = e.EstimateDate,
                                    PartyName = e.PartyName,
                                    TotalAmount = e.TotalAmount,
                                    Remarks = e.Remarks,
                                    CreatedDate = e.CreatedDate
                                }).OrderByDescending(x => x.EstimateDate).ThenByDescending(x => x.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(lstEstimates);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit(Guid Id)
        {
            EstimateVM objEstimate = new EstimateVM();

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                objEstimate = (from e in _db.tbl_Estimate
                               where e.EstimateId == Id
                               orderby e.EstimateId
                               select new EstimateVM
                               {
                                   EstimateId = e.EstimateId,
                                   EstimateDate = e.EstimateDate,
                                   PartyName = e.PartyName,
                                   TotalAmount = e.TotalAmount,
                                   Remarks = e.Remarks
                               }).FirstOrDefault();

                if (objEstimate != null)
                {
                    objEstimate.EstimateItem = (from i in _db.tbl_EstimateItem
                                                where i.EstimateId == objEstimate.EstimateId
                                                select new EstimateItemVM
                                                {
                                                    EstimateItemId = i.EstimateItemId,
                                                    ItemName = i.ItemName,
                                                    Nos = i.Nos,
                                                    Qty = i.Qty,
                                                    Rate = i.Rate,
                                                    TotalAmount = i.TotalAmount,
                                                    CreatedDate = i.CreatedDate
                                                }).OrderBy(x => x.CreatedDate).ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(objEstimate);
        }

        [HttpPost]
        public ActionResult SaveEstimate(string EstimateData)
        {
            GeneralResponse response = new GeneralResponse();

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Guid ClientId = new Guid(clsSession.ClientID.ToString());

                    EstimateVM objEstimateInfo = JsonConvert.DeserializeObject<EstimateVM>(EstimateData);

                    DateTime estimate_date = DateTime.ParseExact(objEstimateInfo.strEstimateDate, "dd/MM/yyyy", null);

                    decimal GrandTotalAmt = CalculateGrandTotal(objEstimateInfo.EstimateItem);

                    // Save data in Estimate
                    tbl_Estimate objEstimate = new tbl_Estimate();
                    objEstimate.EstimateId = Guid.NewGuid();
                    objEstimate.EstimateDate = estimate_date;
                    objEstimate.PartyName = objEstimateInfo.PartyName;
                    objEstimate.TotalAmount = GrandTotalAmt;
                    objEstimate.Remarks = objEstimateInfo.Remarks;
                    objEstimate.ClientId = ClientId;
                    objEstimate.IsActive = true;
                    objEstimate.IsDeleted = false;
                    objEstimate.CreatedBy = clsSession.UserID;
                    objEstimate.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Estimate.Add(objEstimate);
                    _db.SaveChanges();

                    // Save data in Estimate Item
                    objEstimateInfo.EstimateItem.ForEach(item =>
                    {

                        decimal? TotalAmt = item.Nos * item.Qty * item.Rate;

                        tbl_EstimateItem objItem = new tbl_EstimateItem();
                        objItem.EstimateItemId = Guid.NewGuid();
                        objItem.EstimateId = objEstimate.EstimateId;
                        objItem.ItemName = item.ItemName;
                        objItem.Nos = item.Nos;
                        objItem.Qty = item.Qty;
                        objItem.Rate = item.Rate;
                        objItem.TotalAmount = Convert.ToDecimal(TotalAmt);
                        objItem.CreatedDate = DateTime.UtcNow;
                        _db.tbl_EstimateItem.Add(objItem);
                        _db.SaveChanges();
                    });

                    response.IsError = false;
                    response.RedirectUrl = Url.Action("Index", "Estimate");
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
        public ActionResult UpdateEstimate(string EstimateData)
        {
            GeneralResponse response = new GeneralResponse();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    EstimateVM objEstimateInfo = JsonConvert.DeserializeObject<EstimateVM>(EstimateData);

                    if (objEstimateInfo.EstimateItem != null)
                    {
                        List<Guid> lstItemIds = objEstimateInfo.EstimateItem.Select(x => x.EstimateItemId.Value).ToList();

                        List<tbl_EstimateItem> EstimateItemToDelete = (from i in _db.tbl_EstimateItem
                                                                       where !lstItemIds.Contains(i.EstimateItemId)
                                                                       select i).ToList();

                        if (EstimateItemToDelete != null && EstimateItemToDelete.Count > 0)
                        {
                            _db.tbl_EstimateItem.RemoveRange(EstimateItemToDelete);
                            _db.SaveChanges();
                        }

                    }

                    DateTime estimate_date = DateTime.ParseExact(objEstimateInfo.strEstimateDate, "dd/MM/yyyy", null);

                    decimal GrandTotalAmt = CalculateGrandTotal(objEstimateInfo.EstimateItem);

                    // Save data in Estimate
                    tbl_Estimate objEstimate = _db.tbl_Estimate.Where(x => x.EstimateId == objEstimateInfo.EstimateId).FirstOrDefault();
                    objEstimate.EstimateDate = estimate_date;
                    objEstimate.PartyName = objEstimateInfo.PartyName;
                    objEstimate.TotalAmount = GrandTotalAmt;
                    objEstimate.Remarks = objEstimateInfo.Remarks;
                    objEstimate.ModifiedBy = clsSession.UserID;
                    objEstimate.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();

                    // Save data in Estimate Item
                    objEstimateInfo.EstimateItem.ForEach(item =>
                    {
                        decimal? TotalAmt = item.Nos * item.Qty * item.Rate;

                        tbl_EstimateItem objEstimateItem = _db.tbl_EstimateItem.Where(x => x.EstimateItemId == item.EstimateItemId).FirstOrDefault();
                        if (objEstimateItem == null)
                        {
                            // Add
                            tbl_EstimateItem objItem = new tbl_EstimateItem();
                            objItem.EstimateItemId = Guid.NewGuid();
                            objItem.EstimateId = objEstimate.EstimateId;
                            objItem.ItemName = item.ItemName;
                            objItem.Nos = item.Nos;
                            objItem.Qty = item.Qty;
                            objItem.Rate = item.Rate;
                            objItem.TotalAmount = Convert.ToDecimal(TotalAmt);
                            objItem.CreatedDate = DateTime.UtcNow;
                            _db.tbl_EstimateItem.Add(objItem);
                            _db.SaveChanges();
                        }
                        else
                        {
                            // Update  
                            objEstimateItem.ItemName = item.ItemName;
                            objEstimateItem.Nos = item.Nos;
                            objEstimateItem.Qty = item.Qty;
                            objEstimateItem.Rate = item.Rate;
                            objEstimateItem.TotalAmount = Convert.ToDecimal(TotalAmt);
                            _db.SaveChanges();
                        }

                    });

                    response.IsError = false;
                    response.RedirectUrl = Url.Action("Index", "Estimate");
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

        private decimal CalculateGrandTotal(List<EstimateItemVM> lstItem)
        {
            decimal grandTotal = 0;

            lstItem.ForEach(item =>
            {
                decimal totalAmt = Convert.ToDecimal(item.Nos) * Convert.ToDecimal(item.Qty) * Convert.ToDecimal(item.Rate);
                grandTotal += totalAmt;

            });

            return grandTotal;
        }

        public string ExportPDFOfEstimate(Guid Id, bool IsLetterRequired) // Id = EstimateId
        {

            string Result = "";
            try
            {
                // Get Customer Name
                Guid ClientId = new Guid(clsSession.ClientID.ToString());
                tbl_Clients objClient = _db.tbl_Clients.Where(x => x.ClientId == ClientId).FirstOrDefault();

                tbl_Estimate objEstimate = _db.tbl_Estimate.Where(x => x.EstimateId == Id).FirstOrDefault();

                List<EstimateItemVM> lstEstimateItem = (from i in _db.tbl_EstimateItem
                                                        orderby i.CreatedDate
                                                        where i.EstimateId == Id
                                                        select new EstimateItemVM
                                                        {
                                                            EstimateItemId = i.EstimateItemId,
                                                            ItemName = i.ItemName,
                                                            Nos = i.Nos,
                                                            Qty = i.Qty,
                                                            Rate = i.Rate,
                                                            TotalAmount = i.TotalAmount,
                                                            CreatedDate = i.CreatedDate
                                                        }).OrderBy(x => x.CreatedDate).ToList();

                string[] strColumns = new string[6] { "Sr No", "Item Description", "Nos", "Qty", "Rate", "Amount" };
                if (lstEstimateItem != null && lstEstimateItem.Count() > 0)
                {

                    List<DateTime> lstDateTemp = new List<DateTime>();
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<!DOCTYPE html>");
                    strHTML.Append("<style>");
                    strHTML.Append("@page {@bottom-center {content: \"Page \" counter(page) \" of \" counter(pages);}}");
                    strHTML.Append("</style>");

                    if (IsLetterRequired && !string.IsNullOrEmpty(objClient.HeaderImageLetterPage))
                    {
                        //string headerImage = Server.MapPath("~/Images/LetterHead/" + objClient.HeaderImageLetterPage);
                        string headerImage = System.Web.Hosting.HostingEnvironment.MapPath("~\\Images\\LetterHead\\" + objClient.HeaderImageLetterPage);

                        strHTML.Append(" <table width=100% cellspacing=0 cellpadding=2 class=table1>");
                        strHTML.Append("<tr><th> <img src='" + headerImage + "' style='width:740px; height: 120px; text-align: center;' /> </th></tr>");
                        strHTML.Append("<tr><td>");
                    }

                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #000000;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<thead style=\"display:table-header-group;\">");

                    strHTML.Append("<tr>");
                    strHTML.Append("<th colspan=\"" + strColumns.Length + "\" style='border: 1px solid #000000; text-align:center; background-color: #DCDCDC;'>");
                    strHTML.Append("ESTIMATE");
                    strHTML.Append("</th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr>");
                    strHTML.Append("<th colspan=\"" + strColumns.Length + "\" style='border: 1px solid #000000; text-align:center; height: 30px;'>");
                    strHTML.Append("");
                    strHTML.Append("</th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr>");
                    strHTML.Append("<th colspan=\"" + strColumns.Length + "\" style='border: 1px solid #000000; text-align: center;'>");
                    strHTML.Append(objEstimate.PartyName);
                    strHTML.Append("</th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='height: 120px; vertical-align: top;'>");
                    strHTML.Append("<th colspan='4'>From: " + objClient.ClientName + " <br /> " + objClient.Address + " </th>");
                    strHTML.Append("<th colspan='2'>Date: " + objEstimate.EstimateDate.ToString("dd/MM/yyyy") + "</th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr>");
                    for (int idx = 0; idx < strColumns.Length; idx++)
                    {
                        strHTML.Append("<th style=\"border: 1px solid #000000\">");
                        strHTML.Append(strColumns[idx]);
                        strHTML.Append("</th>");
                    }
                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody>");

                    int counter = 1;
                    foreach (var obj in lstEstimateItem)
                    {

                        if (obj != null)
                        {

                            strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                            for (int Col = 0; Col < strColumns.Length; Col++)
                            {
                                #region Set column width
                                string colWidth = "";
                                if (strColumns[Col] == "Sr No")
                                {
                                    colWidth = "6%";
                                }
                                else if (strColumns[Col] == "Nos")
                                {
                                    colWidth = "10%";
                                }
                                else if (strColumns[Col] == "Qty")
                                {
                                    colWidth = "10%";
                                }
                                else if (strColumns[Col] == "Rate")
                                {
                                    colWidth = "10%";
                                }
                                else if (strColumns[Col] == "Amount")
                                {
                                    colWidth = "15%";
                                }
                                else
                                {
                                    colWidth = "auto";
                                }
                                #endregion

                                string strcolval = "";
                                switch (strColumns[Col])
                                {
                                    case "Sr No":
                                        {
                                            strcolval = counter.ToString() + ".";
                                            break;
                                        }
                                    case "Item Description":
                                        {
                                            strcolval = obj.ItemName;
                                            break;
                                        }
                                    case "Nos":
                                        {
                                            strcolval = obj.Nos.ToString();
                                            break;
                                        }
                                    case "Qty":
                                        {
                                            strcolval = obj.Qty.ToString();
                                            break;
                                        }
                                    case "Rate":
                                        {
                                            strcolval = obj.Rate.ToString();
                                            break;
                                        }
                                    case "Amount":
                                        {
                                            strcolval = obj.TotalAmount.ToString();
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }

                                }
                                strHTML.Append("<td style='width: " + colWidth + "; border: 1px solid #000000;'>");
                                strHTML.Append(strcolval);
                                strHTML.Append("</td>");
                            }
                            strHTML.Append("</tr>");
                        }

                        counter++;
                    }

                    // Total
                    strHTML.Append("<tr>");
                    strHTML.Append("<th colspan='5' style='text-align:right; border: 1px solid #000000;'>Total</th>");
                    strHTML.Append("<th style='border: 1px solid #000000;'>" + lstEstimateItem.Sum(x => x.TotalAmount) + "</th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr>");
                    strHTML.Append("<th colspan='6' style='border: 1px solid #000000;'>Note:</th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='height:80px;'>");
                    strHTML.Append("<th colspan='4' style='border: 1px solid #000000; border-right::none;'>Receiver's Signature <br /><br /> _________________</th>");
                    strHTML.Append("<th colspan='2' style='border: 1px solid #000000; float:right; text-align:right;'>For ESTIMATE<br /><br /> <span style='font-size:10px;font-style: italic;'>(Authorized Signature)</span></th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("</tbody>");
                    strHTML.Append("</table>");

                    if (IsLetterRequired && !string.IsNullOrEmpty(objClient.HeaderImageLetterPage))
                    {
                        strHTML.Append("</td></tr></table>");
                    }

                    StringReader sr = new StringReader(strHTML.ToString());

                    var myString = strHTML.ToString();
                    var myByteArray = System.Text.Encoding.UTF8.GetBytes(myString);
                    var ms = new MemoryStream(myByteArray);

                    Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    writer.PageEvent = new PDFGeneratePageEventHelper();
                    pdfDoc.Open();

                    XMLWorkerHelper objHelp = XMLWorkerHelper.GetInstance();
                    objHelp.ParseXHtml(writer, pdfDoc, ms, null, Encoding.UTF8, new UnicodeFontFactory());

                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "download;filename=Estimate Of " + objEstimate.PartyName + ".pdf");
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

        [HttpPost]
        public string DeleteEstimate(Guid EstimateId)
        {
            string ReturnMessage = "";

            try
            {

                tbl_Estimate objEstimate = _db.tbl_Estimate.Where(x => x.EstimateId == EstimateId && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objEstimate == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    List<tbl_EstimateItem> lstItems = _db.tbl_EstimateItem.Where(x => x.EstimateId == EstimateId).ToList();
                    if (lstItems.Count > 0)
                    {
                        _db.tbl_EstimateItem.RemoveRange(lstItems);
                        _db.SaveChanges();
                    }

                    // hard delete
                    _db.tbl_Estimate.Remove(objEstimate);
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

    }
}