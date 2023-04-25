using Microsoft.Owin;
using Owin;
using System;
using Hangfire;
using Hangfire.SqlServer;
using mainMasterpiesce.Controllers;
using Serilog;
using Serilog.Formatting.Compact;
using System.Web.Mvc;
using mainMasterpiesce.Models;
using System.Web.Routing;

[assembly: OwinStartupAttribute(typeof(mainMasterpiesce.Startup))]
namespace mainMasterpiesce
{
  
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            string connectionString = "Data Source=DESKTOP-PND235Q\\SQLEXPRESS;Initial Catalog=Findingpeace;Integrated Security=True"; 

            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString); // Configure Hangfire to use SQL Server storage with connection string

            // Configure Serilog for Hangfire logging
            //Log.Logger = new LoggerConfiguration()
            //    .Enrich.FromLogContext()
            //    .WriteTo.File(new CompactJsonFormatter(), "hangfire.log") // Specify file name and formatter
            //    .CreateLogger();
            //RecurringJob.AddOrUpdate("DoctorTransaction", () => DoctorTransaction(), Cron.Weekly(DayOfWeek.Monday, 16));

            //var transactionsDoctorsController = new transactionsdoctorsController();
            //RecurringJob.AddOrUpdate("DoctorTransaction", () => transactionsDoctorsController.DoctorTransaction(null), Cron.Weekly(DayOfWeek.Monday, 16));
            /*   RecurringJob.AddOrUpdate("DoctorTransaction", () => DoctorTransaction1(), Cron.Weekly(DayOfWeek.Monday, 16));*/ // Update the desired day and hour here
                                                                                                                                 //RecurringJob.AddOrUpdate("DoctorTransaction", () => new transactionsdoctorsController().DoctorTransaction(null), Cron.Weekly(DayOfWeek.Monday, 16)); // Update the desired day and hour here
            //RouteConfig.RegisterRoutes(RouteTable.Routes);

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            ConfigureAuth(app);
    
        }
        public static void ConfigureHangfire()
        {
            // Schedule the DoctorTransaction action method to run every hour
            RecurringJob.AddOrUpdate<transactionsdoctorsController>(x => x.DoctorTransaction(null,null), Cron.Minutely);
        }


        //public ActionResult DoctorTransaction1()
        //{
        //    // Call the method from another controller
        //    var transactionsController = new transactionsdoctorsController();
        //     var ok=transactionsController.DoctorTransaction(null);

        //    return ok;
        //}






    }
}



