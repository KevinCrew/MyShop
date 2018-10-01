using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Basket :BaseEntity
    {
        // By setting this to a virtual iCollection it will tell the Entity framework to get 
        // both the basket and any items within it - this is lazy Loading !!!!!  
        public virtual ICollection<BasketItem> BasketItems { get; set; }

        // create a contrutor to create an emply basked on load 
        public Basket()
        {
            this.BasketItems = new List<BasketItem>();
        }
    }
}
