namespace DemoSiteForHttpExamples
{
    using System.Web.Mvc;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Web.Routing.RouteTable.Routes.MapMvcAttributeRoutes();
        }
    }
}
