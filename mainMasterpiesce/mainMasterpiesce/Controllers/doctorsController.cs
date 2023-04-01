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

namespace mainMasterpiesce.Controllers
{
    public class doctorsController : Controller
    {
        private FindingpeaceEntities1 db = new FindingpeaceEntities1();

        // GET: doctors
        public ActionResult 
            AdminDoctor(string id, string Block,string Accept,string idaccep)
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
              
         

            }





            return View(doctors.ToList());
        }
  
        public ActionResult Acceptt([Bind(Include = "statedoctor")] doctor doctor, string Accept)
        {

          var doctorr=db.doctors.Find(TempData["Id"]);
            doctorr.statedoctor = 1;

            db.Entry(doctorr).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("AdminDoctor");




        }

        public ActionResult Block(int id, [Bind(Include = "statedoctor")] doctor doctor, string Accept)
        {
       


            var doctorr = db.doctors.Find(TempData["IdBLOCK"]);
            doctorr.statedoctor = 0;

            db.Entry(doctorr).State = EntityState.Modified;
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
                return RedirectToAction("Index");
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
            doctor doctor = db.doctors.Find(id);
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
            db.doctors.Remove(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
