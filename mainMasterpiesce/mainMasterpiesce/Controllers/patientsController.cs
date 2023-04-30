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
    [Authorize(Roles = "Admin")]
    public class patientsController : Controller
    {
        private FindingpeaceEntities1 db = new FindingpeaceEntities1();

        // GET: patients


        //        public ActionResult patientDashboard()
        //{
        //    var appointmentsByPatient = db.appointments.GroupBy(c => c.patientId).Count();
        //    ViewBag.ccount = appointmentsByPatient;
        //    var patients = db.patients
        //        .Include(p => p.AspNetUser)
        //        .ToList();
        //    var firstPatient = patients.FirstOrDefault();
        //    var orderedPatients = patients.OrderByDescending(p => p.appointments.Count(a => a.patientId == firstPatient.PatiantId)).ToList();
        //    return View(orderedPatients);
        //}




        public ActionResult patientDashboard(string search)
        {
            var appointmentsByPatient = db.appointments.GroupBy(c => c.patientId).Count();
            ViewBag.ccount = appointmentsByPatient;
            var patients = db.patients
       .Include(p => p.AspNetUser).OrderBy(c=>c.startedate).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                var searchh = db.patients.Where(c => c.patientName.Contains(search)).ToList();

                return View(searchh);

            }
            else
            {
                return View(patients);

            }



         
        }

        // GET: patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            patient patient = db.patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

       
            return View(patient);
        }

        // GET: patients/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatiantId,Id,locationpatent,picpatient,patientName,patientemail,startedate,wallet,Gender,Email,birthday,locationdetails")] patient patient)
        {
            if (ModelState.IsValid)
            {
                db.patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", patient.Id);
            return View(patient);
        }

        // GET: patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            patient patient = db.patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", patient.Id);
            return View(patient);
        }

        // POST: patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatiantId,Id,locationpatent,picpatient,patientName,patientemail,startedate,wallet,Gender,Email,birthday,locationdetails")] patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", patient.Id);
            return View(patient);
        }

        // GET: patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            patient patient = db.patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            patient patient = db.patients.Find(id);
            db.patients.Remove(patient);
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
