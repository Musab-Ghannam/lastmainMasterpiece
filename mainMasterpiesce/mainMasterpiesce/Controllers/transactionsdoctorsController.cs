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
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Hangfire;
using Hangfire.SqlServer;
using Lw.Data.Entity;
using Lw.Data;

namespace mainMasterpiesce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class transactionsdoctorsController : Controller
    {
        public IDbContext DbContext { get; set; }

        private FindingpeaceEntities1 db = new FindingpeaceEntities1();

        // GET: transactionsdoctors
        [AllowAnonymous]
        public ActionResult DoctorTransaction(string search,string Send)
        {
            if (TempData["list"] == null)
            {


                TempData["list"] = "Acceptlist";
            }






            //DateTime transcatedate = DateTime.ParseExact(DateTime.Now.ToString("h:mm ttdd/MM"), "h:mm ttdd/MM", CultureInfo.InvariantCulture);

            DateTime transactionDate = DateTime.Now;
            //transactionDate.DayOfWeek == DayOfWeek.Monday && transactionDate.Hour == 18
            if (true)
            {
          
       

                var doctorInfo = db.appointments
    .Where(a => a.confirmappointment == 0)
    .Select(a => new { a.doctor.doctorId, a.doctor.doctorName })
    .DistinctBy(a => a.doctorId)
    .ToList();
                var doctorAppointmentSum = db.appointments
               .Where(a => a.confirmappointment == 0)
               .GroupBy(a => a.doctorId)
               .Select(g => new { DoctorId = g.Key, TotalAppointmentPrice = g.Sum(a => a.apointmentprice) })
               .ToList();

                //ViewBag.DoctorNames = string.Join(",", doctorNames);

                var Isexist=db.transactionsdoctors.Where(c=>c.status!="2").ToList();
                List<string> doctorIdExist = new List<string>();
                string doctorIdExistString = "";
                foreach (var item in Isexist)
                {
                    doctorIdExist.Add(item.doctorId.ToString());
                    doctorIdExistString = string.Join(",", doctorIdExist);

                }



            

                foreach (var doctor in doctorInfo)
                {

                    if (!doctorIdExist.Contains(doctor.doctorId.ToString()))
                    {


                 
                      var transactionDoctor = new transactionsdoctor
                    {
                        DOctorName = doctor.doctorName,
                        doctorId = doctor.doctorId,
                        amount=0,
                        status = "1",
                       

                };

                    // Add the new transaction doctor to the database
                    db.transactionsdoctors.Add(transactionDoctor);
              
                db.SaveChanges();
                    }
                }
                //var doctorr = db.doctors.FirstOrDefault(c => c.doctorId == docId);
                //doctorr.statedoctor = 1;

                //db.Entry(doctorr).State = EntityState.Modified;

                foreach (var appointment in doctorAppointmentSum)
                {


                    var transactionDoctor = db.transactionsdoctors.FirstOrDefault(c => c.doctorId == appointment.DoctorId&&c.status!="2");

                    transactionDoctor.amount = appointment.TotalAppointmentPrice * .95+ transactionDoctor.amount;
                    db.Entry(transactionDoctor).State = EntityState.Modified;

                    //    var transactionDoctor = new transactionsdoctor
                    //    {
                    //       amount=appointment.TotalAppointmentPrice*.95,



                    //};

                    // Add the new transaction doctor to the database
                    //db.transactionsdoctors.Add(transactionDoctor);
                }





                var doc = db.appointments.Where(c => c.confirmappointment == 0).ToList();

                foreach (var item in doc)
                {

                   item.confirmappointment = 1;//when I say 1 the info reach transactiodoc table without money



                 
                }

                // Save changes to the database
                db.SaveChanges();



            }
            //var checktime = db.transactionsdoctors.OrderBy(c=>c.Tansactiontime).FirstOrDefault();
            var checktime = db.transactionsdoctors.OrderByDescending(c=>c.Tansactiontime).FirstOrDefault();
            if (checktime != null)
            {

                TimeSpan? timeDifference = DateTime.Now - checktime.Tansactiontime;
                ViewBag.DEFER = timeDifference.Value.Days + (timeDifference.Value.Hours / 24);

                ViewBag.exact = checktime.Tansactiontime.Value.Date.ToString("dd/MM/yyyy");
            }




                if (Send != null)
            {

                TempData["list"] = "rejectlisttrans";
               
                int countapoint = 0;


                var doctorAppointmentSum = db.appointments
                 .Where(a => a.confirmappointment == 1)
                 .GroupBy(a => a.doctorId)
                 .Select(g => new { DoctorId = g.Key, TotalAppointmentPrice = g.Sum(a => a.apointmentprice) })
                 .ToList();


                if (countapoint > 0 && ViewBag.DEFER < 7)
                {
                    TempData["list"] = "Rejectlist";
                    TempData["swal_message"] = "Please note that transactions can only be initiated after a minimum of 7 days from your last transaction for optimal care. Thank you for your understanding.";
                    ViewBag.title = "Warning";
                    ViewBag.icon = "warning";
                    

               


                }


                foreach (var appointment in doctorAppointmentSum)
                {
                    if (ViewBag.DEFER < 7)
                    {
                        break;
                    }




                    countapoint++;
                    var transaction = db.transactionsdoctors
                        .SingleOrDefault(t => t.status == "1" && t.doctorId == appointment.DoctorId);

                    if (transaction != null)
                    {
                        //transaction.amount = appointment.TotalAppointmentPrice*.95;
                        transaction.Tansactiontime = DateTime.Now;
                        transaction.status = "2";
                    }

                    var appconfirm = db.appointments.Where(c => c.confirmappointment == 1).ToList();







                    foreach (var item in appconfirm)
                    {
                      


                        item.confirmappointment = 2;

                    }


                }







                if (countapoint > 0&& ViewBag.DEFER>=7)
                {

                    TempData["swal_message"] = $" We are delighted to inform you that the transaction for the doctors has been successfully completed. Thank you for your cooperation and promptness in this process";

                    ViewBag.title = "success";
                    ViewBag.icon = "success";


                    db.SaveChanges();
                }
                else if(countapoint ==0)
                {
                    TempData["swal_message"] = "We would like to inform you that there are no pending transactions to be sent at this time. If you have any questions or require further assistance, please do not hesitate to contact us. Thank you for your attention to this matter";
                    ViewBag.title = "Warning";
                    ViewBag.icon = "warning";

                }

     

           

            }

            // Add 7 days to the appointmentTime variable
            //transcatedate = transcatedate.AddDays(7);






                var transactionsdoctors = db.transactionsdoctors.Include(t => t.doctor);

            if (!string.IsNullOrEmpty(search))
            {
                var searchh = db.transactionsdoctors.Where(c => c.doctor.doctorName.Contains(search) ).ToList();

                return View(searchh);

            }
            else
            {
                return View(transactionsdoctors.ToList());

            }


  

        }









        public ActionResult Acceptlist([Bind(Include = "status")] transactionsdoctor transacytiondoc, string Accept)
        {

            TempData["list"] = "Acceptlisttrans";

            return RedirectToAction("DoctorTransaction", new { listType = "Acceptlisttrans" });

        }
        public ActionResult Rejectlist([Bind(Include = "status")] transactionsdoctor transactiondoc, string Accept)
        {

            TempData["list"] = "Rejectlisttrans";


            return RedirectToAction("DoctorTransaction", new { listType = "rejectlisttrans" });


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
