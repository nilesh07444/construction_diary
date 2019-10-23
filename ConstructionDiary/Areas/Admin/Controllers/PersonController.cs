using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.Models;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    public class PersonController : Controller
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
            return View();
        }

        [HttpPost]
        public ActionResult Add(PersonVM person, HttpPostedFileBase postedFile)
        {
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
                    objPerson.PersonPhoto = fileName;
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
            tbl_Persons obj = _db.tbl_Persons.Where(x => x.PersonId == id).FirstOrDefault();
            if (obj != null)
            {
                person.PersonId = obj.PersonId;
                person.Firstname = obj.PersonFirstName;
                person.Address = obj.PersonAddress;
                person.MobileNo = obj.MobileNo;
                person.strPersonPhoto = obj.PersonPhoto;
            }

            return View(person);
        }

        [HttpPost]
        public ActionResult Edit(PersonVM person, HttpPostedFileBase postedFile)
        {
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
                                                 join user in _db.tbl_Users
                                                 on finance.GivenAmountBy equals user.UserId
                                                 where finance.PersonId == id
                                                 select new FinanceList
                                                 {
                                                     FinanceId = finance.FinanceId,
                                                     PersonId = finance.PersonId,
                                                     SelectedDate = finance.SelectedDate,
                                                     Amount = finance.Amount,
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