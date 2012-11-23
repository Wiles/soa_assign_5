using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace server.Sql
{
    public class TableQuery
    {
        private string TableName;
        private string ColumnName;
        private string Value;

        public TableQuery(string tableName, string columnName, string value)
        {
            this.TableName = tableName;
            this.ColumnName = columnName;
            this.Value = value;
        }

        public override string ToString()
        {
            return String.Format("Table {0} Column {1} Value {2}", TableName, ColumnName, Value);
        }

        private static Dictionary<string, Type> tableTypes = new Dictionary<string, Type>()
        {
            { "Product", typeof(Product)},
            { "Cart", typeof(Cart)},
            { "Order", typeof(Order)},
            { "Customer", typeof(Customer)}
        };

        private enum ParamType
        {
            None,
            Table,
            Column,
            Value
        }

        public static List<TableQuery> ListQueriesFromPath(string[] values)
        {
            using (var context = new SoaDataContext())
            {
                var tables = context.Mapping.GetTables();
                var tableNames = from t in tables
                                 select DbTools.CleanTableName(t.TableName);

                var tableQueries = new List<TableQuery>();

                var lastParamType = ParamType.None;
                var tableName = "";
                var columnName = "";
                foreach (var value in values)
                {
                    var expectingTable = (lastParamType == ParamType.None || lastParamType == ParamType.Value);
                    if (expectingTable)
                    {
                        var isTableName = tableNames.Where(t => value == t).Count() > 0;
                        if (isTableName)
                        {
                            tableName = tableNames.Where(t => value == t).FirstOrDefault();
                            lastParamType = ParamType.Table;
                        }
                        else
                        {
                            throw new Exception(String.Format("Table {0} not found", value));
                        }
                    }
                    else
                    {
                        var tableType = tableTypes[DbTools.CleanTableName(tableName)];

                        var columns = context.Mapping.MappingSource
                          .GetModel(typeof(SoaDataContext))
                          .GetMetaType(tableType)
                          .DataMembers;

                        var columnNames = from c in columns
                                          select c.MappedName;


                        if (lastParamType == ParamType.Table)
                        {
                            var isColumn = columnNames.Where(c => value == c).Count() > 0;
                            if (isColumn)
                            {
                                // This is a column
                                columnName = value;
                                lastParamType = ParamType.Column;
                            }
                            else
                            {
                                throw new Exception(String.Format("Table {0} must be followed by a column", tableName));
                            }
                        }
                        else if (lastParamType == ParamType.Column)
                        {
                            // This is a value

                            tableQueries.Add(new TableQuery(tableName, columnName, value));
                            tableName = "";
                            columnName = "";
                            lastParamType = ParamType.Value;
                        }
                        else
                        {
                            // This should never happen
                            Debug.Assert(false);
                        }
                    }
                }

                return tableQueries;
            }
        }
    }
}