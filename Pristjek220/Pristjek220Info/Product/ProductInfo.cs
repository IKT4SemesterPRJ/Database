namespace Pristjek220Data
{
    /// <summary>
    /// Class to contain the information about the product and the Quantity of it
    /// </summary>
    public class ProductInfo
    {
        /// <summary>
        /// Constructor that sets the name and the Quantity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="quantity"></param>
        public ProductInfo(string name, string quantity = "1")
        {
            Name = char.ToUpper(name[0]) + name.Substring(1).ToLower();
            Quantity = quantity;
        }

        /// <summary>
        /// Propperty to contains the name of the product
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// Propperty to contain the Quantity of the product
        /// </summary>
        public string Quantity { set; get; }
    }
}
