using ShoppingCart.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;


namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        ShoppingCartEntities db = new ShoppingCartEntities();
        int pageSize = 4;

        // GET: Cart
        public List<Cart> GetCart()
        {
            List<Cart> listCartItem = Session["Cart"] as List<Cart>;

            // If cart is null, create a new cart
            if (listCartItem == null)
            {
                listCartItem = new List<Cart>();
                Session["Cart"] = listCartItem;
            }

            return listCartItem;
        }

        public ActionResult Cart(int? page)
        {
            int pageNumber = (page ?? 1);
            List<Cart> listCartItem = GetCart();

            return View(listCartItem.ToPagedList(pageNumber, pageSize));
        }

        public ViewResult EditCart()
        {
            List<Cart> listCartItem = GetCart();

            return View(listCartItem);
        }

        public ActionResult AddToCart(int productID, string strUrl, FormCollection Form)
        {
            var product = FindProduct(productID);

            // Get current session cart
            List<Cart> listCartItem = GetCart();

            // Check if chosen product is exit or not
            Cart cartItem = listCartItem.Find(n => n.ProductID == productID);

            if (cartItem == null)
            {
                cartItem = new Cart(productID);
                cartItem.Quantity = int.Parse(Form["TxtQuantity"].ToString());
                listCartItem.Add(cartItem);
                return RedirectToAction("EditCart");
            }
            else
            {
                cartItem.Quantity += int.Parse(Form["TxtQuantity"].ToString());
            }
            return RedirectToAction("EditCart");
        }

        public ActionResult UpdateCart(int productID, FormCollection Form)
        {
            var product = FindProduct(productID);

            List<Cart> listCartItem = GetCart();

            Cart cartItem = listCartItem.SingleOrDefault(n => n.ProductID == productID);

            if (cartItem != null)
            {
                cartItem.Quantity = int.Parse(Form["TxtQuantity"].ToString());

            }
            return RedirectToAction("EditCart");
        }

        public ActionResult DeleteCart(int productID)
        {
            var product = FindProduct(productID);

            List<Cart> listCartItem = GetCart();

            Cart cartItem = listCartItem.SingleOrDefault(item => item.ProductID == productID);

            if (cartItem != null)
            {
                listCartItem.RemoveAll(item => item.ProductID == productID);

            }
            return RedirectToAction("Cart");
        }

        public int TotalQuantity()
        {
            int totalQuantity = 0;

            List<Cart> listCartItem = Session["Cart"] as List<Cart>;

            if (listCartItem != null)
            {
                totalQuantity = listCartItem.Sum(n => n.Quantity);
            }
            ViewBag.TotalQuantity = totalQuantity;
            return totalQuantity;
        }

        public double TotalPrice()
        {
            double totalMoney = 0;

            List<Cart> listCartItem = Session["Cart"] as List<Cart>;

            if (listCartItem != null)
            {
                totalMoney = listCartItem.Sum(n => n.TotalPrice);
            }

            ViewBag.TotalPrice = totalMoney;
            return totalMoney;
        }

        public ActionResult CartPartial()
        {
            if (TotalQuantity() == 0)
            {
                return PartialView();
            }

            ViewBag.TotalQuantity = TotalQuantity();

            ViewBag.TotalPrice = TotalPrice();

            return PartialView();
        }

        private Product FindProduct(int productID)
        {
            var product = db.Product.SingleOrDefault(n => n.ProductID == productID);

            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return product;
        }

    }
}