using mainMasterpiesce.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
            if (TempData["swal_message"] == "We are soryy you can not Add Feedback beacause you are Adminstration")
            {


                TempData["swal_message"] = "We are soryy you can not Add Feedback beacause you are Adminstration";
                ViewBag.title = "Warning";
                ViewBag.icon = "warning";

            }
       

            var feedbacks = doct.feedbackwebsites.ToList();





            if (User.IsInRole("patient") && PatiantId != null)
            {


                Session["User"] = PatiantId.PatiantId;
            }
            else if(User.IsInRole("doctor") && doctorId != null)
            {
                Session["User"] = doctorId.doctorId;
            }

            return View(Tuple.Create(doctors, specializations, feedbacks));
        }



        public ActionResult ConTact()
        {


            if (TempData["swal_message"] == "Thank You for your feedback")
            {
                TempData["swal_message"] = "Thank you for contacting us. We have received your message and will get back to you soon.";
                ViewBag.title = "info";
                ViewBag.icon = "success";



            }






            return View();
        }
        // POST: Home/SendEmail
        [HttpPost]
        public ActionResult SendEmail(string name, string email, string phone, string message)
        {
            try
            {
                // Create a new MailMessage object
                MailMessage mail = new MailMessage();

                // Set the sender's email address
                mail.From = new MailAddress("mosabg613@outlook.com");

                // Set the recipient's email address

                string emaill = "mosabg613@gmail.com";
                mail.To.Add(emaill);

                // Set the subject of the email
                mail.Subject = "New message from " + name;

                // Set the body of the email
                mail.Body = "Email: " + email + "<br><br>" + "Phone: " + phone + "<br><br>" + message;

                // Set the body format to HTML
                mail.IsBodyHtml = true;

                // Create a new SmtpClient object
                SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("mosabg613@outlook.com", "124816326455@Mo");
                smtp.EnableSsl = true;

                // Send the email
                smtp.Send(mail);
            }
            catch (SmtpException ex)
            {


            }
            TempData["swal_message"] = "Thank You for your feedback";
            // Redirect the user to a confirmation page
            return RedirectToAction("ConTact");
        }

      
        public ActionResult About()
        {
            var feedbacks = doct.feedbackwebsites.ToList();

            var specilization = doct.specializations.ToList();

            var doctors = doct.doctors.ToList();



            return View(Tuple.Create(doctors, specilization, feedbacks));
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
            if (User.IsInRole("Admin"))
            {

                TempData["swal_message"] = "We are soryy you can not Add Feedback beacause you are Adminstration";
                ViewBag.title = "Warning";
                ViewBag.icon = "warning";

            }

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


                        TempData["swal_message"] = "Thank You for your feedback";
                        ViewBag.title = "Warning";
                        ViewBag.icon = "warning";
                    
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
            else if(!User.Identity.IsAuthenticated) 
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