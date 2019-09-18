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

        }
    }
}