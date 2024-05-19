using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using ConstructionDiary.Models;

namespace ConstructionDiary
{
    public class CoreHelper
    {

        public static string GetFormatterAmount(Decimal? Amt)
        {
            string ConvertedString = "";
            if (Amt != null)
            {
                double ammountDouble = Convert.ToDouble(Amt);
                CultureInfo cultureInfo = new CultureInfo("en-IN");
                ConvertedString = string.Format(cultureInfo, "{0:N0}", ammountDouble);
            }
            return ConvertedString;
        }

        public static string GetFormatterAmountWithZero(Decimal? Amt)
        {
            string ConvertedString = "0";
            if (Amt != null)
            {
                double ammountDouble = Convert.ToDouble(Amt);
                CultureInfo cultureInfo = new CultureInfo("en-IN");
                ConvertedString = string.Format(cultureInfo, "{0:N0}", ammountDouble);
            }
            return ConvertedString;
        }

        public static string GetAttendanceStatusText(string status)
        {
            if (status == "0.0")
                return "0";
            else if (status == "1.0")
                return "1";
            else
                return status;
        }
        public static string Encrypt(string Text)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Text);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Decrypt(string Text)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(Text);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static List<UserPageModuleAccessVM> GetAssignedUserPageAccessList(Guid userId)
        {
            ConstructionDiaryEntities _db = new ConstructionDiaryEntities();

            List<UserPageModuleAccessVM> list = null;
            try
            {
                if (userId == Guid.Empty)
                {
                    list = (from m in _db.tbl_PageModule
                            select new UserPageModuleAccessVM
                            {
                                PageModuleId = m.PageModuleId,
                                PageModuleName = m.PageModuleName
                            }).OrderBy(x => x.PageModuleName).ToList();
                }
                else
                {
                    list = (from m in _db.tbl_PageModule
                            join um in _db.tbl_UserPageModuleAccess.Where(x => x.UserId == userId) on m.PageModuleId equals um.PageModuleId into userpagemoduleOuter
                            from um in userpagemoduleOuter.DefaultIfEmpty()
                            select new UserPageModuleAccessVM
                            {
                                PageModuleId = m.PageModuleId,
                                PageModuleName = m.PageModuleName,
                                UserId = userId,
                                IsAssigned = um != null ? true : false
                            }).OrderBy(x => x.PageModuleName).ToList();
                }

            }
            catch (Exception ex)
            {
            }
            return list;
        }

        public static List<UserPageModuleAccessVM> GetAssignedUserPageAccessListWhileLogin(Guid userId, int roleId)
        {
            ConstructionDiaryEntities _db = new ConstructionDiaryEntities();

            List<UserPageModuleAccessVM> list = null;
            try
            {
                if (userId == Guid.Empty)
                {
                    list = (from m in _db.tbl_PageModule
                            select new UserPageModuleAccessVM
                            {
                                PageModuleId = m.PageModuleId,
                                PageModuleName = m.PageModuleName
                            }).OrderBy(x => x.PageModuleName).ToList();
                }
                else
                {
                    bool isAdminUser = roleId == (int)UserRoles.SiteOwner || roleId == (int)UserRoles.Admin;

                    list = (from m in _db.tbl_PageModule
                            join um in _db.tbl_UserPageModuleAccess.Where(x => x.UserId == userId) on m.PageModuleId equals um.PageModuleId into userpagemoduleOuter
                            from um in userpagemoduleOuter.DefaultIfEmpty()
                            select new UserPageModuleAccessVM
                            {
                                PageModuleId = m.PageModuleId,
                                PageModuleName = m.PageModuleName,
                                UserId = userId,
                                IsAssigned = um != null || isAdminUser ? true : false
                            }).OrderBy(x => x.PageModuleName).ToList();
                }

            }
            catch (Exception ex)
            {
            }
            return list;
        }

        public static string GetChallanNumbersFromChallanGroupId(Guid ChallanGroupId)
        {
            string challanGroup = string.Empty;

            try
            {
                ConstructionDiaryEntities _db = new ConstructionDiaryEntities();

                challanGroup = string.Join(",", (from c in _db.tbl_Challan
                                                 join cg in _db.tbl_ChallanGroup on c.ChallanId equals cg.ChallanId into outerJoinCreatedBy
                                                 from cg in outerJoinCreatedBy.DefaultIfEmpty()
                                                 where cg.ChallanGroupId == ChallanGroupId
                                                 select new tbl_Challan
                                                 {
                                                     ChallanNo = c.ChallanNo
                                                 }).ToList().Select(x => x.ChallanNo).ToArray());
            }
            catch (Exception ex)
            {
                challanGroup = "ERROR:";
            }
            return challanGroup;
        }

    }
}