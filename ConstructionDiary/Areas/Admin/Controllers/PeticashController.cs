using ConstructionDiary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionDiary.ResourceFiles;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.tool.xml;
using System.Text;
using ConstructionDiary.Helper;
using System.IO;

namespace ConstructionDiary.Areas.Admin.Controllers
{
    [filters]
    public class PeticashController : MyBaseController
    {
        ConstructionDiaryEntities _db;
        public PeticashController()
        {
            _db = new ConstructionDiaryEntities();
        }
        public ActionResult Index()
        {
            List<PeticashVM> lstPeticase = new List<PeticashVM>();
            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());
                int RoleID = clsSession.RoleID;

                lstPeticase = (from p in _db.tbl_Peticase
                               join usr in _db.tbl_Users on p.UserId equals usr.UserId
                               where !p.IsDeleted && p.ClientId == ClientId
                               select new PeticashVM
                               {
                                   PeticashId = p.PeticashId,
                                   UserId = p.UserId,
                                   dtSelectedDate = p.SelectedDate,
                                   CreditDebit = p.CreditDebit,
                                   Amount = p.Amount,
                                   TableId = p.TableId,
                                   AmountWhereUsed = p.AmountWhereUsed,
                                   Remarks = p.Remarks,
                                   IsActive = p.IsActive,
                                   StaffName = usr.FirstName,
                                   CreatedDate = p.CreatedDate
                               }).OrderByDescending(x => x.CreatedDate).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(lstPeticase);
        }

        public ActionResult Add()
        {
            PeticashVM objPeticase = new PeticashVM();
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            objPeticase.StaffList = _db.tbl_Users.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId && x.RoleId == (int)UserRoles.Staff)
                         .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName }).OrderBy(o => o.Text)
                         .ToList();

            return View(objPeticase);
        }

        [HttpPost]
        public ActionResult Add(PeticashVM peticase)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            Guid ClientId = new Guid(clsSession.ClientID.ToString());

            peticase.StaffList = _db.tbl_Users.Where(x => x.IsActive == true && x.IsDeleted == false && x.ClientId == ClientId && x.RoleId == (int)UserRoles.Staff)
                         .Select(o => new SelectListItem { Value = o.UserId.ToString(), Text = o.FirstName }).OrderBy(o => o.Text)
                         .ToList();

            if (ModelState.IsValid)
            {
                try
                {

                    DateTime exp_date = DateTime.ParseExact(peticase.SelectedDate, "dd/MM/yyyy", null);

                    tbl_Peticase objPeticase = new tbl_Peticase();
                    objPeticase.PeticashId = Guid.NewGuid();
                    objPeticase.UserId = peticase.UserId;
                    objPeticase.SelectedDate = exp_date;
                    objPeticase.Amount = Convert.ToDecimal(peticase.Amount);
                    objPeticase.Remarks = peticase.Remarks;
                    objPeticase.CreditDebit = "Credit";
                    objPeticase.ClientId = ClientId;
                    objPeticase.IsActive = true;
                    objPeticase.IsDeleted = false;
                    objPeticase.CreatedBy = clsSession.UserID;
                    objPeticase.CreatedDate = DateTime.UtcNow;
                    _db.tbl_Peticase.Add(objPeticase);
                    _db.SaveChanges();

                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    throw ex;
                }
                return RedirectToAction("Index");
            }

            return View(peticase);
        }

        public ActionResult MyBalance()
        {
            List<PeticashVM> lstPeticase = new List<PeticashVM>();
            try
            {
                Guid ClientId = new Guid(clsSession.ClientID.ToString());
                Guid loggedInUserId = new Guid(clsSession.UserID.ToString());
                int RoleID = clsSession.RoleID;

                lstPeticase = (from p in _db.tbl_Peticase 
                               where !p.IsDeleted && p.ClientId == ClientId && p.UserId == loggedInUserId
                               select new PeticashVM
                               {
                                   PeticashId = p.PeticashId,
                                   UserId = p.UserId,
                                   dtSelectedDate = p.SelectedDate,
                                   CreditDebit = p.CreditDebit,
                                   Amount = p.Amount,
                                   TableId = p.TableId,
                                   AmountWhereUsed = p.AmountWhereUsed,
                                   Remarks = p.Remarks,
                                   IsActive = p.IsActive, 
                                   CreatedDate = p.CreatedDate
                               }).OrderByDescending(x => x.CreatedDate).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(lstPeticase);
        }

    }
}