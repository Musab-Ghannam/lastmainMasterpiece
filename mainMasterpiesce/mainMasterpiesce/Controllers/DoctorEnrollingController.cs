using mainMasterpiesce.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.Owin;

namespace mainMasterpiesce.Controllers
{
    public class DoctorEnrollingController : Controller
    {
       
        //public ActionResult STDform([Bind(Include = "Student_ID,Id,Name,Email,Password,NationalNum,Grad,Pic,Status,PhoneNumber,BirthFile,PersonalIdFile,CertificateFile,Gender,Major_ID,Wallet")] Student student, HttpPostedFileBase Pic1, HttpPostedFileBase PersonalIdFile1, HttpPostedFileBase CertificateFile1, HttpPostedFileBase BirthFile1)
       FindingpeaceEntities1 db=new FindingpeaceEntities1();
        // GET: DoctorEnrolling
        [Authorize(Roles = "doctor")]
     
        public ActionResult EnrollDoctor()
        {
          
            var mainId=User.Identity.GetUserId();
            dynamic data = new ExpandoObject();

            var specializations = db.specializations.ToList();
            var doct=db.doctors.Where(c=>c.Id==mainId).ToList();

         

          
          
            data.Specializations = specializations;
            data.doc = doct;
            //data.doctor = doctors;

            return View(data);


            
        }







        [HttpPost]
        public ActionResult EnrollDoctor([Bind(Include = "Id,locationdoctor,doctorName,email,phoneNumber,qualiification,specialization,startedate,idCardfile,picdoctor,certificationfile,PersonalIdFile,CertificateFile,birthfile,specializationId,statedoctor,earningDoctortotal,AmountsDue,IBAN,Gender,infodoctor,pricePerHour,ratingdoctor,ratingint")] doctor doctorr, string location, string locationLink, string price, string qualif, string Iban, string exp, string info, int specializationId,string yeargrad,string University,string edu, HttpPostedFileBase PersonalIdFile1, HttpPostedFileBase Certification, HttpPostedFileBase BirthFile1, HttpPostedFileBase ExperienceFile1)
        {
            var doctorId = User.Identity.GetUserId();
            var doctor = db.doctors.FirstOrDefault(d => d.Id == doctorId);
            var specializ = db.specializations.ToList();
            ViewBag.special=specializ;
            dynamic data = new ExpandoObject();

            var specializations = db.specializations.ToList();
            var doct = db.doctors.Where(c=>c.Id==doctorId).ToList();
            data.Specializations = specializations;
            data.doc = doct;
            if (ModelState.IsValid)
            {
                if (PersonalIdFile1 != null)
                {
                    //string fileName = Path.GetFileName(image.FileName);
                    string path = Server.MapPath("~/FormalFile/") + PersonalIdFile1.FileName;
                    PersonalIdFile1.SaveAs(path);
                    doctor.idCardfile = PersonalIdFile1.FileName;
                }

                if (Certification != null)
                {
                    //string fileName = Path.GetFileName(cv.FileName);
                    string path = Server.MapPath("~/FormalFile/") + Certification.FileName;
                    Certification.SaveAs(path);
                    doctor.certificationfile = Certification.FileName;
                }
                if (ExperienceFile1 != null)
                {
                    //string fileName = Path.GetFileName(cv.FileName);
                    string path = Server.MapPath("~/FormalFile/") + ExperienceFile1.FileName;
                    ExperienceFile1.SaveAs(path);
                    doctor.experience = ExperienceFile1.FileName;
                }

                if (BirthFile1 != null)
                {
                    //string fileName = Path.GetFileName(cv.FileName);
                    string path = Server.MapPath("~/FormalFile/") + BirthFile1.FileName;
                    BirthFile1.SaveAs(path);
                    doctor.birthfile = BirthFile1.FileName;
                }
                var SPECIALIZATIONNAME = db.specializations.FirstOrDefault(C => C.specializationId == specializationId).namespecialization;
                doctor.pricePerHour = Convert.ToInt16(price);
                doctor.qualiification = qualif;
                doctor.locationdoctor = doctor.locationdoctor+'_' + locationLink;
                doctor.specialization= SPECIALIZATIONNAME;
                doctor.infodoctor = info;
                doctor.IBAN = Iban;
                doctor.AspNetUser.EmailConfirmed= true;
                doctor.addresss = location;
                doctor.educationdetails = edu + '_' + University + '_' + yeargrad;
                if (specializationId != null)
                {

                    doctor.specializationId = specializationId;
                }


                ViewBag.doctorname = doctor.doctorName;

                string[] nameParts = Regex.Replace(User.Identity.Name, "[^a-zA-Z]+", " ").Split(' ');
                string firstName =nameParts[0];

                var docemail = db.doctors.FirstOrDefault(c => c.Id == doctorId).email;
                var docName= db.doctors.FirstOrDefault(c => c.Id == doctorId).doctorName;
                //emaiiil


                // Create a new MailMessage object
                //MailMessage mail = new MailMessage();

                //// Set the sender's email address
                //mail.From = new MailAddress("musab.ghannam@outlook.com");

                //// Set the recipient's email address

                //mail.To.Add(docemail);

                //// Set the subject of the email
                //mail.Subject = "New message from " + "Finding piece";

                //// Set the body of the email

                //mail.Body = $"<b>Welcome to Finding Peace!</b><br/><br/>Dr-{docName}, your registration has been submitted and is waiting for approval. You will receive an email notification when your account has been accepted.";

                //// Set the body format to HTML
                //mail.IsBodyHtml = true;

                //// Create a new SmtpClient object
                //SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential("musab.ghannam@outlook.com", "124816326455@Mo");
                //smtp.EnableSsl = true;

                //// Send the email
                //smtp.Send(mail);


                //email

                db.SaveChanges();

                doctor.statedoctor = 0;

                TempData["swal_message"] = $"Dr-{docName}\tYour registration has been submitted and is waiting for approval. You will receive an email notification when your account has been accepted.";
                ViewBag.title = "success";
                ViewBag.icon = "success";
                ViewBag.redirectUrl = Url.Action("Index", "mainHome");

                return View("EnrollDoctor", data);
            }



            data.Specializations = specializations;
            data.doc = doct;


            //data.doctor = doctors;


            return View("EnrollDoctor", data);



        }

