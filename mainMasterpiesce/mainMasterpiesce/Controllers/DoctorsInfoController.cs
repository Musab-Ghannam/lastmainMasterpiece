using mainMasterpiesce.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Diagnostics;
using Microsoft.Ajax.Utilities;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using System.Net.Http;


using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
using System.Web.Services.Description;
using System.Xml.Linq;
using static mainMasterpiesce.Controllers.ManageController;

namespace mainMasterpiesce.Controllers
{
    public class DoctorsInfoController : Controller
    {
        // GET: DoctorsInfo
        FindingpeaceEntities1 doc=new FindingpeaceEntities1();

       
        public ActionResult therapiestlist(string therapistName, string specializationsearch, string Male, string rating, string therapytype, string desc)
        {

            TempData["Removedata"] = "Remove";


            var avgfeedback = doc.feedbacks.GroupBy(c => c.doctorId).Select(g => new { doctorId = g.Key, avgrating = g.Average(c => c.rating) });


            foreach (var item in avgfeedback)
            {
                ViewBag.avg += item;

            }
            string url = Request.Url.AbsoluteUri;
            ViewBag.url = url;// get the absolute URL of the current request
            string[] segments = url.Split('/'); // split the URL into segments
            string lastSegment = segments[segments.Length - 1];
            string beforlast = segments[segments.Length - 2];
            ViewBag.Last = lastSegment;
            ViewBag.befLast = beforlast;
            TempData["lasrseg"] = lastSegment;


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




            //var feedbackcoumt = doc.feedbacks.Where(c => c.statefeedback == 1).Count();








            var APPOINT = doc.appointments.ToList();
            


            var alldoctors = doc.doctors.Where(c => c.statedoctor == 1).ToList();
            var specialization = doc.specializations.ToList();
            int therapyTypeInt, ratingInt;
            var accepteddoctors = doc.doctors.Where(c => c.statedoctor == 1).ToList();
            if (desc != null && desc == "highest")
            {
                var highestrate = doc.doctors.Where(c => c.statedoctor == 1).OrderByDescending(c => c.ratingint).ToList();


                return View(Tuple.Create(highestrate, specialization));
            }
            else if (desc != null && desc == "lowest")
            {

                var lowestprice = doc.doctors.Where(c => c.statedoctor == 1).OrderBy(c => c.pricePerHour).ToList();
                return View(Tuple.Create(lowestprice, specialization));

            }



            if (therapistName != null)
            {
                var searchDoctor = doc.doctors.Where(c => c.doctorName.Contains(therapistName) && c.statedoctor == 1).ToList();

                return View(Tuple.Create(searchDoctor, specialization));

            }

            //male=
            if (Male != null && Male == Convert.ToString(1) && rating == null && therapytype == null)
            {

                var genderfiltering = doc.doctors.Where(c => c.Gender == true && c.statedoctor == 1).ToList();
                return View(Tuple.Create(genderfiltering, specialization));
            }
            else if (Male != null && Male == Convert.ToString(0) && rating == null && therapytype == null)
            {
                var genderfiltering0 = doc.doctors.Where(c => c.Gender == false && c.statedoctor == 1).ToList();
                return View(Tuple.Create(genderfiltering0, specialization));
            }
            //rating

            if (rating != null && therapytype == null && Male == null)
            {
                int ratingdecimalradio = Convert.ToInt16(rating);
                var ratingdoctor = doc.doctors.Where(c => c.statedoctor == 1 && c.ratingint == ratingdecimalradio)
                                             .ToList();

                return View(Tuple.Create(ratingdoctor, specialization));








       






            }

            if (Male == null && rating == null && int.TryParse(therapytype, out therapyTypeInt))
            {
                var therapyfiltering = doc.doctors.Where(c => c.specializationId == therapyTypeInt && c.statedoctor == 1).ToList();
                return View(Tuple.Create(therapyfiltering, specialization));

            }
            //1 all exist and female


            if (int.TryParse(therapytype, out therapyTypeInt) && int.TryParse(rating, out ratingInt) && Male == Convert.ToString(0))
            {
                var generalfiltering = doc.doctors.Where(c => c.specializationId == therapyTypeInt && c.ratingint == ratingInt && c.Gender == false && c.statedoctor == 1).ToList();
                return View(Tuple.Create(generalfiltering, specialization));
            }
            //all exist and maleALL
            if (int.TryParse(therapytype, out therapyTypeInt) && int.TryParse(rating, out ratingInt) && Male == Convert.ToString(1))
            {
                var generalfiltering = doc.doctors.Where(c => c.specializationId == therapyTypeInt && c.ratingint == ratingInt && c.Gender == true && c.statedoctor == 1).ToList();
                return View(Tuple.Create(generalfiltering, specialization));
            }
            //2exist gender and thyarapytyprgender=0
            if (Male == Convert.ToString(0) && int.TryParse(therapytype, out therapyTypeInt) && rating == null)
            {

                var filtergendtherapy0 = doc.doctors.Where(c => c.Gender == false && c.specializationId == therapyTypeInt && c.statedoctor == 1).ToList();

                return View(Tuple.Create(filtergendtherapy0, specialization));


            }
            //2exist gender and thyarapytyprgender=1
            if (Male == Convert.ToString(1) && int.TryParse(therapytype, out therapyTypeInt) && rating == null)
            {

                var filtergendtherapy1 = doc.doctors.Where(c => c.Gender == true && c.specializationId == therapyTypeInt && c.statedoctor == 1).ToList();

                return View(Tuple.Create(filtergendtherapy1, specialization));


            }   //2exist therapytype and rating...
            if (Male == null && int.TryParse(therapytype, out therapyTypeInt) && int.TryParse(rating, out ratingInt))
            {

                var filterratingtherapy = doc.doctors.Where(c => c.ratingint == ratingInt && c.specializationId == therapyTypeInt && c.statedoctor == 1).ToList();

                return View(Tuple.Create(filterratingtherapy, specialization));


            }
            //2exist gender and rating=0
            if (Male == Convert.ToString(0) && therapytype == null && int.TryParse(rating, out ratingInt))
            {

                var filtergendrating0 = doc.doctors.Where(c => c.Gender == false && c.ratingint == ratingInt && c.statedoctor == 1).ToList();

                return View(Tuple.Create(filtergendrating0, specialization));


            }
            //2exist gender and rating=1
            if (Male == Convert.ToString(1) && therapytype == null && int.TryParse(rating, out ratingInt))
            {

                var filtergendrating1 = doc.doctors.Where(c => c.Gender == true && c.ratingint == ratingInt && c.statedoctor == 1).ToList();

                return View(Tuple.Create(filtergendrating1, specialization));


            }

            if (TempData["LoadMore"] == null)
            {


                TempData["more"] = 10;

            }else if (TempData["LoadMore"] == "LoadMore")
            {


                TempData["more"] = 20;
            }


            return View(Tuple.Create(alldoctors, specialization));
        }





