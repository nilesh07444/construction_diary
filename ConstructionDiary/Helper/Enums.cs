using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConstructionDiary
{
    public enum PaymentTypes
    {
        Cash, 
        Cheque
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

    

}