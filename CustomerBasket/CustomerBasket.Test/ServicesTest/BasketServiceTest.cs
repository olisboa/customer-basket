using CustomerBasket.Model;
using CustomerBasket.Services;
using CustomerBasket.Test.Helper;
using NSubstitute;
using NUnit.Framework;

namespace CustomerBasket.Test.ServicesTest
{
    [TestFixture]
    public class BasketServiceTest
    {
        IAccount account;

        [SetUp]
        public void Init()
        {
            //create a fake of the offer repository so that our test would not fail on the repository implementation details... 
            ///so that we dont need to access the real db.
           account = Substitute.For<IAccount>();
        }

        [Test]
        public void WhenProductIsAddedToBasket_Then_BasketIsNotEmpty()
        {
            //GIVEN
            var product = new Product()
            {
                Name = "Milk",
                Currency = "£",
                Price = 1.20d
            };

            var basketService = new BasketService(account);

            //WHEN
            basketService.AddProduct(product);

            //THEN
            Assert.IsNotEmpty(basketService.Basket?.Products);
            Assert.AreEqual(1, basketService.Basket?.Products?.Count);
        }

        [Test]
        public void WhenNullProductIsAddedToBasket_Then_BasketDoesNotContainProduct()
        {
            //GIVEN
            var basketService = new BasketService(account);

            //WHEN
            Assert.That(() => basketService.AddProduct(null), Throws.ArgumentNullException);

            //THEN
            Assert.IsNull(basketService.Basket?.Products);
            Assert.AreEqual(null, basketService.Basket?.Products?.Count);
        }

        [Test]
        [TestCase(new object[]{ "1,Bread,£,1.00,3", "1,Butter,£,0.80,1", "1,Milk,£,1.15,2"}, 2.95d)]
        [TestCase(new object[] { "1,Bread,£,1.00,3", "1,Butter,£,0.80,1" }, 1.80d)]
        [TestCase(new object[] { "1,Bread,£,1.00,3", "1,Milk,£,1.15,2" }, 2.15d)]
        [TestCase(new object[] { "1,Butter,£,0.80,1", "1,Milk,£,1.15,2" }, 1.95d)]
        [TestCase(new object[] { "1,Bread,£,1.00,3" }, 1.00d)]
        [TestCase(new object[] { "1,Butter,£,0.80,1"}, 0.80d)]
        [TestCase(new object[] { "1,Milk,£,1.15,2" }, 1.15d)]
        public void WhenProductsAreAddedToBasket_Then_BasketTotalGivesCorrectTotal(object[] productstrings, double expectedTotal)
        {
            //GIVEN
            var basketService = new BasketService(account);
            var products = TestHelper.ConstructProduct(productstrings);
            
            //WHEN
            basketService.AddProducts(products);

            //THEN
            Assert.IsNotNull(basketService.Basket?.Products);
            Assert.AreEqual(products?.Count, basketService.Basket?.Products?.Count);
            Assert.AreEqual(expectedTotal, basketService.Basket?.Total);
        }

    }

}
