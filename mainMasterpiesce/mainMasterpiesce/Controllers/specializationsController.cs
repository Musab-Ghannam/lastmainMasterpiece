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
    public class specializationsController : Controller
    {
        private FindingpeaceEntities1 db = new FindingpeaceEntities1();

        // GET: specializations
        public ActionResult Specialization(string search)
        {


            if (!string.IsNullOrEmpty(search))
            {
                var searchh = db.specializations.Where(c=>c.namespecialization.Contains(search)).ToList();

                return View(searchh);

            }
            else
            {
                return View(db.specializations.ToList());

            }

          
        }

        // GET: specializations/Details/5
        public ActionResult Details(int? id)
        {
            var desc = db.specializations.FirstOrDefault(c => c.specializationId == id).descriptionspecialization;
            ViewBag.desc = desc.Substring(desc.Length / 2);
            ViewBag.desc2 = desc.Substring(0, desc.Length / 2);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            specialization specialization = db.specializations.Find(id);
            if (specialization == null)
            {
                return HttpNotFound();
            }
            return View(specialization);
        }

        // GET: specializations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: specializations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "specializationId,namespecialization,descriptionspecialization,picturespecialization,videospecializaion")] specialization specialization)
        {
            if (ModelState.IsValid)
            {
                db.specializations.Add(specialization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(specialization);
        }

        // GET: specializations/Edit/5
        public ActionResult Edit(int? id)
        {
            var desc = db.specializations.FirstOrDefault(c => c.specializationId == id).descriptionspecialization;
            ViewBag.desc = desc.Substring(desc.Length / 2);
            ViewBag.desc2 = desc.Substring(0, desc.Length / 2);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            specialization specialization = db.specializations.Find(id);
            if (specialization == null)
            {
                return HttpNotFound();
            }
            return View(specialization);
        }

        // POST: specializations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "specializationId,namespecialization,descriptionspecialization,picturespecialization,videospecializaion")] specialization specialization,string desc)
        {
            if (ModelState.IsValid)
            {
                specialization.descriptionspecialization= desc;
                db.Entry(specialization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(specialization);
        }

        // GET: specializations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            specialization specialization = db.specializations.Find(id);
            if (specialization == null)
            {
                return HttpNotFound();
            }
            return View(specialization);
        }

        // POST: specializations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            specialization specialization = db.specializations.Find(id);
            var doctorsWithSpecialization = db.doctors.Where(d => d.specializationId == id);

            if (doctorsWithSpecialization.Any())
            {
                ModelState.AddModelError("", "Cannot delete specialization because there are doctors associated with it.");
                return View(specialization);
            }

            db.specializations.Remove(specialization);
            db.SaveChanges();



            return RedirectToAction("Index");

            //specialization specialization = db.specializations.Find(id);
            //db.specializations.Remove(specialization);
            //db.SaveChanges();
            //return RedirectToAction("Index");
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
