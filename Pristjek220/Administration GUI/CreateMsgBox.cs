using System.Windows.Forms;

namespace Administration_GUI
{
    class CreateMsgBox : ICreateMsgBox
    {
        public CreateMsgBox()
        {}

        public DialogResult DeleteProductMgsConfirmation(string product)
        {
           return CustomMsgBox.Show($"Vil du fjerne produktet \"{product}\" fra din forretning?","Bekræftelse", "Ja", "Nej");
        }

        public DialogResult AddProductMgsConfirmation(string shoppingListItem, string shoppingListItemPrice)
        {
            return CustomMsgBox.Show(
                        $"Vil du tilføje produktet \"{shoppingListItem}\" med prisen {shoppingListItemPrice} kr til din forretning?",
                        "Bekræftelse", "Ja", "Nej");
        }

        public DialogResult ChangePriceMgsConfirmation(string shoppingListItem, string shoppingListItemPrice)
        {
            return CustomMsgBox.Show(
                            $"Vil du ændre prisen på produktet \"{shoppingListItem}\" til {shoppingListItemPrice} kr?",
                            "Bekræftelse", "Ja", "Nej");
        }

        public DialogResult DeleteStoreMgsConfirmation(string storeName)
        {
            return CustomMsgBox.Show($"Vil du fjerne forretningen \"{storeName}\" fra Pristjek220?", "Bekræftelse", "Ja", "Nej");
        }
    }
}
