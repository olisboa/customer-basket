using System.Collections.Generic;
using CustomerBasket.Model;
using CustomerBasket.Services.Model;

namespace CustomerBasket.Services
{
    public interface IOfferService
    {
        void AddOffer(Offer offer);

        void AddOffer(IList<Offer> offer);

        IList<Offer> GetOffers();

        void ApplyOffers(ref Basket basket);
    }
}
