using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pristjek220Data;

namespace SharedFunctionalities
{
    public class DatabaseFunctions : IDatabaseFunctions
    {
        private readonly IUnitOfWork _unit;

        public DatabaseFunctions(IUnitOfWork unit)
        {
            _unit = unit;
        }
        public bool ConnectToDB()
        {
            return _unit.Products.ConnectToDb();
        }
    }
}
