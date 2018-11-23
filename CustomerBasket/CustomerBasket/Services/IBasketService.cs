using System.Collections.Generic;
using CustomerBasket.Model;

namespace CustomerBasket.Services
{
    public interface IBasketService
    {
        void AddProduct(Product product);

        void AddProducts(IList<Product> products);
        void ProcessBasket();
    }
}