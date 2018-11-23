using System;
using System.Collections.Generic;
using CustomerBasket.Model;

namespace CustomerBasket.Services
{
    public class BasketService : IBasketService
    {
        private Basket _basket;
        private IOfferService _offerService;
        private IAccount _customer;
        public BasketService(IAccount customer, IOfferService offerService = null)
        {
            _customer = customer;
            _offerService = offerService;
        }

        /// <summary>
        /// Returns the private _basket object to maintain the basket state
        /// </summary>
        public Basket Basket
        {
            get { return _basket; }
        }

        /// <summary>
        /// Adds a single product to basket
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {   if (product == null) throw new ArgumentNullException("Product can't be null");
            InitiateBasket();
            _basket.AddProduct(product);
        }

        /// <summary>
        /// Adds multiple products to basket;
        /// </summary>
        /// <param name="products"></param>
        public void AddProducts(IList<Product> products)
        {
            InitiateBasket();
            _basket.AddProduct(products);
        }

        /// <summary>
        /// Applies the sets of given offers(when not null) that are applicable to the baskets products
        /// </summary>
        public void ProcessBasket()
        {
            if (_offerService != null)
            {
               _offerService.ApplyOffers( ref _basket);
            }
        }

       /// <summary>
       /// Private helper method to instanciate the basket incase its null;
       /// </summary>
        private void InitiateBasket()
        {
            if (_basket == null) _basket = new Basket(_customer.Id);
        }
    }
}