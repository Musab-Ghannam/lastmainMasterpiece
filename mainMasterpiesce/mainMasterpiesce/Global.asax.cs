using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace mainMasterpiesce
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


          


            string connectionString = "Data Source=DESKTOP-PND235Q\\SQLEXPRESS;Initial Catalog=Findingpeace;Integrated Security=True";

            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString); // Configure Hangfire to use SQL Server storage with connection string




            Startup.ConfigureHangfire();


        }
    }
}
