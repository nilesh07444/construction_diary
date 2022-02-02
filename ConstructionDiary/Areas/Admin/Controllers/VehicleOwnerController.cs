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
    public class VehicleOwnerController : MyBaseController
    {
        private readonly ConstructionDiaryEntities _db;
        public VehicleOwnerController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            List<VehicleOwnerVM> lstVehicleOwners = new List<VehicleOwnerVM>();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            try
            {
                lstVehicleOwners = (from owner in _db.tbl_VehicleOwner
                                    where owner.ClientId == ClientId
                                    select new VehicleOwnerVM
                                    {
                                        VehicleOwnerId = owner.VehicleOwnerId,
                                        VehicleOwnerName = owner.VehicleOwnerName,
                                        MobileNo = owner.MobileNo,
                                        IsActive = owner.IsActive
                                    }).OrderByDescending(x => x.VehicleOwnerId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(lstVehicleOwners);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(VehicleOwnerVM vehicleOwnerVM)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            if (ModelState.IsValid)
            {
                try
                {
                    tbl_VehicleOwner objVehicleOwner = new tbl_VehicleOwner();
                    objVehicleOwner.VehicleOwnerId = Guid.NewGuid();
                    objVehicleOwner.VehicleOwnerName = vehicleOwnerVM.VehicleOwnerName;
                    objVehicleOwner.MobileNo = vehicleOwnerVM.MobileNo;
                    objVehicleOwner.ClientId = ClientId;
                    objVehicleOwner.IsActive = true;
                    objVehicleOwner.CreatedBy = clsSession.UserID;
                    objVehicleOwner.CreatedDate = DateTime.UtcNow;
                    _db.tbl_VehicleOwner.Add(objVehicleOwner);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
                }
                return RedirectToAction("Index");
            }

            return View(vehicleOwnerVM);
        }

        public ActionResult Edit(Guid Id)
        {
            VehicleOwnerVM objVehicleOwner = new VehicleOwnerVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            try
            {
                objVehicleOwner = (from owner in _db.tbl_VehicleOwner
                                   where owner.ClientId == ClientId && owner.VehicleOwnerId == Id
                                   select new VehicleOwnerVM
                                   {
                                       VehicleOwnerId = owner.VehicleOwnerId,
                                       VehicleOwnerName = owner.VehicleOwnerName,
                                       MobileNo = owner.MobileNo,
                                       IsActive = owner.IsActive
                                   }).OrderByDescending(x => x.VehicleOwnerId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(objVehicleOwner);
        }

        [HttpPost]
        public ActionResult Edit(VehicleOwnerVM vehicleOwnerVM)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            if (ModelState.IsValid)
            {
                try
                {
                    tbl_VehicleOwner objVehicleOwner = _db.tbl_VehicleOwner.Where(x => x.VehicleOwnerId == vehicleOwnerVM.VehicleOwnerId).FirstOrDefault();
                    objVehicleOwner.VehicleOwnerName = vehicleOwnerVM.VehicleOwnerName;
                    objVehicleOwner.MobileNo = vehicleOwnerVM.MobileNo;
                    objVehicleOwner.ModifiedBy = clsSession.UserID;
                    objVehicleOwner.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
                }
                return RedirectToAction("Index");
            }

            return View(vehicleOwnerVM);
        }
         
        [HttpPost]
        public string DeleteVehicleOwner(Guid Id)
        {
            string ReturnMessage = "";

            try
            {
                tbl_VehicleOwner objVehicleOwner = _db.tbl_VehicleOwner.Where(x => x.VehicleOwnerId == Id).FirstOrDefault();

                if (objVehicleOwner == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    List<tbl_VehicleRent> lstVehicleRents = _db.tbl_VehicleRent.Where(x => x.VehicleOwnerId == objVehicleOwner.VehicleOwnerId).ToList();
                    if (lstVehicleRents != null && lstVehicleRents.Count > 0)
                    {
                        ReturnMessage = "rentdatafound";
                    }
                    else
                    {
                        _db.tbl_VehicleOwner.Remove(objVehicleOwner);
                        _db.SaveChanges();
                        ReturnMessage = "success";
                    }
                     
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