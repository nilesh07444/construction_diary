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
using Newtonsoft.Json;
using ConstructionDiary.ResourceFiles;
using System.Security.Policy;

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

        public ActionResult Index(string duration, string start, string end)
        {
            if (string.IsNullOrEmpty(duration))
                duration = "month";

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

            //

            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<AttendanceVM> lstAttendance = (from atte in _db.tbl_Attendance
                                                join createdByUser in _db.tbl_Users on atte.CreatedBy equals createdByUser.UserId into outerJoinCreatedBy
                                                from createdByUser in outerJoinCreatedBy.DefaultIfEmpty()
                                                join updatedByUser in _db.tbl_Users on atte.ModifiedBy equals updatedByUser.UserId into outerJoinUpdatedBy
                                                from updatedByUser in outerJoinUpdatedBy.DefaultIfEmpty()
                                                where atte.ClientId == ClientId
                                                && atte.AttendanceDate >= startDate && atte.AttendanceDate <= endDate
                                                select new AttendanceVM
                                                {
                                                    ClientId = atte.ClientId,
                                                    AttendanceDate = atte.AttendanceDate,
                                                    AttendaceId = atte.AttendaceId,
                                                    TotalPaidAmount = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.PayableAmount != null).Select(x => x.PayableAmount).Sum(),

                                                    CreatedBy = (createdByUser != null ? createdByUser.FirstName : ""),
                                                    UpdatedBy = (updatedByUser != null ? updatedByUser.FirstName : "")

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
                var personList = _db.tbl_Persons.Where(x => x.ClientId == ClientId && x.IsAttendancePerson == true && x.IsActive == true && x.IsDeleted == false)
                    .OrderBy(x => x.OrderNo).ThenBy(x => x.PersonFirstName).ToList();
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
                    objAttandance.CreatedBy = new Guid(clsSession.UserID.ToString());
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

                AttendanceVM objAttendanceData = (from atte in _db.tbl_Attendance
                                    join createdByUser in _db.tbl_Users on atte.CreatedBy equals createdByUser.UserId into outerJoinCreatedBy
                                    from createdByUser in outerJoinCreatedBy.DefaultIfEmpty()
                                    join updatedByUser in _db.tbl_Users on atte.ModifiedBy equals updatedByUser.UserId into outerJoinUpdatedBy
                                    from updatedByUser in outerJoinUpdatedBy.DefaultIfEmpty()
                                    where atte.AttendaceId == Id
                                    select new AttendanceVM
                                    {
                                        ClientId = atte.ClientId,
                                        AttendanceDate = atte.AttendanceDate,
                                        AttendaceId = atte.AttendaceId,
                                        TotalPaidAmount = _db.tbl_PersonAttendance.Where(x => x.AttendanceId == atte.AttendaceId && x.PayableAmount != null).Select(x => x.PayableAmount).Sum(),

                                        CreatedBy = (createdByUser != null ? createdByUser.FirstName : ""),
                                        UpdatedBy = (updatedByUser != null ? updatedByUser.FirstName : "")
                                         
                                    }).FirstOrDefault();

                //tbl_Attendance objAttendanceData = _db.tbl_Attendance.Where(x => x.AttendaceId == Id).FirstOrDefault();
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
                                                         Remarks = personatta.Remarks,
                                                         OrderNo = person.OrderNo
                                                     }).OrderBy(x => x.OrderNo).ThenBy(x => x.PersonName).ToList();

                objAttendance.CreatedBy = objAttendanceData.CreatedBy;
                objAttendance.UpdatedBy = objAttendanceData.UpdatedBy;

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
                    objAttandance.ModifiedBy = new Guid(clsSession.UserID.ToString());
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

        public ActionResult PersonList(string ftraction)
        {
            bool IsActiveFilter = true;

            if (!string.IsNullOrEmpty(ftraction))
            {
                if (ftraction == "active")
                {
                    IsActiveFilter = true;
                }
                else
                {
                    IsActiveFilter = false;
                }
            }

            ViewBag.ftrAction = ftraction;

            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<AttendancePersonVM> lstPersonsNew = (from p in _db.tbl_Persons
                                                      where p.ClientId == ClientId && p.IsAttendancePerson == true && p.IsDeleted == false
                                                      && (string.IsNullOrEmpty(ftraction) || p.IsActive == IsActiveFilter)
                                                      select new AttendancePersonVM
                                                      {
                                                          PersonId = p.PersonId,
                                                          PersonFirstName = p.PersonFirstName,
                                                          IsActive = p.IsActive,
                                                          IsGroupPerson = _db.tbl_PersonGroupMap.Where(x => x.PersonId == p.PersonId).Any(),
                                                          OrderNo = p.OrderNo
                                                      }).OrderBy(x => x.OrderNo).ThenBy(x => x.PersonFirstName).ToList();

            lstPersonsNew.ForEach(item =>
            {
                if (item.IsGroupPerson)
                    item.PersonGroupId = _db.tbl_PersonGroupMap.Where(x => x.PersonId == item.PersonId).FirstOrDefault().PersonGroupId;


                if (item.IsGroupPerson)
                {
                    item.RemainingAmount = GetRemainingAmountOfGroup(item.PersonGroupId.Value);
                }
                else
                {
                    item.RemainingAmount = GetRemainingAmountOfPerson(item.PersonId);
                }
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
            List<PersonGroupVM> lstGroups = new List<PersonGroupVM>();

            lstGroups = (from grp in _db.tbl_PersonGroup
                         where grp.ClientId == ClientId
                         select new PersonGroupVM
                         {
                             PersonGroupId = grp.PersonGroupId,
                             GroupName = grp.GroupName,
                             TotalGroupPerson = _db.tbl_PersonGroupMap.Where(x => x.PersonGroupId == grp.PersonGroupId).ToList().Count()
                         }).ToList();

            if (lstGroups != null && lstGroups.Count > 0)
            {
                lstGroups.ForEach(item =>
                {
                    item.RemainingAmount = GetRemainingAmountOfGroup(item.PersonGroupId);
                });
            }

            return View(lstGroups);
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
        public JsonResult GetNextPagarInfoOfGroup(Guid GroupId, string strSelectedPagarDate)
        {

            PagarVM obj = new PagarVM();
            obj.GroupId = GroupId;

            try
            {
                DateTime SelectedPagarDate = DateTime.ParseExact(strSelectedPagarDate, "dd/MM/yyyy", null);
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                // Get Group Persons
                List<tbl_PersonGroupMap> lstGropPersons = _db.tbl_PersonGroupMap.Where(x => x.PersonGroupId == GroupId).ToList();
                List<Guid> lstPersonsIds = lstGropPersons.Select(x => x.PersonId).ToList();

                tbl_Pagar objLastPagar = _db.tbl_Pagar.Where(x => x.GroupId == GroupId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (objLastPagar != null)
                {
                    obj.dtPagarStartDate = objLastPagar.PagarEndDate.AddDays(1);
                    obj.dtPagarEndDate = SelectedPagarDate;
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

                    obj.dtPagarEndDate = SelectedPagarDate;
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

                objPagar.RemainingAmount = (objPagar.PagarAmount + objPagar.PrevPagarRemainingAmount) - (objPagar.AmountPay);

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
        public JsonResult GetNextPagarInfoOfPerson(Guid PersonId, string strSelectedPagarDate)
        {

            PagarVM obj = new PagarVM();
            obj.PersonId = PersonId;

            try
            {
                DateTime SelectedPagarDate = DateTime.ParseExact(strSelectedPagarDate, "dd/MM/yyyy", null);

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                // Get Group Persons 
                List<Guid> lstPersonsIds = new List<Guid>();
                lstPersonsIds.Add(PersonId);

                tbl_Pagar objLastPagar = _db.tbl_Pagar.Where(x => x.PersonId == PersonId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (objLastPagar != null)
                {
                    obj.dtPagarStartDate = objLastPagar.PagarEndDate.AddDays(1);
                    obj.dtPagarEndDate = SelectedPagarDate;
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

                    obj.dtPagarEndDate = SelectedPagarDate;
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
                    where attper.PersonId == personId
                    && att.AttendanceDate >= obj.dtPagarStartDate
                    && att.AttendanceDate <= obj.dtPagarEndDate
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

                objPagar.RemainingAmount = (objPagar.PagarAmount + objPagar.PrevPagarRemainingAmount) - (objPagar.AmountPay);

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

        public string ExportPDFOfGroupPagar(Guid id) // id = pagarid
        {
            PagarVM objPagar = new PagarVM();

            string Result = "";

            try
            {
                tbl_Pagar personGroup = _db.tbl_Pagar.Where(x => x.PagarId == id).FirstOrDefault();

                objPagar.PagarId = id;
                objPagar.dtPagarStartDate = personGroup.PagarStartDate;
                objPagar.dtPagarEndDate = personGroup.PagarEndDate;
                objPagar.GroupId = personGroup.GroupId;
                objPagar.PagarAmount = personGroup.PagarAmount;
                objPagar.TotalUpadAmount = personGroup.TotalUpadAmount;
                objPagar.TotalOvertimeAmount = personGroup.TotalOvertimeAmount;
                objPagar.AmountPay = personGroup.AmountPay;
                objPagar.PrevPagarRemainingAmount = personGroup.PrevPagarRemainingAmount;
                objPagar.RemainingAmount = personGroup.RemainingAmount;
                objPagar.Remarks = personGroup.Remarks;

                List<PagarPersonDetailVM> lstPagarPersonDetail = (from detail in _db.tbl_PagarPersonDetail
                                                                  join person in _db.tbl_Persons on detail.PagarPersonId equals person.PersonId
                                                                  where detail.PagarId == id
                                                                  select new PagarPersonDetailVM
                                                                  {
                                                                      PagarPersonDetailId = detail.PagarPersonDetailId,
                                                                      PagarId = detail.PagarId,
                                                                      PagarPersonId = detail.PagarPersonId,
                                                                      TotalAttendanceDays = detail.TotalAttendanceDays,
                                                                      TotalPagarAmount = detail.TotalPagarAmount,
                                                                      PersonName = person.PersonFirstName
                                                                  }).ToList();

                tbl_PersonGroup objPersonGroup = _db.tbl_PersonGroup.Where(x => x.PersonGroupId == objPagar.GroupId).FirstOrDefault();
                string GroupName = objPersonGroup.GroupName;

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                decimal? TotalDays = lstPagarPersonDetail.Select(x => x.TotalAttendanceDays).Sum();
                string strTotalDays = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalDays));

                decimal? TotalPagarAmount = lstPagarPersonDetail.Select(x => x.TotalPagarAmount).Sum();
                string strTotalPagarAmount = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalPagarAmount));

                string[] strColumns = new string[3] { "Person Name", "Total Days", "Amount" };

                if (lstPagarPersonDetail != null && lstPagarPersonDetail.Count() > 0)
                {

                    List<DateTime> lstDateTemp = new List<DateTime>();
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<!DOCTYPE html>");
                    strHTML.Append("<style>");
                    strHTML.Append("@page {@bottom-center {content: \"Page \" counter(page) \" of \" counter(pages);}}");
                    strHTML.Append("</style>");

                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<thead style=\"display:table-header-group;\">");
                    string Title = "Pagar Of " + GroupName;

                    strHTML.Append("<tr>");
                    strHTML.Append("<th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #000000\">");
                    strHTML.Append(Title);
                    strHTML.Append("</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("<tr><th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #000000\">From " + objPagar.dtPagarStartDate.ToString("dd/MM/yyyy") + " To " + objPagar.dtPagarEndDate.ToString("dd/MM/yyyy") + " </th></tr>");
                    strHTML.Append("<tr>");

                    for (int idx = 0; idx < strColumns.Length; idx++)
                    {
                        strHTML.Append("<th style=\"border: 1px solid #000000\">");
                        strHTML.Append(strColumns[idx]);
                        strHTML.Append("</th>");
                    }

                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody>");
                    foreach (var obj in lstPagarPersonDetail)
                    {

                        if (obj != null)
                        {

                            strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                            for (int Col = 0; Col < strColumns.Length; Col++)
                            {
                                string strcolval = "";
                                switch (strColumns[Col])
                                {

                                    case "Person Name":
                                        {
                                            strcolval = obj.PersonName;
                                            break;
                                        }
                                    case "Total Days":
                                        {
                                            strcolval = Convert.ToDecimal(obj.TotalAttendanceDays).ToString("0.##");
                                            break;
                                        }
                                    case "Amount":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.TotalPagarAmount);
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }

                                }
                                strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\">");
                                strHTML.Append(strcolval);
                                strHTML.Append("</td>");
                            }
                            strHTML.Append("</tr>");
                        }
                    }

                    // Total
                    strHTML.Append("<tr>");
                    strHTML.Append("<th style='text-align:right; border: 1px solid #000000;'>Total</th>");
                    strHTML.Append("<th style='border: 1px solid #000000;'> " + strTotalDays + " </th>");
                    strHTML.Append("<th style='border: 1px solid #000000;'> " + strTotalPagarAmount + " </th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("</tbody>");
                    strHTML.Append("</table>");

                    // Calculation table

                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='margin-top:20px; width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<tbody>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"> " + Resource.TotalPagar + "</td>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"><strong> " + objPagar.PagarAmount.ToString("0.##") + "</strong></td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"> " + Resource.Withdraw + "</td>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"> " + objPagar.TotalUpadAmount.ToString("0.##") + "</td>");
                    strHTML.Append("</tr>");

                    decimal AfterWithdrawLessAmt = objPagar.PagarAmount - objPagar.TotalUpadAmount;

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"></td>"); // " + Resource.TotalPagar + "
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"> <strong>" + AfterWithdrawLessAmt.ToString("0.##") + "</strong></td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"> " + Resource.Overtime + "</td>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"> " + objPagar.TotalOvertimeAmount.ToString("0.##") + "</td>");
                    strHTML.Append("</tr>");

                    decimal AfterOvertimeLessAmt = AfterWithdrawLessAmt + objPagar.TotalOvertimeAmount;

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"></td>"); // " + Resource.TotalPagar + "
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"><strong> " + AfterOvertimeLessAmt.ToString("0.##") + "</strong></td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\">" + Resource.PreviousPagarAmount + "</td>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"> " + objPagar.PrevPagarRemainingAmount.ToString("0.##") + "</td>");
                    strHTML.Append("</tr>");

                    decimal CreditDebitAmount = AfterOvertimeLessAmt + objPagar.PrevPagarRemainingAmount;

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"></td>"); // " + Resource.PrevCreditDebit + "
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"> <strong>" + CreditDebitAmount.ToString("0.##") + "</strong></td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\">" + Resource.TotalGivenAmount + "</td>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"> " + objPagar.AmountPay.ToString("0.##") + "</td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\">" + Resource.RemainingAmount + "</td>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"><strong> " + objPagar.RemainingAmount.ToString("0.##") + "</strong></td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\">" + Resource.Remarks + "</td>");
                    strHTML.Append("<td style=\"width: auto; text-align:right; border: 1px solid #000000\"> " + objPagar.Remarks + "</td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("</tbody>");
                    strHTML.Append("</table>");

                    StringReader sr = new StringReader(strHTML.ToString());

                    var myString = strHTML.ToString();
                    var myByteArray = System.Text.Encoding.UTF8.GetBytes(myString);
                    var ms = new MemoryStream(myByteArray);

                    Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    writer.PageEvent = new PDFGeneratePageEventHelper();
                    pdfDoc.Open();

                    XMLWorkerHelper objHelp = XMLWorkerHelper.GetInstance();
                    objHelp.ParseXHtml(writer, pdfDoc, ms, null, Encoding.UTF8, new UnicodeFontFactory());

                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "download;filename=Pagar Of " + GroupName + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }

                return Result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
            }

        }

        public ActionResult ReOrderPerson()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            List<AttendancePersonVM> lstPersons = (from p in _db.tbl_Persons
                                                   join t in _db.tbl_PersonType on p.PersonTypeId equals t.Id
                                                   where p.ClientId == ClientId && p.IsAttendancePerson == true && p.IsDeleted == false && p.IsActive == true
                                                   select new AttendancePersonVM
                                                   {
                                                       PersonId = p.PersonId,
                                                       PersonFirstName = p.PersonFirstName,
                                                       IsActive = p.IsActive,
                                                       PersonTypeId = p.PersonTypeId,
                                                       PersonTypeName = t.PersonType,
                                                       IsGroupPerson = _db.tbl_PersonGroupMap.Where(x => x.PersonId == p.PersonId).Any(),
                                                       OrderNo = p.OrderNo
                                                   }).OrderBy(x => x.OrderNo).ThenBy(x => x.PersonFirstName).ToList();

            return View(lstPersons);
        }

        [HttpPost]
        public JsonResult UpdatePersonOrder(List<Guid> selectedPersonIds)
        {
            string message = string.Empty;

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                List<tbl_Persons> lstAllPersons = _db.tbl_Persons.Where(p => p.ClientId == ClientId && p.IsAttendancePerson == true && p.IsDeleted == false && p.IsActive == true).ToList();
                if (lstAllPersons != null && lstAllPersons.Count > 0)
                {
                    // Update all person's order to null
                    lstAllPersons.ForEach(x => { x.OrderNo = null; });
                    _db.SaveChanges();

                    // Update selected person's order from 1 to n. 
                    if (selectedPersonIds != null && selectedPersonIds.Count > 0)
                    {
                        int counter = 1;
                        selectedPersonIds.ForEach(personId =>
                        {
                            tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == personId).FirstOrDefault();
                            if (objPerson != null)
                            {
                                objPerson.OrderNo = counter;
                                _db.SaveChanges();
                            }
                            counter++;
                        });

                    }

                    message = "SUCCESS";
                }
            }
            catch (Exception ex)
            {
                message = "ERROR";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public string ExportPDFOfPersonPagar(Guid id) // id = pagarid
        {
            PagarVM objPagar = new PagarVM();

            string Result = "";

            try
            {
                tbl_Pagar personPagar = _db.tbl_Pagar.Where(x => x.PagarId == id).FirstOrDefault();

                objPagar.PagarId = id;
                objPagar.dtPagarStartDate = personPagar.PagarStartDate;
                objPagar.dtPagarEndDate = personPagar.PagarEndDate;
                objPagar.PersonId = personPagar.PersonId;
                objPagar.GroupId = personPagar.GroupId;
                objPagar.PagarAmount = personPagar.PagarAmount;
                objPagar.TotalUpadAmount = personPagar.TotalUpadAmount;
                objPagar.TotalOvertimeAmount = personPagar.TotalOvertimeAmount;
                objPagar.AmountPay = personPagar.AmountPay;
                objPagar.PrevPagarRemainingAmount = personPagar.PrevPagarRemainingAmount;
                objPagar.RemainingAmount = personPagar.RemainingAmount;
                objPagar.Remarks = personPagar.Remarks;

                List<PagarPersonDetailVM> lstPagarPersonDetail = (from detail in _db.tbl_PagarPersonDetail
                                                                  join person in _db.tbl_Persons on detail.PagarPersonId equals person.PersonId
                                                                  where detail.PagarId == id
                                                                  select new PagarPersonDetailVM
                                                                  {
                                                                      PagarPersonDetailId = detail.PagarPersonDetailId,
                                                                      PagarId = detail.PagarId,
                                                                      PagarPersonId = detail.PagarPersonId,
                                                                      TotalAttendanceDays = detail.TotalAttendanceDays,
                                                                      TotalPagarAmount = detail.TotalPagarAmount,
                                                                      PersonName = person.PersonFirstName
                                                                  }).ToList();

                tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == objPagar.PersonId).FirstOrDefault();
                string PersonName = objPerson.PersonFirstName;

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                decimal? TotalDays = lstPagarPersonDetail.Select(x => x.TotalAttendanceDays).Sum();
                string strTotalDays = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalDays));

                decimal? TotalPagarAmount = lstPagarPersonDetail.Select(x => x.TotalPagarAmount).Sum();
                string strTotalPagarAmount = CoreHelper.GetFormatterAmount(Convert.ToDecimal(TotalPagarAmount));

                string[] strColumns = new string[3] { "Person Name", "Total Days", "Amount" };

                if (lstPagarPersonDetail != null && lstPagarPersonDetail.Count() > 0)
                {

                    List<DateTime> lstDateTemp = new List<DateTime>();
                    StringBuilder strHTML = new StringBuilder();
                    strHTML.Append("<!DOCTYPE html>");
                    strHTML.Append("<style>");
                    strHTML.Append("@page {@bottom-center {content: \"Page \" counter(page) \" of \" counter(pages);}}");
                    strHTML.Append("</style>");

                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<thead style=\"display:table-header-group;\">");
                    string Title = "Pagar Of " + PersonName;

                    strHTML.Append("<tr>");
                    strHTML.Append("<th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #000000\">");
                    strHTML.Append(Title);
                    strHTML.Append("</th>");
                    strHTML.Append("</tr>");
                    strHTML.Append("<tr><th colspan=\"" + strColumns.Length + "\" style=\"border: 1px solid #000000\">From " + objPagar.dtPagarStartDate.ToString("dd/MM/yyyy") + " To " + objPagar.dtPagarEndDate.ToString("dd/MM/yyyy") + " </th></tr>");
                    strHTML.Append("<tr>");

                    for (int idx = 0; idx < strColumns.Length; idx++)
                    {
                        strHTML.Append("<th style=\"border: 1px solid #000000\">");
                        strHTML.Append(strColumns[idx]);
                        strHTML.Append("</th>");
                    }

                    strHTML.Append("</tr>");
                    strHTML.Append("</thead>");
                    strHTML.Append("<tbody>");
                    foreach (var obj in lstPagarPersonDetail)
                    {

                        if (obj != null)
                        {

                            strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                            for (int Col = 0; Col < strColumns.Length; Col++)
                            {
                                string strcolval = "";
                                switch (strColumns[Col])
                                {

                                    case "Person Name":
                                        {
                                            strcolval = obj.PersonName;
                                            break;
                                        }
                                    case "Total Days":
                                        {
                                            strcolval = Convert.ToDecimal(obj.TotalAttendanceDays).ToString("0.##");
                                            break;
                                        }
                                    case "Amount":
                                        {
                                            strcolval = CoreHelper.GetFormatterAmount(obj.TotalPagarAmount);
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }

                                }
                                strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\">");
                                strHTML.Append(strcolval);
                                strHTML.Append("</td>");
                            }
                            strHTML.Append("</tr>");
                        }
                    }

                    // Total
                    strHTML.Append("<tr>");
                    strHTML.Append("<th style='text-align:right; border: 1px solid #000000;'>Total</th>");
                    strHTML.Append("<th style='border: 1px solid #000000;'> " + strTotalDays + " </th>");
                    strHTML.Append("<th style='border: 1px solid #000000;'> " + strTotalPagarAmount + " </th>");
                    strHTML.Append("</tr>");

                    strHTML.Append("</tbody>");
                    strHTML.Append("</table>");

                    // Calculation table

                    strHTML.Append("<table cellspacing='0' border='1' cellpadding='5' style='margin-top:20px; width:100%; repeat-header:yes;repeat-footer:yes;border-collapse: collapse;border: 1px solid #ccc;font-size: 12pt;page-break-inside:auto;'>");
                    strHTML.Append("<tbody>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"> " + Resource.TotalPagar + "</td>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"><strong> " + objPagar.PagarAmount.ToString("0.##") + "</strong></td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"> " + Resource.Withdraw + "</td>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"> " + objPagar.TotalUpadAmount.ToString("0.##") + "</td>");
                    strHTML.Append("</tr>");

                    decimal AfterWithdrawLessAmt = objPagar.PagarAmount - objPagar.TotalUpadAmount;

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"></td>"); // " + Resource.TotalPagar + "
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"> <strong>" + AfterWithdrawLessAmt.ToString("0.##") + "</strong></td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"> " + Resource.Overtime + "</td>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"> " + objPagar.TotalOvertimeAmount.ToString("0.##") + "</td>");
                    strHTML.Append("</tr>");

                    decimal AfterOvertimeLessAmt = AfterWithdrawLessAmt + objPagar.TotalOvertimeAmount;

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"></td>"); // " + Resource.TotalPagar + "
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"><strong> " + AfterOvertimeLessAmt.ToString("0.##") + "</strong></td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\">" + Resource.PreviousPagarAmount + "</td>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"> " + objPagar.PrevPagarRemainingAmount.ToString("0.##") + "</td>");
                    strHTML.Append("</tr>");

                    decimal CreditDebitAmount = AfterOvertimeLessAmt + objPagar.PrevPagarRemainingAmount;

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"></td>"); // " + Resource.PrevCreditDebit + "
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"> <strong>" + CreditDebitAmount.ToString("0.##") + "</strong></td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\">" + Resource.TotalGivenAmount + "</td>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"> " + objPagar.AmountPay.ToString("0.##") + "</td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\">" + Resource.RemainingAmount + "</td>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"><strong> " + objPagar.RemainingAmount.ToString("0.##") + "</strong></td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("<tr style='page-break-inside:avoid; page-break-after:auto;'>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\">" + Resource.Remarks + "</td>");
                    strHTML.Append("<td style=\"width: auto; border: 1px solid #000000\"> " + objPagar.Remarks + "</td>");
                    strHTML.Append("</tr>");

                    strHTML.Append("</tbody>");
                    strHTML.Append("</table>");

                    StringReader sr = new StringReader(strHTML.ToString());

                    var myString = strHTML.ToString();
                    var myByteArray = System.Text.Encoding.UTF8.GetBytes(myString);
                    var ms = new MemoryStream(myByteArray);

                    Document pdfDoc = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    writer.PageEvent = new PDFGeneratePageEventHelper();
                    pdfDoc.Open();

                    XMLWorkerHelper objHelp = XMLWorkerHelper.GetInstance();
                    objHelp.ParseXHtml(writer, pdfDoc, ms, null, Encoding.UTF8, new UnicodeFontFactory());

                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "download;filename=Pagar Of " + PersonName + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }

                return Result;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
            }

        }

        public decimal GetRemainingAmountOfPerson(Guid PersonId)
        {
            decimal remainingAmount = 0;

            PagarVM obj = new PagarVM();
            obj.PersonId = PersonId;

            try
            {
                DateTime SelectedPagarDate = DateTime.Now;

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                // Get Group Persons 
                List<Guid> lstPersonsIds = new List<Guid>();
                lstPersonsIds.Add(PersonId);

                tbl_Pagar objLastPagar = _db.tbl_Pagar.Where(x => x.PersonId == PersonId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (objLastPagar != null)
                {
                    obj.dtPagarStartDate = objLastPagar.PagarEndDate.AddDays(1);
                    obj.dtPagarEndDate = SelectedPagarDate;
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

                    obj.dtPagarEndDate = SelectedPagarDate;
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
                    where attper.PersonId == personId
                    && att.AttendanceDate >= obj.dtPagarStartDate
                    && att.AttendanceDate <= obj.dtPagarEndDate
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

                remainingAmount = obj.RemainingAmount;
            }
            catch (Exception ex)
            {
            }

            return remainingAmount;
        }

        public decimal GetRemainingAmountOfGroup(Guid GroupId)
        {
            decimal remainingAmount = 0;

            PagarVM obj = new PagarVM();

            try
            {
                DateTime SelectedPagarDate = DateTime.Now;

                // Get Group Persons
                List<tbl_PersonGroupMap> lstGropPersons = _db.tbl_PersonGroupMap.Where(x => x.PersonGroupId == GroupId).ToList();
                List<Guid> lstPersonsIds = lstGropPersons.Select(x => x.PersonId).ToList();

                tbl_Pagar objLastPagar = _db.tbl_Pagar.Where(x => x.GroupId == GroupId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (objLastPagar != null)
                {
                    obj.dtPagarStartDate = objLastPagar.PagarEndDate.AddDays(1);
                    obj.dtPagarEndDate = SelectedPagarDate;
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

                    obj.dtPagarEndDate = SelectedPagarDate;
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

                remainingAmount = obj.RemainingAmount;
            }
            catch (Exception ex)
            {
            }

            return remainingAmount;
        }
    }
}