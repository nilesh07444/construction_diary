using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;
using Newtonsoft.Json;

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

                                                    //TotalPerson = (_db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.PersonTypeId != 6).ToList().Count() +
                                                    //                _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.PersonTypeId == 6).ToList().Select(x=>x.TotalRokadiya).Sum()),

                                                    //TotalFullDay = (_db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == 1 && x.PersonTypeId != 6).ToList().Count() +
                                                    //                _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == 1 && x.PersonTypeId == 6).ToList().Select(x => x.TotalRokadiya).Sum()),

                                                    //TotalHalfDay = (_db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == (decimal)0.5 && x.PersonTypeId != 6).ToList().Count() +
                                                    //                _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == (decimal)0.5 && x.PersonTypeId == 6).ToList().Select(x => x.TotalRokadiya).Sum()),

                                                    //TotalAbsent = (_db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == 0 && x.PersonTypeId != 6).ToList().Count() +
                                                    //                _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == 0 && x.PersonTypeId == 6).ToList().Select(x => x.TotalRokadiya).Sum()),

                                                }).OrderByDescending(x => x.AttendanceDate).ToList();

            lstAttendance.ForEach(atte =>
            {

                decimal? TotalPerson1 = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.PersonTypeId != 6).ToList().Count();
                decimal? TotalPerson2 = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.PersonTypeId == 6).ToList().Select(x => x.TotalRokadiya).Sum();

                decimal? TotalPerson = TotalPerson1 + TotalPerson2;

                decimal? TotalFullDay1 = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == 1 && x.PersonTypeId != 6).ToList().Count();
                decimal? TotalFullDay2 = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == 1 && x.PersonTypeId == 6).ToList().Select(x => x.TotalRokadiya).Sum();
                decimal? TotalFullDay = TotalFullDay1 + TotalFullDay2;

                decimal? TotalHalfDay1 = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == (decimal)0.5 && x.PersonTypeId != 6).ToList().Count();
                decimal? TotalHalfDay2 = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == (decimal)0.5 && x.PersonTypeId == 6).ToList().Select(x => x.TotalRokadiya).Sum();
                decimal? TotalHalfDay = TotalHalfDay1 + TotalHalfDay2;

                decimal? TotalAbsent1 = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == 0 && x.PersonTypeId != 6).ToList().Count();
                decimal? TotalAbsent2 = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.AttendanceStatus == 0 && x.PersonTypeId == 6).ToList().Select(x => x.TotalRokadiya).Sum();
                decimal? TotalAbsent = TotalAbsent1 + TotalAbsent2;

                atte.TotalPerson = TotalPerson;
                atte.TotalFullDay = TotalFullDay;
                atte.TotalHalfDay = TotalHalfDay;
                atte.TotalAbsent = TotalAbsent;

            });

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
                    objPersonAttendance.PersonTypeId = (person.PersonTypeId == null ? 0 : Convert.ToInt32(person.PersonTypeId));
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
                        objPersonAttendance.PersonDailyRate = x.PersonDailyRate;

                        objPersonAttendance.PersonTypeId = x.PersonTypeId;

                        if (objPersonAttendance.AttendanceStatus != 0)
                        {
                            if (x.PersonTypeId == 6)
                            {
                                objPersonAttendance.TotalRokadiya = x.TotalRokadiya;

                                objPersonAttendance.PayableAmount = x.TotalRokadiya * objPersonAttendance.PersonDailyRate * objPersonAttendance.AttendanceStatus;
                                objPersonAttendance.SiteId = x.SiteId;
                            }
                            else
                            {
                                objPersonAttendance.TotalRokadiya = 0;

                                objPersonAttendance.PayableAmount = x.PersonDailyRate * objPersonAttendance.AttendanceStatus;
                                objPersonAttendance.SiteId = x.SiteId;
                            }
                        }
                        else
                        {
                            objPersonAttendance.PayableAmount = null;
                            objPersonAttendance.SiteId = null;
                            objPersonAttendance.TotalRokadiya = 0;
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

                //var personList = _db.tbl_Persons.Where(x => x.ClientId == ClientId && x.IsActive == true && x.IsDeleted == false).ToList();
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
                                                         PersonTypeId = personatta.PersonTypeId,
                                                         TotalRokadiya = personatta.TotalRokadiya,
                                                         PersonName = person.PersonFirstName,
                                                         AttendanceStatus = personatta.AttendanceStatus,
                                                         PersonDailyRate = person.DailyRate,
                                                         SiteId = personatta.SiteId,
                                                         WithdrawAmount = personatta.WithdrawAmount,
                                                         OvertimeAmount = personatta.OvertimeAmount,
                                                         Remarks = personatta.Remarks
                                                     }).OrderBy(x => x.PersonName).ToList();

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

                        if (personAttendance.AttendanceStatus != 0)
                        {

                            if (personAttendance.PersonTypeId == 6)
                            {
                                objPersonAttendance.TotalRokadiya = personAttendance.TotalRokadiya;

                                objPersonAttendance.PayableAmount = personAttendance.TotalRokadiya * objPersonAttendance.PersonDailyRate * objPersonAttendance.AttendanceStatus;
                                objPersonAttendance.SiteId = personAttendance.SiteId;
                            }
                            else
                            {
                                objPersonAttendance.TotalRokadiya = 0;

                                objPersonAttendance.PayableAmount = personAttendance.PersonDailyRate * personAttendance.AttendanceStatus;
                                objPersonAttendance.SiteId = personAttendance.SiteId;
                            }

                        }
                        else
                        {
                            objPersonAttendance.PayableAmount = null;
                            objPersonAttendance.SiteId = null;
                            objPersonAttendance.TotalRokadiya = 0;
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

            //List<tbl_Persons> lstPersonsOLD = _db.tbl_Persons.Where(x => x.ClientId == ClientId && x.IsAttendancePerson == true && x.IsDeleted == false).ToList();

            List<AttendancePersonVM> lstPersonsNew = (from p in _db.tbl_Persons
                                                      where p.ClientId == ClientId && p.IsAttendancePerson == true && p.IsDeleted == false
                                                      select new AttendancePersonVM
                                                      {
                                                          PersonId = p.PersonId,
                                                          PersonFirstName = p.PersonFirstName,
                                                          IsActive = p.IsActive,
                                                          IsGroupPerson = _db.tbl_PersonGroupMap.Where(x => x.PersonId == p.PersonId).Any()
                                                      }).ToList();

            lstPersonsNew.ForEach(item =>
            {
                if (item.IsGroupPerson)
                    item.PersonGroupId = _db.tbl_PersonGroupMap.Where(x => x.PersonId == item.PersonId).FirstOrDefault().PersonGroupId;
            });

            return View(lstPersonsNew);
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
                /*
                string fileName = string.Empty;
                string path = Server.MapPath("~/Images/PersonPhoto/");
                if (postedFile != null)
                {
                    fileName = Guid.NewGuid() + Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(path + fileName);
                }
                */

                try
                {
                    tbl_Persons objPerson = new tbl_Persons();
                    objPerson.PersonId = Guid.NewGuid();
                    objPerson.PersonFirstName = person.Firstname;
                    objPerson.PersonAddress = person.Address;
                    //objPerson.MobileNo = person.MobileNo;
                    objPerson.DailyRate = person.DailyRate;
                    objPerson.PersonTypeId = person.PersonTypeId;
                    //objPerson.PersonPhoto = fileName;
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

        [HttpPost]
        public string ChangePersonStatus(Guid PersonId, bool Status)
        {
            string ReturnMessage = "";

            try
            {
                tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == PersonId && x.IsDeleted == false).FirstOrDefault();

                if (objPerson == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objPerson.IsActive = Status;
                    objPerson.ModifiedDate = DateTime.UtcNow;
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

        public ActionResult PersonGroup()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());
            List<PersonGroupVM> objGroup = new List<PersonGroupVM>();

            objGroup = (from grp in _db.tbl_PersonGroup
                        where grp.ClientId == ClientId
                        select new PersonGroupVM
                        {
                            PersonGroupId = grp.PersonGroupId,
                            GroupName = grp.GroupName,
                            TotalGroupPerson = _db.tbl_PersonGroupMap.Where(x => x.PersonGroupId == grp.PersonGroupId).ToList().Count()
                        }).ToList();

            return View(objGroup);
        }

        public ActionResult AddPersonGroup()
        {
            PersonGroupVM objGroup = new PersonGroupVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            objGroup.PersonList = _db.tbl_Persons.Where(x => x.IsActive == true && x.IsDeleted == false && x.IsAttendancePerson == true && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.PersonId.ToString(), Text = o.PersonFirstName })
                         .OrderBy(x => x.Text).ToList();

            return View(objGroup);
        }

        public ActionResult EditPersonGroup(Guid id) // id = PersonGroupId
        {
            PersonGroupVM response = new PersonGroupVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            response.PersonList = _db.tbl_Persons.Where(x => x.IsActive == true && x.IsDeleted == false && x.IsAttendancePerson == true && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.PersonId.ToString(), Text = o.PersonFirstName })
                         .OrderBy(x => x.Text).ToList();

            tbl_PersonGroup objPersonGroup = _db.tbl_PersonGroup.Where(x => x.PersonGroupId == id).FirstOrDefault();
            List<tbl_PersonGroupMap> lstGroupMap = _db.tbl_PersonGroupMap.Where(x => x.PersonGroupId == id).ToList();

            response.SelectedPersonList = lstGroupMap
                         .Select(o => new SelectedPersonGroupVM { PersonGroupId = o.PersonGroupId, PersonId = o.PersonId })
                         .ToList();

            response.PersonGroupId = objPersonGroup.PersonGroupId;
            response.GroupName = objPersonGroup.GroupName;

            return View(response);
        }

        [HttpPost]
        public ActionResult SavePersonGroup(string groupName, Array personGroup)
        {
            GeneralResponse response = new GeneralResponse();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // Save Group Name
                    tbl_PersonGroup objGroup = new tbl_PersonGroup();
                    objGroup.PersonGroupId = Guid.NewGuid();
                    objGroup.GroupName = groupName;
                    objGroup.ClientId = ClientId;
                    objGroup.CreatedBy = new Guid(clsSession.UserID.ToString());
                    objGroup.CreatedDate = DateTime.UtcNow;
                    _db.tbl_PersonGroup.Add(objGroup);
                    _db.SaveChanges();

                    // Save Group in Person
                    foreach (var item in personGroup)
                    {
                        tbl_PersonGroupMap objGroupMap = new tbl_PersonGroupMap();
                        objGroupMap.PersonGroupMapId = Guid.NewGuid();
                        objGroupMap.PersonGroupId = objGroup.PersonGroupId;
                        objGroupMap.PersonId = new Guid(item.ToString());
                        _db.tbl_PersonGroupMap.Add(objGroupMap);
                        _db.SaveChanges();
                    }

                    response.IsError = false;
                    response.RedirectUrl = Url.Action("PersonGroup", "Attendance");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsError = true;
                    response.ErrorMessage = ex.Message.ToString();
                }

            }
            return Json(response);
        }

        [HttpPost]
        public string DeletePersonGroup(Guid PersonGroupId)
        {
            string ReturnMessage = "";

            try
            {
                tbl_PersonGroup objGroup = _db.tbl_PersonGroup.Where(x => x.PersonGroupId == PersonGroupId).FirstOrDefault();

                if (objGroup == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    List<tbl_Pagar> lstGroupPagar = _db.tbl_Pagar.Where(x => x.GroupId == PersonGroupId).ToList();
                    if (lstGroupPagar != null && lstGroupPagar.Count > 0)
                    {
                        ReturnMessage = "cannotdelete";
                    }
                    else
                    {
                        List<tbl_PersonGroupMap> lstGroupMap = _db.tbl_PersonGroupMap.Where(x => x.PersonGroupId == PersonGroupId).ToList();
                        if (lstGroupMap.Count > 0)
                        {
                            _db.tbl_PersonGroupMap.RemoveRange(lstGroupMap);
                        }
                        _db.tbl_PersonGroup.Remove(objGroup);
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

        [HttpPost]
        public ActionResult UpdatePersonGroup(Guid personGroupId, string groupName, Array personGroup)
        {
            GeneralResponse response = new GeneralResponse();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // Save Group Name
                    tbl_PersonGroup objGroup = _db.tbl_PersonGroup.Where(x => x.PersonGroupId == personGroupId).FirstOrDefault();
                    objGroup.GroupName = groupName;
                    objGroup.ModifiedBy = new Guid(clsSession.UserID.ToString());
                    objGroup.ModifiedDate = DateTime.UtcNow;
                    _db.SaveChanges();

                    //Delete existing before insert
                    List<tbl_PersonGroupMap> lstGroupMap = _db.tbl_PersonGroupMap.Where(x => x.PersonGroupId == personGroupId).ToList();
                    _db.tbl_PersonGroupMap.RemoveRange(lstGroupMap);

                    // Save Group in Person
                    foreach (var item in personGroup)
                    {
                        tbl_PersonGroupMap objGroupMap = new tbl_PersonGroupMap();
                        objGroupMap.PersonGroupMapId = Guid.NewGuid();
                        objGroupMap.PersonGroupId = objGroup.PersonGroupId;
                        objGroupMap.PersonId = new Guid(item.ToString());
                        _db.tbl_PersonGroupMap.Add(objGroupMap);
                        _db.SaveChanges();
                    }

                    response.IsError = false;
                    response.RedirectUrl = Url.Action("PersonGroup", "Attendance");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    response.IsError = true;
                    response.ErrorMessage = ex.Message.ToString();
                }

            }
            return Json(response);
        }

        public ActionResult ViewPersonGroup(Guid Id, string duration, string start, string end)
        {

            GroupAttendanceStatusVM objGroupStatus = new GroupAttendanceStatusVM();

            if (string.IsNullOrEmpty(duration))
                duration = "month";

            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            ViewBag.Duration = duration;
            ViewBag.StartDate = start;
            ViewBag.EndDate = end;

            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            if (duration == "month")
            {
                var myDate = DateTime.Now;
                startDate = new DateTime(myDate.Year, myDate.Month, 1);

                DateTime lastDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);
                endDate = lastDay;
            }
            else if (duration == "custom")
            {
                if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
                {
                    startDate = DateTime.ParseExact(start, "dd/MM/yyyy", null);
                    endDate = DateTime.ParseExact(end, "dd/MM/yyyy", null);
                }
            }

            objGroupStatus = getGroupAttendanceByFilter(Id, startDate, endDate);

            return View(objGroupStatus);
        }

        public GroupAttendanceStatusVM getGroupAttendanceByFilter(Guid Id, DateTime startDate, DateTime endDate)
        {
            GroupAttendanceStatusVM objGroupStatus = new GroupAttendanceStatusVM();

            // Get Group Info
            tbl_PersonGroup objGroupInfo = _db.tbl_PersonGroup.Where(x => x.PersonGroupId == Id).FirstOrDefault();

            objGroupStatus.PersonGroupId = objGroupInfo.PersonGroupId;
            objGroupStatus.GroupName = objGroupInfo.GroupName;

            // Get Group Persons
            List<tbl_PersonGroupMap> lstGroupMapInfo = _db.tbl_PersonGroupMap.Where(x => x.PersonGroupId == Id).ToList();

            // Get Attendance of each Person
            List<GroupPersonPaymentInfoVM> lstGroupPersonPayment = new List<GroupPersonPaymentInfoVM>();

            lstGroupMapInfo.ForEach(person =>
            {

                // Get Person data
                tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == person.PersonId).FirstOrDefault();

                if (objPerson != null)
                {
                    List<ReportPersonAttendanceVM> personReport = GetPersonAttendance(person.PersonId, startDate, endDate);

                    GroupPersonPaymentInfoVM objGroupPersonPayment = new GroupPersonPaymentInfoVM();
                    objGroupPersonPayment.PersonId = person.PersonId;
                    objGroupPersonPayment.PersonName = objPerson.PersonFirstName;

                    objGroupPersonPayment.TotalAttendance = personReport.Sum(x => x.AttendanceStatus);
                    objGroupPersonPayment.TotalPayableAmount = personReport.Sum(x => x.PayableAmount);
                    objGroupPersonPayment.TotalWithdrawAmount = personReport.Sum(x => x.WithdrawAmount);
                    objGroupPersonPayment.TotalOvertimeAmount = personReport.Sum(x => x.OvertimeAmount);

                    objGroupPersonPayment.TotalRemainingAmount = (objGroupPersonPayment.TotalPayableAmount + objGroupPersonPayment.TotalOvertimeAmount) - objGroupPersonPayment.TotalWithdrawAmount;

                    lstGroupPersonPayment.Add(objGroupPersonPayment);
                }

            });

            if (lstGroupPersonPayment.Count > 0)
            {
                objGroupStatus.GrandAttendance = lstGroupPersonPayment.Sum(x => x.TotalAttendance);
                objGroupStatus.GrandPayableAmount = lstGroupPersonPayment.Sum(x => x.TotalPayableAmount);
                objGroupStatus.GrandWithdrawAmount = lstGroupPersonPayment.Sum(x => x.TotalWithdrawAmount);
                objGroupStatus.GrandOvertimeAmount = lstGroupPersonPayment.Sum(x => x.TotalOvertimeAmount);
                objGroupStatus.GrandRemainingAmount = (objGroupStatus.GrandPayableAmount + objGroupStatus.GrandOvertimeAmount) - objGroupStatus.GrandWithdrawAmount;
            }

            objGroupStatus.GroupPersonPayment = lstGroupPersonPayment;

            return objGroupStatus;

        }

        private List<ReportPersonAttendanceVM> GetPersonAttendance(Guid PersonId, DateTime startDate, DateTime endDate)
        {
            List<ReportPersonAttendanceVM> lst = (
                    from attper in _db.tbl_PersonAttendance
                    join att in _db.tbl_Attendance on attper.AttendanceId equals att.AttendaceId into outerJoinAttendance
                    from att in outerJoinAttendance.DefaultIfEmpty()
                    join site in _db.tbl_Sites on attper.SiteId equals site.SiteId into outerJoinSite
                    from site in outerJoinSite.DefaultIfEmpty()
                    where attper.PersonId == PersonId && att.AttendanceDate >= startDate && att.AttendanceDate <= endDate
                    select new ReportPersonAttendanceVM
                    {
                        PersonAttendanceId = attper.PersonAttendanceId,
                        AttendanceDate = att.AttendanceDate,
                        PersonId = attper.PersonId,
                        AttendanceStatus = attper.AttendanceStatus,
                        OvertimeAmount = attper.OvertimeAmount,
                        PersonDailyRate = attper.PersonDailyRate,
                        WithdrawAmount = attper.WithdrawAmount,
                        SiteId = attper.SiteId,
                        SiteName = site.SiteName,
                        PayableAmount = attper.PayableAmount,
                        Remarks = attper.Remarks
                    }).OrderBy(x => x.AttendanceDate).ToList();

            return lst;
        }

        public ActionResult ViewGroupPagar(Guid Id) // Id = PersonGroupId
        {

            // Get Group Info
            tbl_PersonGroup objGroupInfo = _db.tbl_PersonGroup.Where(x => x.PersonGroupId == Id).FirstOrDefault();

            List<PagarVM> lstPagar = (from p in _db.tbl_Pagar
                                      where p.GroupId == Id
                                      select new PagarVM
                                      {
                                          PagarId = p.PagarId,
                                          PagarAmount = p.PagarAmount,
                                          AmountPay = p.AmountPay,
                                          PrevPagarRemainingAmount = p.PrevPagarRemainingAmount,
                                          TotalUpadAmount = p.TotalUpadAmount,
                                          TotalOvertimeAmount = p.TotalOvertimeAmount,
                                          dtPagarStartDate = p.PagarStartDate,
                                          dtPagarEndDate = p.PagarEndDate,
                                          RemainingAmount = p.RemainingAmount
                                      }).OrderByDescending(x => x.dtPagarStartDate).ToList();

            ViewBag.PersonGroupId = Id;
            ViewBag.GroupName = objGroupInfo != null ? objGroupInfo.GroupName : "";

            return View(lstPagar);
        }

        [HttpPost]
        public JsonResult GetNextPagarInfoOfGroup(Guid GroupId)
        {

            PagarVM obj = new PagarVM();
            obj.GroupId = GroupId;

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                // Get Group Persons
                List<tbl_PersonGroupMap> lstGropPersons = _db.tbl_PersonGroupMap.Where(x => x.PersonGroupId == GroupId).ToList();
                List<Guid> lstPersonsIds = lstGropPersons.Select(x => x.PersonId).ToList();

                tbl_Pagar objLastPagar = _db.tbl_Pagar.Where(x => x.GroupId == GroupId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (objLastPagar != null)
                {
                    obj.dtPagarStartDate = objLastPagar.PagarEndDate.AddDays(1);
                    obj.dtPagarEndDate = DateTime.Today;
                    obj.PrevPagarRemainingAmount = objLastPagar.RemainingAmount;
                }
                else
                {
                    // Get minimum StartDate from all persons
                    tbl_Attendance objMinStartAttendanceDate = (from a in _db.tbl_Attendance
                                                                join pa in _db.tbl_PersonAttendance on a.AttendaceId equals pa.AttendanceId
                                                                where lstPersonsIds.Contains(pa.PersonId)
                                                                select a
                                                    ).OrderBy(x => x.AttendanceDate).FirstOrDefault();

                    if (objMinStartAttendanceDate == null)
                        obj.dtPagarStartDate = DateTime.Today;
                    else
                        obj.dtPagarStartDate = objMinStartAttendanceDate.AttendanceDate;

                    obj.dtPagarEndDate = DateTime.Today;
                }

                if (obj.dtPagarStartDate.Date > DateTime.UtcNow.Date)
                    obj.dtPagarStartDate = DateTime.UtcNow;

                obj.PagarStartDate = Convert.ToDateTime(obj.dtPagarStartDate).ToString("dd/MM/yyyy");
                obj.PagarEndDate = Convert.ToDateTime(obj.dtPagarEndDate).ToString("dd/MM/yyyy");

                GroupAttendanceStatusVM GroupAttendanceStatus = getGroupAttendanceByFilter(GroupId, obj.dtPagarStartDate, obj.dtPagarEndDate);

                List<PagarPersonDetailVM> lstPagarPersonDetail = new List<PagarPersonDetailVM>();

                lstGropPersons.ForEach(item =>
                {
                    PagarPersonDetailVM objPagarPersonDetailVM = new PagarPersonDetailVM();
                    objPagarPersonDetailVM.PagarPersonId = item.PersonId;
                    objPagarPersonDetailVM.GroupId = GroupId;


                    List<ReportPersonAttendanceVM> lst = (
                    from attper in _db.tbl_PersonAttendance
                    join att in _db.tbl_Attendance on attper.AttendanceId equals att.AttendaceId into outerJoinAttendance
                    from att in outerJoinAttendance.DefaultIfEmpty()
                    join site in _db.tbl_Sites on attper.SiteId equals site.SiteId into outerJoinSite
                    from site in outerJoinSite.DefaultIfEmpty()
                    where attper.PersonId == item.PersonId && att.AttendanceDate >= obj.dtPagarStartDate && att.AttendanceDate <= obj.dtPagarEndDate
                    select new ReportPersonAttendanceVM
                    {
                        PersonAttendanceId = attper.PersonAttendanceId,
                        AttendanceDate = att.AttendanceDate,
                        PersonId = attper.PersonId,
                        PersonTypeId = attper.PersonTypeId,
                        TotalRokadiya = attper.TotalRokadiya,
                        AttendanceStatus = attper.AttendanceStatus,
                        OvertimeAmount = attper.OvertimeAmount,
                        PersonDailyRate = attper.PersonDailyRate,
                        WithdrawAmount = attper.WithdrawAmount,
                        SiteId = attper.SiteId,
                        SiteName = site.SiteName,
                        PayableAmount = attper.PayableAmount,
                        Remarks = attper.Remarks
                    }).OrderBy(x => x.AttendanceDate).ToList();

                    if (lst != null)
                    {
                        objPagarPersonDetailVM.TotalAttendanceDays = lst.Sum(x => x.AttendanceStatus);
                        objPagarPersonDetailVM.TotalPagarAmount = lst.Sum(x => x.PayableAmount);
                        objPagarPersonDetailVM.TotalWithdrawAmount = (decimal)lst.Sum(x => x.WithdrawAmount);
                        objPagarPersonDetailVM.TotalOvertimeAmount = (decimal)lst.Sum(x => x.OvertimeAmount);
                    }

                    lstPagarPersonDetail.Add(objPagarPersonDetailVM);

                });

                if (lstPagarPersonDetail != null && lstPagarPersonDetail.Count > 0)
                {
                    obj.PagarAmount = Convert.ToDecimal(lstPagarPersonDetail.Sum(x => x.TotalPagarAmount));
                    obj.TotalUpadAmount = Convert.ToDecimal(lstPagarPersonDetail.Sum(x => x.TotalWithdrawAmount));
                    obj.TotalOvertimeAmount = Convert.ToDecimal(lstPagarPersonDetail.Sum(x => x.TotalOvertimeAmount));
                }

                obj.RemainingAmount = (obj.PagarAmount + obj.TotalOvertimeAmount + obj.PrevPagarRemainingAmount) - obj.TotalUpadAmount;
                obj.GroupAttendanceData = GroupAttendanceStatus;
            }
            catch (Exception ex)
            {
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveGroupPagar(string pagardetail)
        {
            string message = "";
            try
            {
                PagarVM pagarVM = JsonConvert.DeserializeObject<PagarVM>(pagardetail);
                pagarVM.dtPagarStartDate = DateTime.ParseExact(pagarVM.PagarStartDate, "dd/MM/yyyy", null);
                pagarVM.dtPagarEndDate = DateTime.ParseExact(pagarVM.PagarEndDate, "dd/MM/yyyy", null);

                List<tbl_PersonGroupMap> lstGropPersons = _db.tbl_PersonGroupMap.Where(x => x.PersonGroupId == pagarVM.GroupId).ToList();
                List<Guid> lstPersonsIds = lstGropPersons.Select(x => x.PersonId).ToList();

                // Save in tbl_Pagar
                tbl_Pagar objPagar = new tbl_Pagar();
                objPagar.PagarId = Guid.NewGuid();
                objPagar.GroupId = pagarVM.GroupId;
                objPagar.PagarStartDate = pagarVM.dtPagarStartDate;
                objPagar.PagarEndDate = pagarVM.dtPagarEndDate;
                objPagar.PagarAmount = pagarVM.PagarAmount;

                objPagar.TotalUpadAmount = pagarVM.TotalUpadAmount;
                objPagar.TotalOvertimeAmount = pagarVM.TotalOvertimeAmount;
                objPagar.AmountPay = pagarVM.AmountPay;
                objPagar.PrevPagarRemainingAmount = pagarVM.PrevPagarRemainingAmount;
                objPagar.RemainingAmount = pagarVM.RemainingAmount;

                objPagar.RemainingAmount = (objPagar.PagarAmount + objPagar.TotalOvertimeAmount + objPagar.PrevPagarRemainingAmount) - (objPagar.AmountPay + objPagar.TotalUpadAmount);

                objPagar.PagarPaidToPerson = null;
                objPagar.Remarks = pagarVM.Remarks;

                objPagar.CreatedBy = clsSession.UserID;
                objPagar.CreatedDate = DateTime.UtcNow;
                objPagar.ModifiedBy = clsSession.UserID;
                objPagar.ModifiedDate = DateTime.UtcNow;

                _db.tbl_Pagar.Add(objPagar);
                _db.SaveChanges();

                lstGropPersons.ForEach(item =>
                {

                    List<ReportPersonAttendanceVM> lst = (
                    from attper in _db.tbl_PersonAttendance
                    join att in _db.tbl_Attendance on attper.AttendanceId equals att.AttendaceId into outerJoinAttendance
                    from att in outerJoinAttendance.DefaultIfEmpty()
                    join site in _db.tbl_Sites on attper.SiteId equals site.SiteId into outerJoinSite
                    from site in outerJoinSite.DefaultIfEmpty()
                    where attper.PersonId == item.PersonId && att.AttendanceDate >= pagarVM.dtPagarStartDate && att.AttendanceDate <= pagarVM.dtPagarEndDate
                    select new ReportPersonAttendanceVM
                    {
                        PersonAttendanceId = attper.PersonAttendanceId,
                        AttendanceDate = att.AttendanceDate,
                        PersonId = attper.PersonId,
                        PersonTypeId = attper.PersonTypeId,
                        TotalRokadiya = attper.TotalRokadiya,
                        AttendanceStatus = attper.AttendanceStatus,
                        OvertimeAmount = attper.OvertimeAmount,
                        PersonDailyRate = attper.PersonDailyRate,
                        WithdrawAmount = attper.WithdrawAmount,
                        SiteId = attper.SiteId,
                        SiteName = site.SiteName,
                        PayableAmount = attper.PayableAmount,
                        Remarks = attper.Remarks
                    }).OrderBy(x => x.AttendanceDate).ToList();

                    tbl_PagarPersonDetail objPagarDetail = new tbl_PagarPersonDetail();
                    objPagarDetail.PagarPersonDetailId = Guid.NewGuid();
                    objPagarDetail.PagarId = objPagar.PagarId;
                    objPagarDetail.PagarPersonId = item.PersonId;
                    objPagarDetail.TotalAttendanceDays = lst.Sum(x => x.AttendanceStatus);
                    objPagarDetail.TotalPagarAmount = lst.Sum(x => x.PayableAmount);
                    objPagarDetail.CreatedDate = DateTime.UtcNow;
                    _db.tbl_PagarPersonDetail.Add(objPagarDetail);
                    _db.SaveChanges();

                });

                message = "SUCCESS";

            }
            catch (Exception ex)
            {
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string DeletePagar(Guid PagarId)
        {
            string ReturnMessage = "";

            try
            {
                tbl_Pagar objGroupPagar = _db.tbl_Pagar.Where(x => x.PagarId == PagarId).FirstOrDefault();

                if (objGroupPagar == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {

                    List<tbl_PagarPersonDetail> lstPagarDetails = _db.tbl_PagarPersonDetail.Where(x => x.PagarId == PagarId).ToList();
                    if (lstPagarDetails.Count > 0)
                    {
                        _db.tbl_PagarPersonDetail.RemoveRange(lstPagarDetails);
                    }
                    _db.tbl_Pagar.Remove(objGroupPagar);
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

        public ActionResult ViewPersonPagar(Guid Id) // Id = PersonId
        {

            // Get Group Info
            tbl_Persons objPersonInfo = _db.tbl_Persons.Where(x => x.PersonId == Id).FirstOrDefault();

            List<PagarVM> lstPagar = (from p in _db.tbl_Pagar
                                      where p.PersonId == Id
                                      select new PagarVM
                                      {
                                          PagarId = p.PagarId,
                                          PagarAmount = p.PagarAmount,
                                          AmountPay = p.AmountPay,
                                          PrevPagarRemainingAmount = p.PrevPagarRemainingAmount,
                                          TotalUpadAmount = p.TotalUpadAmount,
                                          TotalOvertimeAmount = p.TotalOvertimeAmount,
                                          dtPagarStartDate = p.PagarStartDate,
                                          dtPagarEndDate = p.PagarEndDate,
                                          RemainingAmount = p.RemainingAmount
                                      }).OrderByDescending(x => x.dtPagarStartDate).ToList();

            ViewBag.PersonId = Id;
            ViewBag.PersonName = objPersonInfo != null ? objPersonInfo.PersonFirstName : "";

            return View(lstPagar);
        }
         
        [HttpPost]
        public JsonResult GetNextPagarInfoOfPerson(Guid PersonId)
        {

            PagarVM obj = new PagarVM();
            obj.PersonId = PersonId;

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                // Get Group Persons 
                List<Guid> lstPersonsIds = new List<Guid>();
                lstPersonsIds.Add(PersonId);

                tbl_Pagar objLastPagar = _db.tbl_Pagar.Where(x => x.PersonId == PersonId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (objLastPagar != null)
                {
                    obj.dtPagarStartDate = objLastPagar.PagarEndDate.AddDays(1);
                    obj.dtPagarEndDate = DateTime.Today;
                    obj.PrevPagarRemainingAmount = objLastPagar.RemainingAmount;
                }
                else
                {
                    // Get minimum StartDate from all persons
                    tbl_Attendance objMinStartAttendanceDate = (from a in _db.tbl_Attendance
                                                                join pa in _db.tbl_PersonAttendance on a.AttendaceId equals pa.AttendanceId
                                                                where lstPersonsIds.Contains(pa.PersonId)
                                                                select a
                                                    ).OrderBy(x => x.AttendanceDate).FirstOrDefault();

                    if (objMinStartAttendanceDate == null)
                        obj.dtPagarStartDate = DateTime.Today;
                    else
                        obj.dtPagarStartDate = objMinStartAttendanceDate.AttendanceDate;

                    obj.dtPagarEndDate = DateTime.Today;
                }

                if (obj.dtPagarStartDate.Date > DateTime.UtcNow.Date)
                    obj.dtPagarStartDate = DateTime.UtcNow;

                obj.PagarStartDate = Convert.ToDateTime(obj.dtPagarStartDate).ToString("dd/MM/yyyy");
                obj.PagarEndDate = Convert.ToDateTime(obj.dtPagarEndDate).ToString("dd/MM/yyyy");

                GroupAttendanceStatusVM GroupAttendanceStatus = getPersonAttendanceByFilter(PersonId, obj.dtPagarStartDate, obj.dtPagarEndDate);

                List<PagarPersonDetailVM> lstPagarPersonDetail = new List<PagarPersonDetailVM>();

                lstPersonsIds.ForEach(personId =>
                {
                    PagarPersonDetailVM objPagarPersonDetailVM = new PagarPersonDetailVM();
                    objPagarPersonDetailVM.PagarPersonId = personId;
                      
                    List<ReportPersonAttendanceVM> lst = (
                    from attper in _db.tbl_PersonAttendance
                    join att in _db.tbl_Attendance on attper.AttendanceId equals att.AttendaceId into outerJoinAttendance
                    from att in outerJoinAttendance.DefaultIfEmpty()
                    join site in _db.tbl_Sites on attper.SiteId equals site.SiteId into outerJoinSite
                    from site in outerJoinSite.DefaultIfEmpty()
                    where attper.PersonId == personId && att.AttendanceDate >= obj.dtPagarStartDate && att.AttendanceDate <= obj.dtPagarEndDate
                    select new ReportPersonAttendanceVM
                    {
                        PersonAttendanceId = attper.PersonAttendanceId,
                        AttendanceDate = att.AttendanceDate,
                        PersonId = attper.PersonId,
                        PersonTypeId = attper.PersonTypeId,
                        TotalRokadiya = attper.TotalRokadiya,
                        AttendanceStatus = attper.AttendanceStatus,
                        OvertimeAmount = attper.OvertimeAmount,
                        PersonDailyRate = attper.PersonDailyRate,
                        WithdrawAmount = attper.WithdrawAmount,
                        SiteId = attper.SiteId,
                        SiteName = site.SiteName,
                        PayableAmount = attper.PayableAmount,
                        Remarks = attper.Remarks
                    }).OrderBy(x => x.AttendanceDate).ToList();

                    if (lst != null)
                    {
                        objPagarPersonDetailVM.TotalAttendanceDays = lst.Sum(x => x.AttendanceStatus);
                        objPagarPersonDetailVM.TotalPagarAmount = lst.Sum(x => x.PayableAmount);
                        objPagarPersonDetailVM.TotalWithdrawAmount = (decimal)lst.Sum(x => x.WithdrawAmount);
                        objPagarPersonDetailVM.TotalOvertimeAmount = (decimal)lst.Sum(x => x.OvertimeAmount);
                    }

                    lstPagarPersonDetail.Add(objPagarPersonDetailVM);

                });

                if (lstPagarPersonDetail != null && lstPagarPersonDetail.Count > 0)
                {
                    obj.PagarAmount = Convert.ToDecimal(lstPagarPersonDetail.Sum(x => x.TotalPagarAmount));
                    obj.TotalUpadAmount = Convert.ToDecimal(lstPagarPersonDetail.Sum(x => x.TotalWithdrawAmount));
                    obj.TotalOvertimeAmount = Convert.ToDecimal(lstPagarPersonDetail.Sum(x => x.TotalOvertimeAmount));
                }

                obj.RemainingAmount = (obj.PagarAmount + obj.TotalOvertimeAmount + obj.PrevPagarRemainingAmount) - obj.TotalUpadAmount;
                obj.GroupAttendanceData = GroupAttendanceStatus;
            }
            catch (Exception ex)
            {
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public GroupAttendanceStatusVM getPersonAttendanceByFilter(Guid Id, DateTime startDate, DateTime endDate)
        {
            GroupAttendanceStatusVM objGroupStatus = new GroupAttendanceStatusVM();
             
            tbl_Persons objPersonInfo = _db.tbl_Persons.Where(x => x.PersonId == Id).FirstOrDefault();

            objGroupStatus.PersonGroupId = Id;
            objGroupStatus.GroupName = objPersonInfo.PersonFirstName;
             
            List<Guid> lstPersons = new List<Guid>();
            lstPersons.Add(Id);
             
            // Get Attendance of each Person
            List<GroupPersonPaymentInfoVM> lstGroupPersonPayment = new List<GroupPersonPaymentInfoVM>();

            lstPersons.ForEach(personId =>
            {

                // Get Person data
                tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == personId).FirstOrDefault();

                if (objPerson != null)
                {
                    List<ReportPersonAttendanceVM> personReport = GetPersonAttendance(personId, startDate, endDate);

                    GroupPersonPaymentInfoVM objGroupPersonPayment = new GroupPersonPaymentInfoVM();
                    objGroupPersonPayment.PersonId = personId;
                    objGroupPersonPayment.PersonName = objPerson.PersonFirstName;

                    objGroupPersonPayment.TotalAttendance = personReport.Sum(x => x.AttendanceStatus);
                    objGroupPersonPayment.TotalPayableAmount = personReport.Sum(x => x.PayableAmount);
                    objGroupPersonPayment.TotalWithdrawAmount = personReport.Sum(x => x.WithdrawAmount);
                    objGroupPersonPayment.TotalOvertimeAmount = personReport.Sum(x => x.OvertimeAmount);

                    objGroupPersonPayment.TotalRemainingAmount = (objGroupPersonPayment.TotalPayableAmount + objGroupPersonPayment.TotalOvertimeAmount) - objGroupPersonPayment.TotalWithdrawAmount;

                    lstGroupPersonPayment.Add(objGroupPersonPayment);
                }

            });

            if (lstGroupPersonPayment.Count > 0)
            {
                objGroupStatus.GrandAttendance = lstGroupPersonPayment.Sum(x => x.TotalAttendance);
                objGroupStatus.GrandPayableAmount = lstGroupPersonPayment.Sum(x => x.TotalPayableAmount);
                objGroupStatus.GrandWithdrawAmount = lstGroupPersonPayment.Sum(x => x.TotalWithdrawAmount);
                objGroupStatus.GrandOvertimeAmount = lstGroupPersonPayment.Sum(x => x.TotalOvertimeAmount);
                objGroupStatus.GrandRemainingAmount = (objGroupStatus.GrandPayableAmount + objGroupStatus.GrandOvertimeAmount) - objGroupStatus.GrandWithdrawAmount;
            }

            objGroupStatus.GroupPersonPayment = lstGroupPersonPayment;

            return objGroupStatus;

        }
         
        [HttpPost]
        public JsonResult SavePersonPagar(string pagardetail)
        {
            string message = "";
            try
            {
                PagarVM pagarVM = JsonConvert.DeserializeObject<PagarVM>(pagardetail);
                pagarVM.dtPagarStartDate = DateTime.ParseExact(pagarVM.PagarStartDate, "dd/MM/yyyy", null);
                pagarVM.dtPagarEndDate = DateTime.ParseExact(pagarVM.PagarEndDate, "dd/MM/yyyy", null);
                 
                List<Guid> lstPersonsIds = new List<Guid>();
                lstPersonsIds.Add((Guid)pagarVM.PersonId);

                // Save in tbl_Pagar
                tbl_Pagar objPagar = new tbl_Pagar();
                objPagar.PagarId = Guid.NewGuid();
                objPagar.PersonId = pagarVM.PersonId;
                objPagar.PagarStartDate = pagarVM.dtPagarStartDate;
                objPagar.PagarEndDate = pagarVM.dtPagarEndDate;
                objPagar.PagarAmount = pagarVM.PagarAmount;

                objPagar.TotalUpadAmount = pagarVM.TotalUpadAmount;
                objPagar.TotalOvertimeAmount = pagarVM.TotalOvertimeAmount;
                objPagar.AmountPay = pagarVM.AmountPay;
                objPagar.PrevPagarRemainingAmount = pagarVM.PrevPagarRemainingAmount;
                objPagar.RemainingAmount = pagarVM.RemainingAmount;

                objPagar.RemainingAmount = (objPagar.PagarAmount + objPagar.TotalOvertimeAmount + objPagar.PrevPagarRemainingAmount) - (objPagar.AmountPay + objPagar.TotalUpadAmount);

                objPagar.PagarPaidToPerson = null;
                objPagar.Remarks = pagarVM.Remarks;

                objPagar.CreatedBy = clsSession.UserID;
                objPagar.CreatedDate = DateTime.UtcNow;
                objPagar.ModifiedBy = clsSession.UserID;
                objPagar.ModifiedDate = DateTime.UtcNow;

                _db.tbl_Pagar.Add(objPagar);
                _db.SaveChanges();

                lstPersonsIds.ForEach(personId =>
                {

                    List<ReportPersonAttendanceVM> lst = (
                    from attper in _db.tbl_PersonAttendance
                    join att in _db.tbl_Attendance on attper.AttendanceId equals att.AttendaceId into outerJoinAttendance
                    from att in outerJoinAttendance.DefaultIfEmpty()
                    join site in _db.tbl_Sites on attper.SiteId equals site.SiteId into outerJoinSite
                    from site in outerJoinSite.DefaultIfEmpty()
                    where attper.PersonId == personId && att.AttendanceDate >= pagarVM.dtPagarStartDate && att.AttendanceDate <= pagarVM.dtPagarEndDate
                    select new ReportPersonAttendanceVM
                    {
                        PersonAttendanceId = attper.PersonAttendanceId,
                        AttendanceDate = att.AttendanceDate,
                        PersonId = attper.PersonId,
                        PersonTypeId = attper.PersonTypeId,
                        TotalRokadiya = attper.TotalRokadiya,
                        AttendanceStatus = attper.AttendanceStatus,
                        OvertimeAmount = attper.OvertimeAmount,
                        PersonDailyRate = attper.PersonDailyRate,
                        WithdrawAmount = attper.WithdrawAmount,
                        SiteId = attper.SiteId,
                        SiteName = site.SiteName,
                        PayableAmount = attper.PayableAmount,
                        Remarks = attper.Remarks
                    }).OrderBy(x => x.AttendanceDate).ToList();

                    tbl_PagarPersonDetail objPagarDetail = new tbl_PagarPersonDetail();
                    objPagarDetail.PagarPersonDetailId = Guid.NewGuid();
                    objPagarDetail.PagarId = objPagar.PagarId;
                    objPagarDetail.PagarPersonId = personId;
                    objPagarDetail.TotalAttendanceDays = lst.Sum(x => x.AttendanceStatus);
                    objPagarDetail.TotalPagarAmount = lst.Sum(x => x.PayableAmount);
                    objPagarDetail.CreatedDate = DateTime.UtcNow;
                    _db.tbl_PagarPersonDetail.Add(objPagarDetail);
                    _db.SaveChanges();

                });

                message = "SUCCESS";

            }
            catch (Exception ex)
            {
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }
        
    }
}