using ConstructionDiary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class MerchantController : MyBaseController
    {
        private readonly ConstructionDiaryEntities _db;

        public MerchantController()
        {
            _db = new ConstructionDiaryEntities();
        }
        public ActionResult Index()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            List<MerchantVM> lstMerchant = (from c in _db.tbl_Merchant
                                             where !c.IsDeleted && c.ClientId == ClientId
                                            select new MerchantVM
                                             {
                                                 MerchantId = c.MerchantId,
                                                 MerchantName = c.MerchantName,
                                                 FirmName = c.FirmName,
                                                 MobileNo = c.MobileNo,
                                                 Address = c.Address, 
                                                 IsActive = c.IsActive
                                             }).OrderByDescending(x => x.MerchantId).ToList();
            return View(lstMerchant);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(MerchantVM merchant)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                try
                {
                    Guid ClientId = new Guid(clsSession.ClientID.ToString());
                      
                    tbl_Merchant objMerchant = new tbl_Merchant();
                    objMerchant.MerchantId = Guid.NewGuid();
                    objMerchant.MerchantName = merchant.MerchantName;
                    objMerchant.FirmName = merchant.FirmName;
                    objMerchant.MobileNo = merchant.MobileNo; 
                    objMerchant.Address = merchant.Address;
                    objMerchant.ClientId = ClientId;
                    objMerchant.IsActive = true;
                    objMerchant.IsDeleted = false;
                    objMerchant.CreatedBy = clsSession.UserID;
                    objMerchant.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Merchant.Add(objMerchant);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                }
                return RedirectToAction("Index");
            }

            return View(merchant);
        }

        public ActionResult Edit(Guid Id)
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            MerchantVM objMerchant = (from c in _db.tbl_Merchant
                                            where c.MerchantId == Id
                                            select new MerchantVM
                                            {
                                                MerchantId = c.MerchantId,
                                                MerchantName = c.MerchantName,
                                                FirmName = c.FirmName,
                                                MobileNo = c.MobileNo,
                                                Address = c.Address, 
                                                IsActive = c.IsActive
                                            }).FirstOrDefault();

            return View(objMerchant);
        }

        [HttpPost]
        public ActionResult Edit(MerchantVM merchant)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                try
                {
                      
                    tbl_Merchant objMerchant = _db.tbl_Merchant.Where(x => x.MerchantId == merchant.MerchantId).FirstOrDefault();
                    objMerchant.MerchantName = merchant.MerchantName;
                    objMerchant.FirmName = merchant.FirmName;
                    objMerchant.MobileNo = merchant.MobileNo; 
                    objMerchant.Address = merchant.Address; 
                    objMerchant.ModifiedBy = clsSession.UserID;
                    objMerchant.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                }
                return RedirectToAction("Index");
            }

            return View(merchant);
        }

        [HttpPost]
        public string DeleteMerchant(Guid MerchantId)
        {
            string ReturnMessage = "";

            try
            {
                tbl_Merchant objMechant = _db.tbl_Merchant.Where(x => x.MerchantId == MerchantId && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objMechant == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objMechant.IsDeleted = true;
                    objMechant.ModifiedDate = DateTime.UtcNow;
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


    }
}