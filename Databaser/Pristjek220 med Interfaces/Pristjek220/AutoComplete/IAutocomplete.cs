using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoComplete
{
    public interface IAutocomplete
    {
        List<string> AutoCompleteProduct(string lookUpWord);
        List<string> AutoCompleteStore(string lookUpWord);
    }
}
