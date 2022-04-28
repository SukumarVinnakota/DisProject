using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DisProject.Models;

namespace DisProject.Controllers
{
    public class VehicleRegistrationController : Controller
    {
        ProjectEntities1 db = new ProjectEntities1();
        // GET: VehicleRegistration
        public ActionResult Index()
        {
            var registration = db.VehicleRegistrations.ToList();
            return View(registration);
        }

        // GET: VehicleRegistration/Details/5
        public ActionResult Details(String id=null)
        {
            VehicleRegistration vg = db.VehicleRegistrations.Find(id);
            if (vg == null)
            {
                return HttpNotFound();
            }
            return View(vg);
        }

        public ActionResult OrderByModel()
        {
            var name = from n in db.VehicleRegistrations
                       orderby n.Model ascending
                       select n;
            return View(name);
        }
        // GET: VehicleRegistration/Create
        public ActionResult Create()
        {
             var items = db.Cities.ToList();
            if (items != null)
            {
                ViewBag.data = items;
            }
            return View();
        }

        // POST: VehicleRegistration/Create
        [HttpPost]
        public ActionResult Create(VehicleRegistration vg)
        {
            try
            {
                
                var items = db.Cities.ToList();
                ViewBag.data = items;
                vg.State = "WA";
                ViewBag.error = "";
                VehicleRegistration vgs = db.VehicleRegistrations.Find(vg.VehcileRegistrationId);
                if (vgs == null)
                {
                    db.VehicleRegistrations.Add(vg);
                    db.SaveChanges();
                    // TODO: Add insert logic here
                    //   return RedirectToAction("Index");
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Vehicle is already registerd";
                    return View();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //Console.WriteLine("AN ERROR OCCURED IN COPYING CHROMEPASSFILE: " + EX.Message);
                return View();
            }
        }

        // GET: VehicleRegistration/Edit/5
        public ActionResult Edit(String id=null)
        {
            VehicleRegistration vg = db.VehicleRegistrations.Find(id);
            var items = db.Cities.ToList();
            ViewBag.data = items;
            if (vg == null)
            {
                return HttpNotFound();
            }
            return View(vg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehicleRegistration vg)
        {
            var items = db.Cities.ToList();
            ViewBag.data = items;
            if (ModelState.IsValid)
            {
                db.Entry(vg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vg);
        }
        // POST: VehicleRegistration/Edit/5
       /* [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }*/

        // GET: VehicleRegistration/Delete/5
        public ActionResult Delete(String id=null)
        {
            VehicleRegistration vg = db.VehicleRegistrations.Find(id);
            if (vg == null)
            {
                return HttpNotFound();
            }
            return View(vg);
        }

        // POST: VehicleRegistration/Delete/5
       /* [HttpPost]
        public ActionResult Delete(String id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }*/

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            VehicleRegistration vg = db.VehicleRegistrations.Find(id);
            db.VehicleRegistrations.Remove(vg);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
