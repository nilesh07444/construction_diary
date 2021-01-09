using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class DashboardController : MyBaseController
    {
        ConstructionDiaryEntities _db;
        public DashboardController()
        {
            _db = new ConstructionDiaryEntities();
        }

        // GET: Dashboard 
        public ActionResult Index()
        {
             
            DashboardCount objDashboard = new DashboardCount();

            if (clsSession.RoleID == (int)UserRoles.SiteOwner)
            {
                // For Site Owner
                objDashboard.TotalClients = _db.tbl_Clients.Where(x => !x.IsDeleted).ToList().Count;
            }
            else
            {

                // For Clients
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                objDashboard.TotalSites = _db.tbl_Sites.Where(x => x.IsActive && !x.IsDeleted && x.ClientId == ClientId).ToList().Count;

                /*
                objDashboard.TotalPersons = _db.tbl_Persons.Where(x => x.IsActive == true && x.IsDeleted == false).ToList().Count;

                decimal? TotalGivenAmount = (from finance in _db.tbl_Finance
                                             join person in _db.tbl_Persons
                                             on finance.PersonId equals person.PersonId
                                             where person.IsActive == true && person.IsDeleted == false && finance.IsActive == true && finance.IsDeleted == false
                                             select finance
                    ).ToList().Select(x => x.Amount).Sum();

                decimal? TotalTakenAmount = (from finance in _db.tbl_ContractorFinance
                                             join site in _db.tbl_Sites
                                             on finance.SiteId equals site.SiteId
                                             where site.IsActive && !site.IsDeleted && finance.IsActive == true && finance.IsDeleted == false
                                                && site.ClientId == ClientId
                                             select finance
                    ).ToList().Select(x => x.Amount).Sum();

                objDashboard.TotalGivenAmountToPersons = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalGivenAmount));

                objDashboard.TotalTakenAmountFromSites = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalTakenAmount));

                objDashboard.TotalBalanceAmount = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalTakenAmount) - Convert.ToDecimal(TotalGivenAmount));
                */
            }
            return View(objDashboard);
        }

    }
}