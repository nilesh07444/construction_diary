using ConstructionDiary.Helper;
using ConstructionDiary.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class SettingController : MyBaseController
    {
        // GET: Admin/Setting
        public ActionResult Index()
        {
            ViewBag.SelectedLanguage = Request.Cookies["culture"].Value;
            return View();
        }
        public ActionResult ChangeLanguage(string lang)
        {
            new LanguageMang().SetLanguage(lang);
            return RedirectToAction("Index");
        }

        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public string DownloadPDF(ChallanVM challan, HttpPostedFileBase ChallanPhotoFile)
        {
            List<ExportPdfVM> lstExcelData = new List<ExportPdfVM>();
            string Result = "";
            try
            {
                string filePath = string.Empty;
                OleDbConnection con = new OleDbConnection();
                OleDbDataAdapter da;
                DataSet ds = new DataSet();

                string ext = Path.GetExtension(ChallanPhotoFile.FileName);
                  
                string path = Server.MapPath("~/DataFiles/UploadsExcel/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string extension = Path.GetExtension(ChallanPhotoFile.FileName);
                string TempFlName = Path.GetFileNameWithoutExtension(ChallanPhotoFile.FileName) + "_" + DateTime.Now.Ticks;
                filePath = path + TempFlName + extension;
                ChallanPhotoFile.SaveAs(filePath);

                string conString = string.Empty;

                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES";
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = "Provider=Microsoft.ACE.OLEDB.12.0;data source=" + filePath + ";Extended Properties=Excel 12.0";
                        break;
                }

                con = new OleDbConnection(conString);
                con.Open();

                DataTable dt = con.GetSchema("Tables");
                string firstSheetName = dt.Rows[0]["TABLE_NAME"].ToString();
                da = new OleDbDataAdapter("select * from [" + firstSheetName + "]", con);
                da.Fill(ds);

                try
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ExportPdfVM objExcelData = new ExportPdfVM();

                        string SR = dr["SR"].ToString();
                        string NAME = dr["NAME"].ToString();
                        string CONTACT = dr["CONTACT"].ToString();
                        string ADDRESS = dr["ADDRESS"].ToString();
                        string CITY = dr["CITY"].ToString();

                        objExcelData.Sr = SR;
                        objExcelData.Name = NAME;
                        objExcelData.Address = ADDRESS;
                        objExcelData.Contact = CONTACT;
                        objExcelData.City = CITY;

                        lstExcelData.Add(objExcelData);

                    }
                }
                catch (Exception ex)
                {
                }

                string[] strColumns = new string[4] { "અનુ નં.", "નામ", "સંપર્ક સૂત્ર", "સરનામું" };
                if (lstExcelData != null && lstExcelData.Count() > 0)
                {

                    List<DateTime> lstDateTemp = new List<DateTime>();
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<!DOCTYPE html>");
                    strHTML.Append("<style>");
                    strHTML.Append("@page {@bottom-center {content: \"Page \" counter(page) \" of \" counter(pages);}}");
                    strHTML.Append("</style>");

                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<thead style=\"display:table-header-group;\">");
                    string Title = "Adisara Parivar";
                    strHTML.Append("<tr>");
                    strHTML.Append("<th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #ccc\">");
                    strHTML.Append(Title);
                    strHTML.Append("</th>");
                    strHTML.Append("</tr>");
                    //strHTML.Append("<tr><th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #ccc\">From " + start_date.ToString("dd/MM/yyyy") + " To " + end_date.ToString("dd/MM/yyyy") + " </th></tr>");
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
                    foreach (var obj in lstExcelData)
                    {

                        if (obj != null)
                        {

                            strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                            for (int Col = 0; Col < strColumns.Length; Col++)
                            {
                                string strcolval = "";
                                switch (strColumns[Col])
                                {

                                    case "અનુ નં.":
                                        {
                                            strcolval = obj.Sr;
                                            break;
                                        }
                                    case "નામ":
                                        {
                                            strcolval = obj.Name;
                                            break;
                                        }
                                    case "સંપર્ક સૂત્ર":
                                        {
                                            strcolval = obj.Contact;
                                            break;
                                        }
                                    case "સરનામું":
                                        {
                                            strcolval = obj.Address;
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
                    //strHTML.Append("<th style='border: 1px solid #ccc;'> </th>");
                    strHTML.Append("<th colspan='4' style='border: 1px solid #ccc;'></th>");
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
                    Response.AddHeader("content-disposition", "download;filename=Adisara Parivar List" + ".pdf");
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