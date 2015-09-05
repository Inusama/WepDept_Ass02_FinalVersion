using ShoppingCart.Models;
using System.Linq;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class CategoryController : Controller
    {
        ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: Category
        public ViewResult ProductByCategory(int categoryID)
        {
            return View(db.Product.Where(p => p.CategoryID == categoryID).ToList());
        }

        public string GetCategory(int categoryID)
        {
            var category = db.Category.Single(c => c.CategoryID == categoryID);

            return (category.Title);
        }


        public PartialViewResult CategoryPartial()
        {
            return PartialView(db.Category.ToList());
        }
    }
}