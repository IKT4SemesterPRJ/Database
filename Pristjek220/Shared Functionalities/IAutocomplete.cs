using System.Collections.Generic;

namespace SharedFunctionalities
{
    /// <summary>
    ///     Interface for the Business logic layer for Autocomplete
    /// </summary>
    public interface IAutocomplete
    {
        /// <summary>
        ///     Takes a string and look up what products start with that string
        /// </summary>
        /// <param name="lookUpWord"></param>
        /// <returns>A list with the looked up products</returns>
        List<string> AutoCompleteProduct(string lookUpWord);

        /// <summary>
        ///     Takes a string and look up what stores start with that string
        /// </summary>
        /// <param name="lookUpWord"></param>
        /// <returns>A list with the looked up stores</returns>
        List<string> AutoCompleteStore(string lookUpWord);

        /// <summary>
        ///     Takes a string and look up what products start with that string in specific store
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="lookUpWord"></param>
        /// <returns>A list with the looked up products</returns>
        List<string> AutoCompleteProductForOneStore(string storeName, string lookUpWord);
    }
}