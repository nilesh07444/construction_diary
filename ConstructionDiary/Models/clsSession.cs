﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConstructionDiary.Models
{
    public class clsSession
    {
 
        public static Guid UserID
        {
            get
            {
                return HttpContext.Current.Session["UserID"] != null ? new Guid(Convert.ToString(HttpContext.Current.Session["UserID"])) : Guid.Empty;
            }
            set
            {
                HttpContext.Current.Session["UserID"] = value;
            }
        }
        public static Guid? ClientID
        {
            get
            {
                return HttpContext.Current.Session["ClientID"] != null ? new Guid(Convert.ToString(HttpContext.Current.Session["ClientID"])) : Guid.Empty;
            }
            set
            {
                HttpContext.Current.Session["ClientID"] = value;
            }
        }
        public static int RoleID
        {
            get
            {
                return HttpContext.Current.Session["RoleID"] != null ? Int32.Parse(Convert.ToString(HttpContext.Current.Session["RoleID"])) : 0;
            }
            set
            {
                HttpContext.Current.Session["RoleID"] = value;
            }
        }
        public static String SessionID
        {
            get
            {
                return HttpContext.Current.Session["SessionID"] != null ? Convert.ToString(HttpContext.Current.Session["SessionID"]) : String.Empty;
            }
            set
            {
                HttpContext.Current.Session["SessionID"] = value;
            }
        }
        public static String RoleName
        {
            get
            {
                return HttpContext.Current.Session["RoleName"] != null ? Convert.ToString(HttpContext.Current.Session["RoleName"]) : String.Empty;
            }
            set
            {
                HttpContext.Current.Session["RoleName"] = value;
            }
        }

        public static String FirmName
        {
            get
            {
                return HttpContext.Current.Session["FirmName"] != null ? Convert.ToString(HttpContext.Current.Session["FirmName"]) : String.Empty;
            }
            set
            {
                HttpContext.Current.Session["FirmName"] = value;
            }
        }
        public static string ImagePath
        {
            get
            {
                return HttpContext.Current.Session["ImagePath"] != null ? Convert.ToString(HttpContext.Current.Session["ImagePath"]) : string.Empty;
            }
            set
            {
                HttpContext.Current.Session["ImagePath"] = value;
            }
        }
        public static string UserName
        {
            get
            {
                return HttpContext.Current.Session["UserName"] != null ? Convert.ToString(HttpContext.Current.Session["UserName"]) : string.Empty;
            }
            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }

        public static string UserPermission
        {
            get
            {
                return HttpContext.Current.Session["UserPermission"] != null ? Convert.ToString(HttpContext.Current.Session["UserPermission"]) : string.Empty;
            }
            set
            {
                HttpContext.Current.Session["UserPermission"] = value;
            }
        }


    }
}
