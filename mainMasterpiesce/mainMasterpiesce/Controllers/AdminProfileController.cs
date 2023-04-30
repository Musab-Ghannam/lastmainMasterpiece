using mainMasterpiesce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mainMasterpiesce.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminProfileController : Controller
    {
        FindingpeaceEntities1 db = new FindingpeaceEntities1();
        // GET: AdminProfile
        public ActionResult ProfileAdmin()
        {
            if (User.IsInRole("Admin"))
            {
                var admin = db.AspNetUsers.Select(c => c.PhoneNumber);
                TempData["Admin"] = admin;

            }
         
            var info = db.AspNetUsers.ToList();
            var INFO1 = db.AspNetRoles.ToList();
            return View(Tuple.Create(info, INFO1));
        }
    }
}