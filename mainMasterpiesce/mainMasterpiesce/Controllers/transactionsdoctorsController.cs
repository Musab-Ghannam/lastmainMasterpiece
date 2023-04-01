using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mainMasterpiesce.Models;
using Microsoft.Ajax.Utilities;

namespace mainMasterpiesce.Controllers
{
    public class transactionsdoctorsController : Controller
    {
        private FindingpeaceEntities1 db = new FindingpeaceEntities1();

        // GET: transactionsdoctors
        public ActionResult DoctorTransaction()
        {
            //DateTime transcatedate = DateTime.ParseExact(DateTime.Now.ToString("h:mm ttdd/MM"), "h:mm ttdd/MM", CultureInfo.InvariantCulture);

            DateTime transactionDate = DateTime.Now;
            if (transactionDate.DayOfWeek == DayOfWeek.Friday && transactionDate.Hour == 18)
            {
          
       

                var doctorInfo = db.appointments
    .Where(a => a.confirmappointment == 0)
    .Select(a => new { a.doctor.doctorId, a.doctor.doctorName })
    .DistinctBy(a => a.doctorId)
    .ToList();


                //ViewBag.DoctorNames = string.Join(",", doctorNames);

                foreach (var doctor in doctorInfo)
                {
                    var transactionDoctor = new transactionsdoctor
                    {
                        DOctorName = doctor.doctorName,
                        doctorId = doctor.doctorId,
                        status = "1"
                        
                    };

                    // Add the new transaction doctor to the database
                    db.transactionsdoctors.Add(transactionDoctor);
                }

                var doc = db.appointments.Where(c => c.confirmappointment == 0).ToList();

                foreach (var item in doc)
                {

                   item.confirmappointment = 1;//when I say 1 the info reach transactiodoc table without money



                 
                }

                // Save changes to the database
                db.SaveChanges();



            }

            if (transactionDate.DayOfWeek == DayOfWeek.Friday && transactionDate.Hour == 18)
            {






                var doctorAppointmentSum = db.appointments
                 .Where(a => a.confirmappointment == 1)
                 .GroupBy(a => a.doctorId)
                 .Select(g => new { DoctorId = g.Key, TotalAppointmentPrice = g.Sum(a => a.apointmentprice) })
                 .ToList();

                foreach (var appointment in doctorAppointmentSum)
                {
                    var transaction = db.transactionsdoctors
                        .SingleOrDefault(t => t.status == "1" && t.doctorId == appointment.DoctorId);

                    if (transaction != null)
                    {
                        transaction.amount = appointment.TotalAppointmentPrice*.95;
                        transaction.Tansactiontime = DateTime.Now;
                        transaction.status = "2";
                    }

                    var appconfirm = db.appointments.Where(c => c.confirmappointment == 1).ToList();

                    foreach (var item in appconfirm)
                    {
                        item.confirmappointment = 2;

                    }


                }

                db.SaveChanges();

            }

            // Add 7 days to the appointmentTime variable
            //transcatedate = transcatedate.AddDays(7);






                var transactionsdoctors = db.transactionsdoctors.Include(t => t.doctor);
           
          

            return View(transactionsdoctors.ToList());

        }

        // GET: transactionsdoctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transactionsdoctor transactionsdoctor = db.transactionsdoctors.FirstOrDefault(c=>c.doctorId==id);
            if (transactionsdoctor == null)
            {
                return HttpNotFound();
            }
            return View(transactionsdoctor);
        }

        // GET: transactionsdoctors/Create
        public ActionResult Create()
        {
            ViewBag.doctorId = new SelectList(db.doctors, "doctorId", "Id");
            return View();
        }

        // POST: transactionsdoctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "transDoctorId,doctorId,transactiontype,amount,paymentmethod,transactionDate,status,description,Tansactiontime")] transactionsdoctor transactionsdoctor)
        {
            if (ModelState.IsValid)
            {
                db.transactionsdoctors.Add(transactionsdoctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.doctorId = new SelectList(db.doctors, "doctorId", "Id", transactionsdoctor.doctorId);
            return View(transactionsdoctor);
        }

        // GET: transactionsdoctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transactionsdoctor transactionsdoctor = db.transactionsdoctors.Find(id);
            if (transactionsdoctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.doctorId = new SelectList(db.doctors, "doctorId", "Id", transactionsdoctor.doctorId);
            return View(transactionsdoctor);
        }

        // POST: transactionsdoctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "transDoctorId,doctorId,transactiontype,amount,paymentmethod,transactionDate,status,description,Tansactiontime")] transactionsdoctor transactionsdoctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transactionsdoctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.doctorId = new SelectList(db.doctors, "doctorId", "Id", transactionsdoctor.doctorId);
            return View(transactionsdoctor);
        }

        // GET: transactionsdoctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transactionsdoctor transactionsdoctor = db.transactionsdoctors.Find(id);
            if (transactionsdoctor == null)
            {
                return HttpNotFound();
            }
            return View(transactionsdoctor);
        }

        // POST: transactionsdoctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            transactionsdoctor transactionsdoctor = db.transactionsdoctors.Find(id);
            db.transactionsdoctors.Remove(transactionsdoctor);
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