        public ActionResult DoctorDashboard()
        {
            string zoomLink = Session["link"] as string;
            var mainId = User.Identity.GetUserId();



            var doctorId = db.doctors.FirstOrDefault(x => x.Id == mainId).doctorId;


            var appointment = db.appointments.Where(c => c.doctorId == doctorId).ToList();
            var doct = db.doctors.Where(c => c.Id == mainId).ToList();


            var patientcount = db.appointments.Where(c=>c.doctorId==doctorId).Select(l=>l.patientId).Distinct().Count();

         

            var datestring = db.appointments
                           .Where(a => a.doctorId == doctorId)
                           .Select(a => new { a.starttime })
                           .ToList();

            var currentDate = DateTime.Now;

            var patientCountToday = datestring
                                        .Where(a => DateTime.TryParseExact(a.starttime, "h:mm ttdd/MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out var appointmentTime) && appointmentTime.Date == currentDate.Date)
                                        .Count();

            if (patientCountToday == 0) {

                ViewBag.patientcountToday = 0;

            }
            else
            {

                ViewBag.patientcountToday = patientCountToday;

            }

         



            // Get today's date
            //DateTime today = DateTime.Today;
            //var patienttoday=db.appointments.Where(c=>c.doctorId==doctorId&&)

            var appointmentcount = db.appointments.Where(c => c.doctorId == doctorId).ToList().Count();

            ViewBag.appointmentcount= appointmentcount;


            ViewBag.patientcount = patientcount;

            if  (TempData["welcome"] != null)
                {
                if (TempData["welcome"].ToString() == "wait")
                {

                    TempData["swal_message"] = $"Your appointment has been confirmed. Please wait for your appointment to start before beginning your session.";

                    ViewBag.title = "Appointment Reminder";
                    ViewBag.icon = "info";
                    ViewBag.redirectUrl = Url.Action("DoctorDashboard");

                }

                var aspid = User.Identity.GetUserId();
                var docname = db.doctors.FirstOrDefault(c => c.Id == aspid).doctorName;


                if (TempData["welcome"].ToString() == "GetStart")
            {
                    TempData["swal_message"] = $"Welcome, Dr-{docname}. Your appointment session has started. We will be creating a Zoom meeting link and sending it to your patient shortly.";

                    ViewBag.title = "Appointment Started";
                    ViewBag.icon = "success";
                    ViewBag.redirectUrl = Url.Action("DoctorDashboard");
                }



            }
            Session["countarrow"] = 0;
            Session["count"] = 0;

            Session["fullcount"] = 0;
            Session["fulldisplay"] = null;
            System.Web.HttpContext.Current.Session["btnValues"] = 0;

            var feedback = db.feedbacks.ToList();


            var model = Tuple.Create(doct, appointment, feedback);

            return View(model);



        }
        public ActionResult ChangePass()
        {
            TempData["swal_message"] = "changed Successfully";









            return RedirectToAction("ChangePassworddoctor");
        }
        public ActionResult errorpass()
        {
            TempData["swal_message"] = "errorpass";



            return RedirectToAction("ChangePassworddoctor");
        }


        public ActionResult changepassworddoctor()
        {
            if (TempData["swal_message"] == "changed Successfully")
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

            var mainId = User.Identity.GetUserId();




            var doctorId = db.doctors?.FirstOrDefault(x => x.Id == mainId)?.doctorId;

            var appointment = db.appointments.Where(c => c.doctorId == doctorId).ToList();
            var doct = db.doctors.Where(c => c.Id == mainId).ToList();


            var patientcount = db.appointments.Where(c => c.doctorId == doctorId).Select(l => l.patientId).Distinct().Count();


            var appointmentcount = db.appointments.Where(c => c.doctorId == doctorId).ToList().Count();

            ViewBag.appointmentcount = appointmentcount;


            ViewBag.patientcount = patientcount;


      







            var feedback = db.feedbacks.Where(c => c.doctorId == doctorId).ToList();


            var model = Tuple.Create(doct, appointment, feedback);

            return View(model);



        }

        public ActionResult DoctorDashSetting()
        {
         
            var mainId = User.Identity.GetUserId();


           

            var doctorId = db.doctors?.FirstOrDefault(x => x.Id == mainId)?.doctorId;

            var appointment = db.appointments.Where(c => c.doctorId == doctorId).ToList();
            var doct = db.doctors.Where(c => c.Id == mainId).ToList();


            var patientcount = db.appointments.Where(c => c.doctorId == doctorId).Select(l => l.patientId).Distinct().Count();


            var appointmentcount = db.appointments.Where(c => c.doctorId == doctorId).ToList().Count();

            ViewBag.appointmentcount = appointmentcount;


            ViewBag.patientcount = patientcount;


            if (TempData["edit"] == "edit")
            {
                TempData["swal_message"] = $"Your information has been updated successfully.";

                ViewBag.title = "Information Updated";
                ViewBag.icon = "success";



            }
            if (TempData["edit"] == "noedit")
            {
                TempData["swal_message"] = "No changes were made.";
                ViewBag.title = "No Changes Made";
                ViewBag.icon = "warning";



            }







            var feedback = db.feedbacks.Where(c=>c.doctorId==doctorId).ToList();


            var model = Tuple.Create(doct, appointment, feedback);

            return View(model);



        }

        [HttpPost]

        public async Task<ActionResult> EditDoctor([Bind(Include = "doctorId,Id,locationdoctor,doctorName,email,phoneNumber,qualiification,specialization,startedate,earningDoctortotal,AmountsDue,IBAN,Gender,infodoctor,pricePerHour,ratingdoctor,ratingdoctor,ratingint,experience,birthday,addresss,educationdetails")] doctor doctor, string docname, string email, string phonenumb, string birth, string gender, string Iban, string info, string address, string locationLink, string city, string country, string price, string special,string Degree,string college,string yeargrad, HttpPostedFileBase profpic)
        {
            var AspId = User.Identity.GetUserId();
            var doctorr = db.doctors.FirstOrDefault(c => c.Id == AspId);

            bool isUpdated = false; // set to true if any information is updated

            if (doctorr.locationdoctor != city + ',' + country + '_' + locationLink)
            {
                doctorr.locationdoctor = city + ',' + country + '_' + locationLink;
                isUpdated = true;
            }
            if (doctorr.educationdetails != Degree + '_' + college + '_' + yeargrad)
            {
                doctorr.educationdetails = Degree + '_' + college + '_' + yeargrad;
                isUpdated = true;
            }
            if (doctorr.doctorName != docname)
            {
                doctorr.doctorName = docname;
                isUpdated = true;
            }

            DateTime.TryParse(birth, out DateTime parsedBirthday);
            if (doctorr.birthday != parsedBirthday)
            {
                doctorr.birthday = parsedBirthday;
                isUpdated = true;
            }

            if (doctorr.pricePerHour != Convert.ToInt64(price))
            {
                doctorr.pricePerHour = Convert.ToInt64(price);
                isUpdated = true;
            }

            if (doctorr.AspNetUser.PhoneNumber != phonenumb)
            {
                doctorr.AspNetUser.PhoneNumber = phonenumb;
                isUpdated = true;
            }

            if (doctorr.IBAN != Iban)
            {
                doctorr.IBAN = Iban;
                isUpdated = true;
            }

            if (doctorr.addresss != address)
            {
                doctorr.addresss = address;
                isUpdated = true;
            }

            if (doctorr.infodoctor != info)
            {
                doctorr.infodoctor = info;
                isUpdated = true;
            }

            if (doctorr.specializationId != Convert.ToInt16(special))
            {
                doctorr.specializationId = Convert.ToInt16(special);
                isUpdated = true;
            }

            if (doctorr.AspNetUser.Email != email)
            {
                doctorr.AspNetUser.Email = email;
                doctorr.AspNetUser.UserName = email;
                doctorr.email = email;
                
                isUpdated = true;
            }

            if (profpic != null)
            {
                string path = Server.MapPath("~/Content/images/") + profpic.FileName;
                profpic.SaveAs(path);
                doctorr.picdoctor = "~/Content/images/" + profpic.FileName;
                isUpdated = true;
            }

            if (isUpdated)
            {
                TempData["edit"] = "edit";
                db.Entry(doctorr).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                TempData["edit"] = "noedit";
            }

            return RedirectToAction("DoctorDashSetting");
        }









        public ActionResult PendingReviews(int? id)
        {

            var mainId = User.Identity.GetUserId();
            var doctorId = db.doctors.FirstOrDefault(x => x.Id == mainId).doctorId;

            var appoint = db.appointments.Where(c => c.doctorId == doctorId).ToList();
            var doct = db.doctors.Where(c => c.Id == mainId).ToList();


            var patientcount = db.appointments.Where(c => c.doctorId == doctorId).Select(l => l.patientId).Distinct();


            var appointmentcount = db.appointments.Where(c => c.doctorId == doctorId).ToList().Count();

            ViewBag.appointmentcount = appointmentcount;


            ViewBag.patientcount = patientcount;
            var feedback = db.feedbacks.Where(c => c.doctorId == doctorId).ToList();



            if (TempData["IsAccept"] == "samestate")
            {




                TempData["swal_message"] = $"Yoy already in this state";
                ViewBag.title = "warning";
                ViewBag.icon = "warning";

            }





            if (TempData["IsAccept"] == "firstAccept")
            {

                // Define the sweet alert message and options
                string sweetAlertMessage = "Are you sure you want to display this Review?";
                string sweetAlertTitle = "Confirm Block";
                string sweetAlertIcon = "warning";
                string sweetAlertCancelButton = "Cancel";

                // Update the TempData and ViewBag variables
                TempData["swal_message"] = sweetAlertMessage;
                ViewBag.title = sweetAlertTitle;
                ViewBag.icon = sweetAlertIcon;
                ViewBag.cancelButton = sweetAlertCancelButton;


            }

























            var model = Tuple.Create(doct, appoint, feedback);

            return View(model);



        }


        public ActionResult MyReviews(int?id)
        {
          
            var mainId = User.Identity.GetUserId();
            var doctorId = db.doctors.FirstOrDefault(x => x.Id == mainId).doctorId;

            var appoint = db.appointments.Where(c => c.doctorId == doctorId).ToList();
            var doct = db.doctors.Where(c => c.Id == mainId).ToList();


            var patientcount = db.appointments.Where(c => c.doctorId == doctorId).Select(l => l.patientId).Distinct();


            var appointmentcount = db.appointments.Where(c => c.doctorId == doctorId).ToList().Count();

            ViewBag.appointmentcount = appointmentcount;


            ViewBag.patientcount = patientcount;
            var feedback = db.feedbacks.Where(c=>c.doctorId== doctorId).ToList();



            if (TempData["IsAccept"] == "samestate")
            {



          
                TempData["swal_message"] = $"Yoy already in this state";
            ViewBag.title = "warning";
            ViewBag.icon = "warning";

            }





            if (TempData["IsAccept"] == "firstAccept")
            {

                // Define the sweet alert message and options
                string sweetAlertMessage = "Are you sure you want to display this Review?";
                string sweetAlertTitle = "Confirm Block";
                string sweetAlertIcon = "warning";
                string sweetAlertCancelButton = "Cancel";

                // Update the TempData and ViewBag variables
                TempData["swal_message"] = sweetAlertMessage;
                ViewBag.title = sweetAlertTitle;
                ViewBag.icon = sweetAlertIcon;
                ViewBag.cancelButton = sweetAlertCancelButton;


            }
         







        

       


            if (TempData["IsAccept"] == "firstReject")
            {

                // Define the sweet alert message and options
                string sweetAlertMessage = "Are you sure you want to disappear this Review?";
        string sweetAlertTitle = "Confirm Block";
        string sweetAlertIcon = "warning";
        string sweetAlertCancelButton = "Cancel";

        // Update the TempData and ViewBag variables
        TempData["swal_message"] = sweetAlertMessage;
                ViewBag.title = sweetAlertTitle;
                ViewBag.icon = sweetAlertIcon;
                ViewBag.cancelButton = sweetAlertCancelButton;
           
              
         

            }





         






            var model = Tuple.Create(doct, appoint, feedback);

            return View(model);



        }
        public ActionResult samestate(int? id)
        {
            TempData["Id"] = Convert.ToInt16(id);
            TempData["IsAccept"] = "samestate";

            return RedirectToAction("MyReviews");



        }
        public ActionResult samestatepending(int? id)
        {
            TempData["Id"] = Convert.ToInt16(id);
            TempData["IsAccept"] = "samestate";

            return RedirectToAction("PendingReviews");



        }


        public ActionResult firstAccept(int?id)
        {
            TempData["Id"] = Convert.ToInt16(id);
            TempData["IsAccept"] = "firstAccept";

            return RedirectToAction("PendingReviews");



        }
        public ActionResult firstReject(int?id)
        {

            TempData["Id"] = Convert.ToInt16(id);
            TempData["IsAccept"] = "firstReject";



            return RedirectToAction("MyReviews");



        }


        public ActionResult Acceptt([Bind(Include = "statefeedback")] feedback feed, string Accept)
        {

            var feedback = db.feedbacks.Find(TempData["Id"]);
            feedback.statefeedback = 1;

            db.Entry(feedback).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("MyReviews");




        }

        public ActionResult Block( [Bind(Include = "statefeedback")] feedback feed, string Accept)
        {



            var feedback = db.feedbacks.Find(TempData["Id"]);
            feedback.statefeedback = 0;

            db.Entry(feedback).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("PendingReviews");




        }


     
        public ActionResult MyPatient()
        {
            string zoomLink = Session["link"] as string;
            var mainId = User.Identity.GetUserId();
            var doctorId = db.doctors.FirstOrDefault(x => x.Id == mainId).doctorId;

            var appointment = db.appointments.Where(c => c.doctorId == doctorId).ToList();
            var doct = db.doctors.Where(c => c.Id == mainId).ToList();


            var patientcount = db.appointments.Where(c => c.doctorId == doctorId).Select(l => l.patientId).Distinct();


            var appointmentcount = db.appointments.Where(c => c.doctorId == doctorId).ToList().Count();

            ViewBag.appointmentcount = appointmentcount;


            ViewBag.patientcount = patientcount;

            var feedback = db.feedbacks.ToList();


            var model = Tuple.Create(doct, appointment, feedback);

            return View(model);



        }

        public ActionResult NotAvailable(string day,string btnn,string valueToRemove,string wholeday,string senddata, string selectnotavailabale)
        {


            if (selectnotavailabale == "empty")
            {
                TempData["swal_message"] = $"Please select a time slot before submitting .";
                ViewBag.title = "warning";
                ViewBag.icon = "warning";
           



            }


            var AspId = User.Identity.GetUserId();

            var doctorsId = db.doctors.FirstOrDefault(c => c.Id == AspId).doctorId;
            var notavailable = db.NotAvailableTimes.Where(c => c.doctorId == doctorsId).ToList();

            string notavailabletime="";
            foreach (var item in notavailable)
            {

                notavailabletime= item.timenotavailble               ; 
                ViewBag.notavailabletile+= notavailabletime;

            }  




            if (senddata != null)
            {


            }
     

            ViewBag.fulldisplay = Session["fulldisplay"];
            int count = (int)(Session["countarrow"] ?? 0);


            ViewBag.weeks = count;


            ViewBag.currentday = day;


            ViewBag.InputStyle = "color:#20BBBD; font-weight:bold";

            //flaggg

            if (day != null)
            {
                Session["day"] = day;
            }

            bool flagsession = true;

         


            bool flag = false;
            if (day == null && Session["day"] != null)
            {

               

                day = Session["day"].ToString();
           
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
                if (ViewBag.currentday == @DateTime.Now.AddDays(i).ToString("dd/MM/yy"))
                {



                    flag = true;

                }


            }


            ViewBag.flag = flag;








        

            ViewBag.btn = System.Web.HttpContext.Current.Session["btnValues"] as string ?? string.Empty;

            string btnValuedate = Request.Form["btnn"];
     
            string btnValue = Request.Form["btnn"];
            ViewBag.flagcolor = false;
            bool flagcolor = false;


            if (btnn == btnValue)
            {
                ViewBag.selectedslot = ";background-color: red;";

            }




            string buttonStyle = "background-color: #E9E9E9; margin-top: 20px;";
            ViewBag.ButtonStyle = buttonStyle;
            int btncount = (int)(Session["count"] ?? 0);


            int fullcount = (int)(Session["fullcount"] ?? 0);
            bool flagbtnselectedseasion = false;



            bool flaghavereserv = true;

            if (!string.IsNullOrEmpty(wholeday))
            {

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

                    if(btnValue.Length > 20)
                    {
                        fullcount++;
                        Session["fullcount"] = fullcount;

                    }





                    bool flaffully = true;

                    if (!string.IsNullOrEmpty(ViewBag.currentday))
                    {     
                        ViewBag.btn += btnValue + ViewBag.currentday.Substring(0, 5) +  ",";
                        if (btnValue.Length > 20)
                        {
                            flaffully = false;
                            ViewBag.fulldisplay += btnValue;
                    
                            System.Web.HttpContext.Current.Session["fulldisplay"] = ViewBag.fulldisplay;



                        }

                        ViewBag.flagcolor = true;

                    }



                    ViewBag.fully = flaffully;

                    flagbtnselectedseasion = true;

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
          


          






            var mainId = User.Identity.GetUserId();
            var doctorId = db.doctors.FirstOrDefault(x => x.Id == mainId).doctorId;

            var appointment = db.appointments.Where(c => c.doctorId == doctorId).ToList();
            var doct = db.doctors.Where(c => c.Id == mainId).ToList();


            var patientcount = db.appointments.Where(c => c.doctorId == doctorId).Select(l => l.patientId).Distinct();


            var appointmentcount = db.appointments.Where(c => c.doctorId == doctorId).ToList().Count();

            ViewBag.appointmentcount = appointmentcount;


            ViewBag.patientcount = patientcount;






            bool available = true;

      
            ViewBag.available = available;







            var feedback = db.feedbacks.ToList();


            var model = Tuple.Create(doct, appointment, feedback);

            return View(model);



        }





 

        public ActionResult fullday(string wholeday,string valueToRemove,string senddata)
        {

            int fullcount = (int)(Session["fullcount"] ?? 0);
       
            string fulldisplay = (string)Session["fulldisplay"] ?? "";

            if (!string.IsNullOrEmpty(wholeday))
            {

                fullcount++;
                Session["fullcount"] = fullcount;


              fulldisplay = (string)Session["fulldisplay"] ?? "";

                // Append the new value to the existing value
                fulldisplay += wholeday+',';

                // Store the new value back in the session
                Session["fulldisplay"] = fulldisplay;




            }



            if (!string.IsNullOrEmpty(valueToRemove))
            {

                fulldisplay = fulldisplay.Replace(valueToRemove, string.Empty);

                if (System.Web.HttpContext.Current.Session["fulldisplay"] != null && System.Web.HttpContext.Current.Session["fulldisplay"].ToString().Contains(valueToRemove))
                {
                    string btnValues = System.Web.HttpContext.Current.Session["fulldisplay"].ToString();
                    btnValues = btnValues.Replace(valueToRemove, string.Empty);
                    System.Web.HttpContext.Current.Session["fulldisplay"] = btnValues;
                    fullcount--;
                    Session["fullcount"] = fullcount;
                }


            }
















            return RedirectToAction("NotAvailable");
        }



        public ActionResult plusweeks( string arrow)
        {
            int count = (int)(Session["countarrow"] ?? 0);

            count += 7;
            ViewBag.weeks = count;

            // Store the updated count value in session state
            Session["countarrow"] = count;

















            return RedirectToAction("NotAvailable");
        }
        public ActionResult minusWeek( string arrow)
        {
            int count = (int)(Session["countarrow"] ?? 0);

            if (count > 0)
            {



                count -= 7;
                ViewBag.weeks = count;





                // Store the updated count value in session state
                Session["countarrow"] = count;



            }


            return RedirectToAction("NotAvailable");
        }







        public ActionResult Waiting( string wait)
        {

      

            TempData["welcome"] = "wait";


            return RedirectToAction("DoctorDashboard");
        }







        public ActionResult beforeGetStart(string wait)
        {

            
            TempData["welcome"] = "GetStart";

            return RedirectToAction("DoctorDashboard");
        }







        public ActionResult GetStart(string wait)
        {

            var aspid=User.Identity.GetUserId();
            var specialization = db.doctors.FirstOrDefault(c => c.Id == aspid).specialization1.namespecialization;
            var docid = db.doctors.FirstOrDefault(c => c.Id == aspid).doctorId;
            var apttime = db.appointments.FirstOrDefault(c => c.doctorId == docid).starttime;


            DateTime storedDatt = DateTime.ParseExact(apttime, "h:mm ttdd/MM", CultureInfo.InvariantCulture);

            string topic = specialization;
            string startTime = storedDatt.ToString();
            int duration = 60; // in minutes

            //TempData["welcome"] = "GetStart";

            return RedirectToAction("GenerateZoomLink", new { topic = topic, startTime = startTime, duration = duration });
        }



        public ActionResult SendNotAvailable( [Bind(Include = "notavailableId,doctorId,timenotavailble")] NotAvailableTime notavailable, string storedata,string wholeday)
        {

            ViewBag.weeks = 0;


            ViewBag.flag = true;


            ViewBag.currentday = DateTime.Now.AddDays(0).ToString("dddd");

         
         
      

          


            var AspId = User.Identity.GetUserId();

            var doctors = db.doctors.Where(c => c.Id == AspId).ToList();



            var mainId = User.Identity.GetUserId();
            var doctorId = db.doctors.FirstOrDefault(x => x.Id == mainId).doctorId;

            var appointment = db.appointments.Where(c => c.doctorId == doctorId).ToList();
            var doct = db.doctors.Where(c => c.Id == mainId).ToList();


            var patientcount = db.appointments.Where(c => c.doctorId == doctorId).Select(l => l.patientId).Distinct();


            var appointmentcount = db.appointments.Where(c => c.doctorId == doctorId).ToList().Count();

            ViewBag.appointmentcount = appointmentcount;


            ViewBag.patientcount = patientcount;



            if (Session["fulldisplay"] != null && !string.IsNullOrEmpty(Session["fulldisplay"].ToString()))
            {


                string[] tableslots = Session["fulldisplay"].ToString().Split(',');







                for (int i = 0; i < tableslots.Length - 1; i++)
                {
                    notavailable.doctorId = doctorId;
                    notavailable.timenotavailble = tableslots[i] + '.';


                    using (var db = new FindingpeaceEntities1())
                    {
                        db.NotAvailableTimes.Add(notavailable);

                        Session["count"] = 0;

                        Session["fullcount"] = 0;
                        Session["fulldisplay"] = null;
                        Session["btnValues"] = null;

                        db.SaveChanges();
                    }
                }






            }



            if (Session["btnValues"] != null)
            {

                string[] days = Session["btnValues"].ToString().Split(',');
                for (int i = 0; i < days.Length-1; i++)
                {


                    notavailable.doctorId = doctorId;
                    notavailable.timenotavailble = days[i]+'.';


                    using (var db = new FindingpeaceEntities1())
                    {
                        db.NotAvailableTimes.Add(notavailable);

                        Session["count"] = 0;

                        Session["fullcount"] = 0;
                        Session["fulldisplay"] = null;
                        Session["btnValues"] = null;


                        db.SaveChanges();
                    }


                }
            }





            var feedback = db.feedbacks.ToList();

            return View("NotAvailable", Tuple.Create(doct, appointment, feedback));
        }








        public async Task<ActionResult> GenerateZoomLink(string topic, string startTime, int duration)
        {
            string apiKey = "O0HA03Z8RjKy0WLdl4HuoA";
            string apiSecret = "efUaUpNA1q7LV648Tjp25NL8MWjMcwYyBmmh";

            try
            {
                var jwt = await GenerateJwt(apiKey, apiSecret);

                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://api.zoom.us/v2/")
                };
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", jwt);

                var data = new
                {
                    topic,
                    type = 2,
                    start_time = startTime,
                    duration
                };
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("users/me/meetings", content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var meeting = JsonConvert.DeserializeObject<ZoomMeeting>(responseContent);

                TempData["ZoomLink"] = meeting.JoinUrl;
                Session["link"] = TempData["ZoomLink"];




                var aspid = User.Identity.GetUserId();
                var docid = db.doctors.FirstOrDefault(c => c.Id == aspid).doctorId;
                var apttime = db.appointments.Where(c => c.doctorId == docid).Select(c => c.starttime).ToList();
                foreach (var item in apttime)
                {
                    DateTime storedDatt = DateTime.ParseExact(item, "h:mm ttdd/MM", CultureInfo.InvariantCulture);
                    string datbasetimeday = storedDatt.ToString("MM/dd");
                    string today = DateTime.Now.ToString("MM/dd");


                    if (datbasetimeday == today)
                    {
                        if (storedDatt.Hour == DateTime.Now.Hour)
                        {
                            var appointment = db.appointments.FirstOrDefault(c => c.doctorId == docid && c.starttime == item);
                            if (appointment != null)
                            {
                                // Update the appointment's JoinUrl with the Zoom link
                                appointment.JoinUrl = meeting.JoinUrl;
                                db.SaveChanges();



                            }


                        }


                    }
                }



                //string topic = specialization;
                //string startTime = storedDatt.ToString();










                return RedirectToAction("DoctorDashboard");
            }

            catch (HttpRequestException ex)
            {
                // Handle the error
                // You can log the exception details, display an error message to the user, etc.
                // Here's an example of displaying a generic error message:
                ModelState.AddModelError("", "An error occurred while generating the Zoom link. Please try again later.");
                return RedirectToAction("DoctorDashboard");
            }


        }

        private async Task<string> GenerateJwt(string apiKey, string apiSecret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(1), // Set expiration time to 1 hour from now
                Issuer = apiKey,
                Audience = "https://api.zoom.us",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiSecret)), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private class ZoomMeeting
        {
            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("join_url")]
            public string JoinUrl { get; set; }
        }





















    }
}