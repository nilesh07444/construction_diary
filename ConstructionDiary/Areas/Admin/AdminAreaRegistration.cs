﻿using System.Web.Mvc;

namespace ConstructionDiary.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_peticash",
                "admin/peticash",
                new { controller = "Peticash", action = "Index", id = UrlParameter.Optional }
            );

            // Dashboard Routes

            context.MapRoute(
                "Admin_dashboard",
                "admin/dashboard",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );

            // Site Routes

            context.MapRoute(
                "Admin_site",
                "admin/site",
                new { controller = "Site", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_sitedetail",
                "admin/site/detail",
                new { controller = "Site", action = "Detail", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_profile",
                "admin/profile",
                new { controller = "Profile", action = "Index", id = UrlParameter.Optional }
            );

            //Client routes
            context.MapRoute(
                "Admin_client",
                "admin/client",
                new { controller = "Client", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_client_add",
                "admin/client/add",
                new { controller = "Client", action = "Add", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_client_edit",
                "admin/client/edit",
                new { controller = "Client", action = "Edit", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_client_users",
                "admin/client/users",
                new { controller = "Client", action = "Users", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_client_users_adduser",
                "admin/client/adduser",
                new { controller = "Client", action = "AddUser", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_client_users_edituser",
                "admin/client/edituser",
                new { controller = "Client", action = "EditUser", id = UrlParameter.Optional }
            );

            // MyAccount Routes

            context.MapRoute(
                "Admin_myaccount",
                "admin/myaccount",
                new { controller = "MyAccount", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_myaccount_add",
                "admin/myaccount/add",
                new { controller = "MyAccount", action = "Add", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_myaccount_edit",
                "admin/myaccount/edit",
                new { controller = "MyAccount", action = "Edit", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_myaccount_report",
                "admin/myaccount/report",
                new { controller = "MyAccount", action = "Report", id = UrlParameter.Optional }
            );

            // User Routes

            context.MapRoute(
                "Admin_user",
                "admin/user",
                new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );

            // Person Routes

            context.MapRoute(
                "Admin_person",
                "admin/person",
                new { controller = "Person", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_person_add",
                "admin/person/add",
                new { controller = "Person", action = "Add", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_person_edit",
                "admin/person/edit",
                new { controller = "Person", action = "Edit", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_person_view",
                "admin/person/view",
                new { controller = "Person", action = "View", id = UrlParameter.Optional }
            );

            // Attendance Routes

            context.MapRoute(
                "Admin_attendance",
                "admin/attendance",
                new { controller = "Attendance", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_attendance_add",
                "admin/attendance/add",
                new { controller = "Attendance", action = "Add", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_attendance_edit",
                "admin/attendance/edit",
                new { controller = "Attendance", action = "Edit", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_attendance_view",
                "admin/attendance/view",
                new { controller = "Attendance", action = "View", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_attendance_personlist",
                "admin/attendance/personlist",
                new { controller = "Attendance", action = "PersonList", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_attendance_addperson",
                "admin/attendance/addperson",
                new { controller = "Attendance", action = "AddPerson", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_attendance_editperson",
                "admin/attendance/editperson",
                new { controller = "Attendance", action = "EditPerson", id = UrlParameter.Optional }
            );

            context.MapRoute(
                            "Admin_Party",
                            "admin/party",
                            new { controller = "Party", action = "Index", id = UrlParameter.Optional }
                        );

        }
    }
}
