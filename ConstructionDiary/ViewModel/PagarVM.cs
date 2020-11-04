using ConstructionDiary.Models;
using ConstructionDiary.ResourceFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConstructionDiary
{
    public class PagarVM
    {
        public Guid PagarId { get; set; }
        public string PagarStartDate { get; set; }
        public string PagarEndDate { get; set; } 
        public Guid? PersonId { get; set; }
        public Guid? GroupId { get; set; } 
        public decimal PagarAmount { get; set; }
        public decimal TotalUpadAmount { get; set; }
        public decimal TotalOvertimeAmount { get; set; }
        public decimal PrevPagarRemainingAmount { get; set; }
        public decimal AmountPay { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Remarks { get; set; } 

        // other fields
        public DateTime dtPagarStartDate { get; set; }
        public DateTime dtPagarEndDate { get; set; }

        public GroupAttendanceStatusVM GroupAttendanceData { get; set; }
    }

    public class PagarPersonDetailVM
    {
        public Guid PagarPersonDetailId { get; set; }
        public Guid PagarId { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? PagarPersonId { get; set; }
        public decimal? TotalAttendanceDays { get; set; }
        public decimal? TotalPagarAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalWithdrawAmount { get; set; }
        public decimal TotalOvertimeAmount { get; set; }

        //
        public string PersonName { get; set; }
    }

}