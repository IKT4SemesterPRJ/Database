namespace Pristjek220Data
{
    /// <summary>
    ///     A class that contains the information about a product found in a specific store, and the quantity of that product that the Consumer wants
    /// </summary>
    public class StoreProductAndPrice
    {
        /// <summary>
        /// The name of the store
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// The name of the Product
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        ///     The price of the product
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        ///     The quantity of the product
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        ///     The sum is the price multiplied witht the quantity
        /// </summary>
        public double Sum { get; set; }
    }
}
