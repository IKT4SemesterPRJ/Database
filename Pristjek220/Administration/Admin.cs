﻿using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Pristjek220Data;

namespace Administration
{
    public class Admin : IAdmin
    {
        private readonly IUnitOfWork _unitOfWork;

        public Admin(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int CheckPasswords(SecureString pass1, SecureString pass2)
        {
            var password1 = ConvertToUnsecureString(pass1);
            var password2 = ConvertToUnsecureString(pass2);

            return password2 != password1 ? 0 : 1;
        }

        public int CreateLogin(string userName, SecureString password, string storeName)
        {
            if (_unitOfWork.Stores.FindStore(storeName) != null)
            {
                return -1;
            }

            var code = ConvertToUnsecureString(password);

            if (code == "")
                return -2;

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;

                //the user id is the salt. 
                //So 2 users with same password have different hashes. 
                //For example if someone knows his own hash he can't see who has same password
                string input = code;
                Byte[] result = hash.ComputeHash(enc.GetBytes(input));

                StringBuilder Sb = new StringBuilder();
                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
                code = Sb.ToString();
            }


            Store store = new Store() {StoreName = storeName};
            AddStore(store);
            userName = char.ToUpper(userName[0]) + userName.Substring(1).ToLower();
            var login = new Pristjek220Data.Login() {Username = userName, Password = code, Store = store};
            _unitOfWork.Logins.Add(login);
            return _unitOfWork.Complete();
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

            IntPtr unmanagedString = IntPtr.Zero;
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
