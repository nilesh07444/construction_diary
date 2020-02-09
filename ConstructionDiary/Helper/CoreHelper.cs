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
        ConstructionDiaryEntities _db = new ConstructionDiaryEntities();
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
               
    }
}