        public ActionResult LoadMore()
        {
            TempData["LoadMore"] = "LoadMore";

     

           return RedirectToAction("therapiestlist");
        }





        public ActionResult alldoctors(string therapistName, string specializationsearch,string Male,string rating,string therapytype,string desc)
        {
            var avgfeedback = doc.feedbacks.GroupBy(c => c.doctorId).Select(g => new { doctorId = g.Key, avgrating = g.Average(c => c.rating) });

            foreach (var item in avgfeedback)
            {
                ViewBag.avg += item;
            }



            var alldoctors = doc.doctors.Where(c=>c.statedoctor==1).ToList();
            var specialization = doc.specializations.ToList();
            int therapyTypeInt, ratingInt;
            var accepteddoctors=doc.doctors.Where(c=>c.statedoctor==1).ToList();
       if(desc!=null&&desc=="highest")
            {
                var highestrate = doc.doctors.Where(c=>c.statedoctor == 1).OrderByDescending(c => c.ratingdoctor).ToList();

           
                return View(Tuple.Create(highestrate, specialization));
            }else if(desc != null && desc == "lowest")
            {

                var lowestprice = doc.doctors.Where(c => c.statedoctor == 1).OrderBy(c => c.pricePerHour).ToList();
                return View(Tuple.Create(lowestprice, specialization));

            }



            if (therapistName != null)
            {
                var searchDoctor = doc.doctors.Where(c => c.doctorName.Contains(therapistName)&&c.statedoctor==1).ToList();

                return View(Tuple.Create(searchDoctor, specialization));

            }

            //if (specializationsearch != null)
            //{
            //    var searchspaecialization = doc.doctors.Where(c => c.specializationId == Convert.ToInt16(specializationsearch)).ToList();

            //    return View(Tuple.Create(searchspaecialization, specialization));
            //}
            //male=
            if (Male != null && Male == Convert.ToString(1)&& rating==null&& therapytype==null)
            {

                var genderfiltering = doc.doctors.Where(c => c.Gender == true && c.statedoctor == 1).ToList();
                return View(Tuple.Create(genderfiltering, specialization));
            }
            else if (Male != null && Male == Convert.ToString(0) && rating == null && therapytype == null)
            {
                var genderfiltering0 = doc.doctors.Where(c => c.Gender == false && c.statedoctor == 1).ToList();
                return View(Tuple.Create(genderfiltering0, specialization));
            }
            //rating

            if (rating != null && therapytype == null && Male == null)
            {
                int ratingdecimalradio = Convert.ToInt16(rating);
                var ratingdoctor = doc.doctors.Where(c => c.statedoctor == 1 &&c.ratingint == ratingdecimalradio)
                                             .ToList();

                return View(Tuple.Create(ratingdoctor, specialization));








  





            }

            if (Male==null&&rating==null&& int.TryParse(therapytype, out therapyTypeInt))
            {
                var therapyfiltering= doc.doctors.Where(c => c.specializationId == therapyTypeInt && c.statedoctor == 1).ToList();
                return View(Tuple.Create(therapyfiltering, specialization));

            }
            //1 all exist and female


            if (int.TryParse(therapytype, out therapyTypeInt) && int.TryParse(rating, out ratingInt)&& Male == Convert.ToString(0))
            {
                var generalfiltering = doc.doctors.Where(c => c.specializationId == therapyTypeInt && c.ratingint == ratingInt&& c.Gender == false && c.statedoctor == 1).ToList();
                return View(Tuple.Create(generalfiltering, specialization));
            }
            //all exist and maleALL
            if (int.TryParse(therapytype, out therapyTypeInt) && int.TryParse(rating, out ratingInt) && Male == Convert.ToString(1))
            {
                var generalfiltering = doc.doctors.Where(c => c.specializationId == therapyTypeInt && c.ratingint == ratingInt && c.Gender == true && c.statedoctor == 1).ToList();
                return View(Tuple.Create(generalfiltering, specialization));
            }
            //2exist gender and thyarapytyprgender=0
            if(Male== Convert.ToString(0)&& int.TryParse(therapytype, out therapyTypeInt) && rating ==null) { 
            
                var filtergendtherapy0=doc.doctors.Where(c=>c.Gender==false&&c.specializationId==therapyTypeInt && c.statedoctor == 1).ToList();

                return View(Tuple.Create(filtergendtherapy0, specialization));


            }
            //2exist gender and thyarapytyprgender=1
            if (Male == Convert.ToString(1) && int.TryParse(therapytype, out therapyTypeInt) && rating == null)
            {

                var filtergendtherapy1 = doc.doctors.Where(c => c.Gender == true && c.specializationId == therapyTypeInt && c.statedoctor == 1).ToList();

                return View(Tuple.Create(filtergendtherapy1, specialization));


            }   //2exist therapytype and rating...
            if (Male ==null&& int.TryParse(therapytype, out therapyTypeInt) && int.TryParse(rating, out ratingInt))
            {

                var filterratingtherapy = doc.doctors.Where(c => c.ratingint == ratingInt && c.specializationId == therapyTypeInt && c.statedoctor == 1).ToList();

                return View(Tuple.Create(filterratingtherapy, specialization));


            }
            //2exist gender and rating=0
            if (Male == Convert.ToString(0) && therapytype == null && int.TryParse(rating, out ratingInt))
            {

                var filtergendrating0 = doc.doctors.Where(c => c.Gender == false && c.ratingint == ratingInt && c.statedoctor == 1).ToList();

                return View(Tuple.Create(filtergendrating0, specialization));


            }
            //2exist gender and rating=1
            if (Male == Convert.ToString(1) && therapytype==null && int.TryParse(rating, out ratingInt))
            { 

                var filtergendrating1 = doc.doctors.Where(c => c.Gender == true && c.ratingint == ratingInt && c.statedoctor == 1).ToList();

                return View(Tuple.Create(filtergendrating1, specialization));


            }
           

            return View(Tuple.Create(alldoctors, specialization));
        }

