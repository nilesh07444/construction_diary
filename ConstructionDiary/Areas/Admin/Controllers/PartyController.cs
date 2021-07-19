using ConstructionDiary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class PartyController : MyBaseController
    {
        private readonly ConstructionDiaryEntities _db;
        public PartyController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            int RoleID = clsSession.RoleID;

            List<PartyVM> lstParties = new List<PartyVM>();

            try
            {

                lstParties = (from p in _db.tbl_Party
                              where p.ClientId == ClientId && !p.IsDeleted
                              select new PartyVM
                              {
                                  PartyId = p.PartyId,
                                  PartyName = p.PartyName,
                                  Remarks = p.Remarks,
                                  IsActive = p.IsActive
                              }).OrderByDescending(x => x.PartyId).ToList();

            }
            catch (Exception ex)
            {
            }

            return View(lstParties);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(PartyVM partyVM)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            if (ModelState.IsValid)
            {
                try
                {
                    tbl_Party objParty = new tbl_Party();
                    objParty.PartyId = Guid.NewGuid();
                    objParty.PartyName = partyVM.PartyName;
                    objParty.Remarks = partyVM.Remarks;
                    objParty.ClientId = ClientId;
                    objParty.IsActive = true;
                    objParty.IsDeleted = false;
                    objParty.CreatedBy = clsSession.UserID;
                    objParty.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Party.Add(objParty);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
                }
                return RedirectToAction("Index");
            }

            return View(partyVM);
        }


        public ActionResult Edit(Guid Id)
        {
            PartyVM objParty = new PartyVM();

            try
            {

                objParty = (from p in _db.tbl_Party
                              where p.PartyId == Id 
                              select new PartyVM
                              {
                                  PartyId = p.PartyId,
                                  PartyName = p.PartyName,
                                  Remarks = p.Remarks,
                                  IsActive = p.IsActive
                              }).FirstOrDefault();

            }
            catch (Exception ex)
            {
            }

            return View(objParty);
        }

        [HttpPost]
        public ActionResult Edit(PartyVM partyVM)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            if (ModelState.IsValid)
            {
                try
                {
                    tbl_Party objParty = _db.tbl_Party.Where(x => x.PartyId == partyVM.PartyId).FirstOrDefault();
                    objParty.PartyName = partyVM.PartyName;
                    objParty.Remarks = partyVM.Remarks;
                    objParty.ClientId = ClientId;
                    objParty.ModifiedBy = clsSession.UserID;
                    objParty.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
                }
                return RedirectToAction("Index");
            }

            return View(partyVM);
        }

        [HttpPost]
        public string DeleteParty(Guid Id)
        {
            string ReturnMessage = "";

            try
            {
                tbl_Party objParty = _db.tbl_Party.Where(x => x.PartyId == Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();

                if (objParty == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objParty.IsDeleted = true;
                    objParty.ModifiedDate = DateTime.UtcNow;
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