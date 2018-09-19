using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepostory
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        // This is the constructor 
        public ProductRepostory()
        {
            products = cache["products"] as List<Product>;
            if (products == null)
            {
                products = new List<Product>(); 
            }
        }

        // this is the method 
        public void Commit()
        {
            cache["products"] = products;
        }


        // this is the add 
        public void Insert(Product product)
        {
            products.Add(product);
        }

        // update method takes a product 
        public void Update(Product product)
        {
            //create a new object of type product and set it to the product that we find in the products list 
            // this uses a lamda expressein 
            Product productToUpdate = products.Find(p => p.Id == product.Id);
            if (productToUpdate != null)
            {
                productToUpdate = product;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        // find the item 
        public Product Find(string Id)
        {
            Product product = products.Find(p => p.Id == Id);

            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("Product not found!");
            }

        }

        // return a list that can be queried 
    public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        public void Delete (string Id)
        {
            Product productToDelete = products.Find(p => p.Id == Id);
            
            if(productToDelete != null){
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception("Product not found !");
            }

        }
    }
}
