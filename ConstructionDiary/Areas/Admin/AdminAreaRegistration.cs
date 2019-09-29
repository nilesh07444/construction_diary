using System.Web.Mvc;

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
                "Admin_dashboard",
                "admin/dashboard",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_site",
                "admin/site",
                new { controller = "Site", action = "Index", id = UrlParameter.Optional }
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

        }
    }
}
