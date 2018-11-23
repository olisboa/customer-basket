
using System.Collections.Generic;
using AutoFixture;
using CustomerBasket.Model;
using CustomerBasket.Services.Model;

namespace CustomerBasket.Test.Helper
{
    /// <summary>
    /// Helper static class to create required test data
    /// </summary>
    public static class TestHelper
    {
        private static Fixture any = new Fixture();

        /// <summary>
        /// Returns instance of the given type where the values of the instance are irrelevant
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static T Instance<T>()
        {
            return any.Create<T>();
        }


        internal static IList<Product> ConstructProduct(object[] productstring)
        {
            var products = new List<Product>();
            foreach (var text in productstring)
            {
                var item = text.ToString().Split(',');
                var quantity = int.Parse(item[0]);
                for (int i = 0; i < quantity; i++)
                {
                    products.Add(new Product()
                    {  
                        Name = item[1],
                        Currency = item[2],
                        Price = double.Parse(item[3]),
                        ProductId = int.Parse(item[4]),
                    });
                }
              
            }
            return products;
        }


        internal static IList<Product> GetProducts()
        {
            return new List<Product>()
            {
                new  Product()
                {
                    ProductId = 1,
                    Name = "Butter",
                    Currency = "£",
                    Price = 0.80d
                },
                new  Product()
                {
                    ProductId = 2,
                    Name = "Milk",
                    Currency = "£",
                    Price = 1.15d
                },
                new  Product()
                {
                    ProductId = 3,
                    Name = "Bread",
                    Currency = "£",
                    Price = 1d
                }
            };
        }

        internal static IList<Offer> GetOffers()
        {
            return new List<Offer>()
            {
                ButterOffer(),
                MilkOffer()
            };
        }

        internal static Offer ButterOffer()
        {
            return new Offer()
            {
                OfferId = 1,
                OfferName = "2ButterGetBreadAt50%Off" ,
                OfferDescription= "Buy 2 Butter and get a Bread at 50% off" ,
                RequiredProductId =  1,
                RequiredProductQuantity = 2,

                OfferProductId =  3,
                OfferProductQuantity =  1,
                DiscountRate =  50d,

            };
        }

        internal static Offer MilkOffer()
        {
            return new Offer()
            {
                OfferId = 2,
                OfferName = "3MilkGet4thFree",
                OfferDescription = "Buy 3 Milk and get the 4th milk for free",
                RequiredProductId = 2,
                RequiredProductQuantity = 3,

                OfferProductId = 2,
                OfferProductQuantity = 1,
                DiscountRate = 100d,

            };
        }

    }
}