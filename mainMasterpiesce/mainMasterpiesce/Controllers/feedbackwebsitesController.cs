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
    public class feedbackwebsitesController : Controller
    {
        private FindingpeaceEntities1 db = new FindingpeaceEntities1();

        // GET: feedbackwebsites
        public ActionResult feedbackweb(string Accept,string Block)
        {
            if (Accept != null)
            {

                // Define the sweet alert message and options
                string sweetAlertMessage = "Are you sure you want to Accept Feedback?";
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



            if (Block != null )
            {

                // Define the sweet alert message and options
                string sweetAlertMessage = "Are you sure you want to block this feedBack?";
                string sweetAlertTitle = "Confirm Block";
                string sweetAlertIcon = "warning";
                string sweetAlertCancelButton = "Cancel";

                // Update the TempData and ViewBag variables
                TempData["swal_message"] = sweetAlertMessage;
                ViewBag.title = sweetAlertTitle;
                ViewBag.icon = sweetAlertIcon;
                ViewBag.cancelButton = sweetAlertCancelButton;

                TempData["IdBLOCK"] = Convert.ToInt16(Block);


            }



            var feedbackwebsites = db.feedbackwebsites.Include(f => f.patient);
            return View(feedbackwebsites.ToList());
        }


        public ActionResult Acceptt([Bind(Include = "statee")] feedbackwebsite feed, string Accept)
        {

            var feedbackk = db.feedbackwebsites.Find(TempData["Id"]);
            feedbackk.statee = 1;

            db.Entry(feedbackk).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("feedbackweb");




        }

        public ActionResult Block(int? id, [Bind(Include = "statee")] feedbackwebsite feed, string Accept)
        {

            int idd =Convert.ToInt16( TempData["IdBLOCK"]);

            var feedbackk = db.feedbackwebsites.FirstOrDefault(c=>c.id== idd);
            feedbackk.statee = 0;

            db.Entry(feedbackk).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("feedbackweb");




        }










        // GET: feedbackwebsites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            feedbackwebsite feedbackwebsite = db.feedbackwebsites.Find(id);
            if (feedbackwebsite == null)
            {
                return HttpNotFound();
            }
            return View(feedbackwebsite);
        }

        // GET: feedbackwebsites/Create
        public ActionResult Create()
        {
            ViewBag.patientId = new SelectList(db.patients, "PatiantId", "Id");
            return View();
        }

        // POST: feedbackwebsites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,patientId,name,email,message,created_at,statee")] feedbackwebsite feedbackwebsite)
        {
            if (ModelState.IsValid)
            {
                db.feedbackwebsites.Add(feedbackwebsite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.patientId = new SelectList(db.patients, "PatiantId", "Id", feedbackwebsite.patientId);
            return View(feedbackwebsite);
        }

        // GET: feedbackwebsites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            feedbackwebsite feedbackwebsite = db.feedbackwebsites.Find(id);
            if (feedbackwebsite == null)
            {
                return HttpNotFound();
            }
            ViewBag.patientId = new SelectList(db.patients, "PatiantId", "Id", feedbackwebsite.patientId);
            return View(feedbackwebsite);
        }

        // POST: feedbackwebsites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,patientId,name,email,message,created_at,statee")] feedbackwebsite feedbackwebsite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedbackwebsite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.patientId = new SelectList(db.patients, "PatiantId", "Id", feedbackwebsite.patientId);
            return View(feedbackwebsite);
        }

        // GET: feedbackwebsites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            feedbackwebsite feedbackwebsite = db.feedbackwebsites.Find(id);
            if (feedbackwebsite == null)
            {
                return HttpNotFound();
            }
            return View(feedbackwebsite);
        }

        // POST: feedbackwebsites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            

        




            feedbackwebsite feedbackwebsite = db.feedbackwebsites.Find(id);
            db.feedbackwebsites.Remove(feedbackwebsite);
            db.SaveChanges();
            return RedirectToAction("feedbackweb");
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
