using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Pristjek220Data;

namespace Administration
{
    /// <summary>
    ///     Business logic layer for Admin
    /// </summary>
    public class Admin : IAdmin
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        ///     Admin constructor takes a UnitOfWork to access the database
        /// </summary>
        /// <param name="unitOfWork"></param>
        public Admin(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///     Takes two SecureStrings and see if they are identical
        /// </summary>
        /// <param name="pass1"></param>
        /// <param name="pass2"></param>
        /// <returns>1 if the passwords are identical and 0 if they are not </returns>
        public int CheckPasswords(SecureString pass1, SecureString pass2)
        {
            var password1 = ConvertToUnsecureString(pass1);
            var password2 = ConvertToUnsecureString(pass2);

            return password2 != password1 ? 0 : 1;
        }

        /// <summary>
        ///     Takes a Username, password and storename, if the store does not exist and the password is not empty the Store will
        ///     be created in the database with the password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="storeName"></param>
        /// <returns>-1 if store is not found, retruns -2 if password is empty and returns 1 if the login has been created</returns>
        public int CreateLogin(string userName, SecureString password, string storeName)
        {
            if (_unitOfWork.Stores.FindStore(storeName) != null)
            {
                return -1;
            }

            var code = ConvertToUnsecureString(password);

            if (code == "")
                return -2;

            using (var hash = SHA256.Create())
            {
                var enc = Encoding.UTF8;
                
                var input = code;
                var result = hash.ComputeHash(enc.GetBytes(input));

                var sb = new StringBuilder();
                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
                code = sb.ToString();
            }


            var store = new Store {StoreName = storeName};
            AddStore(store);
            userName = char.ToUpper(userName[0]) + userName.Substring(1).ToLower();
            var login = new Login {Username = userName, Password = code, Store = store};
            _unitOfWork.Logins.Add(login);
            return _unitOfWork.Complete();
        }

        /// <summary>
        ///     Finds a store with the requested storeName
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>Returns the store that got the storeName</returns>
        public Store FindStore(string storeName)
        {
            return _unitOfWork.Stores.FindStore(storeName);
        }

        /// <summary>
        ///     Delete store if it exist
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>0 if the store is delted and -1 if it does not exist in the database</returns>
        public int DeleteStore(string storeName)
        {
            var store = _unitOfWork.Stores.FindStore(storeName);
            if (store == null)
                return -1;

            _unitOfWork.Stores.Remove(store);
            _unitOfWork.Complete();

            return 0;
        }

        private void AddStore(Store store)
        {
            store.StoreName = char.ToUpper(store.StoreName[0]) + store.StoreName.Substring(1).ToLower();
            _unitOfWork.Stores.Add(store);
            _unitOfWork.Complete();
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
            {
                return string.Empty;
            }

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}