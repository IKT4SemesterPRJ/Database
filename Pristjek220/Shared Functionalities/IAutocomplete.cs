using System.Collections.Generic;
using Pristjek220Data;

namespace SharedFunctionalities
{
    public interface IAutocomplete
    {
        List<string> AutoCompleteProduct(string lookUpWord);
        List<string> AutoCompleteStore(string lookUpWord);
        List<string> AutoCompleteProductForOneStore(string storeName, string lookUpWord);
    }
}