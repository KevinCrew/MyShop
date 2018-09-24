using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;


namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        // This is the constructor 
        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }

        // this is the method 
        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }


        // this is the add 
        public void Insert(ProductCategory p)
        {
            productCategories.Add(p);
        }

        // update method takes a product 
        public void Update(ProductCategory productCategory)
        {
            //create a new object of type product and set it to the product that we find in the products list 
            // this uses a lamda expressein 
            ProductCategory productCategoryToUpdate = productCategories.Find(p => p.Id == productCategory.Id);
            if (productCategoryToUpdate != null)
            {
                productCategoryToUpdate = productCategory;
            }
            else
            {
                throw new Exception("Product Category not found");
            }
        }

        // find the item 
        public ProductCategory Find(string Id)
        {
            ProductCategory productCategory = productCategories.Find(p => p.Id == Id);

            if (productCategory != null)
            {
                return productCategory;
            }
            else
            {
                throw new Exception("Product Category not found!");
            }

        }

        // return a list that can be queried 
        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string Id)
        {
            ProductCategory productCategoryToDelete = productCategories.Find(p => p.Id == Id);

            if (productCategoryToDelete != null)
            {
                productCategories.Remove(productCategoryToDelete);
            }
            else
            {
                throw new Exception("Product Category not found !");
            }

        }

    }
}
