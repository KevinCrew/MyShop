﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;


namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        ProductRepostory context;
        // this is added to get the product categoires from from database 
        ProductCategoryRepository productCategories;

        public ProductManagerController() {
            context = new ProductRepostory();
            productCategories = new ProductCategoryRepository();
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
        [HttpPost]
        public ActionResult Create(Product product)
        {
            // check to see if the validation has been met and if not redirect to the add page 
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
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
        public ActionResult Edit(Product product, string Id)
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
                    productToEdit.Category = product.Category;
                    productToEdit.Description = product.Description;
                    productToEdit.Image = product.Image;
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