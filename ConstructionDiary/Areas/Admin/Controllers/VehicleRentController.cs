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
    public class VehicleRentController : MyBaseController
    {
        private readonly ConstructionDiaryEntities _db;
        public VehicleRentController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            List<VehicleRentVM> lstVehicleRents = new List<VehicleRentVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            try
            {
                lstVehicleRents = (from rent in _db.tbl_VehicleRent
                                   join owner in _db.tbl_VehicleOwner on rent.VehicleOwnerId equals owner.VehicleOwnerId
                                   where rent.ClientId == ClientId
                                   select new VehicleRentVM
                                   {
                                       VehicleRentId = rent.VehicleRentId,
                                       dtVehicleRentDate = rent.VehicleRentDate,
                                       VehicleOwnerId = rent.VehicleOwnerId,
                                       Amount = rent.Amount,
                                       CreatedDate = rent.CreatedDate,
                                       FromLocation = rent.FromLocation,
                                       ToLocation = rent.ToLocation,
                                       IsActive = rent.IsActive,
                                       IsPaid = rent.IsPaid,
                                       VehicleOwnerName = owner.VehicleOwnerName
                                   }).OrderByDescending(x => x.dtVehicleRentDate).ThenByDescending(x => x.VehicleRentId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(lstVehicleRents);
        }

        public ActionResult Add()
        {
            VehicleRentVM objVehicleRent = new VehicleRentVM();

            try
            {
                objVehicleRent.VehicleOwnerList = GetActiveVehicleOwnerList();
            }
            catch (Exception ex)
            {
            }
            return View(objVehicleRent);
        }

        public List<SelectListItem> GetActiveVehicleOwnerList()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            try
            {
                var data = _db.tbl_VehicleOwner.Where(x => x.IsActive == true && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.VehicleOwnerId.ToString(), Text = o.VehicleOwnerName }).OrderBy(o => o.Text)
                         .ToList();

                if (data != null && data.Count > 0)
                {
                    lst = data;
                }

            }
            catch (Exception ex)
            {
            }
            return lst;
        }

        [HttpPost]
        public ActionResult Add(VehicleRentVM vehicleRentVM)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            vehicleRentVM.VehicleOwnerList = GetActiveVehicleOwnerList();

            if (ModelState.IsValid)
            {
                try
                {
                    DateTime rent_date = DateTime.ParseExact(vehicleRentVM.VehicleRentDate, "dd/MM/yyyy", null);

                    tbl_VehicleRent objVehicleRent = new tbl_VehicleRent();
                    objVehicleRent.VehicleRentId = Guid.NewGuid();
                    objVehicleRent.VehicleRentDate = rent_date;
                    objVehicleRent.VehicleOwnerId = vehicleRentVM.VehicleOwnerId;
                    objVehicleRent.VehicleNumber = vehicleRentVM.VehicleNumber;
                    objVehicleRent.FromLocation = vehicleRentVM.FromLocation;
                    objVehicleRent.ToLocation = vehicleRentVM.ToLocation;
                    objVehicleRent.Amount = vehicleRentVM.Amount;
                    objVehicleRent.IsPaid = vehicleRentVM.IsPaid;
                    objVehicleRent.Remarks = vehicleRentVM.Remarks;
                    objVehicleRent.ClientId = ClientId;
                    objVehicleRent.IsActive = true;
                    objVehicleRent.CreatedBy = clsSession.UserID;
                    objVehicleRent.CreatedDate = DateTime.UtcNow;
                    _db.tbl_VehicleRent.Add(objVehicleRent);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
                }
                return RedirectToAction("Index");
            }

            return View(vehicleRentVM);
        }

        public ActionResult Edit(Guid Id)
        {
            VehicleRentVM objVehicleRent = new VehicleRentVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            try
            {
                objVehicleRent = (from rent in _db.tbl_VehicleRent
                                  where rent.ClientId == ClientId && rent.VehicleRentId == Id
                                  select new VehicleRentVM
                                  {
                                      VehicleRentId = rent.VehicleRentId,
                                      dtVehicleRentDate = rent.VehicleRentDate,
                                      VehicleOwnerId = rent.VehicleOwnerId,
                                      Amount = rent.Amount,
                                      CreatedDate = rent.CreatedDate,
                                      Remarks = rent.Remarks,
                                      VehicleNumber = rent.VehicleNumber,                                      
                                      FromLocation = rent.FromLocation,
                                      ToLocation = rent.ToLocation,
                                      IsActive = rent.IsActive,
                                      IsPaid = rent.IsPaid
                                  }).FirstOrDefault();

                objVehicleRent.VehicleRentDate = objVehicleRent.dtVehicleRentDate.ToString("dd/MM/yyyy");

                objVehicleRent.VehicleOwnerList = GetActiveVehicleOwnerList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(objVehicleRent);
        }

        [HttpPost]
        public ActionResult Edit(VehicleRentVM vehicleRentVM)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            vehicleRentVM.VehicleOwnerList = GetActiveVehicleOwnerList();

            if (ModelState.IsValid)
            {
                try
                {
                    DateTime rent_date = DateTime.ParseExact(vehicleRentVM.VehicleRentDate, "dd/MM/yyyy", null);

                    tbl_VehicleRent objVehicleRent = _db.tbl_VehicleRent.Where(x => x.VehicleRentId == vehicleRentVM.VehicleRentId).FirstOrDefault(); 
                    objVehicleRent.VehicleRentDate = rent_date;
                    objVehicleRent.VehicleOwnerId = vehicleRentVM.VehicleOwnerId;
                    objVehicleRent.VehicleNumber = vehicleRentVM.VehicleNumber;
                    objVehicleRent.FromLocation = vehicleRentVM.FromLocation;
                    objVehicleRent.ToLocation = vehicleRentVM.ToLocation;
                    objVehicleRent.Amount = vehicleRentVM.Amount;
                    objVehicleRent.Remarks = vehicleRentVM.Remarks;
                    objVehicleRent.IsPaid = vehicleRentVM.IsPaid; 
                    objVehicleRent.ModifiedBy = clsSession.UserID;
                    objVehicleRent.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
                }
                return RedirectToAction("Index");
            }

            return View(vehicleRentVM);
        }
         
        [HttpPost]
        public string DeleteVehicleRent(Guid Id)
        {
            string ReturnMessage = "";

            try
            {
                tbl_VehicleRent objVehicleRent = _db.tbl_VehicleRent.Where(x => x.VehicleRentId == Id).FirstOrDefault();

                if (objVehicleRent == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    _db.tbl_VehicleRent.Remove(objVehicleRent);
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