using System.Collections.Generic;

namespace Autocomplete
{
    public interface IAutocomplete
    {
        List<string> AutoCompleteProduct(string lookUpWord);
        List<string> AutoCompleteStore(string lookUpWord);
    }
}