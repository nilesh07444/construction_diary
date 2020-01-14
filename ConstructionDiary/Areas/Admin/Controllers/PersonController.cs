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
    public class PersonController : MyBaseController
    {
        ConstructionDiaryEntities _db; 
        public PersonController()
        {
            _db = new ConstructionDiaryEntities(); 
        }
        public ActionResult Index()
        {
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            var lstPersons = (from p in _db.SP_GetPersonsList(ClientId)
                              select p).ToList();

            return View(lstPersons);
        }

        public ActionResult Add()
        {
            PersonVM person = new PersonVM();
            person.PersonTypeList = _db.tbl_PersonType
                         .Select(o => new SelectListItem { Value = o.Id.ToString(), Text = o.PersonType })
                         .ToList();

            return View(person);
        }

        [HttpPost]
        public ActionResult Add(PersonVM person, HttpPostedFileBase postedFile)
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
                    //objPerson.DailyRate = person.DailyRate;
                    //objPerson.PersonTypeId = person.PersonTypeId;
                    objPerson.PersonPhoto = fileName;
                    objPerson.IsAttendancePerson = false;
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
                return RedirectToAction("Index");
            }

            return View(person);
        }

        public ActionResult Edit(Guid id)
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
                //person.DailyRate = obj.DailyRate;
                //person.PersonTypeId = obj.PersonTypeId;
                person.strPersonPhoto = obj.PersonPhoto;
            }

            return View(person);
        }

        [HttpPost]
        public ActionResult Edit(PersonVM person, HttpPostedFileBase postedFile)
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
                        //objPerson.DailyRate = person.DailyRate;
                        //objPerson.PersonTypeId = person.PersonTypeId;
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
                return RedirectToAction("Index");
            }

            return View(person);
        }

        [HttpPost]
        public string DeletePerson(string PersonId)
        {
            string ReturnMessage = "";

            try
            {
                Guid PersonIdToDelete = new Guid(PersonId);

                tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == PersonIdToDelete && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objPerson == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objPerson.IsDeleted = true;
                    objPerson.UpdatedBy = new Guid(clsSession.UserID.ToString());
                    objPerson.ModifiedDate = DateTime.UtcNow;
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

        public ActionResult View(Guid id)
        {
            PersonVM person = new PersonVM();
            try
            {
                tbl_Persons obj = _db.tbl_Persons.Where(x => x.PersonId == id).FirstOrDefault();
                if (obj != null)
                {
                    person.PersonId = obj.PersonId;
                    person.Firstname = obj.PersonFirstName;
                    person.MobileNo = obj.MobileNo;
                    person.Address = obj.PersonAddress;
                    person.strPersonPhoto = obj.PersonPhoto;
                    person.CreatedBy = GetUserName(obj.CreatedBy);
                    person.UpdatedBy = GetUserName(obj.UpdatedBy);
                }
            }
            catch (Exception ex)
            {
            }
            return View(person);
        }

        public ActionResult Finance(Guid id)
        {
            PersonFinanceVM objFinance = new PersonFinanceVM();
            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                List<FinanceList> financeList = (from finance in _db.tbl_Finance
                                                 join user in _db.tbl_Users on finance.GivenAmountBy equals user.UserId
                                                 join site in _db.tbl_Sites on finance.SiteId equals site.SiteId into outerJoinSite
                                                 from site in outerJoinSite.DefaultIfEmpty()
                                                 where finance.PersonId == id
                                                 select new FinanceList
                                                 {
                                                     FinanceId = finance.FinanceId,
                                                     PersonId = finance.PersonId,
                                                     SelectedDate = finance.SelectedDate,
                                                     Amount = finance.Amount,
                                                     SiteId = finance.SiteId,
                                                     SiteName = (site != null) ? site.SiteName : "",
                                                     CreditOrDebit = finance.CreditOrDebit,
                                                     GivenAmountBy = finance.GivenAmountBy,
                                                     PaymentType = finance.PaymentType,
                                                     ChequeNo = finance.ChequeNo,
                                                     BankName = finance.BankName,
                                                     ChequeFor = finance.ChequeFor,
                                                     Remarks = finance.Remarks,
                                                     IsActive = finance.IsActive,
                                                     IsDeleted = finance.IsDeleted,
                                                     CreatedBy = finance.CreatedBy,
                                                     UpdatedBy = finance.UpdatedBy,
                                                     CreatedDate = finance.CreatedDate,
                                                     ModifiedDate = finance.ModifiedDate,
                                                     FirstName = user.FirstName,
                                                 }).Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.SelectedDate).ToList();
                ViewData["FinanceList"] = financeList;

                objFinance.PersonId = id;
                objFinance.UsersList = _db.tbl_Users.Where(x => x.ClientId == ClientId && x.IsActive == true && x.IsDeleted == false)
                         .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                         .OrderBy(x => x.Text).ToList();

                objFinance.SitesList = _db.tbl_Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId)
                         .Select(o => new SelectListItem { Value = o.SiteId.ToString(), Text = o.SiteName }).OrderBy(o => o.Text)
                         .ToList();

                ViewBag.PersonName = GetPersonName(id);

            }
            catch (Exception ex)
            {
            }
            return View(objFinance);
        }

        [HttpPost]
        public ActionResult Finance(PersonFinanceVM finance)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                finance.UsersList = _db.tbl_Users.Where(x => x.ClientId == ClientId)
                     .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName })
                     .OrderBy(x => x.Text).ToList();

                if (ModelState.IsValid)
                {
                    DateTime date = DateTime.ParseExact(finance.SelectedDate, "dd/MM/yyyy", null);

                    tbl_Finance objFinance = new tbl_Finance();
                    objFinance.FinanceId = Guid.NewGuid();
                    objFinance.PersonId = finance.PersonId;
                    objFinance.SelectedDate = date;
                    objFinance.GivenAmountBy = finance.GivenAmountBy;
                    objFinance.Amount = Convert.ToDecimal(finance.Amount);
                    objFinance.SiteId = finance.SiteId;
                    objFinance.CreditOrDebit = finance.CreditOrDebit;
                    objFinance.PaymentType = finance.PaymentType;
                    objFinance.ChequeNo = finance.ChequeNo;
                    objFinance.BankName = finance.BankName;
                    objFinance.ChequeFor = finance.ChequeFor;
                    objFinance.Remarks = finance.Remarks;
                    objFinance.IsActive = true;
                    objFinance.IsDeleted = false;
                    objFinance.CreatedBy = clsSession.UserID;
                    objFinance.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Finance.Add(objFinance);
                    _db.SaveChanges();

                    return RedirectToAction("Finance", new { id = finance.PersonId });

                }
            }
            catch (Exception ex)
            {

            }

            return View(finance);
        }

        [HttpPost]
        public string DeleteFinance(string FinanceId)
        {
            string ReturnMessage = "";

            try
            {
                Guid FinanceIdToDelete = new Guid(FinanceId);
                tbl_Finance objFinance = _db.tbl_Finance.Where(x => x.FinanceId == FinanceIdToDelete && x.IsActive == true
                                                            && x.IsDeleted == false).FirstOrDefault();

                if (objFinance == null)
                {
                    ReturnMessage = "notfound";
                }
                else
                {
                    objFinance.IsDeleted = true;
                    objFinance.UpdatedBy = new Guid(clsSession.UserID.ToString());
                    objFinance.ModifiedDate = DateTime.UtcNow;
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

        public ActionResult Attandance(Guid id, string duration, string start, string end)
        {
            List<ReportPersonAttendanceVM> lstPersonAttendanceVM = new List<ReportPersonAttendanceVM>();
            try
            {
                if (string.IsNullOrEmpty(duration))
                    duration = "month";

                Guid ClientId = new Guid(clsSession.ClientID.ToString());

                ViewBag.PersonName = GetPersonName(id);
                ViewBag.PersonId = id;
                ViewBag.Duration = duration;
                ViewBag.StartDate = start;
                ViewBag.EndDate = end;

                DateTime startDate = DateTime.Today;
                DateTime endDate = DateTime.Today;

                if (duration == "month")
                {
                    var myDate = DateTime.Now;
                    startDate = new DateTime(myDate.Year, myDate.Month, 1);
                }
                else if (duration == "custom")
                {
                    if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
                    {
                        startDate = DateTime.ParseExact(start, "dd/MM/yyyy", null);
                        endDate = DateTime.ParseExact(end, "dd/MM/yyyy", null);
                    }
                }

                lstPersonAttendanceVM = getAttendanceByFilter(id, startDate, endDate);

            }
            catch (Exception ex)
            {
            }
            return View(lstPersonAttendanceVM);
        }

        public List<ReportPersonAttendanceVM> getAttendanceByFilter(Guid PersonId, DateTime startDate, DateTime endDate)
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

        public string GetUserName(Guid? Id)
        {
            string userName = "";

            if (Id != null)
            {
                tbl_Users objUser = _db.tbl_Users.Where(x => x.UserId == Id).FirstOrDefault();
                if (objUser != null)
                    userName = objUser.FirstName;
            }
            return userName;
        }

        public string GetPersonName(Guid Id)
        {
            string personName = "";

            tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == Id).FirstOrDefault();
            if (objPerson != null)
                personName = objPerson.PersonFirstName;

            return personName;
        }

    }
}