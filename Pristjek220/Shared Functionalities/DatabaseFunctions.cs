using Pristjek220Data;

namespace SharedFunctionalities
{
    /// <summary>
    ///     Business logic layer for SharedFunctionalities
    /// </summary>
    public class DatabaseFunctions : IDatabaseFunctions
    {
        private readonly IUnitOfWork _unit;

        /// <summary>
        ///     DatabaseFunctions constructor takes a UnitOfWork to access the database
        /// </summary>
        /// <param name="unit"></param>
        public DatabaseFunctions(IUnitOfWork unit)
        {
            _unit = unit;
        }

        /// <summary>
        ///     Force a connects to the database
        /// </summary>
        /// <returns></returns>
        public bool ConnectToDb()
        {
            return _unit.Products.ConnectToDb();
        }
    }
}