using System.Collections.Generic;
using System.Linq;
using CustomerBasket.Data;
using CustomerBasket.Model;
using CustomerBasket.Services.Model;

namespace CustomerBasket.Services
{
    public class OfferService : IOfferService
    {
        /// <summary>
        /// Represents the Offer repository
        /// </summary>
        private IRepository<Offer> _offerRepository;

        public OfferService(IRepository<Offer> offerRepository)
        {
            _offerRepository = offerRepository;
        }
        /// <summary>
        /// Adds a single Offer to the repository
        /// </summary>
        /// <param name="offer"></param>
        public void AddOffer(Offer offer)
        {
            if (offer != null)
                _offerRepository.Save(offer);
        }

        /// <summary>
        /// Adds a list of offers to the repository
        /// </summary>
        /// <param name="offers"></param>
        public void AddOffer(IList<Offer> offers)
        {
            if (offers != null)  _offerRepository.Save(offers);
        }

        /// <summary>
        /// Returns the list of offers in the repository
        /// </summary>
        /// <returns></returns>
        public IList<Offer> GetOffers()
        {
            return _offerRepository.GetAll();
        }


        /// <summary>
        /// Applies the applicable available offers to the products in the basket
        /// </summary>
        /// <param name="basket"></param>
        public void ApplyOffers(ref Basket basket)
        {
            var offers = GetOffers();
            if ( basket == null || basket.Products == null || !basket.Products.Any() || offers == null || !offers.Any()) return;

            var productIds = basket.Products.Select(p => p.Product.ProductId);
            var applicableOffers = offers.Where(o => productIds.Contains(o.RequiredProductId))?.ToList();

            if (applicableOffers.Count() == 0) return;
            CalculateOffers(applicableOffers, ref basket);

        }


        /// <summary>
        /// For each of the offers, if the offer condition is met by the given producs in the baskets, the sets of product's prices on offer are updated accordingly
        /// </summary>
        /// <param name="applicableOffers"></param>
        /// <param name="basket"></param>
        private void CalculateOffers(List<Offer> applicableOffers, ref Basket basket)
        {

            foreach (var offer in applicableOffers)
            {
                if (ItemInBasket(offer.RequiredProductId, offer.RequiredProductQuantity, basket.Products) &&
                    ItemInBasket(offer.OfferProductId, offer.OfferProductQuantity, basket.Products))
                {
                    UpdateBasketProductPrice(ref basket, offer.OfferProductId, offer.OfferProductQuantity, offer.DiscountRate, offer.RequiredProductQuantity);
                }

            }

        }

        /// <summary>
        /// Updates a number of given product's discount price(calculated based on the given percentage) in the given basket;
       
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="offerProductId"></param>
        /// <param name="offerProductQuantity"></param>
        /// <param name="offerProductPriceDiscount"></param>
        private void UpdateBasketProductPrice(ref Basket basket, int offerProductId, int offerProductQuantity, double discountRate, int requiredProductQuanity)
        {
            var product = basket.Products.FirstOrDefault(p => p.Product.ProductId == offerProductId);
            if (product != null)
            {
                //check the number products in the basket that the offer is applicable to
                int cycle = product.Quantity / requiredProductQuanity;
                offerProductQuantity = cycle == 0 ? offerProductQuantity : offerProductQuantity * cycle;
                var originalPrice = product.Product.Price;
                /// if the discounted price is 0 then set it to negative of the original price
                var price = originalPrice - (originalPrice * (discountRate / 100));

                basket.UpdateProductDiscountedPrice(product.Product.ProductId, price == 0 ? -originalPrice : price, offerProductQuantity);
                
            }
        }

        /// <summary>
        /// Helper method to verify if the basket contains an item and the specified quantity;
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="requiredQuantity"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        private bool ItemInBasket(int Id, int requiredQuantity, IList<ProductQuantity> products)
        {
            return products.FirstOrDefault(p => p.Product.ProductId == Id && p.Quantity >= requiredQuantity) != null;
        }
    }
}
