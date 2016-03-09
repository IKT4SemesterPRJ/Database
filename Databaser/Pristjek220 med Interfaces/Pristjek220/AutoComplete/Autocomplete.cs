using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pristjek220Data;

namespace AutoComplete
{
    public class Autocomplete : IAutocomplete
    {
        private IUnitOfWork _unit;

        public Autocomplete(UnitOfWork unit)
        {
            _unit = unit;
        }

        public List<string> AutoComplete(string lookUpWord)
        {
            var productList = _unit.Products.FindProductStartingWith(lookUpWord);

            if (productList == null)
                return null;          //Produktet findes ikke i databasen

            List<string> autoCopmpleteList = new List<string>();
            for (int i = 0; i < productList.Count; i++)
            {
                autoCopmpleteList.Add(productList[i].ProductName);
                if (i == 2)
                    break;
            }

            return autoCopmpleteList;
        }
    }
}
