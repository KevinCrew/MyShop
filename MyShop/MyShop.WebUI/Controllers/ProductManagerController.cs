using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;


namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        // The contexr is now using the Irepository 
        IRepository<Product> context;
        // this is added to get the product categoires from from database 
        IRepository<ProductCategory> productCategories;

        // this is the dependancy injection of the irepositorys 
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext) {
            context = productContext;
            productCategories = productCategoryContext;
        }

        // GET: ProductManager and displays a list 
        public ActionResult Index()
        {
            List<Product> products = context.Collection().ToList(); 

            return View(products);
        }

        // Create the add page 
        // this first part calls the create page with an empty form 
        public ActionResult Create()
        {
            // with a view model this becomes 
            // first we need to initalise the view model 
            ProductManagerViewModel viewModel = new ProductManagerViewModel();

            // then we need to create the empty product from the view model 
            viewModel.Product = new Product();
            // then because the view model also includes a property of product categories we can call it 
            viewModel.productCategories = productCategories.Collection();

            // now we return the view model not the product model 
            return View(viewModel);

            // This is without the view model 
            //Product product = new Product();
            //return View(product);
        }

        // This creates the add page after it has been submited 
        // the HTTPPostedFileBase is to accept an uploaded file 
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            // check to see if the validation has been met and if not redirect to the add page 
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                if (file != null)
                {
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }

                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
        }

        // Create the edit 
        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if(product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.productCategories = productCategories.Collection();

                return View(viewModel);
            }
        }

        [HttpPost]
        //Create the actual save of the edit 
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
        {
            Product productToEdit = context.Find(Id);
            if(productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid) {
                    return View(product);
                }
                else
                {
                    if(file != null)
                    {
                        productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                    }

                    productToEdit.Category = product.Category;
                    productToEdit.Description = product.Description;
                    productToEdit.Name = product.Name;
                    productToEdit.Price = product.Price;

                    context.Commit();

                    return RedirectToAction("Index");
                } 

                    
            }


        }

        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);

            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);

            if(productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }

            
        }

    }
}