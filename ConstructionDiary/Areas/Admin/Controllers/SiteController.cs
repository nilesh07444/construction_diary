using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class SiteController : Controller
    {
        ConstructionDiaryEntities _db;
        public SiteController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            var lstSites = (from p in _db.SP_GetSitesList(ClientId)
                            select p).ToList();

            //List<tbl_Sites> lstSites = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == clsSession.ClientID).ToList();
            return View(lstSites);
        }

        [HttpPost]
        public string SaveSite(string site_id, string site_name, string site_desc)
        {
            string ReturnMessage = "";

            try
            {
                if (site_id == "0")
                {
                    // Add Site
                    tbl_Sites objOldSite = _db.tbl_Sites.Where(x => x.SiteName == site_name && x.IsActive == true
                                                                && x.IsDeleted == false).FirstOrDefault();
                    if (objOldSite != null)
                    {
                        ReturnMessage = "alreadyexist";
                    }
                    else
                    {
                        tbl_Sites objSite = new tbl_Sites();
                        objSite.SiteId = Guid.NewGuid();
                        objSite.SiteName = site_name;
                        objSite.SiteDescription = site_desc;
                        objSite.ClientId = new Guid(clsSession.ClientID.ToString());
                        objSite.IsActive = true;
                        objSite.IsDeleted = false;
                        objSite.CreatedBy = new Guid(clsSession.UserID.ToString());
                        objSite.CreatedDate = DateTime.UtcNow;
                        _db.tbl_Sites.Add(objSite);
                        _db.SaveChanges();
                        ReturnMessage = "created";
                    }
                }
                else
                {
                    // Edit Site
                    Guid SiteIdToEdit = new Guid(site_id);

                    // Check to already exist or not
                    tbl_Sites objSite = _db.tbl_Sites.Where(x => x.SiteId != SiteIdToEdit && x.SiteName.ToLower() == site_name.ToLower()
                                                                && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                    if (objSite != null)
                    {
                        // already exist 
                        ReturnMessage = "alreadyexist";
                    }
                    else
                    {
                        tbl_Sites objSiteToUpdate = _db.tbl_Sites.Where(x => x.SiteId == SiteIdToEdit
                                                        && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();

                        objSiteToUpdate.SiteName = site_name;
                        objSiteToUpdate.SiteDescription = site_desc;
                        objSiteToUpdate.UpdatedBy = new Guid(clsSession.UserID.ToString());
                        objSiteToUpdate.ModifiedDate = DateTime.UtcNow;
                        _db.SaveChanges();

                        ReturnMessage = "updated";
                    }

                }

            }
            catch (Exception ex)
            {
                ReturnMessage = "exception";
            }

            return ReturnMessage;
        }

        [HttpPost]
        public string DeleteSite(string SiteId)
        {
            string ReturnMessage = "";

            try
            {
                Guid SiteIdToDelete = new Guid(SiteId);

                tbl_Sites objSite = _db.tbl_Sites.Where(x => x.SiteId == SiteIdToDelete && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objSite == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objSite.IsDeleted = true;
                    objSite.UpdatedBy = new Guid(clsSession.UserID.ToString());
                    objSite.ModifiedDate = DateTime.UtcNow;
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

        public ActionResult Detail(Guid id)
        {

            tbl_Sites objSite = new tbl_Sites();

            try
            {
                objSite = _db.tbl_Sites.Where(x => x.SiteId == id).FirstOrDefault();

                ViewData["SiteFinanceList"] = (from p in _db.SP_GetSiteDetailById(id)
                                               select p).ToList();

            }
            catch (Exception ex)
            {

            }

            return View(objSite);
        }
    }
}