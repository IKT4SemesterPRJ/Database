using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Pristjek220Data;

namespace SharedFunctionalities
{
    /// <summary>
    /// The namespace <c>Autocomplete</c> contains the class <see cref="Autocomplete"/> 
    /// and the interface <see cref="IAutocomplete"/> which it implements and is placed 
    /// in the Business Logic Layer.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    class NamespaceDoc
    { }
    /// <summary>
    /// This class is used to generate the lists used for the autocomplete list in the GUI. 
    /// There is a version for products and one for stores.
    /// </summary>
    public class Autocomplete : IAutocomplete
    {
        private readonly IUnitOfWork _unit;

        /// <summary>
        ///     Autocomplete constructor takes a UnitOfWork to access the database
        /// </summary>
        /// <param name="unit"></param>
        public Autocomplete(IUnitOfWork unit)
        {
            _unit = unit;
        }

        /// <summary>
        /// Method for making an autocomplete list for use with store names.
        /// </summary>
        /// <param name="lookUpWord">One or more characters to create the autocomplete list from.</param>
        /// <returns>Returns a list containing maximum three products that matches the parameter given.
        /// If no matches exist the method returns null.</returns>
        public List<string> AutoCompleteProduct(string lookUpWord)
        {
            var productList = _unit.Products.FindProductStartingWith(lookUpWord).OrderBy(x => x.ProductName).ToList();

           var autoCompleteList = new List<string>();
            for (var i = 0; i < productList.Count; i++)
            {
                autoCompleteList.Add(productList[i].ProductName);
                if (i == 2)
                    break;
            }
            return autoCompleteList;
        }

        /// <summary>
        /// Method for making an autocomplete list for use with store names.
        /// </summary>
        /// <param name="lookUpWord">One or more characters to create the autocomplete list from.</param>
        /// <returns>Returns a list containing maximum three store names that matches the parameter given.
        /// If no matches exist the method returns null.</returns>
        public List<string> AutoCompleteStore(string lookUpWord)
        {
            var storeList = _unit.Stores.FindStoreStartingWith(lookUpWord).OrderBy(x => x.StoreName).ToList();

            var autoCompleteList = new List<string>();
            for (var i = 0; i < storeList.Count; i++)
            {
                if (storeList[i].StoreName != "Admin")
                    autoCompleteList.Add(storeList[i].StoreName);
                if (i == 2)
                        break;
                }
            return autoCompleteList;
        }

        /// <summary>
        ///     Method for making an autocomplete list for use with productnames in one store
        /// </summary>
        /// <param name="storeName">The Store to search for matches in</param>
        /// <param name="lookUpWord">One or more characters to create the autocomplete list from.</param>
        /// <returns>A list with the looked up products
        /// If no matches exist the method returns null.</returns>
        public List<string> AutoCompleteProductForOneStore(string storeName, string lookUpWord)
        {
            var productList = _unit.Stores.FindProductsInStoreStartingWith(storeName, lookUpWord).OrderBy(x => x.Name).ToList();
            
            var autoCompleteList = new List<string>();
            for (var i = 0; i < productList.Count; i++)
            {
                autoCompleteList.Add(productList[i].Name);
                if (i == 2)
                    break;
            }
            autoCompleteList.Sort();
            return autoCompleteList;
        }
    }
}