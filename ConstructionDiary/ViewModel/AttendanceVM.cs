using ConstructionDiary.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary
{
    public class AttendanceVM
    {
        public Guid AttendaceId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public Guid ClientId { get; set; }
        public decimal? TotalPaidAmount { get; set; }
        public decimal? TotalPerson { get; set; }
        public decimal? TotalFullDay { get; set; }
        public decimal? TotalHalfDay { get; set; }
        public decimal? TotalAbsent { get; set; }
    }

    public class PersonAttendanceVM
    {
        public Guid PersonAttendanceId { get; set; }
        public Guid PersonId { get; set; }
        public int? PersonTypeId { get; set; }
        public string PersonName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public decimal AttendanceStatus { get; set; }
        public Guid? SiteId { get; set; }
        public decimal? TotalRokadiya { get; set; }
        public int? PersonDailyRate { get; set; }
        public int? WithdrawAmount { get; set; }
        public int? OvertimeAmount { get; set; }
        public string Remarks { get; set; }

        public long? OrderNo { get; set; }
    }

    public class ReportPersonAttendanceVM : PersonAttendanceVM {
        public DateTime AttendanceDate { get; set; }
        public string AttendanceStatusText { get; set; }
        public string SiteName { get; set; }
        public decimal? PayableAmount { get; set; }
    }

    public class AttendanceFormVM
    {
        public Guid? AttendaceId { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "Required")]
        public string AttendanceDate { get; set; }
        public List<PersonAttendanceVM> lstPersonAttendance { get; set; }

        public List<SelectListItem> SitesList { get; set; }
        public List<SelectListItem> AttendanceStatusList { get; set; }
    }

    public class AttendanceStatusVM
    {
        public string StatusValue { get; set; }
        public string StatusText { get; set; } 
    }
    public class SiteAttendanceVM
    {
        public Guid PersonAttendanceId { get; set; }
        public Guid? AttendaceId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public Guid PersonId { get; set; }
        public int? PersonTypeId { get; set; }
        public string PersonName { get; set; } 
        public decimal AttendanceStatus { get; set; } 
        public decimal? TotalRokadiya { get; set; }
        public int? PersonDailyRate { get; set; }
        public decimal? PayableAmount { get; set; }
        public int? WithdrawAmount { get; set; }
        public int? OvertimeAmount { get; set; }
        public string Remarks { get; set; }
    }
     
}