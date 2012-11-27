using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using shared.FormData;

namespace server.Sql
{
    public class TableQuery
    {
        /// <summary>
        /// The table types
        /// </summary>
        private static Dictionary<string, Type> tableTypes = new Dictionary<string, Type>()
        {
            { "Product", typeof(Product)},
            { "Cart", typeof(Cart)},
            { "Order", typeof(Order)},
            { "Customer", typeof(Customer)}
        };

        /// <summary>
        /// The parameter types
        /// </summary>
        private enum ParamType
        {
            None,
            Table,
            Column,
            Value
        }

        /// <summary>
        /// The table name
        /// </summary>
        private string TableName;

        /// <summary>
        /// The column name
        /// </summary>
        private string ColumnName;

        /// <summary>
        /// The value
        /// </summary>
        private string Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableQuery" /> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="value">The value.</param>
        public TableQuery(string tableName, string columnName, string value)
        {
            this.TableName = tableName;
            this.ColumnName = columnName;
            this.Value = value;
        }

        /// <summary>
        /// To the table column value.
        /// </summary>
        /// <returns></returns>
        public TableColumnValue ToTableColumnValue()
        {
            return new TableColumnValue(TableName, ColumnName, Value);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("Table {0} Column {1} Value {2}", TableName, ColumnName, Value);
        }

        /// <summary>
        /// Lists the queries from path.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
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