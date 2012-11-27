namespace server.Sql
{
    public class DbTools
    {
        /// <summary>
        /// Cleans the name of the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>The table name</returns>
        public static string CleanTableName(string tableName)
        {
            return tableName.Replace("dbo.", "").Replace("[", "").Replace("]", "");
        }
    }
}