﻿using System;
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
    public class feedbacksController : Controller
    {
        private FindingpeaceEntities1 db = new FindingpeaceEntities1();

        // GET: feedbacks
        public ActionResult Review(string search)
        {
            var feedbacks = db.feedbacks.Include(f => f.doctor).Include(f => f.patient);

            if (!string.IsNullOrEmpty(search))
            {
                var searchh = db.feedbacks.Where(c => c.doctor.doctorName.Contains(search) || c.patient.patientName.Contains(search)).ToList();

                return View(searchh);

            }
            else
            {
                return View(feedbacks.ToList());

            }
        
        }

        // GET: feedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            feedback feedback = db.feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: feedbacks/Create
        public ActionResult Create()
        {
            ViewBag.doctorId = new SelectList(db.doctors, "doctorId", "Id");
            ViewBag.patientId = new SelectList(db.patients, "PatiantId", "Id");
            return View();
        }

        // POST: feedbacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "feedbackId,doctorId,patientId,rating,comment,title,statefeedback,feedbacktime")] feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.doctorId = new SelectList(db.doctors, "doctorId", "Id", feedback.doctorId);
            ViewBag.patientId = new SelectList(db.patients, "PatiantId", "Id", feedback.patientId);
            return View(feedback);
        }

        // GET: feedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            feedback feedback = db.feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.doctorId = new SelectList(db.doctors, "doctorId", "Id", feedback.doctorId);
            ViewBag.patientId = new SelectList(db.patients, "PatiantId", "Id", feedback.patientId);
            return View(feedback);
        }

        // POST: feedbacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "feedbackId,doctorId,patientId,rating,comment,title,statefeedback,feedbacktime")] feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.doctorId = new SelectList(db.doctors, "doctorId", "Id", feedback.doctorId);
            ViewBag.patientId = new SelectList(db.patients, "PatiantId", "Id", feedback.patientId);
            return View(feedback);
        }

        // GET: feedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            feedback feedback = db.feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            feedback feedback = db.feedbacks.Find(id);
            db.feedbacks.Remove(feedback);
            db.SaveChanges();
            return RedirectToAction("Review");
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
