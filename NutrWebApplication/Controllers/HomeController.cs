using DAO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                    List<Dish> t = db.Dishes.OrderBy(d => d.Name).ToList();
                    json = JsonConvert.SerializeObject(t);
                }
                catch (Exception ex)
                {
                    json = ex.Message;
                }
            }

            return Json(new { success = true, json });
        }
        public ActionResult checkToAddDish(string tables, string dish)
        {
            ViewBag.Message = "Your contact page.";

            JArray tablesObject = JArray.Parse(tables);
            JObject dishObject = JObject.Parse(dish);

            int countDish = (int)dishObject["count"];
            int idDish = (int)dishObject["Id"];
            foreach (JObject table in tablesObject)
            {
                JArray listDishes = (JArray)table["listDishes"];
                if (listDishes != null)
                {
                    JObject dishInList = (JObject)listDishes.Where(d => (int)d["Id"] == idDish).FirstOrDefault();
                    if (dishInList != null)
                        countDish += (int)dishInList["count"];
                }
            }
            string errors = "";
            using (BurDBEntities db = new BurDBEntities())
            {
                foreach (DishIngredientList dishIngredient in db.DishIngredientLists.Where(i => i.IdDish == idDish))
                {
                    Ingredient ingredient = db.Ingredients.First(i => i.Id == dishIngredient.IdIngredient);
                    if (dishIngredient.Amount * countDish > dishIngredient.Ingredient.Amount)
                    {
                        errors += "Недостаточно \"" + ingredient.Name + "\" на складе. " + "Осталось: " + ingredient.Amount + ", нужно: " + dishIngredient.Amount * countDish + "\n";
                        //needBuy += "\t" + ingred.Ingredient.Name + "\n";
                    }
                }
            }
            if (errors != "")
                return Json(errors);
            else
                return Json(true);
        }
        
    }
}