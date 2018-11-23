using System;
using System.Collections.Generic;
using System.Linq;
using CustomerBasket.Helper;

namespace CustomerBasket.Model
{
    public class Basket
    {
        private IList<Product> _products;
        public Basket(int customerId)
        {
            CustomerId = customerId;
            _products = new List<Product>();
        }
   
        /// <summary>
        /// Represent the CustomerId the basket belongs to
        /// </summary>
        public int CustomerId { get; private set; }
        /// <summary>
        /// Lists of Products with its quanity value added to the baskets
        /// </summary>
        public IList<ProductQuantity> Products {
            get {
                    return ProductGroup();
                }
        }

        /// <summary>
        /// Represents basket total of the products original prices
        /// </summary>
        public double SubTotal
        {
            get
            {
                return CalculateBasketTotal();
            }
        }

        /// <summary>
        /// Represents basket total of the products discounted prices
        /// </summary>
        public double DicountTotal
        {
            get
            {
                return CalculateBasketTotal("discount");
            }
        }

        /// <summary>
        /// Represents basket's subtotal less the total basket discounts
        /// </summary>
        public double Total
        {
            get
            {
                return Math.Round (SubTotal - DicountTotal, 2);
            }
        }

        /// <summary>
        /// Adds product to basket
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            if (product != null) _products.Add(product);
        }

        /// <summary>
        /// Adds products to basket
        /// </summary>
        /// <param name="products"></param>
        public void AddProduct(IList<Product> products)
        {
            if (products != null) _products.AddRange(products);
        }

        /// <summary>
        /// Updates a given product's discounted price value
        /// </summary>
        /// <param name="id"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        public void UpdateProductDiscountedPrice(int id, double price, int quantity)
        {
            //incase the product has more than the required quantity, ensure that only the required quantity is updated
            var products = _products.Where(p => p.ProductId == id).Take(quantity).ToList();
            products.ForEach(p => p.DiscountedPrice = price);
        }

       
        /// <summary>
        /// Returns the total price of products
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private double CalculateBasketTotal(string type = "sub")
        {
            var total = 0.00d;
            foreach (var item in _products ?? Enumerable.Empty<Product>())
            {
                //SubTotal
                if (type == "sub") total += item.Price;
                //Discounts Total
                else total += item.DiscountedPrice < 0 ? -item.DiscountedPrice : item.DiscountedPrice;
            }
            return total;
        }

        /// <summary>
        /// Represents the products in the basket with their respective quantities
        /// </summary>
        /// <returns></returns>
        private IList<ProductQuantity> ProductGroup()
        {
            var result = new List<ProductQuantity>();
            if (!_products.Any())
            {
                return result;
            }

            var groups = _products.GroupBy(p => new { p.ProductId, p.DiscountedPrice });
            foreach (var item in groups)
            {
                result.Add(new ProductQuantity()
                {
                    Product = item.FirstOrDefault(),
                    Quantity = item.Count()
                });
            }
            return result;
        }          
    }
}
