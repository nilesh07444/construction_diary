using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class AttendanceController : MyBaseController
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
                                                    AttendaceId = atte.AttendaceId,
                                                    TotalPaidAmount = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.PayableAmount != null).Select(x => x.PayableAmount).Sum(),
                                                    TotalPerson = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId).ToList().Count(),
                                                    TotalFullDay = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == 1).ToList().Count(),
                                                    TotalHalfDay = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == (decimal)0.5).ToList().Count(),
                                                    TotalAbsent = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == 0).ToList().Count()
                                                }).OrderByDescending(x => x.AttendanceDate).ToList();

            return View(lstAttendance);
        }

        public ActionResult Add()
        {
            AttendanceFormVM objAttendance = new AttendanceFormVM();

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                var personList = _db.tbl_Persons.Where(x => x.ClientId == ClientId && x.IsAttendancePerson == true && x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.PersonFirstName).ToList();
                var siteList = _db.tbl_Sites.Where(x => x.ClientId == ClientId && x.IsActive == true && x.IsDeleted == false).ToList();

                objAttendance.SitesList = siteList.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

                var attandanceStatuses = GetAttandanceStatus();
                objAttendance.AttendanceStatusList = attandanceStatuses
                     .Select(o => new SelectListItem { Value = o.StatusValue.ToString(), Text = o.StatusText })
                     .ToList();

                List<PersonAttendanceVM> liststPersonAttendance = new List<PersonAttendanceVM>();
                personList.ForEach(person =>
                {
                    PersonAttendanceVM objPersonAttendance = new PersonAttendanceVM();
                    objPersonAttendance.PersonId = person.PersonId;
                    objPersonAttendance.PersonName = person.PersonFirstName;
                    objPersonAttendance.PersonDailyRate = person.DailyRate;
                    liststPersonAttendance.Add(objPersonAttendance);
                });

                objAttendance.lstPersonAttendance = liststPersonAttendance;

            }
            catch (Exception ex)
            {
            }

            return View(objAttendance);
        }

        [HttpPost]
        public ActionResult Add(AttendanceFormVM attandance)
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            var siteList = _db.tbl_Sites.Where(x => x.ClientId == ClientId && x.IsActive == true && x.IsDeleted == false).ToList();
            attandance.SitesList = siteList.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            var attandanceStatuses = GetAttandanceStatus();
            attandance.AttendanceStatusList = attandanceStatuses
                 .Select(o => new SelectListItem { Value = o.StatusValue.ToString(), Text = o.StatusText })
                 .ToList();

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {

                DateTime aDate = DateTime.ParseExact(attandance.AttendanceDate, "dd/MM/yyyy", null);
                var dateExist = _db.tbl_Attendance.Where(x => x.AttendanceDate == aDate && x.ClientId == ClientId).FirstOrDefault();

                if (dateExist != null)
                {
                    ModelState.AddModelError("AttendanceDate", "Attendance Date is already exist");
                    return View(attandance);
                }
                else
                {
                    tbl_Attendance objAttandance = new tbl_Attendance();
                    objAttandance.AttendaceId = Guid.NewGuid();
                    objAttandance.AttendanceDate = aDate;
                    objAttandance.ClientId = ClientId;
                    objAttandance.CreatedDate = DateTime.Now;
                    _db.tbl_Attendance.Add(objAttandance);
                    _db.SaveChanges();

                    Guid AttandanceId = objAttandance.AttendaceId;

                    List<PersonAttendanceVM> listPersons = attandance.lstPersonAttendance;

                    listPersons.ForEach(x =>
                    {

                        tbl_PersonAttendance objPersonAttendance = new tbl_PersonAttendance();
                        objPersonAttendance.PersonAttendanceId = Guid.NewGuid();
                        objPersonAttendance.AttendanceId = AttandanceId;
                        objPersonAttendance.PersonId = x.PersonId;
                        objPersonAttendance.AttendanceStatus = x.AttendanceStatus;
                        objPersonAttendance.WithdrawAmount = x.WithdrawAmount;
                        objPersonAttendance.OvertimeAmount = x.OvertimeAmount;
                        objPersonAttendance.Remarks = x.Remarks;


                        if (objPersonAttendance.AttendanceStatus != 0)
                        {
                            objPersonAttendance.PersonDailyRate = x.PersonDailyRate;
                            objPersonAttendance.PayableAmount = x.PersonDailyRate * objPersonAttendance.AttendanceStatus;
                            objPersonAttendance.SiteId = x.SiteId;
                        }
                        objPersonAttendance.CreatedBy = new Guid(clsSession.UserID.ToString());
                        objPersonAttendance.CreatedDate = DateTime.UtcNow;
                        _db.tbl_PersonAttendance.Add(objPersonAttendance);
                        _db.SaveChanges();

                    });

                    return RedirectToAction("Index");
                }

            }
            else
            {
                string err = "";
            }

            return View(attandance);
        }

        public ActionResult Edit(Guid Id)
        {
            AttendanceFormVM objAttendance = new AttendanceFormVM();
            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                var personList = _db.tbl_Persons.Where(x => x.ClientId == ClientId && x.IsActive == true && x.IsDeleted == false).ToList();
                var siteList = _db.tbl_Sites.Where(x => x.ClientId == ClientId && x.IsActive == true && x.IsDeleted == false).ToList();

                objAttendance.SitesList = siteList.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

                var attandanceStatuses = GetAttandanceStatus();
                objAttendance.AttendanceStatusList = attandanceStatuses
                     .Select(o => new SelectListItem { Value = o.StatusValue.ToString(), Text = o.StatusText })
                     .ToList();

                tbl_Attendance objAttendanceData = _db.tbl_Attendance.Where(x => x.AttendaceId == Id).FirstOrDefault();
                if (objAttendanceData != null)
                {
                    objAttendance.AttendaceId = objAttendanceData.AttendaceId;
                    objAttendance.AttendanceDate = Convert.ToDateTime(objAttendanceData.AttendanceDate).ToString("dd/MM/yyyy");
                }

                objAttendance.lstPersonAttendance = (from personatta in _db.tbl_PersonAttendance
                                                     join person in _db.tbl_Persons on personatta.PersonId equals person.PersonId
                                                     where personatta.AttendanceId == Id
                                                     select new PersonAttendanceVM
                                                     {
                                                         PersonAttendanceId = personatta.PersonAttendanceId,
                                                         PersonId = personatta.PersonId,
                                                         PersonName = person.PersonFirstName,
                                                         AttendanceStatus = personatta.AttendanceStatus,
                                                         PersonDailyRate = person.DailyRate,
                                                         SiteId = personatta.SiteId,
                                                         WithdrawAmount = personatta.WithdrawAmount,
                                                         OvertimeAmount = personatta.OvertimeAmount,
                                                         Remarks = personatta.Remarks
                                                     }).OrderBy(x=>x.PersonName).ToList();

            }
            catch (Exception ex)
            {
            }
            return View(objAttendance);
        }

        [HttpPost]
        public ActionResult Edit(AttendanceFormVM attandance)
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            var siteList = _db.tbl_Sites.Where(x => x.ClientId == ClientId && x.IsActive == true && x.IsDeleted == false).ToList();
            attandance.SitesList = siteList.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

            var attandanceStatuses = GetAttandanceStatus();
            attandance.AttendanceStatusList = attandanceStatuses
                 .Select(o => new SelectListItem { Value = o.StatusValue.ToString(), Text = o.StatusText })
                 .ToList();

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                DateTime aDate = DateTime.ParseExact(attandance.AttendanceDate, "dd/MM/yyyy", null);
                var dateExist = _db.tbl_Attendance.Where(x => x.AttendanceDate == aDate && x.AttendaceId != attandance.AttendaceId && x.ClientId == ClientId).FirstOrDefault();

                if (dateExist != null)
                {
                    ModelState.AddModelError("AttendanceDate", "Attendance Date is already exist");
                    return View(attandance);
                }
                else
                {
                    tbl_Attendance objAttandance = _db.tbl_Attendance.Where(x => x.AttendaceId == attandance.AttendaceId).FirstOrDefault();
                    objAttandance.AttendanceDate = aDate;
                    objAttandance.ModifiedDate = DateTime.UtcNow;
                    //_db.SaveChanges();

                    List<PersonAttendanceVM> listPersons = attandance.lstPersonAttendance;

                    listPersons.ForEach(personAttendance =>
                    {
                        tbl_PersonAttendance objPersonAttendance = _db.tbl_PersonAttendance.Where(p => p.PersonAttendanceId == personAttendance.PersonAttendanceId).FirstOrDefault();
                        objPersonAttendance.PersonId = personAttendance.PersonId;
                        objPersonAttendance.AttendanceStatus = personAttendance.AttendanceStatus;
                        objPersonAttendance.PersonDailyRate = personAttendance.PersonDailyRate;

                        objPersonAttendance.WithdrawAmount = personAttendance.WithdrawAmount;
                        objPersonAttendance.OvertimeAmount = personAttendance.OvertimeAmount;

                        if (objPersonAttendance.AttendanceStatus != 0)
                        {
                            objPersonAttendance.PayableAmount = personAttendance.PersonDailyRate * objPersonAttendance.AttendanceStatus;
                            objPersonAttendance.SiteId = personAttendance.SiteId;
                        }
                        else
                        {
                            objPersonAttendance.PayableAmount = null;
                            objPersonAttendance.SiteId = null;
                        }

                        objPersonAttendance.ModifiedBy = new Guid(clsSession.UserID.ToString());
                        objPersonAttendance.ModifiedDate = DateTime.UtcNow;
                        _db.SaveChanges();
                    });

                    return RedirectToAction("Index");
                }
            }
            else
            {
                string err = "";
            }

            return View(attandance);
        }

        [HttpPost]
        public string DeleteAttendance(Guid AttendanceId)
        {
            string ReturnMessage = "";

            try
            {
                tbl_Attendance objAttendance = _db.tbl_Attendance.Where(x => x.AttendaceId == AttendanceId).FirstOrDefault();

                if (objAttendance == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    List<tbl_PersonAttendance> lstPersonAttendances = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == AttendanceId).ToList();
                    _db.tbl_PersonAttendance.RemoveRange(lstPersonAttendances);

                    _db.tbl_Attendance.Remove(objAttendance);
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

        public List<AttendanceStatusVM> GetAttandanceStatus()
        {
            List<AttendanceStatusVM> lstAttendanceStatus = new List<AttendanceStatusVM>();

            lstAttendanceStatus.Add(new AttendanceStatusVM { StatusText = "Full Day", StatusValue = "1.0" });
            lstAttendanceStatus.Add(new AttendanceStatusVM { StatusText = "Half Day", StatusValue = "0.5" });
            lstAttendanceStatus.Add(new AttendanceStatusVM { StatusText = "Absent", StatusValue = "0.0" });

            return lstAttendanceStatus;

        }

        public ActionResult PersonList()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<tbl_Persons> lstPersons = _db.tbl_Persons.Where(x => x.ClientId == ClientId && x.IsAttendancePerson == true && x.IsDeleted == false).ToList();

            return View(lstPersons);
        }

        public ActionResult AddPerson()
        {
            PersonVM person = new PersonVM();
            person.PersonTypeList = _db.tbl_PersonType
                         .Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.PersonType })
                         .ToList();

            return View(person);
        }

        [HttpPost]
        public ActionResult AddPerson(PersonVM person, HttpPostedFileBase postedFile)
        {
            person.PersonTypeList = _db.tbl_PersonType
                         .Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.PersonType })
                         .ToList();

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                string fileName = string.Empty;
                string path = Server.MapPath("~/Images/PersonPhoto/");
                if (postedFile != null)
                {
                    fileName = Guid.NewGuid() + Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(path + fileName);
                }

                try
                {
                    tbl_Persons objPerson = new tbl_Persons();
                    objPerson.PersonId = Guid.NewGuid();
                    objPerson.PersonFirstName = person.Firstname;
                    objPerson.PersonAddress = person.Address;
                    objPerson.MobileNo = person.MobileNo;
                    objPerson.DailyRate = person.DailyRate;
                    objPerson.PersonTypeId = person.PersonTypeId;
                    objPerson.PersonPhoto = fileName;
                    objPerson.IsAttendancePerson = true;
                    objPerson.IsActive = true;
                    objPerson.IsDeleted = false;
                    objPerson.ClientId = clsSession.ClientID;
                    objPerson.CreatedBy = new Guid(clsSession.UserID.ToString());
                    objPerson.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Persons.Add(objPerson);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                { 
                }
                return RedirectToAction("PersonList");
            }

            return View(person);
        }


        public ActionResult EditPerson(Guid id)
        {
            PersonVM person = new PersonVM();

            person.PersonTypeList = _db.tbl_PersonType
                         .Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.PersonType })
                         .ToList();

            tbl_Persons obj = _db.tbl_Persons.Where(x => x.PersonId == id).FirstOrDefault();
            if (obj != null)
            {
                person.PersonId = obj.PersonId;
                person.Firstname = obj.PersonFirstName;
                person.Address = obj.PersonAddress;
                person.MobileNo = obj.MobileNo;
                person.DailyRate = obj.DailyRate;
                person.PersonTypeId = obj.PersonTypeId;
                person.strPersonPhoto = obj.PersonPhoto;
            }

            return View(person);
        }

        [HttpPost]
        public ActionResult EditPerson(PersonVM person, HttpPostedFileBase postedFile)
        {

            person.PersonTypeList = _db.tbl_PersonType
                         .Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.PersonType })
                         .ToList();

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                try
                {
                    tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == person.PersonId).FirstOrDefault();

                    if (objPerson != null)
                    {

                        string fileName = string.Empty;
                        string path = Server.MapPath("~/Images/PersonPhoto/");
                        if (postedFile != null)
                        {
                            fileName = Guid.NewGuid() + Path.GetFileName(postedFile.FileName);
                            postedFile.SaveAs(path + fileName);
                        }
                        else
                        {
                            fileName = person.strPersonPhoto;
                        }

                        objPerson.PersonFirstName = person.Firstname;
                        objPerson.PersonAddress = person.Address;
                        objPerson.PersonPhoto = fileName;
                        objPerson.DailyRate = person.DailyRate;
                        objPerson.PersonTypeId = person.PersonTypeId;
                        objPerson.MobileNo = person.MobileNo;
                        objPerson.ClientId = clsSession.ClientID;
                        objPerson.UpdatedBy = new Guid(clsSession.UserID.ToString());
                        objPerson.ModifiedDate = DateTime.UtcNow;
                        _db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                }
                return RedirectToAction("PersonList");
            }

            return View(person);
        }


    }
}