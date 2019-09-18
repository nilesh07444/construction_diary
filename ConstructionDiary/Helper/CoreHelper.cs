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
        public static string GetFormatterAmount(Decimal? TotalGivenAmount)
        {
            string ConvertedString = "";
            if (TotalGivenAmount != null)
            {
                double ammountDouble = Convert.ToDouble(TotalGivenAmount);
                CultureInfo cultureInfo = new CultureInfo("en-IN");
                ConvertedString = string.Format(cultureInfo, "{0:N}", ammountDouble);
            }
            return ConvertedString;
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

        //public static string GetUserName(Guid? Id) {
        //    string userName = "";

        //    if (Id != null)
        //    {
        //        tbl_Users objUser = _db.tbl_Users.Where(x => x.UserId == Id).FirstOrDefault();
        //        if (objUser != null)
        //            userName = objUser.FirstName + " " + objUser.LastName;
        //    }
        //    return userName;
        //}

        //public string GetPersonName(Guid Id)
        //{
        //    string personName = "";

        //    tbl_Persons objPerson = _db.tbl_Persons.Where(x => x.PersonId == Id).FirstOrDefault();
        //    if(objPerson != null)
        //        personName = objPerson.PersonFirstName + " " + objPerson.PersonLastName;

        //    return personName;
        //}


    }
}