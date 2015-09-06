using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using System.Diagnostics;

namespace ShoppingCart.Controllers
{
    public class OrderController : Controller
    {       
            private ShoppingCartEntities db = new ShoppingCartEntities();

            [HttpGet]
            public ActionResult Order()
            {
                return View();
            }

            public ActionResult Summary(int orderID)
            {
                var order = db.Order.SingleOrDefault(o => o.OrderID == orderID);
                var orderDetails = db.OrderDetail.Where(o => o.OrderID == orderID).ToList();

                var orderSummary = new OrderSummary(order, orderDetails);

                return View(orderSummary);
            }

            [HttpPost]
            public ActionResult Order([Bind(Include = "Address,CreditCard")] Order Order, FormCollection form)
            {
                if (Session["Account"] == null)
                {
                    return RedirectToAction("Login", "Customer");
                }
                if (Session["Cart"] == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                if (Order.Address == null || form["TxtCreditCard"] == null)
                {
                    return RedirectToAction("Order", "Order");
                }

                Customer Customer = (Customer) Session["Account"];
                List<Cart> Cart = GetCart();
                Order.CustomerID = Customer.CustomerID;
                Order.DateOrder = DateTime.Now;
                Order.TotalGrand = (decimal) TotalGrand();
                Order.CreditCard = form["TxtCreditCard"];
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
                return RedirectToAction("Summary", "Order", new { orderID = Order.OrderID});
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