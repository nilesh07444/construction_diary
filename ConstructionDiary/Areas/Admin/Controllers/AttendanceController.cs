using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    public class AttendanceController : Controller
    {
        ConstructionDiaryEntities _db;
        public AttendanceController()
        {
            _db = new ConstructionDiaryEntities();
        }

        public ActionResult Index()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<AttendanceVM> lstAttendance = (from atte in _db.tbl_Attendance
                                                where atte.ClientId == ClientId
                                                select new AttendanceVM
                                                {
                                                    ClientId = atte.ClientId,
                                                    AttendanceDate = atte.AttendanceDate,
                                                    AttendaceId = atte.AttendaceId
                                                }).ToList();

            return View(lstAttendance);
        }

        public ActionResult Add()
        {
            AttendanceFormVM objAttendance = new AttendanceFormVM();

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                var personList = _db.tbl_Persons.Where(x => x.ClientId == ClientId && x.IsActive == true && x.IsDeleted == false).ToList();
                var siteList = _db.tbl_Sites.Where(x => x.ClientId == ClientId && x.IsActive == true && x.IsDeleted == false).ToList();

                List<PersonAttendanceVM> liststPersonAttendance = new List<PersonAttendanceVM>();
                personList.ForEach(person =>
                {
                    PersonAttendanceVM objPersonAttendance = new PersonAttendanceVM();
                    objPersonAttendance.PersonId = person.PersonId;
                    objPersonAttendance.PersonName = person.PersonFirstName;

                    objPersonAttendance.PersonDailyRate = person.DailyRate; 

                    objPersonAttendance.SitesList = siteList.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

                    liststPersonAttendance.Add(objPersonAttendance);

                });

                objAttendance.lstPersonAttendance = liststPersonAttendance;

            }
            catch (Exception ex)
            { 
            }

            return View(objAttendance);
        }

    }
}