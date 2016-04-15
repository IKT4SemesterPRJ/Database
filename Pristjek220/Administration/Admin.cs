using System.Security;
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

        public void CreateLogin(string userName, string password, string storeName)
        {
            var store = _unitOfWork.Stores.FindStore(storeName);
            var securedpassword = new SecureString();

            foreach (var character in password)
            {
                securedpassword.AppendChar(character);
            }

            var login = new Pristjek220Data.Login() {Username = userName, Store = store};
            _unitOfWork.Logins.Add(login);
            _unitOfWork.Complete();
        }

        public void AddStore(Store store)
        {
            store.StoreName = char.ToUpper(store.StoreName[0]) + store.StoreName.Substring(1).ToLower();
            _unitOfWork.Stores.Add(store);
            _unitOfWork.Complete();
        }
    }
}
