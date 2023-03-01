using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary
{
    public class PersonGroupVM
    {
        public Guid PersonGroupId { get; set; }
        public string GroupName { get; set; }
        public int TotalGroupPerson { get; set; }
        public List<SelectListItem> PersonList { get; set; }

        public List<SelectedPersonGroupVM> SelectedPersonList { get; set; }

        //
        public decimal RemainingAmount { get; set; }
    }

    public class SelectedPersonGroupVM
    {
        public Guid PersonGroupId { get; set; }
        public Guid PersonId { get; set; }
    }

    public class GroupAttendanceStatusVM
    {
        public Guid PersonGroupId { get; set; }
        public string GroupName { get; set; } 
        public List<GroupPersonPaymentInfoVM> GroupPersonPayment { get; set; }
        public decimal? GrandAttendance { get; set; }
        public decimal? GrandPayableAmount { get; set; }
        public decimal? GrandWithdrawAmount { get; set; }
        public decimal? GrandOvertimeAmount { get; set; }
        public decimal? GrandRemainingAmount { get; set; }
    }

    public class GroupPersonPaymentInfoVM
    {
        public Guid PersonId { get; set; }
        public string PersonName { get; set; }
        public decimal? TotalAttendance { get; set; }
        public decimal? TotalPayableAmount { get; set; }
        public decimal? TotalWithdrawAmount { get; set; }
        public decimal? TotalOvertimeAmount { get; set; }
        public decimal? TotalRemainingAmount { get; set; }
    }

}