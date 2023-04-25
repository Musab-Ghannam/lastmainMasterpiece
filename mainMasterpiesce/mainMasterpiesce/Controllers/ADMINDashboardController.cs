﻿using mainMasterpiesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mainMasterpiesce.Controllers
{
    public class ADMINDashboardController : Controller
    {
        FindingpeaceEntities1 db=new FindingpeaceEntities1();
        // GET: ADMINDashboard
        public ActionResult AdminDashboard()
        {
            var doctors = db.doctors.Where(c => c.statedoctor == 1).ToList();
            var patients = db.patients.ToList();
            var appointments = db.appointments.ToList();

            var totalprice = db.appointments.ToList();
            int sum = 0;

            foreach (var item in totalprice)
            {

                sum += item.apointmentprice ?? 0;
            }
            double websitedue = sum * .05;

            var trandoc = db.transactionsdoctors.Select(c => c.amount).Sum();

            ViewBag.doctransac = trandoc;
            ViewBag.Sum = sum;
            ViewBag.webdue = websitedue;



      






            return View(Tuple.Create(doctors, patients, appointments));
       
        }

















    }
}