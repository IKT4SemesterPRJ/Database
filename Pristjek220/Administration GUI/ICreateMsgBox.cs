using System.Windows.Forms;

namespace Administration_GUI
{
    /// <summary>
    /// Interface to the class CreateMsgBox
    /// </summary>
    public interface ICreateMsgBox
    {
        /// <summary>
        /// Creates a custom messages box for confirmation of delete product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returns DialogResult depending on what the users chooses</returns>
        DialogResult DeleteProductMgsConfirmation(string product);

        /// <summary>
        /// Creates a custom messages box for confirmation of add product
        /// </summary>
        /// <param name="shoppingListItem"></param>
        /// <param name="shoppingListItemPrice"></param>
        /// <returns>Returns DialogResult depending on what the users chooses</returns>
        DialogResult AddProductMgsConfirmation(string shoppingListItem, string shoppingListItemPrice);

        /// <summary>
        /// Creates a custom messages box for confirmation of change price
        /// </summary>
        /// <param name="shoppingListItem"></param>
        /// <param name="shoppingListItemPrice"></param>
        /// <returns>Returns DialogResult depending on what the users chooses</returns>
        DialogResult ChangePriceMgsConfirmation(string shoppingListItem, string shoppingListItemPrice);

        /// <summary>
        /// Creates a custom messages box for confirmation of Delete store
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>Returns DialogResult depending on what the users chooses</returns>
        DialogResult DeleteStoreMgsConfirmation(string storeName);
    }
}