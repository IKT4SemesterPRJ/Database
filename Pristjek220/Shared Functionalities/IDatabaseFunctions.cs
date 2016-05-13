namespace SharedFunctionalities
{
    /// <summary>
    ///     Interface for the Business logic layer for DatabaseFunctions
    /// </summary>
    public interface IDatabaseFunctions
    {
        /// <summary>
        ///     Force a connects to the database
        /// </summary>
        /// <returns></returns>
        bool ConnectToDb();
    }
}