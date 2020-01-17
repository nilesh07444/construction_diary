using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;
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

        public ActionResult Index()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            List<MaterialPurchaseVM> lstPurchase = (from mat in _db.tbl_MaterialPurchase
                                                    join site in _db.tbl_Sites on mat.SiteId equals site.SiteId
                                                    join marchant in _db.tbl_Merchant on mat.MerchantId equals marchant.MerchantId into outerJoinMerchant
                                                    from marchant in outerJoinMerchant.DefaultIfEmpty()
                                                    where !mat.IsDeleted && mat.IsActive && mat.ClientId == ClientId
                                                    select new MaterialPurchaseVM
                                                    {
                                                        MaterialPurchaseId = mat.MaterialPurchaseId,
                                                        dtPurchaseDate = mat.PurchaseDate, 
                                                        SiteId = mat.SiteId,
                                                        Total = mat.Total,
                                                        GST_Per = mat.GST_Per,
                                                        SiteName = site.SiteName,
                                                        MerchantName = marchant.FirmName
                                                    }).ToList(); 
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

    }
}