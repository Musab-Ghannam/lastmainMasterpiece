using mainMasterpiesce.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mainMasterpiesce.Controllers
{
    public class mainHomeController : Controller
    {
        FindingpeaceEntities1 doct = new FindingpeaceEntities1();
        // GET: mainHome
        public ActionResult Index()
        { bool flag = true;
            var existuser = User.Identity.GetUserId();

            var ASPEmail = doct.AspNetUsers.Where(c => c.Id == existuser).Select(c => c.Email).FirstOrDefault();
            var patientEmail = doct.patients.FirstOrDefault(c => c.patientemail == ASPEmail);
            var doctorEmail = doct.doctors.FirstOrDefault(c => c.email== ASPEmail);
            var role = doct.AspNetUserRoles.ToList();
        
        var doctors=doct.doctors.ToList();
            var specializations=doct.specializations.ToList();
            
            var PatiantId = doct.patients.FirstOrDefault(c => c.Id == existuser);

            var doctorId = doct.doctors.FirstOrDefault(c => c.Id == existuser);

            string url = Request.Url.AbsoluteUri;
            ViewBag.url = url;// get the absolute URL of the current request
            string[] segments = url.Split('/'); // split the URL into segments
            string lastSegment = segments[segments.Length - 1];
            string beforlast = segments[segments.Length - 2];
            ViewBag.Last = lastSegment;
            ViewBag.befLast = beforlast;
            TempData["lasrseg"] = "Index";
            TempData["beforlast"] = beforlast;

            if (TempData["swal_message"] == "Thank You for your feedback")
            {
                TempData["swal_message"] = "Thank You for your feedback";
                ViewBag.title = "info";
                ViewBag.icon = "success";



            }

            if (TempData["swal_message"] == "Please log in to add Feedback.")
            {

                TempData["swal_message"] = "Please log in to add Feedback.";
                ViewBag.title = "Warning";
                ViewBag.icon = "warning";
                ViewBag.redirectUrl = Url.Action("Login", "Account");

            }


            if (TempData["swal_message"] == "please Add Feedback before submit")
            {
                TempData["swal_message"] = "please Add Feedback before submit";
                ViewBag.title = "Warning";
                ViewBag.icon = "warning";
                ViewBag.redirectUrl = Url.Action("Login", "Account");


            }









            if (User.IsInRole("patient") && PatiantId != null)
            {


                Session["User"] = PatiantId.PatiantId;
            }
            else if(User.IsInRole("doctor") && doctorId != null)
            {
                Session["User"] = doctorId.doctorId;
            }

            return View(Tuple.Create(doctors, specializations));
        }



        public ActionResult ConTact()
        {
            string url = Request.Url.AbsoluteUri;
            ViewBag.url = url;// get the absolute URL of the current request
            string[] segments = url.Split('/'); // split the URL into segments
            string lastSegment = segments[segments.Length - 1];
            string beforlast = segments[segments.Length - 2];
            ViewBag.Last = lastSegment;
            ViewBag.befLast = beforlast;
            TempData["lasrseg"] = "ConTact";
            TempData["beforlast"] = beforlast;

            if (TempData["swal_message"] == "Thank You for your feedback")
            {
                TempData["swal_message"] = "Thank You for your feedback";
                ViewBag.title = "info";
                ViewBag.icon = "success";



            }

            if (TempData["swal_message"] == "Please log in to add Feedback.")
            {

                TempData["swal_message"] = "Please log in to add Feedback.";
                ViewBag.title = "Warning";
                ViewBag.icon = "warning";
                ViewBag.redirectUrl = Url.Action("Login", "Account");

            }


            if (TempData["swal_message"] == "please Add Feedback before submit")
            {
                TempData["swal_message"] = "please Add Feedback before submit";
                ViewBag.title = "Warning";
                ViewBag.icon = "warning";
                ViewBag.redirectUrl = Url.Action("Login", "Account");


            }








            return View();
        }

        public ActionResult About()
        {
            string url = Request.Url.AbsoluteUri;
            ViewBag.url = url;// get the absolute URL of the current request
            string[] segments = url.Split('/'); // split the URL into segments
            string lastSegment = segments[segments.Length - 1];
            string beforlast = segments[segments.Length - 2];
            ViewBag.Last = lastSegment;
            ViewBag.befLast = beforlast;
            TempData["lasrseg"] = "About";
            TempData["beforlast"] = beforlast;

            if (TempData["swal_message"] == "Thank You for your feedback")
            {
                TempData["swal_message"] = "Thank You for your feedback";
                ViewBag.title = "info";
                ViewBag.icon = "success";



            }

            if (TempData["swal_message"] == "Please log in to add Feedback.")
            {

                TempData["swal_message"] = "Please log in to add Feedback.";
                ViewBag.title = "Warning";
                ViewBag.icon = "warning";
                ViewBag.redirectUrl = Url.Action("Login", "Account");

            }


            if (TempData["swal_message"] == "please Add Feedback before submit")
            {
                TempData["swal_message"] = "please Add Feedback before submit";
                ViewBag.title = "Warning";
                ViewBag.icon = "warning";
                ViewBag.redirectUrl = Url.Action("Login", "Account");


            }





            return View();
        }

        public ActionResult test()
        {
            return View();
        }

        public ActionResult Aboutt()
        {
            return View();
        }

        public ActionResult FeedBackWEBsite([Bind(Include = "id,patientId,name,email,message,created_at")] feedbackwebsite feedbackk, string massage, string add, string title, string yourfeedback)
        {


            string url = Request.Url.AbsoluteUri;
            ViewBag.url = url;// get the absolute URL of the current request
            string[] segments = url.Split('/'); // split the URL into segments
            string lastSegment = segments[segments.Length - 1];
            string beforlast = segments[segments.Length - 2];
            ViewBag.Last = lastSegment;
            ViewBag.befLast = beforlast;


            if (User.IsInRole("doctor"))
            {
                if (massage.Length > 0)
                {


                    string UserId = User.Identity.GetUserId();

                    var user = doct.doctors.FirstOrDefault(c => c.Id == UserId).doctorId;

                    feedbackk.message = massage;
                    feedbackk.patientId = user;
                    feedbackk.statee = 0;
                    feedbackk.created_at = DateTime.Now;

                    using (var db = new FindingpeaceEntities1())
                    {
                        db.feedbackwebsites.Add(feedbackk);
                        db.SaveChanges();


                    }
                }
                else if (massage.Length == 0)
                {



                    TempData["swal_message"] = "please Add Feedback before submit";
                    ViewBag.title = "Warning";
                    ViewBag.icon = "warning";
                    ViewBag.redirectUrl = Url.Action("Login", "Account");

                }


            }





            if (User.IsInRole("patient"))
            {

                if (massage.Length > 0)
                {



                    string UserId = User.Identity.GetUserId();

                    var user = doct.patients.FirstOrDefault(c => c.Id == UserId).PatiantId;

                    feedbackk.message = massage;
                    feedbackk.patientId = user;
                    feedbackk.statee = 0;
                    feedbackk.created_at = DateTime.Now;

                    using (var db = new FindingpeaceEntities1())
                    {
                        db.feedbackwebsites.Add(feedbackk);

                        TempData["swal_message"] = "Thank You for your feedback";
                        ViewBag.title = "Warning";
                        ViewBag.icon = "warning";
                        ViewBag.redirectUrl = Url.Action("Login", "Account");




                        db.SaveChanges();


                    }
                }
                else if (massage.Length == 0)
                {



                    TempData["swal_message"] = "please Add Feedback before submit";
                    ViewBag.title = "Warning";
                    ViewBag.icon = "warning";
                    ViewBag.redirectUrl = Url.Action("Login", "Account");

                }



            }
            else
            {


                TempData["notUser"] = "notuser";





                if (TempData["notUser"] != null)
                {

                    TempData["swal_message"] = "Please log in to add Feedback.";
                    ViewBag.title = "Warning";
                    ViewBag.icon = "warning";
                    ViewBag.redirectUrl = Url.Action("Login", "Account");



                }


            }





            return RedirectToAction($"{TempData["lasrseg"]}");
            //return View(lastSegment);

        }

    }
}