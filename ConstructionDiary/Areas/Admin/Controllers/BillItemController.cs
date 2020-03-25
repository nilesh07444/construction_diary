using ConstructionDiary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    public class BillItemController : MyBaseController
    {
        ConstructionDiaryEntities _db;

        public BillItemController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<BillItemVM> lstBillItem = (from bill in _db.tbl_BillItem
                                            join billtype in _db.tbl_BillItemType on bill.BillItemTypeId equals billtype.BillItemTypeId
                                            where bill.ClientId == ClientId && !bill.IsDeleted
                                            select new BillItemVM
                                            {
                                                BillItemId = bill.BillItemId,
                                                BillItemName = bill.BillItemName,
                                                BillItemTypeId = bill.BillItemTypeId,
                                                BillItemType = billtype.BillItemType,
                                                Remarks = bill.Remarks
                                            }).ToList();

            return View(lstBillItem);
        }
         
        public ActionResult Add()
        {
            BillItemVM bill = new BillItemVM();

            bill.BillItemTypeList = _db.tbl_BillItemType
                         .Select(o => new SelectListItem { Value = o.BillItemTypeId.ToString(), Text = o.BillItemType })
                         .ToList();

            return View(bill);
        }

        [HttpPost]
        public ActionResult Add(BillItemVM billitemVM)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            billitemVM.BillItemTypeList = _db.tbl_BillItemType
                         .Select(o => new SelectListItem { Value = o.BillItemTypeId.ToString(), Text = o.BillItemType })
                         .ToList();

            if (ModelState.IsValid)
            {
                try
                {
                      
                    tbl_BillItem objBillItem = new tbl_BillItem();
                    objBillItem.BillItemId = Guid.NewGuid();
                    objBillItem.BillItemName = billitemVM.BillItemName;
                    objBillItem.BillItemTypeId = billitemVM.BillItemTypeId;
                    objBillItem.Remarks = billitemVM.Remarks;

                    objBillItem.ClientId = ClientId;
                    objBillItem.IsActive = true;
                    objBillItem.IsDeleted = false;
                    objBillItem.CreatedBy = clsSession.UserID;
                    objBillItem.CreatedDate = DateTime.UtcNow;
                    _db.tbl_BillItem.Add(objBillItem);
                    _db.SaveChanges();
  
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
                }
                return RedirectToAction("Index");
            }

            return View(billitemVM);
        }

        public ActionResult Edit(Guid Id)
        {
            BillItemVM bill = (from b in _db.tbl_BillItem
                               where b.BillItemId == Id
                               select new BillItemVM
                               {
                                   BillItemId = b.BillItemId,
                                   BillItemName = b.BillItemName,
                                   BillItemTypeId = b.BillItemTypeId,
                                   Remarks = b.Remarks,
                                   IsActive = b.IsActive 
                               }).First();

            bill.BillItemTypeList = _db.tbl_BillItemType
                         .Select(o => new SelectListItem { Value = o.BillItemTypeId.ToString(), Text = o.BillItemType })
                         .ToList();

            return View(bill);
        }

        [HttpPost]
        public ActionResult Edit(BillItemVM billitemVM)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            billitemVM.BillItemTypeList = _db.tbl_BillItemType
                         .Select(o => new SelectListItem { Value = o.BillItemTypeId.ToString(), Text = o.BillItemType })
                         .ToList();

            if (ModelState.IsValid)
            {
                try
                {

                    tbl_BillItem objBillItem = _db.tbl_BillItem.Where(x=>x.BillItemId == billitemVM.BillItemId).First();
                    objBillItem.BillItemName = billitemVM.BillItemName;
                    objBillItem.BillItemTypeId = billitemVM.BillItemTypeId;
                    objBillItem.Remarks = billitemVM.Remarks; 
                    objBillItem.ModifiedBy = clsSession.UserID;
                    objBillItem.ModifiedDate = DateTime.UtcNow; 
                    _db.SaveChanges();

                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
                }
                return RedirectToAction("Index");
            }

            return View(billitemVM);
        }

        [HttpPost]
        public string DeleteBillItem(Guid BillItemId)
        {
            string ReturnMessage = "";

            try
            {
                tbl_BillItem objBillItem = _db.tbl_BillItem.Where(x => x.BillItemId == BillItemId).FirstOrDefault();

                if (objBillItem == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objBillItem.IsDeleted = true;
                    objBillItem.ModifiedDate = DateTime.UtcNow;
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