using ShoppingCart.Models;
using System.Linq;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{

    public class CustomerController : Controller
    {
        ShoppingCartEntities db = new ShoppingCartEntities();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind(Include = "UserName,PassWord,Email,Phone")] Customer Customer)
        {
            var Check = db.Customer.SingleOrDefault(n => n.UserName == Customer.UserName );

            if (ModelState.IsValid && Check == null)
            {
                db.Customer.Add(Customer);
                db.SaveChanges();
                return RedirectToAction("Success", new { routeValue = 1 });
            }
            if (Check != null) { ViewBag.Message = "UserName Is Already Existed"; }
            else { ViewBag.Message = ""; }
            return View(Customer);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection Form)
        {
            string UserName = Form["TxtUserName"].ToString();
            string PassWord = Form["TxTPassWord"].ToString();

            var Customer = db.Customer.SingleOrDefault(n => n.UserName == UserName && n.PassWord == PassWord);

            if (Customer != null)
            {
                ViewBag.Respond = "Login Success";
                Session["Account"] = Customer;
                Session["UserName"] = Customer.UserName;

                return RedirectToAction("Success", new { routeValue = 2 });
            }

            ViewBag.Respond = "Wrong UserName or PassWord";
            return View();
        }

        public ActionResult Success(int routeValue)
        {
            ViewBag.RouteValue = routeValue;

            return View();
        }
    }
}