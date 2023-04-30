using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mainMasterpiesce.Models;
using System.IO.Compression;
using System.Web.Configuration;
using Microsoft.AspNet.Identity;
using System.Net.Mail;

namespace mainMasterpiesce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class doctorsController : Controller
    {
        private FindingpeaceEntities1 db = new FindingpeaceEntities1();

        // GET: doctors
        public ActionResult 
            AdminDoctor(string id, string Block,string Accept,string idaccep,string search)
        {
            var appointmentsByPatient = db.appointments.GroupBy(c => c.doctorId).Count();
         
            
            ViewBag.sumprice=appointmentsByPatient; 
            var doctors = db.doctors.Include(d => d.AspNetUser).Include(d => d.specialization1);

            if (Block != null)
            {
                TempData["IdBLOCK"] = Convert.ToInt16(Block);


            }

            ViewBag.Block = false;
            if (TempData["IdBLOCK"] != null)
            {

                int doctorId = Convert.ToInt16(TempData["IdBLOCK"]);
                var appointment = db.appointments.Where(c => c.doctorId == doctorId).ToList();
                var rate = db.doctors.FirstOrDefault(c => c.doctorId == doctorId).ratingint;
                int countappoint = 0;
                foreach (var item in appointment)
                {
                    countappoint++;
                }

                if (countappoint >= 10 && rate <= 2)
                {

                    ViewBag.Block = true;

                }
                ViewBag.rate=rate;
                ViewBag.count=countappoint;

                if (Block != null&& ViewBag.Block ==true)
            {
                    TempData["IdBLOCK"] = Convert.ToInt16(Block);
                    // Define the sweet alert message and options
                    string sweetAlertMessage = "Are you sure you want to block this doctor?";
                string sweetAlertTitle = "Confirm Block";
                string sweetAlertIcon = "warning";
                string sweetAlertCancelButton = "Cancel";

                // Update the TempData and ViewBag variables
                TempData["swal_message"] = sweetAlertMessage;
                ViewBag.title = sweetAlertTitle;
                ViewBag.icon = sweetAlertIcon;
                ViewBag.cancelButton = sweetAlertCancelButton;


                }
                else if(Block != null && ViewBag.Block == false)
                {

                    TempData["swal_message"] = $"You cannot block this doctor. To block a doctor, they must have more than 10 appointments with a rating less than 2. Please try again.";
                    ViewBag.title = "warning";
                    ViewBag.icon = "warning";
                    ViewBag.massagee = "You cannot block this doctor. To block a doctor, they must have more than 10 appointments with a rating less than 2. Please try again.";


                }

       


              


            }

       


            if (Accept != null)
            {

                // Define the sweet alert message and options
                string sweetAlertMessage = "Are you sure you want to Accept this doctor?";
                string sweetAlertTitle = "Confirm Block";
                string sweetAlertIcon = "warning";
                string sweetAlertCancelButton = "Cancel";

                // Update the TempData and ViewBag variables
                TempData["swal_message"] = sweetAlertMessage;
                ViewBag.title = sweetAlertTitle;
                ViewBag.icon = sweetAlertIcon;
                ViewBag.cancelButton = sweetAlertCancelButton;
                TempData["Id"] = Convert.ToInt16(Accept);
                TempData["list"] = "Rejectlist";



            }

            if (!string.IsNullOrEmpty(search))
            {
                var searchh = db.doctors.Where(c => c.doctorName.Contains(search)).ToList();

                return View(searchh);

            }
            else
            {



                return View(doctors.ToList());
            }




        }

        public ActionResult Acceptlist([Bind(Include = "statedoctor")] doctor doctor, string Accept)
        {

            TempData["list"] = "Acceptlist";

            return RedirectToAction("AdminDoctor", new { listType = "Acceptlist" });

        }
        public ActionResult Rejectlist([Bind(Include = "statedoctor")] doctor doctor, string Accept)
        {

            TempData["list"] = "Rejectlist";


            return RedirectToAction("AdminDoctor", new { listType = "rejectlist" });


        }


        public ActionResult Acceptt([Bind(Include = "statedoctor")] doctor doctor, string Accept)
        {
            int docId = Convert.ToInt32(TempData["Id"]);

        
            var doctorr=db.doctors.FirstOrDefault(c=>c.doctorId== docId);
            
            doctorr.statedoctor = 1;
            doctorr.AspNetUser.PhoneNumberConfirmed= true;//accepted doctor
            db.Entry(doctorr).State = EntityState.Modified;

            //emaiiil
            try
            {


         
            var docName = db.doctors.FirstOrDefault(c => c.doctorId == docId).doctorName;
            var docemail = db.doctors.FirstOrDefault(c => c.doctorId == docId).email;

            // Create a new MailMessage object
            MailMessage mail = new MailMessage();

            // Set the sender's email address
            mail.From = new MailAddress("musab.ghannam@outlook.com");

            // Set the recipient's email address

            mail.To.Add(docemail);

            // Set the subject of the email
            mail.Subject = "New message from " + "Finding piece";

            // Set the body of the email
            mail.Body = $@"<html>
                  <body>
                      <p>Dear<b> Dr. {docName}<b>,</p>
                      <br/>
                      <p>Welcome to Finding Peace! We are thrilled to have you as part of our community of healthcare professionals dedicated to improving mental health and wellbeing for all.</p>
                      <br/>
                      <p><b>Congratulations</b> on completing your registration! We appreciate your commitment to joining us in this important mission, and we look forward to supporting you in your journey.</p>
                      <br/>
                      <p>After reviewing your experience and qualifications, we are confident that you will be an excellent addition to our network of providers.</p>
                      <br/>
                      <p>As a registered member of Finding Peace, you will have access to a wealth of resources, including our online forum where you can connect with other healthcare providers and share best practices, our library of educational materials to help you stay up-to-date on the latest research and trends, and much more.</p>
                      <br/>
                      <p>We hope that your experience with Finding Peace will be rewarding and fulfilling, and that you will find it to be a valuable tool in your practice.</p>
                      <br/>
                      <p>If you have any questions or concerns, please don't hesitate to reach out to us at support@findingpeace.com.</p>
                      <br/>
                      <p>Best regards,</p>
                      <p>The Finding Peace Team</p>
                  </body>
              </html>";


            // Set the body format to HTML
            mail.IsBodyHtml = true;

            // Create a new SmtpClient object
            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("musab.ghannam@outlook.com", "124816326455@Mo");
            smtp.EnableSsl = true;

            // Send the email
            smtp.Send(mail);

            }
            //email
            catch (Exception ex)
            {


            }

            db.SaveChanges();
            TempData["list"] = "Rejectlist";
            return RedirectToAction("AdminDoctor");
          
          

        
        }

        public ActionResult Block( [Bind(Include = "statedoctor")] doctor doctor, string Accept)
        {

          int docId = Convert.ToInt32(TempData["IdBLOCK"]);  

                var doctorr = db.doctors.FirstOrDefault(c => c.doctorId == docId);
                doctorr.statedoctor = 0;

                db.Entry(doctorr).State = EntityState.Modified;




            //emaiiil
            try
            {


        

            var docName = db.doctors.FirstOrDefault(c => c.doctorId == docId).doctorName;
            var docemail = db.doctors.FirstOrDefault(c => c.doctorId == docId).email;

            // Create a new MailMessage object
            MailMessage mail = new MailMessage();

            // Set the sender's email address
            mail.From = new MailAddress("musab.ghannam@outlook.com");

            // Set the recipient's email address

            mail.To.Add(docemail);

            // Set the subject of the email
            mail.Subject = "New message from " + "Finding piece";

            // Set the body of the email
            mail.Body = @"<html>
                  <body>
                      <p>Dear Dr. {docName},</p>
                      <br/>
                      <p>We regret to inform you that we will be unable to continue working with you on Finding Peace. Despite many appointments, we have received poor ratings from patients who have seen you, and as a result, we have decided to block your account on our platform.</p>
                      <br/>
                      <p>We value the quality of care that we provide to our patients, and we take their feedback seriously. While we appreciate your interest in our platform, we cannot compromise on our commitment to delivering the best possible care to those who rely on us for support.</p>
                      <br/>
                      <p>Thank you for your understanding. If you have any questions or concerns, please don't hesitate to reach out to us at support@findingpeace.com.</p>
                      <br/>
                      <p>Best regards,</p>
                      <p>The Finding Peace Team</p>
                  </body>
              </html>";


            // Set the body format to HTML
            mail.IsBodyHtml = true;

            // Create a new SmtpClient object
            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("musab.ghannam@outlook.com", "124816326455@Mo");
            smtp.EnableSsl = true;

            // Send the email
            smtp.Send(mail);

            }
            //email

            catch (Exception ex)
            {


            }



            db.SaveChanges();

                return RedirectToAction("AdminDoctor");




    


        }

        public ActionResult DownloadAll(int id)
        {
            // Get the list of files to download from the database
          
            var doc = db.doctors.FirstOrDefault(c => c.doctorId == id);
            List<string> fileNames = new List<string>() {
        doc.experience,
        doc.certificationfile,
        doc.idCardfile,
        doc.birthfile
    };

            // Create a memory stream to hold the zip file
            MemoryStream ms = new MemoryStream();

            // Create a new zip archive
            using (ZipArchive archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                // Add each file to the archive
                foreach (string fileName in fileNames)
                {
                    // Get the file path
                    string filePath = Server.MapPath("~/FormalFile/" + fileName);

                    // Check if the file exists
                    if (System.IO.File.Exists(filePath))
                    {
                        // Create a new entry in the zip archive
                        ZipArchiveEntry entry = archive.CreateEntry(fileName);

                        // Open the file and copy its contents to the zip archive entry
                        using (Stream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        using (Stream es = entry.Open())
                        {
                            fs.CopyTo(es);
                        }
                    }
                }
            }

            // Set the position of the memory stream to 0
            ms.Position = 0;

            // Return the zip file as a file result
            return File(ms, "application/zip", "all_files.zip");
        }






















        // GET: doctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            doctor doctor = db.doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // GET: doctors/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.specializationId = new SelectList(db.specializations, "specializationId", "namespecialization");
            return View();
        }

        // POST: doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "doctorId,Id,locationdoctor,doctorName,email,phoneNumber,qualiification,specialization,startedate,idCardfile,picdoctor,certificationfile,birthfile,specializationId,statedoctor,earningDoctortotal,AmountsDue,IBAN,Gender,infodoctor,pricePerHour,ratingdoctor,ratingint,experience,birthday,addresss,educationdetails")] doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.doctors.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", doctor.Id);
            ViewBag.specializationId = new SelectList(db.specializations, "specializationId", "namespecialization", doctor.specializationId);
            return View(doctor);
        }

        // GET: doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            doctor doctor = db.doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", doctor.Id);
            ViewBag.specializationId = new SelectList(db.specializations, "specializationId", "namespecialization", doctor.specializationId);
            return View(doctor);
        }

        // POST: doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "doctorId,Id,locationdoctor,doctorName,email,phoneNumber,qualiification,specialization,startedate,idCardfile,picdoctor,certificationfile,birthfile,specializationId,statedoctor,earningDoctortotal,AmountsDue,IBAN,Gender,infodoctor,pricePerHour,ratingdoctor,ratingint,experience,birthday,addresss,educationdetails")] doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminDoctor");
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", doctor.Id);
            ViewBag.specializationId = new SelectList(db.specializations, "specializationId", "namespecialization", doctor.specializationId);
            return View(doctor);
        }

        // GET: doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            doctor doctor = db.doctors.FirstOrDefault(c=>c.doctorId==id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            doctor doctor = db.doctors.Find(id);
            var doctorappointment = db.appointments.Where(c => c.doctorId == id);
            var doctornotavailable = db.NotAvailableTimes.Where(c => c.doctorId == id);
            var transacdoctor = db.transactionsdoctors.Where(c => c.doctorId == id);


            if (transacdoctor.Any())
            {

                ModelState.AddModelError("", "Cannot delete this doctor because there are transactions associated with it.");
                return View(doctor);

            }

            if (doctorappointment.Any())
            {

                ModelState.AddModelError("", "Cannot delete this doctor because there are patients associated with it.");
                return View(doctor);

            }
            if (doctornotavailable.Any())
            {

                ModelState.AddModelError("", "Cannot delete this doctor because there are slots associated with it.");
                return View(doctor);

            }

            db.doctors.Remove(doctor);

            //emaiiil
            try
            {



        
            var docName = db.doctors.FirstOrDefault(c => c.doctorId == id).doctorName;
            var docemail = db.doctors.FirstOrDefault(c => c.doctorId == id).email;

            // Create a new MailMessage object
            MailMessage mail = new MailMessage();

            // Set the sender's email address
            mail.From = new MailAddress("musab.ghannam@outlook.com");

            // Set the recipient's email address

            mail.To.Add(docemail);

            // Set the subject of the email
            mail.Subject = "New message from " + "Finding piece";

            // Set the body of the email
            mail.Body = $@"<html>
                  <body>
                      <p>Dear Dr. {docName},</p>
                      <br/>
                      <p>We regret to inform you that we will be unable to accept your submission to join Finding Peace. After reviewing your qualifications, we have determined that they do not meet our requirements for participating on our platform.</p>
                      <br/>
                      <p>We value the quality of care that we provide to our patients, and we take the qualifications of our healthcare providers seriously. While we appreciate your interest in our platform, we cannot compromise on our commitment to delivering the best possible care to those who rely on us for support.</p>
                      <br/>
                      <p>Thank you for your understanding. If you have any questions or concerns, please don't hesitate to reach out to us at support@findingpeace.com.</p>
                      <br/>
                      <p>Best regards,</p>
                      <p>The Finding Peace Team</p>
                  </body>
              </html>";


            // Set the body format to HTML
            mail.IsBodyHtml = true;

            // Create a new SmtpClient object
            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("musab.ghannam@outlook.com", "124816326455@Mo");
            smtp.EnableSsl = true;

            // Send the email
            smtp.Send(mail);
            
                   }
            //email
            catch (Exception ex)
            {


            }

    //email


    db.SaveChanges();
            return RedirectToAction("AdminDoctor");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }




        public ActionResult ADminDashboard()
        {
            var doctors=db.doctors.Where(c=>c.statedoctor==1).ToList();
            var patients=db.patients.ToList();
            var appointments=db.appointments.ToList();

         







            return View(Tuple.Create(doctors, appointments, patients));
            //return View();
        }








    }
}
