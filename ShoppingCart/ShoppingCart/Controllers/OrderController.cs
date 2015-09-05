using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class OrderController : Controller
    {
       

            private ShoppingCartEntities db = new ShoppingCartEntities();

            // GET: Order
            public ActionResult Index()
            {
                return View();
            }

            [HttpGet]
            public ActionResult Order()
            {

                return View();
            }

            [HttpPost]
            public ActionResult Order([Bind(Include = "CreditCard,Address")] Order Order)
            {
                if (Session["Account"] == null || Session["Account"] == "")
                {
                    return RedirectToAction("Login", "Customer");
                }
                if (Session["Cart"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (Order.Address == null || Order.CreditCard == null)
                {
                    return RedirectToAction("Order", "Order");
                }

                // View();

                Customer Customer = (Customer) Session["Account"];
                List<Cart> Cart = GetCart();
                Order.CustomerID = Customer.CustomerID;
                Order.DateOrder = DateTime.Now;
                Order.TotalGrand = (decimal) TotalGrand();
                db.Order.Add(Order);
                db.SaveChanges();
                foreach (var item in Cart)
                {
                    OrderDetail OrderDetail = new OrderDetail();
                    OrderDetail.OrderID = Order.OrderID;
                    OrderDetail.ProductID = item.ProductID;
                    OrderDetail.Quantity = item.Quantity;
                    OrderDetail.TotalPrice = (decimal) item.TotalPrice;
                    db.OrderDetail.Add(OrderDetail);
                }
                db.SaveChanges();
                return RedirectToAction("Index", "Home");

                return View();

            }

            public List<Cart> GetCart()
            {
                List<Cart> ListCart = Session["Cart"] as List<Cart>;
                if (ListCart == null)
                {
                    ListCart = new List<Cart>();
                    Session["Cart"] = ListCart;

                }
                return ListCart;

            }

            private double TotalGrand()
            {
                double TotalGrand = 0;
                List<Cart> Cart = GetCart();
                foreach (var item in Cart)
                {
                    TotalGrand += item.TotalPrice;
                }
                return TotalGrand;

            }



        }
    }