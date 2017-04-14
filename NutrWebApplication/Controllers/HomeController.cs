using DAO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NutrWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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

        public ActionResult Students()
        {
            ViewBag.Message = "Your contact page.";

            var t = new Students();

            return View(t);
        }

        public ActionResult Students1(string tmp)
        {
            ViewBag.Message = "Your contact page.";

            var t = new Students();

            string json = JsonConvert.SerializeObject(t);

            return Json(new { success = true, json });
        }

        public ActionResult Dishes()
        {
            ViewBag.Message = "Your contact page.";

            var t = new Dish();
            using (BurDBEntities db = new BurDBEntities())
            {
            }

            return View(t);
        }

        public ActionResult Students2(string tmp)
        {
            ViewBag.Message = "Your contact page.";
            string json = "";
            using (BurDBEntities db = new BurDBEntities())
            {
                db.Database.Connection.Open();
                try
                {
                    List<Dish> t = db.Dishes.ToList();
                
                    json = JsonConvert.SerializeObject(t);
                }
                catch (Exception ex)
                {
                    json = ex.Message;
                }
            }

            return Json(new { success = true, json });
        }
    }
}