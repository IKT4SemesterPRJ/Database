using System.Windows.Forms;

namespace Administration_GUI
{
    public interface ICreateMsgBox
    {
        DialogResult DeleteProductMgsConfirmation(string product);
        DialogResult AddProductMgsConfirmation(string shoppingListItem, string shoppingListItemPrice);
        DialogResult ChangePriceMgsConfirmation(string shoppingListItem, string shoppingListItemPrice);
        DialogResult DeleteStoreMgsConfirmation(string storeName);
    }
}