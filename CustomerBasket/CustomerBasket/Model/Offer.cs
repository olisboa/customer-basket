namespace CustomerBasket.Services.Model
{
    public class Offer
    {
        public int OfferId { get; set; }
        public string OfferName { get; set; }
        public string OfferDescription { get; set; }
        public int RequiredProductId { get; set; }
        public int RequiredProductQuantity { get; set; }

        public int OfferProductId { get; set; }
        public int OfferProductQuantity { get; set; }
        public double DiscountRate  {get; set;}
    }
}