using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models
{
    public class OrderSummary
    {
        ShoppingCartEntities db = new ShoppingCartEntities();

        public Order Order { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }

        public OrderSummary(Order order, List<OrderDetail> orderDetail)
        {
            this.Order = order;
            this.OrderDetail = orderDetail;
        }

        //public int OrderID { get; set; }
        //public int CustomerName { get; set; }
        //public String Address { get; set; }
        //public int CreditCard { get; set; }
        //public DateTime DateOrder { get; set; }

        //public double TotalGrand { get; set; }

        //public List<Product> listCartItem { get; set; }

    }
}