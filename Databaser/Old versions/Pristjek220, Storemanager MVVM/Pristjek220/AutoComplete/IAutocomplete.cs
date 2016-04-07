using System.Collections.Generic;

namespace AutoComplete
{
    public interface IAutocomplete
    {
        List<string> AutoCompleteProduct(string lookUpWord);
        List<string> AutoCompleteStore(string lookUpWord);
    }
}
