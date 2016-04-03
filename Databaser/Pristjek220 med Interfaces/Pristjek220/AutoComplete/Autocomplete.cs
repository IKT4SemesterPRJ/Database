using System.Collections.Generic;
using Pristjek220Data;

namespace AutoComplete
{
    public class Autocomplete : IAutocomplete
    {
        private IUnitOfWork _unit;

        public Autocomplete(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public List<string> AutoCompleteProduct(string lookUpWord)
        {
            var productList = _unit.Products.FindProductStartingWith(lookUpWord);

            if (productList == null)
                return null;          //Produktet findes ikke i databasen

            List<string> autoCompleteList = new List<string>();
            for (int i = 0; i < productList.Count; i++)
            {
                autoCompleteList.Add(productList[i].ProductName);
                if (i == 2)
                    break;
            }

            return autoCompleteList;
        }

        public List<string> AutoCompleteStore(string lookUpWord)
        {
            var storeList = _unit.Stores.FindStoreStartingWith(lookUpWord);

            if (storeList == null)
                return null;          //Produktet findes ikke i databasen

            List<string> autoCompleteList = new List<string>();
            for (int i = 0; i < storeList.Count; i++)
            {
                autoCompleteList.Add(storeList[i].StoreName);
                if (i == 2)
                    break;
            }

            return autoCompleteList;
        }
    }
}
