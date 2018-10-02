using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketServices
    {
        // access the Irepostoiry template for the product and the basket
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        // create a string to hold the basket session 
        public const string BasketSessionName = "eCommerceBasket";

        // create a contructor to use Dependance Injector to get the data
        public BasketServices(IRepository<Product> ProductContext, IRepository<Basket> BasketContext) {
            this.basketContext = BasketContext;
            this.productContext = ProductContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull) {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();

            if (cookie != null)
            {
                string basketId = cookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    basket = basketContext.Find(basketId);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }
            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext) {

            // create a new instance of the basket 
            Basket basket = new Basket();
            // insert the basket into the database 
            basketContext.Insert(basket);
            // commit the insert 
            basketContext.Commit();

            // Create the cookie  
            HttpCookie cookie = new HttpCookie(BasketSessionName);
            // set the value of the cookie to the basket ID 
            cookie.Value = basket.Id;
            // create a cookie exirey 
            cookie.Expires = DateTime.Now.AddDays(1);
            // push the cookie to the browser 
            httpContext.Response.Cookies.Add(cookie);

            return (basket);
        }

        public void AddToBasket(HttpContextBase httpContext, string productid)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.ProductId == productid);

            if (item==null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productid,
                    Quantity = 1
                };

                basket.BasketItems.Add(item);
            }
            else 
            {
                item.Quantity = item.Quantity + 1;
            }

            basketContext.Commit();
        }

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i => i.Id == itemId);

            if(item != null)
            {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }

        }

    }
}
