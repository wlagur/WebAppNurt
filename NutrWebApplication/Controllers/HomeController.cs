using DAO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NutrWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NutrWebApplication.Controllers
{
    [AuthorizeUser]
    public class HomeController : Controller
    {
        [AllowAnonymous]
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
            //using (BurDBEntities db = new BurDBEntities())
            //{
            //    db.Database.Connection.Open();
            //}

            return View(t);
        }


        public ActionResult getDishes(string tmp)
        {
            ViewBag.Message = "Your contact page.";
            string json = "";
            using (BurDBNewEntities db = new BurDBNewEntities())
            {
                db.Database.Connection.Open();
                try
                {
                    List<Dish> t = db.Dishes.OrderBy(d => d.Name).ToList();
                    json = JsonConvert.SerializeObject(t);
                }
                catch (Exception ex)
                {
                    json = ex.InnerException.ToString();
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
            using (BurDBNewEntities db = new BurDBNewEntities())
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

        public ActionResult closeBill(string tables)
        {
            ViewBag.Message = "Your contact page.";
            
            try
            {
                JArray tablesObject = JArray.Parse(tables);
                using (BurDBNewEntities db = new BurDBNewEntities())
                {
                    foreach (JObject dishTable in tablesObject)
                    {
                        int id_user = Convert.ToInt32(User.Identity.Name);
                        var tran = new Transaction()
                        {
                            Name = (string)dishTable["Name"],
                            Date = DateTime.Now,
                            Amount = (double)dishTable["count"],
                            IdType = 1,
                            Price = (double)dishTable["Price"],
                            IdUser = id_user
                        };
                        db.Transactions.Add(tran);
                        int idDish = (int)dishTable["Id"];
                        foreach (DishIngredientList ingred in db.DishIngredientLists.Where(i => i.IdDish == idDish))
                        {
                            double amount = ingred.Amount ?? 0;
                            var updIngred = db.Ingredients.Where(i => i.Id == ingred.IdIngredient).First();
                            updIngred.Amount -= amount * (double)dishTable["count"];
                        }
                    }
                    db.SaveChanges();

                }
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        public ActionResult printBill(string tables)
        {
            ViewBag.Message = "Your contact page.";
            JArray tablesObject = JArray.Parse(tables);

            return PrintBillMethod(tablesObject);
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            var t = new LoginModel();

            return View(t);
        }
        [AllowAnonymous]
        public ActionResult enter(string name, string password)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                using (BurDBNewEntities db = new BurDBNewEntities())
                {
                    user = db.Users.FirstOrDefault(u => u.Login == name && u.Password == password);

                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), true);
                    var isAuthorized = User.Identity.Name;
                    return View("Dishes");
                }
            }
            return Json("Пользователя с таким логином и паролем нет");
        }


        public ActionResult logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        #region Print Methods
        private Font printFont;
        private StreamReader streamToPrint;
        ActionResult PrintBillMethod(JArray dishes)
        {
            StreamWriter file = new StreamWriter(@"c:\print.txt");

            string strInBill1, strInBill2;
            strInBill1 = "\"НУТРЬ\"";
            strInBill2 = new string(' ', (30 - strInBill1.Length) / 2);
            file.WriteLine(strInBill2 + strInBill1 + strInBill2);
            strInBill1 = "ФОП\"Крижанівська О.В.\"";
            strInBill2 = new string(' ', (30 - strInBill1.Length) / 2);
            file.WriteLine(strInBill2 + strInBill1 + strInBill2);
            strInBill1 = "м.Київ,бул.А.Вернадського,79";
            strInBill2 = new string(' ', (30 - strInBill1.Length) / 2);
            file.WriteLine(strInBill2 + strInBill1 + strInBill2);
            strInBill1 = "ІД  3344410140";
            strInBill2 = new string(' ', (30 - strInBill1.Length) / 2);
            file.WriteLine(strInBill2 + strInBill1 + strInBill2);
            strInBill1 = "ЗН АТ401300221  ФН 3000232091";
            strInBill2 = new string(' ', (30 - strInBill1.Length) / 2);
            file.WriteLine(strInBill2 + strInBill1 + strInBill2);
            strInBill1 = "Оператор30 Каса 01";
            strInBill2 = new string(' ', (30 - strInBill1.Length) / 2);
            file.WriteLine(strInBill2 + strInBill1 + strInBill2);
            file.WriteLine(new string('-', 30));

            double AllPrice = 0;
            foreach (JObject dish in dishes)
            {
                strInBill1 = (string)dish["Name"];
                if (strInBill1.Length > 30)
                    strInBill1 = ((string)dish["Name"]).Substring(0, 30);
                file.WriteLine(strInBill1);
                strInBill1 = (double)dish["count"] + " x " + (double)dish["Price"] + " = ";
                AllPrice += (double)dish["count"] * (double)dish["Price"];
                strInBill2 = (double)dish["count"] * (double)dish["Price"] + " грн.";
                strInBill1 = strInBill1 + new string(' ', 30 - strInBill1.Length - strInBill2.Length) + strInBill2;
                file.WriteLine(strInBill1);
            }

            file.WriteLine(new string('-', 30));
            strInBill1 = "Сумма ";
            strInBill2 = AllPrice + " грн.";
            strInBill1 = strInBill1 + new string(' ', 30 - strInBill1.Length - strInBill2.Length) + strInBill2;
            file.WriteLine(strInBill1);

            file.WriteLine();
            strInBill1 = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            strInBill2 = new string(' ', (30 - strInBill1.Length) / 2);
            file.WriteLine(strInBill2 + strInBill1 + strInBill2);

            strInBill1 = "Спасибо:) Всегда рады Вам!!!";
            strInBill2 = new string(' ', (30 - strInBill1.Length) / 2);
            file.WriteLine(strInBill2 + strInBill1 + strInBill2);

            file.Close();
            try
            {
                streamToPrint = new StreamReader
                   (@"c:\print.txt");
                try
                {
                    printFont = new Font("Lucida console", 10);
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler
                       (this.pd_PrintPage);
                    pd.Print();
                }
                finally
                {
                    streamToPrint.Close();
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 2;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;

            // Calculate the number of lines per page..
            linesPerPage = 29;
            //linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count *
                   printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }
        #endregion


    }

    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
                return false;
            return true;
        }
    }
}