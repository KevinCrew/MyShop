using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Product : BaseEntity
    {
        // removed becuase this now uses the Base entity which creates the ID
        //public string Id { get; set; }

        [StringLength(20)]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, 1000)]
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }

        // This is no longer needed as the Base Entity class does this 
        // This is a constructor that ensures that an ID is generated  when the model is called 
        //public Product()
       // {
       //     this.Id = Guid.NewGuid().ToString();
       // }

    }

   
}
