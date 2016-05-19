using System.Windows.Forms;

namespace Administration_GUI
{
    /// <summary>
    /// Wrapper to custom messages boxes so that it is testable
    /// </summary>
    class CreateMsgBox : ICreateMsgBox
    {
        /// <summary>
        /// Creates a custom messages box for confirmation of delete product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returns DialogResult depending on what the users chooses</returns>
        public DialogResult DeleteProductMgsConfirmation(string product)
        {
           return CustomMsgBox.Show($"Vil du fjerne produktet \"{product}\" fra din forretning?","Bekræftelse", "Ja", "Nej");
        }

        /// <summary>
        /// Creates a custom messages box for confirmation of add product
        /// </summary>
        /// <param name="shoppingListItem"></param>
        /// <param name="shoppingListItemPrice"></param>
        /// <returns>Returns DialogResult depending on what the users chooses</returns>
        public DialogResult AddProductMgsConfirmation(string shoppingListItem, string shoppingListItemPrice)
        {
            return CustomMsgBox.Show(
                        $"Vil du tilføje produktet \"{shoppingListItem}\" med prisen {shoppingListItemPrice} kr til din forretning?",
                        "Bekræftelse", "Ja", "Nej");
        }

        /// <summary>
        /// Creates a custom messages box for confirmation of change price
        /// </summary>
        /// <param name="shoppingListItem"></param>
        /// <param name="shoppingListItemPrice"></param>
        /// <returns>Returns DialogResult depending on what the users chooses</returns>
        public DialogResult ChangePriceMgsConfirmation(string shoppingListItem, string shoppingListItemPrice)
        {
            return CustomMsgBox.Show(
                            $"Vil du ændre prisen på produktet \"{shoppingListItem}\" til {shoppingListItemPrice} kr?",
                            "Bekræftelse", "Ja", "Nej");
        }

        /// <summary>
        /// Creates a custom messages box for confirmation of Delete store
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>Returns DialogResult depending on what the users chooses</returns>
        public DialogResult DeleteStoreMgsConfirmation(string storeName)
        {
            return CustomMsgBox.Show($"Vil du fjerne forretningen \"{storeName}\" fra Pristjek220?", "Bekræftelse", "Ja", "Nej");
        }
    }
}
