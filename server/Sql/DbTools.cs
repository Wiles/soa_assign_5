using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace server.Sql
{
    public class DbTools
    {
        public static string CleanTableName(string tableName)
        {
            return tableName.Replace("dbo.", "").Replace("[", "").Replace("]", "");
        }
    }
}