using System.Web.Mvc;
using System.Web.Routing;

namespace LogBook
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "LogBookHome",
                url: "",
                defaults: new { controller = "Home", action = "Logbook" }
            );

            routes.MapRoute(
                name: "LogBookIndex",
                url: "Index",
                defaults: new { controller = "Home", action = "Logbook"}
            );

            routes.MapRoute(
                name: "LogBook",
                url: "Logbook/Index/{id}",
                defaults: new { controller = "Home", action = "Logbook", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "Home",
            //    url: "Home/Index/{id}",
            //    defaults: new { controller = "Home", action = "Logbook", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Logbook", id = UrlParameter.Optional }
            );

           
        }
    }
}
