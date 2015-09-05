using ShoppingCart.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class ProductController : Controller
    {
        ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: Product
        public ViewResult ProductDetail(int productID = 0)
        {
            Product product = db.Product.SingleOrDefault(p => p.ProductID == productID);

            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(product);
        }

        public PartialViewResult ProductListPartial(int id, int categoryID = 0)
        {
            var listProduct = new List<Product>();

            if (id == 1)
            {
                listProduct = db.Product.Take(4).ToList();
            }
            else if (id == 2)
            {
                listProduct = db.Product.Where(p => p.Price > 3333).Take(4).ToList();
            }
            else if (id == 3)
            {
                listProduct = db.Product.Where(p => p.CategoryID == categoryID).ToList();
            }

            return PartialView(listProduct);
        }

        public PartialViewResult ProductDetailPartial(int productID)
        {
            Product product = db.Product.SingleOrDefault(p => p.ProductID == productID);

            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return PartialView(product);
        }

        public PartialViewResult RelatedProductListPartial(int productID, int categoryID)
        {
            var listProduct = db.Product.Where(p => p.CategoryID == categoryID
                && p.ProductID != productID).Take(4).ToList();

            return PartialView(listProduct);
        }
    }
}