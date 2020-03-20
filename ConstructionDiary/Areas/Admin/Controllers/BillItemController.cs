using ConstructionDiary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    public class BillItemController : Controller
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


    }
}