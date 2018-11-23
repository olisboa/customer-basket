using System.Collections.Generic;

namespace CustomerBasket.Data
{
    /// <summary>
    /// Repository Interface to enable decoupling of the repositories from real implementations.
    /// </summary>
    public interface IRepository<T>
    {
        /// <summary>
        /// Returns all the item
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// Stores single Item
        /// </summary>
        /// <param name="item"></param>
        void Save(T item);

        /// <summary>
        /// Stores a list of item
        /// </summary>
        /// <param name="items"></param>
        void Save(IList<T> items);
    }
}
