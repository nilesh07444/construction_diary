using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models; 

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class MaterialTypeController : MyBaseController
    {
        ConstructionDiaryEntities _db;
        public MaterialTypeController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<MaterialTypeVM> lstMaterialType = (from c in _db.tbl_MaterialType
                                                    where !c.IsDeleted && c.ClientId == ClientId
                                                    select new MaterialTypeVM
                                                    {
                                                        MaterialTypeId = c.MaterialTypeId,
                                                        MaterialType = c.MaterialType,
                                                        IsActive = c.IsActive
                                                    }).OrderByDescending(x => x.MaterialTypeId).ToList();
            return View(lstMaterialType);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(MaterialTypeVM material)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                try
                {
                    Guid ClientId = new Guid(clsSession.ClientID.ToString());

                    var existData = _db.tbl_MaterialType.Where(x => !x.IsDeleted && x.ClientId == ClientId
                                    && x.MaterialType.ToLower() == material.MaterialType.ToLower()).FirstOrDefault();

                    if (existData != null)
                    {
                        ModelState.AddModelError("MaterialType", "Material is already exist");
                        return View(material);
                    }

                    tbl_MaterialType objMaterialType = new tbl_MaterialType();
                    objMaterialType.MaterialTypeId = Guid.NewGuid();
                    objMaterialType.MaterialType = material.MaterialType;
                    objMaterialType.ClientId = ClientId;
                    objMaterialType.IsActive = true;
                    objMaterialType.IsDeleted = false;
                    objMaterialType.CreatedBy = clsSession.UserID;
                    objMaterialType.CreatedDate = DateTime.UtcNow;
                    _db.tbl_MaterialType.Add(objMaterialType);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                }
                return RedirectToAction("Index");
            }
            return View(material);
        }

        public ActionResult Edit(Guid Id)
        {
            MaterialTypeVM objMaterialType = (from c in _db.tbl_MaterialType
                                              where c.MaterialTypeId == Id
                                              select new MaterialTypeVM
                                              {
                                                  MaterialTypeId = c.MaterialTypeId,
                                                  MaterialType = c.MaterialType,
                                                  IsActive = c.IsActive
                                              }).FirstOrDefault();
            return View(objMaterialType);
        }

        [HttpPost]
        public ActionResult Edit(MaterialTypeVM material)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                try
                {
                    Guid ClientId = new Guid(clsSession.ClientID.ToString());

                    var existData = _db.tbl_MaterialType.Where(x => x.MaterialTypeId != material.MaterialTypeId && x.ClientId == ClientId
                                && !x.IsDeleted && x.MaterialType.ToLower() == material.MaterialType.ToLower()).FirstOrDefault();

                    if (existData != null)
                    {
                        ModelState.AddModelError("MaterialType", "Material is already exist");
                        return View(material);
                    }

                    tbl_MaterialType objMaterialType = _db.tbl_MaterialType.Where(x => x.MaterialTypeId == material.MaterialTypeId).FirstOrDefault();
                    objMaterialType.MaterialType = material.MaterialType;
                    objMaterialType.ModifiedBy = clsSession.UserID;
                    objMaterialType.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                }
                return RedirectToAction("Index");
            }
            return View(material);
        }

        [HttpPost]
        public string DeleteMaterialType(Guid MaterialTypeId)
        {
            string ReturnMessage = "";
            try
            {
                tbl_MaterialType objMaterialType = _db.tbl_MaterialType.Where(x => x.MaterialTypeId == MaterialTypeId && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objMaterialType == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objMaterialType.IsDeleted = true;
                    objMaterialType.ModifiedDate = DateTime.UtcNow;
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

        [HttpGet]
        public JsonResult GetMaterialList()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<MaterialTypeVM> lstMaterialType = (from c in _db.tbl_MaterialType
                                                    where !c.IsDeleted && c.IsActive && c.ClientId == ClientId
                                                    select new MaterialTypeVM
                                                    {
                                                        MaterialTypeId = c.MaterialTypeId,
                                                        MaterialType = c.MaterialType,
                                                        IsActive = c.IsActive
                                                    }).OrderByDescending(x => x.MaterialType).ToList();

            return Json(lstMaterialType, JsonRequestBehavior.AllowGet);
        }

    }
}