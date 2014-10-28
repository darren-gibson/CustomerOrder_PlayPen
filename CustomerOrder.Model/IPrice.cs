namespace CustomerOrder.Model
{
    public interface IPrice
    {
        /// <summary>
        /// Prices the the customer order and returns a IPricedOrder as a result.
        /// </summary>
        /// <param name="order">The customer order to price</param>
        /// <exception cref="ProductNotFoundException">If the order contains an invalid product, then an ProductNotFoundException will be thrown</exception>
        /// <returns></returns>
        IPricedOrder Price(ICustomerOrder order);
    }
}
