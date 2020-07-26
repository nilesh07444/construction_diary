using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;

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

        public ActionResult Index()
        {
            List<ChallanVM> lstChallan = new List<ChallanVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            lstChallan = (from c in _db.tbl_Challan
                          join site in _db.tbl_Sites on c.SiteId equals site.SiteId
                          join marchant in _db.tbl_Merchant on c.MerchantId equals marchant.MerchantId into outerJoinMerchant
                          from marchant in outerJoinMerchant.DefaultIfEmpty()
                          where c.ClientId == ClientId && !c.IsDeleted
                          select new ChallanVM
                          {
                              ChallanId = c.ChallanId,
                              dtChallanDate = c.ChallanDate,
                              ChallanNo = c.ChallanNo,
                              CarNo = c.CarNo,
                              SiteId = c.SiteId,
                              SiteName = site.SiteName,
                              MerchantName = (marchant != null ? marchant.FirmName : ""),
                              ChallanPhoto = c.ChallanPhoto,
                              Remarks = c.Remarks,
                              ObjFile = _db.tbl_Files.Where(x => x.ParentId == c.ChallanId && x.FileCategory == (int)FileType.Challan).FirstOrDefault()
                          }).OrderByDescending(x => x.dtChallanDate).ToList();

            return View(lstChallan);
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

    }
}