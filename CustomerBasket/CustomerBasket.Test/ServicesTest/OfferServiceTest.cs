using System.Collections.Generic;
using CustomerBasket.Test.Helper;
using CustomerBasket.Services.Model;
using NUnit.Framework;
using CustomerBasket.Data;
using NSubstitute;
using CustomerBasket.Services;
using System.Linq;
using CustomerBasket.Model;

namespace CustomerBasket.Test.ServicesTest
{
    [TestFixture]
    public class OfferServiceTest
    {
        IRepository<Offer> offerRepo;
        IAccount account;

        [SetUp]
        public void Init()
        {
            //create a fake of the offer repository so that our test would not fail on the repository implementation details... 
            ///so that we dont need to access the real db.
            offerRepo = Substitute.For<IRepository<Offer>>();
            account = Substitute.For<IAccount>();
        }

        [Test]
        public void WhenNullOfferIsAdded_Then_NoOfferIsAddedToRepository()
        {
            //GIVEN
            Offer offer = null;
            offerRepo.GetAll().Returns((IList<Offer>) null);
            var offerService = new OfferService(offerRepo);

            //WHEN
            offerService.AddOffer(offer);

            //THEN
            //the mock objected created by NSubstitute record all calls it received. We call its DidNotReceive() method which 
            //checks whether the Save() method was not called with the given object(in this case Offer respository).
            offerRepo.DidNotReceive().Save(offer);
            Assert.AreEqual(0, offerRepo.GetAll()?.Count ?? 0);
        }

        [Test]
        public void WhenOfferIsAdded_Then_OfferIsAddedToRepository()
        {
            //GIVEN
            var offers = new List<Offer>() { TestHelper.Instance<Offer>() };
            
            //set offerRepo.GetAll() method to return Offer Instance
            offerRepo.GetAll().Returns(offers);
            var offerService = new OfferService(offerRepo); 

            //WHEN
            offerService.AddOffer(offers);

            //THEN
            //the mock objected created by NSubstitute record all calls it received. We call its DidNotReceive() method which 
            //checks whether the Save() method was not called with the given object(in this case Offer respository).
            offerRepo.Received().Save(offers);
            Assert.AreEqual(1, offerRepo.GetAll().Count);

        }

        [Test]
        //Given the basket has 2 butter and 2 bread when I total the basket then the total should be £3.10 
        [TestCase(new object[] { "2,Butter,£,0.80,1", "2,Bread,£,1.00,3" }, 3.10d)]
        //Given the basket has 4 milk when I total the basket then the total should be £3.45
        [TestCase(new object[] { "4,Milk,£,1.15,2" }, 3.45d)]
        //Given the basket has 2 butter, 1 bread and 8 milk when I total the basket then the totalshould be £9.00 
        [TestCase(new object[] { "2,Butter,£,0.80,1", "1,Bread,£,1.00,3","8,Milk,£,1.15,2" }, 9.00d)]
        public void GivenABasketWithProducts_AddTheProductsQualifyForOffers_Then_BasketTotalGivesCorrectTotalReducedByTheOffersAmount(object[] productstrings, double expectedTotal)
        {
            //GIVEN  
            var products = TestHelper.ConstructProduct(productstrings);
            //set offerRepo.GetAll() method to return a TestOffers
            offerRepo.GetAll().Returns(TestHelper.GetOffers());

            var offerService = new OfferService(offerRepo);
            var basketService = new BasketService(account, offerService);
            basketService.AddProducts(products);

            //WHEN
            basketService.ProcessBasket();

            //THEN
            Assert.AreEqual(products?.Count, basketService.Basket?.Products?.Sum(p => p.Quantity));
            Assert.AreEqual(expectedTotal, basketService.Basket?.Total);
        }


        [Test]
        //Given the basket has 2 butter and 2 bread when I total the basket then the total should be £3.60 
        [TestCase(new object[] { "2,Butter,£,0.80,1", "2,Bread,£,1.00,3" }, 3.60d)]
        //Given the basket has 4 milk when I total the basket then the total should be £4.60
        [TestCase(new object[] { "4,Milk,£,1.15,2" }, 4.60d)]
        //Given the basket has 2 butter, 1 bread and 8 milk when I total the basket then the totalshould be £11.80 
        [TestCase(new object[] { "2,Butter,£,0.80,1", "1,Bread,£,1.00,3", "8,Milk,£,1.15,2" }, 11.80d)]
        public void GivenABasketWithProducts_AddThereAreNoAvailableOffers_Then_BasketTotalGivesCorrectTotal(object[] productstrings, double expectedTotal)
        {
            //GIVEN  
            var products = TestHelper.ConstructProduct(productstrings);
            IList<Offer> offers = null;
            //set offerRepo.GetAll() method to return null offers
            offerRepo.GetAll().Returns(offers);

            var offerService = new OfferService(offerRepo);
            var basketService = new BasketService(account, offerService);
            basketService.AddProducts(products);

            //WHEN
            basketService.ProcessBasket();

            //THEN
            Assert.AreEqual(products?.Count, basketService.Basket?.Products?.Sum(p => p.Quantity));
            Assert.AreEqual(expectedTotal, basketService.Basket?.Total);
            Assert.AreEqual(0, basketService?.Basket?.DicountTotal);
        }


    }
}
