using System;
using System.Collections.Generic;
using CustomerBasket.Services.Model;

namespace CustomerBasket.Data
{
    ///TODO : Implement the DB logic whether through DBContext or SqlConnection.
    ///This implementation is intentionally left incompleted due to time conttraint. 
    ///Its implements an interface hence it will be testable and the testing data implementation details will be substituted during the test execution.
    /// <summary>
    /// Offer Respository Implementation
    /// </summary>
    public class OfferRepository : IRepository<Offer>
    {
        
        public IList<Offer> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Save(Offer item)
        {
            throw new NotImplementedException();
        }

        public void Save(IList<Offer> items)
        {
            throw new NotImplementedException();
        }
    }
}
