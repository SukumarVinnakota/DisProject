using DisProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisProject.Controllers
{
    public class HomeController : Controller
    {
        ProjectEntities1 db = new ProjectEntities1();
        public ActionResult Index()
        {
            var registration = db.VehicleRegistrations.ToList();
            List<DataPoint> dataPoints = new List<DataPoint>();

            var summaryApproach1 = registration.GroupBy(t => t.Make)
                           .Select(t => new
                           {
                               Category = t.Key,
                               Count = t.Count(),
                           }).ToList();
            int h = (from b in registration
                             select b).Count();
            foreach (var items in summaryApproach1)
            {
                int a = items.Count;
                double per = a * 100 / h;
                dataPoints.Add(new DataPoint(items.Category, per));
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}