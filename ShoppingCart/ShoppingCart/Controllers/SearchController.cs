using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        ShoppingCartEntities db = new ShoppingCartEntities();

        [HttpPost]
        public ActionResult ReturnSearch(FormCollection Form)
        {


            String strSearch = Form["TxtSearch"].ToString();

            List<Product> Result = db.Product.Where(n => n.Title.Contains(strSearch)).ToList();

            if (Result.Count == 0)
            {
                ViewBag.NoProduct = "No Product";
                return View(db.Product.OrderBy(n => n.Title).ToList());
            }

            return View(Result.ToList());
        }
    }
}