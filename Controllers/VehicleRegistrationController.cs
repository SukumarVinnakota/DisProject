using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DisProject.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisProject.Controllers
{
    public class VehicleRegistrationController : Controller
    {
        ProjectEntities1 db = new ProjectEntities1();
        // GET: VehicleRegistration
        public ActionResult Index()
        {
            var registration = db.VehicleRegistrations.Take(50).ToList();
            return View(registration);
        }
        
        public ActionResult Search(String id = null)
        {
            var registration = db.VehicleRegistrations.Where(p => p.VehcileRegistrationId.Contains(id));
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
            if (items != null )
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
                if (vg.AddressLine2.Equals(null)) vg.AddressLine2 = " ";
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
            try
            {
                var items = db.Cities.ToList();
                vg.State = "WA";
                vg.Make.Trim().ToUpper();
                vg.Model.Trim();
                if (vg.AddressLine2.Equals(null)) vg.AddressLine2 = " ";
                ViewBag.data = items;
                if (ModelState.IsValid)
                {
                    db.Entry(vg).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(vg);
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

        public ActionResult Api()
        {
            HttpClient httpClient;

            string BASE_URL = "https://data.wa.gov/resource/f6w7-q2d2.json";
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string NATIONAL_PARK_API_PATH = BASE_URL;
            string parksData = "";

            List<API> parks = null;
            VehicleRegistration vg = new VehicleRegistration();
            //httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);
            httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);

            try
            {
                //HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH)
                //                                        .GetAwaiter().GetResult();
                HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH)
                                                        .GetAwaiter().GetResult();



                if (response.IsSuccessStatusCode)
                {
                    parksData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!parksData.Equals(""))
                {
                    // JsonConvert is part of the NewtonSoft.Json Nuget package
                    parks = JsonConvert.DeserializeObject<API[]>(parksData).ToList();
                }
                List<City> c = db.Cities.ToList();

                foreach (var a in parks)
                {
                    if (a.state.Equals("WA"))
                    {
                        City cc = new City();
                        foreach (var bb in c)
                        {
                            if (a.city.Equals(bb.CityName))
                            {
                                vg.CityId = bb.CityId;
                                cc.CityId = bb.CityId;
                                cc.CityName = bb.CityName;
                                break;
                            }
                        }
                        vg.City = cc;
                        vg.VehcileRegistrationId = a.vin_1_10;
                        vg.Zip = Int32.Parse(a.zip_code);
                        vg.ModelYear = a.model_year;
                        vg.Make = a.make;
                        vg.Model = a.model;
                        vg.VehicleType = a.ev_type;
                        vg.CAFV = a.cafv_type;
                        vg.ElectricRange = Int32.Parse(a.electric_range);
                        vg.AddressLine1 = " ";
                        vg.AddressLine2 = " ";
                        vg.State = a.state;
                        db.VehicleRegistrations.Add(vg);
                        db.SaveChanges();
                        Console.WriteLine("hi");
                    }
                   // db.VehicleRegistrations.Add(vg);
                    //db.SaveChanges();
                }

                // db.API.Add(parks);
                //await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }

            return RedirectToAction("Index");
        }
    }
}