        public ActionResult AddFeedback(int doctorId, [Bind(Include = "feedbackId,doctorId,patientId,rating,comment,title, statefeedback,ratingint")] feedback feedback,doctor doctorr, string ADD,string rating,string title,string yourfeedback)
        {

            string ASPid = User.Identity.GetUserId();
            var patient = doc?.patients?.FirstOrDefault(c => c.Id == ASPid);

            if (patient != null)
            {
                var Mainid = patient.PatiantId;
                // do something with Mainid
          
            //var Mainid = doc.patients.FirstOrDefault(c => c.Id == ASPid).PatiantId;



           




            int ratingintfeedback =Convert.ToInt32(rating);

           
            feedback.title= title;
            feedback.comment = yourfeedback;
            feedback.rating = ratingintfeedback;
            feedback.patientId = Mainid;
            feedback.doctorId = doctorId;
            feedback.feedbacktime= DateTime.Now;
            feedback.statefeedback = 0;

            using (var db = new FindingpeaceEntities1())
            {
                db.feedbacks.Add(feedback);


                    TempData["feed"] = "singledoc";

                    db.SaveChanges();
                }
            }

            var avgfeedback = doc.feedbacks.GroupBy(c => c.doctorId).Select(g => new { doctorId = g.Key, avgrating = g.Average(c => c.rating) });

            using (var db = new FindingpeaceEntities1())
            {
                foreach (var avg in avgfeedback)
                {
                    var doctor = db.doctors.FirstOrDefault(d => d.doctorId == avg.doctorId);
                    if (doctor != null)
                    {
                        doctor.ratingint = Convert.ToInt32(avg.avgrating);
                        db.Entry(doctor).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();
            }


            return RedirectToAction("singledoctor", new { id = doctorId });
        }


        public PartialViewResult _comment(int?id)
        {
            string ASPid = User.Identity.GetUserId();
            var Mainid = doc.patients.FirstOrDefault(c => c.Id == ASPid).PatiantId;
            dynamic model = new ExpandoObject();
            model.feedback = doc.feedbacks.Where(c => c.patientId==Mainid).ToList();
            model.appointment = doc.appointments.Where(c => c.patientId==Mainid).ToList();
            model.doctor = doc.doctors.Where(c => c.doctorId == id).ToList();
         
          
            return PartialView("_comment", model);
        }

        public ActionResult showallfeedback(int? doctorId)
        {
            TempData["allfedback"] = "allfeedback";

            return RedirectToAction("singledoctor", new { id = doctorId });
        }

    public ActionResult singledoctor(int?id)
        {
            TempData["Removedata"] = "Remove";
            string ASPid = User.Identity.GetUserId();
            var patient = doc?.patients?.FirstOrDefault(c => c.Id == ASPid);
            if (patient != null)
            {
                var Mainid=patient.PatiantId;

          


                //var Mainid = doc.patients.FirstOrDefault(c => c.Id == ASPid).PatiantId;
            int flag = 0;   
          
            var appointmentexist=doc.appointments.Where(c=>c.doctorId==id&&c.patientId==Mainid).ToList();
            var reapcoment = doc.feedbacks.Where(c => c.doctorId == id && c.patientId == Mainid).ToList();

            int Iscommentexistinfeedback = 0;


                if (TempData["feed"] == "singledoc")
                {


                    TempData["swal_message"] = "Thank You for your feedback";
                    ViewBag.title = "Success";
                    ViewBag.icon = "success";



                }



                int Isexis = 0;
            foreach (var item in appointmentexist)
            {
                Isexis++;//have apointment ok
            }

            if (Isexis >= 1)
            {
                flag = 1;


            }










            foreach (var item in reapcoment)
            {


                Iscommentexistinfeedback++;





            }

            if(Iscommentexistinfeedback> 0)
            {

                flag = 2;
            }




           


        

            ViewBag.checkit = flag ;


            var SingleDoctor = doc.doctors.Where(c => c.doctorId == id).ToList();
            var feedback = doc.feedbacks.Where(c => c.doctorId == id&&c.statefeedback==1).ToList();
          
            if (SingleDoctor != null)
            {
                return View(Tuple.Create(SingleDoctor, feedback));
            }
            }
            else
            {
                var SingleDoctor = doc.doctors.Where(c => c.doctorId == id).ToList();
                var feedback = doc.feedbacks.Where(c => c.doctorId == id).ToList();

                return View(Tuple.Create(SingleDoctor, feedback));

            }







            //pppp

        
            //Session["btnValues"] = string.Empty;

            //Session["count"] = string.Empty;

            //Session.Remove("btnValues");
            //ViewBag.btn = string.Empty;
            //Session.Remove("count");

            Session["countarrow"] =0;
            Session["count"] = 0;
            //Session["day"] = null;

            //Session["day"] = null;

            Session["counterbtn"] = 0;
            Session["day"] = DateTime.Now.ToString("dd/MM/yy");
            ViewBag.btn = "";
            System.Web.HttpContext.Current.Session["btnValues"] = "";
            return RedirectToAction("alldoctors");
        }


     

        public ActionResult doctorprofile()
        {
            

            return View();
        }

        public ActionResult Doctorprof()
        {
            return View();
        }

        public ActionResult zeropayment(int?id)
        {
            TempData["zeropayment"] = "zeropayment";

            return RedirectToAction("booking", new { id = id });
        }



        public ActionResult booking(int?id,string day,string selectedSlot,string btnn,string close,string valueToRemove)    
        {
            //feedback
            if (TempData["Removedata"] == "Remove")
            {
           

           

                Session["btnValues"] = string.Empty;

                ViewBag.btn = string.Empty;
                Session["count"] = 0;

                ViewBag.btn = ""; // make the string empty
                var btnValues = ViewBag.btn.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);


            }


            string url = Request.Url.AbsoluteUri;
            ViewBag.url = url;// get the absolute URL of the current request
            string[] segments = url.Split('/'); // split the URL into segments
            string lastSegment = segments[segments.Length - 1];
            string beforlast = segments[segments.Length - 2];
            ViewBag.Last = lastSegment;
            ViewBag.befLast = beforlast;
            TempData["lasrseg"] = "booking";
            TempData["beforlast"] = beforlast;

       if (TempData["zeropayment"] == "zeropayment")
            {


                TempData["swal_message"] = "Since no time slot has been selected, may I please request further clarification or direction on how to proceed";
                ViewBag.title = "Warning";
                ViewBag.icon = "warning";
            }




            //feedback



            var notavailable = doc.NotAvailableTimes.Where(c => c.doctorId == id).ToList();



            string notavailabletime = "";
            foreach (var item in notavailable)
            {

                notavailabletime = item.timenotavailble;
                ViewBag.notavailabletile += notavailabletime;

            }






            int count = (int)(Session["countarrow"] ?? 0);
          
            ViewBag.weeks = count;


     
            ViewBag.weeks = count;


            var doctors = doc.doctors.Where(c => c.doctorId == id).ToList();

            var appointment = doc.appointments.Where(c => c.doctorId == id).ToList();
            var pricedoctor = doc.doctors?.FirstOrDefault(c => c.doctorId == id)?.pricePerHour;

         
            dynamic data = new ExpandoObject();
     


            data.doctor = doctors;
           


            data.appoint = appointment;
            data.price = pricedoctor;
            ViewBag.InputStyle = "color:#20BBBD; font-weight:bold";
            if (day != null)
            {
                Session["day"] = day;
            }
       
            bool flagsession = true;

          
            
          
            bool flag = false;
            if (day == null && Session["day"]!=null)
            {

           

        day= Session["day"].ToString();
                //flag = true;
            }
            ViewBag.currentday = day;
            if (day == null && Session["day"] == null)
            {

                ViewBag.currentday = DateTime.Now.AddDays(count).ToString("dd/MM/yy");
      


                flag = true;
            }
          
       
            ViewBag.flagsession = flag;


            for (int i = ViewBag.weeks; i < ViewBag.weeks + 7; i++)
            {
                if(ViewBag.currentday == @DateTime.Now.AddDays(i).ToString("dd/MM/yy")){



                    flag = true;
                    
                }


            }


            ViewBag.flag = flag;
            //int couterbtnn = 0;

            //if (btnn != null)
            //{
            //    couterbtnn++;
            //}

            //for(int i = 0; i < couterbtnn; i++)
            //                {
            //    ViewBag.btn+=btnn;
            //}
            ViewBag.btn = System.Web.HttpContext.Current.Session["btnValues"] as string ?? string.Empty;

            string btnValuedate = Request.Form["btnn"];
            //DateTime dateTime = DateTime.Parse(btnValuedate);
            //string dateAndTime = dateTime.ToString("MM/dd/yyyy h:mm tt");


            //if (!string.IsNullOrEmpty(btnValuedate))
            //{
            //    DateTime dateTime = DateTime.Parse(btnValuedate);
            //    string dateAndTime = dateTime.ToString("MM/dd/yyyy h:mm tt");
            //    // use the dateAndTime value as needed
            //}



            // Get the value of the clicked button from the form data
            string btnValue = Request.Form["btnn"];
            ViewBag.flagcolor = false;
          bool  flagcolor = false;


            if (btnn == btnValue)
            {
                ViewBag.selectedslot = ";background-color: red;";

            }




            string buttonStyle = "background-color: #E9E9E9; margin-top: 20px;";
            ViewBag.ButtonStyle = buttonStyle;
            int btncount = (int)(Session["count"] ?? 0);
            bool flagbtnselectedseasion = false;






            bool flaghavereserv = true;

            //You have this appointment
            var aspidd = User.Identity.GetUserId();
            var mainIdd = doc.patients.FirstOrDefault(c => c.Id == aspidd).PatiantId;
            var apointmentpatient = doc.appointments.Where(c => c.patientId == mainIdd).ToList();

            List<string> reservedSlotsListT = new List<string>();
            foreach (var item in apointmentpatient)
            {
                string reservedSlotT = item.starttime;
                reservedSlotsListT.Add(reservedSlotT);
                ViewBag.youhaveit += reservedSlotT + ',';
            }


            if (!string.IsNullOrEmpty(btnValue)&& ViewBag.youhaveit!=null)
            {

                if (ViewBag.youhaveit.Contains(btnValue + ViewBag.currentday.Substring(0, 5) + ","))
                {

                    TempData["swal_message"] = $"Sorry, you already have an appointment scheduled at {btnValue} on {ViewBag.currentday.Substring(0, 5)}. Please select a different time.";
                    ViewBag.title = "error";
                    ViewBag.icon = "error";





                    flaghavereserv = false;
                }




            }




            if (flaghavereserv)
            {

       
                if (!string.IsNullOrEmpty(btnValue))
            {
                btncount++;
                Session["count"] = btncount;

                //for (int i = 0; i < countbtn; i++)
                //{


                //}
        
            

             





  if (!string.IsNullOrEmpty(ViewBag.currentday))
                    {
                        ViewBag.btn += btnValue + ViewBag.currentday.Substring(0, 5) + ",";

                        ViewBag.flagcolor = true;

                    }

             
            
                  

          
                    //ViewBag.flagcolor = flagcolor;

                    //if (!string.IsNullOrEmpty(valueToRemove))
                    //{
                    //    if (!string.IsNullOrEmpty(ViewBag.btn) && ViewBag.btn.Contains(btnValue + ViewBag.currentday.Substring(0, 5)))
                    //    {
                    //        ViewBag.btn = ViewBag.btn.Replace(btnValue + ViewBag.currentday.Substring(0, 5) + ",",string.Empty);
                    //    }


                    //}

                    // Append the value of the clicked button to the existing value of ViewBag.btn
                    //ViewBag.btn += btnValue + ViewBag.currentday.substring(0,2)+"     ";
                    flagbtnselectedseasion = true;

                // Store the updated value of ViewBag.btn in the session variable
                System.Web.HttpContext.Current.Session["btnValues"] = ViewBag.btn;


                }
            }
            ViewBag.flagbtnselectedseasion = flagbtnselectedseasion;

           
            //ViewBag.counter= countbtn;

            Session["currentday"] = ViewBag.currentday;
            if (!string.IsNullOrEmpty(valueToRemove))
            {

                ViewBag.btn = ViewBag.btn.Replace(valueToRemove, string.Empty);

                if (System.Web.HttpContext.Current.Session["btnValues"] != null && System.Web.HttpContext.Current.Session["btnValues"].ToString().Contains(valueToRemove))
                {
                    string btnValues = System.Web.HttpContext.Current.Session["btnValues"].ToString();
                    btnValues = btnValues.Replace(valueToRemove, string.Empty);
                    System.Web.HttpContext.Current.Session["btnValues"] = btnValues;
                    btncount--;
                    Session["count"] = btncount;
                }


            }
            ViewBag.selectedsloot += Session["currentday"];
            Session["allprice"] = pricedoctor * Convert.ToInt32(Session["count"]);

            Session["doctorId"] = id;

            if (User.IsInRole("doctor"))
            {
                
                ViewBag.disabled = "disabled";
            }
            else
            {
                ViewBag.disabled = "";
            }






            return View(data);
        }


        public ActionResult UpdateInputClass(int doctorId)
        {
            ViewBag.inputClass = "active";
            return RedirectToAction("booking", new { id = doctorId });
        }
        public ActionResult plusweeks(int doctorId, string arrow)
        {
            int count = (int)(Session["countarrow"] ?? 0);

            count+=7;
            ViewBag.weeks = count;

            // Store the updated count value in session state
            Session["countarrow"] = count;




         
               
          










            return RedirectToAction("booking", new { id = doctorId });
        }
        public ActionResult minusWeek(int doctorId, string arrow)
        {
            int count = (int)(Session["countarrow"] ?? 0);

            if (count > 0)
            {


           
            count -= 7;
            ViewBag.weeks = count;

        



            // Store the updated count value in session state
            Session["countarrow"] = count;



            }


            return RedirectToAction("booking", new { id = doctorId });
        }
        //public ActionResult RemoveValue( int doctorId, string valueToRemove)
        //{
        //    if(!string.IsNullOrEmpty(ViewBag.btn))
        //    {

        //        ViewBag.btn = ViewBag.btn.Replace(valueToRemove, "");
        //    }
      
        //    return RedirectToAction("booking", new { id = doctorId });
        //}


        //public ActionResult RemoveButton(int index)
        //{
        //    if (ViewBag.btn != null && ViewBag.btn.Count > index)
        //    {
        //        ViewBag.btn.RemoveAt(index);
        //    }
        //    return RedirectToAction("booking", new { id = doctorId });
        //}

        //public ActionResult RemoveBtn(int doctorId,string close, string index )
        //{
        //    if (ViewBag.btn != null)
        //    {
        //        var btnValues = ViewBag.btn.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        //        // Remove the value from the array of btnValues
        //        btnValues.RemoveAt(Convert.ToInt32(index));
        //        // Join the remaining values back into a comma-separated string
        //        var newBtnValue = string.Join(",", btnValues);
        //        // Store the new string back in ViewBag.btn
        //        ViewBag.btn = newBtnValue;
        //    }

        //    // Return the view with the updated data
        //    return RedirectToAction("booking", new { id = doctorId });
        //}

        public ActionResult selectedslot(int doctorId, string arrow,string day,string btnn)
        {
            //int count = (int)(Session["count"] ?? 0);


            //ViewBag.weeks = count;

            //// Store the updated count value in session state
            //Session["count"] = count;

            //if (!string.IsNullOrEmpty(selectedSlot))
            //{
            //    DateTime d = DateTime.Parse(selectedSlot);
            //    string dateOnly = d.ToString("MM/dd/yyyy");

                ViewBag.selectedsloot += Session["currentday"];
            //// rest of your code here
            //if (Session["day"] != null)
            //{
            //    day = Session["day"].ToString();

            //}
            ViewBag.btn = btnn;

            int slotcount = 0;


            ViewBag.slotCount = slotcount;



            return RedirectToAction("booking", new { id = doctorId });
        }
        public ActionResult determinday(int doctorId, string date,string day)
        {
            int count = (int)(Session["count"] ?? 0);

            count -= 7;
            ViewBag.weeks = count;

            // Store the updated count value in session state
            Session["count"] = count;






            return RedirectToAction("booking", new { id = doctorId });
        }



   











        [Authorize(Roles="patient")]
      
        public ActionResult checkout(int? id)
        {

            string slots = Session["btnValues"].ToString();


            string[] tableslots = slots.Split(',');

       




            var AspId = User.Identity.GetUserId();

            var patientId=doc.patients.FirstOrDefault(c=>c.Id==AspId).PatiantId;
            var doctors = doc.doctors.Where(c => c.doctorId == id).ToList();
            var patientinf=doc.patients.FirstOrDefault(c=>c.PatiantId== patientId);
            var patientinfoo = doc.patients.Where(c => c.PatiantId == patientId).ToList();
            var appointmentt = doc.appointments.Where(c => c.doctorId == id).ToList();
            var pricedoctor = doc.doctors.FirstOrDefault(c => c.doctorId == id).pricePerHour;

            var asppatient = doc.AspNetUsers.Where(c => c.Id == AspId).ToList();
            dynamic data = new ExpandoObject();
        
            data.doctor = doctors;
            data.appoint = appointmentt;

            data.price = pricedoctor;
            data.patientinfoo= patientinfoo;
            data.asppatient = asppatient;
            Session["doctorId"] = null;

            return View(data);
        }
  
        public ActionResult ConfirmBooking(int id, [Bind(Include = "apointmentId,patientId,doctorId,starttime,endtime,doctornotes, patientnotes,apointmentprice,rating,medication,dosage,dosagefrequency,medicationinstructions,confirmappointment")] appointment appoint, string card_name,string acceptt)
        {

            string slots = Session["btnValues"].ToString();


            string[] tableslots = slots.Split(',');




            List<string> starttimeemaile = new List<string>();

            var AspId = User.Identity.GetUserId();
            var patientId = doc.patients.FirstOrDefault(c => c.Id == AspId).PatiantId;
            var doctors = doc.doctors.Where(c => c.doctorId == id).ToList();

            var appointmentt = doc.appointments.Where(c => c.doctorId == id).ToList();
            var pricedoctor = doc.doctors.FirstOrDefault(c => c.doctorId == id).pricePerHour;
            var patient = doc.patients.FirstOrDefault(c => c.Id == AspId).PatiantId;
            var patientinfoo = doc.patients.Where(c => c.PatiantId == patientId).ToList();
            var asppatient = doc.AspNetUsers.Where(c => c.Id == AspId).ToList();

            dynamic data = new ExpandoObject();

            data.patientinfoo = patientinfoo;
            data.doctor = doctors;
            data.appoint = appointmentt;
            data.asppatient = asppatient;
            data.price = pricedoctor;

            if (acceptt != null && card_name != null && Convert.ToInt16(Session["count"]) > 0)
            {


                for (int i = 0; i < (tableslots.Length) - 1; i++)
                {
                    appoint.patientId = patient;
                    appoint.doctorId = id;

                    appoint.starttime = tableslots[i].ToString();
                    starttimeemaile.Add(appoint.starttime + "/2023");

                    string stringtimeemail = string.Join(";", starttimeemaile);

                    if (!string.IsNullOrEmpty(tableslots[i]))
                    {
                        //string startTime = tableslots[i];
                        //int hours = int.Parse(startTime.Substring(0, 2));
                        //int endHours = hours + 1;
                        //string endTime = endHours.ToString().PadLeft(2, '0') + startTime.Substring(2);

                        if (@tableslots[i][1].ToString() == ":")
                        {

                            int endtime = Convert.ToInt32(tableslots[i][0].ToString()) + 1;
                            appoint.endtime = endtime.ToString() + (tableslots[i].ToString()).Substring(1, 11);

                        }
                        else if (@tableslots[i][1].ToString().Contains('2'))
                        {



                            int endtime = 1;
                            appoint.endtime = (tableslots[i].ToString()).Replace("12", "1");

                            //int endtime =1;
                            //appoint.endtime = endtime.ToString()+ " :00 PM " + (tableslots[i].ToString()).Substring(7, 11);

                        }
                        else
                        {
                            int endtime = Convert.ToInt32(tableslots[i][1].ToString()) + 1;

                            if (tableslots[i][1].ToString() == "1")
                            {

                                appoint.endtime = (tableslots[i].ToString()).Replace("11", "12");

                            }
                            else if (tableslots[i][1].ToString() == "0")
                            {



                                appoint.endtime = (tableslots[i].ToString()).Replace("10", "11");

                            }

                        }

                        // rest of your code here
                    }


                    //int endtime = Convert.ToInt32((tableslots[i].ToString())[0]) + 1;

                    appoint.BookingDate = DateTime.Now;
                    appoint.confirmappointment = 0;
                    appoint.apointmentprice = Convert.ToInt16(pricedoctor);

                    var patientName = doc.patients.FirstOrDefault(c => c.Id == AspId).patientName;

                    var patientemil = doc.patients.FirstOrDefault(c => c.Id == AspId).patientemail;

                    using (var db = new FindingpeaceEntities1())
                    {
                        db.appointments.Add(appoint);



                        TempData["swal_message"] = $"Dear-{patientName}, Your appointment has been confirmed. We look forward to seeing you soon. In the meantime, feel free to explore our website and find ways to bring more peace into your life.";

                        ViewBag.title = "success";
                        ViewBag.icon = "success";
                        ViewBag.redirectUrl = Url.Action("successfullyBooking", new { id = id });

                        //emaiiil


                        // Create a new MailMessage object
                        //MailMessage mail = new MailMessage();

                        //// Set the sender's email address
                        //mail.From = new MailAddress("mosabghannam@outlook.com");

                        //// Set the recipient's email address

                        //mail.To.Add(patientemil);

                        //// Set the subject of the email
                        //mail.Subject = "New message from " + "Finding piece";

                        //// Set the body of the email

                        //mail.Body = $"Dear-{patientName}, Your appointment(s) have been confirmed for the following time(s):{stringtimeemail} . We look forward to seeing you soon. In the meantime, feel free to explore our website and find ways to bring more peace into your life.";
                        //// Set the body format to HTML
                        //mail.IsBodyHtml = true;

                        //// Create a new SmtpClient object
                        //SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
                        //smtp.UseDefaultCredentials = false;
                        //smtp.Credentials = new NetworkCredential("mosabghannam@outlook.com", "124816326455@Mo");
                        //smtp.EnableSsl = true;

                        //// Send the email
                        //smtp.Send(mail);


                        //email



                        db.SaveChanges();
                    }
                }
            }
            else if (acceptt == null && card_name != null)
            {
                TempData["swal_message"] = "Please check the Terms and Conditions checkbox";
                ViewBag.title = "Warning";
                ViewBag.icon = "warning";





                //var patientinf = doc.patients.FirstOrDefault(c => c.PatiantId == patientId);



                //ViewBag.patientName = patientinf.patientName;

                //ViewBag.patientEmail = patientinf.patientemail;


                //ViewBag.patientPhHONE = patientinf?.AspNetUser?.PhoneNumber;

                //ViewBag.Patientstartdate = patientinf?.startedate.Value.ToString("dd/MM/yyyy");


            }
           


            return View("checkout", data);
        }




        public ActionResult succefullyBooking(int? id)
        {

            string slots = Session["btnValues"].ToString();


            string[] tableslots = slots.Split(',');






            var AspId = User.Identity.GetUserId();
            var patientId = doc.patients.FirstOrDefault(c => c.Id == AspId).PatiantId;
            var doctors = doc.doctors.Where(c => c.doctorId == id).ToList();

            var appointmentt = doc.appointments.Where(c => c.doctorId == id).ToList();
    
            var pricedoctor = doc.doctors.FirstOrDefault(c => c.doctorId == id).pricePerHour;
            var patient = doc.patients.FirstOrDefault(c => c.Id == AspId).PatiantId;

            var appointcurrentpatient = doc.appointments.Where(c => c.doctorId == id && c.patientId == patient).ToList();
            //var doctorId = doc.doctors.FirstOrDefault(c => c.doctorId == id).doctorId;

            dynamic data = new ExpandoObject();


            data.doctor = doctors;
            data.appoint = appointmentt;
            data.appointcurrentpatient = appointcurrentpatient;
            data.price = pricedoctor;



            return View(data);
        }


        public ActionResult Waiting(string wait)
        {



            TempData["welcome"] = "wait";


            return RedirectToAction("patientProfile");
        }

        [Authorize(Roles = "patient")]
        public ActionResult patientProfile(int? id)
        {




            string url = Request.Url.AbsoluteUri;
            ViewBag.url = url;// get the absolute URL of the current request
            string[] segments = url.Split('/'); // split the URL into segments
            string lastSegment = segments[segments.Length - 1];
            string beforlast = segments[segments.Length - 2];
            ViewBag.Last = lastSegment;
            TempData["lasrseg"] = lastSegment;






            var AspId = User.Identity.GetUserId();
            var patientId = doc.patients.FirstOrDefault(c => c.Id == AspId).PatiantId;
            //var doctors = doc.doctors.Where(c => c.doctorId == id).ToList();
            var patientInfo = doc.patients.Where(c => c.Id == AspId).ToList();
            var appointmentt = doc.appointments.Where(c => c.patientId == patientId).ToList();
          
            //var pricedoctor = doc.doctors.FirstOrDefault(c => c.doctorId == id).pricePerHour;
            var patient = doc.patients.FirstOrDefault(c => c.Id == AspId).PatiantId;

            //var appointcurrentpatient = doc.appointments.Where(c => c.doctorId == id && c.patientId == patient).ToList();
            //var doctorId = doc.doctors.FirstOrDefault(c => c.doctorId == id).doctorId;
            var client = new HttpClient();


            if (TempData["welcome"] != null)
            {
                if (TempData["welcome"].ToString() == "wait")
                {

                    TempData["swal_message"] = $"Your appointment has been confirmed. Please wait for your appointment to start before beginning your session.";

                    ViewBag.title = "Appointment Reminder";
                    ViewBag.icon = "info";
                    ViewBag.redirectUrl = Url.Action("patientProfile");

                }

            }

  



            return View(Tuple.Create(patientInfo,appointmentt));
        }
        [HttpPost]

        public async Task<ActionResult>  Edit(int? id, [Bind(Include = "patientId,Id,locationpatent,picpatient,patientName, patientemail,startedate,wallet,Gender,Email,birthday,locationdetails")] patient patient, string userName,string Email,string city,string country,string birthday,string phone, HttpPostedFileBase profpic)
        {



         
            DateTime.TryParse(birthday, out DateTime parsedBirthday);



            var client = new HttpClient();
            // Create an HttpClient object to send the HTTP request

            // Send an HTTP GET request to the API endpoint and get the response
          
            var AspId= User.Identity.GetUserId();

            var user = doc.patients.FirstOrDefault(c => c.Id == AspId);
            //var birth=Convert.ToDateTime(birthday);
            var locc = user.locationdetails.Split(',');
            user.patientName= userName;
            user.birthday = parsedBirthday;
            locc[1] = city;
            if (country != "")
            {
                var response = await client.GetAsync(country);



                var responseContent = await response.Content.ReadAsStringAsync();

                // Parse the JSON response into a JObject
                var jsonObject = JObject.Parse(responseContent);

                // Extract the name of the country from the JSON object
                string countryName = (string)jsonObject["name"];
                locc[0] = countryName;

            }

            if (profpic != null)
            {
                //string fileName = Path.GetFileName(image.FileName);
                string path = Server.MapPath("~/Content/images/") + profpic.FileName;
                profpic.SaveAs(path);
                user.picpatient = profpic.FileName;
            }



           user.AspNetUser.PhoneNumber= phone;
            user.locationdetails = string.Join(",", locc);
            user.Email = Email;
            user.patientemail = Email;
            user.AspNetUser.Email = Email;
            user.AspNetUser.UserName = Email;
            doc.Entry(user).State = EntityState.Modified;
            TempData["editpatient"] = "editpatient";
            doc.SaveChanges();
            // continue with the method logic
            return RedirectToAction("Profilesetting", new { id = id });
            
        }

        [Authorize(Roles = "patient")]
        public ActionResult Profilesetting(int? id)
        {

            var AspId = User.Identity.GetUserId();
            var patientId = doc.patients.FirstOrDefault(c => c.Id == AspId).PatiantId;
            //var doctors = doc.doctors.Where(c => c.doctorId == id).ToList();
            var patientInfo = doc.patients.Where(c => c.Id == AspId).ToList();
            var appointmentt = doc.appointments
                          .Where(c => c.patientId == patientId)
                          .OrderBy(c => c.starttime)
                          .ToList();
            //var pricedoctor = doc.doctors.FirstOrDefault(c => c.doctorId == id).pricePerHour;
            var patient = doc.patients.FirstOrDefault(c => c.Id == AspId).PatiantId;

            if (TempData["editpatient"] == "editpatient")
            {
                TempData["swal_message"] = "Your information has been updated successfully.";

                ViewBag.title = "Information Updated";
                ViewBag.icon = "success";



            }

            return View(Tuple.Create(patientInfo, appointmentt));
        }

        //   [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<PartialViewResult> _Oprofileuser(ChangePasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
        //    if (result.Succeeded)
        //    {
        //        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
        //        if (user != null)
        //        {
        //            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //        }
        //        return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
        //    }
        //    AddErrors(result);
        //    return View(model);
        //}

        public ActionResult ChangePass()
        {
            TempData["swal_message"] = "changed Successfully";

            







            return RedirectToAction("ChangePasswordpatient");
        }
        public ActionResult errorpass()
        {
            TempData["swal_message"] = "errorpass";



            return RedirectToAction("ChangePasswordpatient");
        }

        [Authorize(Roles = "patient")]
        public ActionResult ChangePasswordpatient()
        {
            if(TempData["swal_message"] == "changed Successfully")
            {

                TempData["swal_message"] = "You are Password has been changed Successfully";

                ViewBag.title = "Information Updated";
                ViewBag.icon = "success";
            }

            if (TempData["swal_message"] == "errorpass")
            {

                TempData["swal_message"] = "Unfourtunitly there is something wrong please try again";
                ViewBag.title = "Warning";
                ViewBag.icon = "warning";
            }



            var AspId = User.Identity.GetUserId();
            var patientId = doc.patients.FirstOrDefault(c => c.Id == AspId).PatiantId;
            //var doctors = doc.doctors.Where(c => c.doctorId == id).ToList();
            var patientInfo = doc.patients.Where(c => c.Id == AspId).ToList();
            var appointmentt = doc.appointments
                          .Where(c => c.patientId == patientId)
                          .OrderBy(c => c.starttime)
                          .ToList();
            //var pricedoctor = doc.doctors.FirstOrDefault(c => c.doctorId == id).pricePerHour;
            var patient = doc.patients.FirstOrDefault(c => c.Id == AspId).PatiantId;



           return View(Tuple.Create(patientInfo, appointmentt));
        }

        public ActionResult FeedBackWEBsite( [Bind(Include = "id,patientId,name,email,message,created_at")]  feedbackwebsite feedbackk, string massage, string add, string title, string yourfeedback)
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

                var user = doc.doctors.FirstOrDefault(c => c.Id == UserId).doctorId;

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





                if (User.IsInRole("patient")) {

                if (massage.Length > 0)
                {


            
                string UserId = User.Identity.GetUserId();

                var user = doc.patients.FirstOrDefault(c => c.Id == UserId).PatiantId;

                feedbackk.message = massage;
                feedbackk.patientId= user;
                feedbackk.statee = 0;
                feedbackk.created_at= DateTime.Now;

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