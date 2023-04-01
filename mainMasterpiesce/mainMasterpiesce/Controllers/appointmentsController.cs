using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mainMasterpiesce.Models;

namespace mainMasterpiesce.Controllers
{
    public class appointmentsController : Controller
    {
        private FindingpeaceEntities1 db = new FindingpeaceEntities1();

        // GET: appointments
        public ActionResult Index()
        {
            var totalprice = db.appointments.ToList();
            int sum = 0;
     
            foreach (var item in totalprice)
            {
               
                sum += item.apointmentprice ?? 0;
            }
            double websitedue = sum * .05;


            ViewBag.Sum = sum;  
            ViewBag.webdue= websitedue;

            var appointments = db.appointments.Include(a => a.doctor).Include(a => a.patient);
            return View(appointments.ToList());
        }

        // GET: appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointment appointment = db.appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: appointments/Create
        public ActionResult Create()
        {
            ViewBag.doctorId = new SelectList(db.doctors, "doctorId", "Id");
            ViewBag.patientId = new SelectList(db.patients, "PatiantId", "Id");
            return View();
        }

        // POST: appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "apointmentId,patientId,doctorId,starttime,endtime,doctornotes,patientnotes,apointmentprice,rating,BookingDate,dosage,dosagefrequency,medicationinstructions,confirmappointment,JoinUrl")] appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.doctorId = new SelectList(db.doctors, "doctorId", "Id", appointment.doctorId);
            ViewBag.patientId = new SelectList(db.patients, "PatiantId", "Id", appointment.patientId);
            return View(appointment);
        }

        // GET: appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointment appointment = db.appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.doctorId = new SelectList(db.doctors, "doctorId", "Id", appointment.doctorId);
            ViewBag.patientId = new SelectList(db.patients, "PatiantId", "Id", appointment.patientId);
            return View(appointment);
        }

        // POST: appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "apointmentId,patientId,doctorId,starttime,endtime,doctornotes,patientnotes,apointmentprice,rating,BookingDate,dosage,dosagefrequency,medicationinstructions,confirmappointment,JoinUrl")] appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.doctorId = new SelectList(db.doctors, "doctorId", "Id", appointment.doctorId);
            ViewBag.patientId = new SelectList(db.patients, "PatiantId", "Id", appointment.patientId);
            return View(appointment);
        }

        // GET: appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointment appointment = db.appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            appointment appointment = db.appointments.Find(id);
            db.appointments.Remove(appointment);
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
