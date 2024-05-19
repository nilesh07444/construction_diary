using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public enum PaymentTypes
    {
        Cash,
        Cheque,
        NEFT,
        RTGS
    }
    public enum UserRoles
    {
        SiteOwner = 1,
        Admin = 2,
        Staff = 3
    }

    public enum PackageTypes
    {
        FreeTrialPackage = 1,
        OneMonthPackage = 2,
        ThreeMonthPackage = 3,
        SixMonthPackage = 4,
        YearPackage = 5,
        OneTimePackage = 6
    }

    public enum FileType
    {
        Credit = 1,
        Debit = 2,
        Material = 3,
        Expense = 4,
        Challan = 5,
        ChallanBill = 6
    }

    public enum PeticaseAmountWhereUsed
    {
        Credit = 1,
        Debit = 2,
        Material = 3,
        Expense = 4,
        Challan = 5
    }
    public enum UserPermissionEnum
    {
        Party = 1,
        Site = 2,
        Debit = 3,
        Credit = 4,
        Expense = 5,
        Material = 6,
        Estimate = 7,
        Challan = 8,
        Attendance = 9,
        Pagar = 10,
        Peticash = 11,
        Merchant = 12
    }

}