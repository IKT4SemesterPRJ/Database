using System.Security;
using Pristjek220Data;

namespace Administration
{
    /// <summary>
    ///     Interface for the Business logic layer for Admin
    /// </summary>
    public interface IAdmin
    {
        /// <summary>
        ///     Takes a Username, password and storename, if the store does not exist and the password is not empty the Store will
        ///     be created in the database with the password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="storeName"></param>
        /// <returns>-1 if store is not found, retruns -2 if password is empty and returns 1 if the login has been created</returns>
        int CreateLogin(string userName, SecureString password, string storeName);

        /// <summary>
        ///     Takes two SecureStrings and see if they are identical
        /// </summary>
        /// <param name="pass1"></param>
        /// <param name="pass2"></param>
        /// <returns>1 if the passwords are identical and 0 if they are not</returns>
        int CheckPasswords(SecureString pass1, SecureString pass2);

        /// <summary>
        ///     Finds a store with the wanted storeName
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>Returns the store that got the storeName</returns>
        Store FindStore(string storeName);

        /// <summary>
        ///     Delete store if it exist
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>0 if the store is delted and -1 if it does not exist in the database</returns>
        int DeleteStore(string storeName);
    }
}