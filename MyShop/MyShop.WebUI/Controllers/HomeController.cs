using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {

        // The contexr is now using the Irepository 
        IRepository<Product> context;
        // this is added to get the product categoires from from database 
        IRepository<ProductCategory> productCategories;

        // this is the dependancy injection of the irepositorys 
        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategories = productCategoryContext;
        }

        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList();

            return View(products);
        }
        public ActionResult Details(string Id)
        {
            Product product = context.Find(Id);
                if (product == null)
            {
                return HttpNotFound();
            }
                else
            {
                return View(product);
            }
            
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
    }
}