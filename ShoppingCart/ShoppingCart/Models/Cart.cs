using System;
using System.Linq;

namespace ShoppingCart.Models
{
    public class Cart
    {
        ShoppingCartEntities db = new ShoppingCartEntities();

        public int ProductID { get; set; }
        public String Title { get; set; }
        public String ImageUrl { get; set; }
        public Double Price { get; set; }
        public int Quantity { get; set; }

        public double TotalPrice
        {
            get { return Quantity * Price; }
        }

        public Cart(int productID)
        {
            var product = db.Product.Single(n => n.ProductID == productID);

            ProductID = productID;
            Title = product.Title;
            ImageUrl = product.ImageUrl;
            Price = double.Parse(product.Price.ToString());
            Quantity = 1;
        }
    }
}