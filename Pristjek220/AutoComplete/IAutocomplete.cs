using System.Collections.Generic;

namespace SharedFunctionalities
{
    public interface IAutocomplete
    {
        List<string> AutoCompleteProduct(string lookUpWord);
        List<string> AutoCompleteStore(string lookUpWord);
    }
}