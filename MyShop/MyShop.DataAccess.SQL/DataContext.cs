using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class DataContext : DbContext
    {
        // We create a contructor that accesses the connection string in the Web Config 
        public DataContext() 
            : base("DefaultConnection")
        {
        }

        // next we have to tell the dbcontext which models we want to create the database tables 

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Basket> Basket { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
    }
}
