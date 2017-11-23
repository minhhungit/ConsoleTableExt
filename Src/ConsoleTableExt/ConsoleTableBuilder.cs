using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ConsoleTableExt
{
    public class ConsoleTableBuilder
    {
        internal List<object> Columns { get; set; }
        internal List<object[]> Rows { get; set; }

        private ConsoleTableBuilder()
        {
            Columns = new List<object>();
            Rows = new List<object[]>();
        }

        public static ConsoleTableBuilder From(DataTable dt)
        {
            var builder = new ConsoleTableBuilder();

            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(x => x.ColumnName)
                .ToList();

            builder.AddColumn(columnNames, true);

            foreach (DataRow row in dt.Rows)
            {
                builder.AddRow(row.ItemArray);
            }

            return builder;
        }

        public static ConsoleTableBuilder From<T>(List<T> values) where T : IConsoleTableDataStore
        {
            var builder = new ConsoleTableBuilder();

            var properties = typeof(T).GetProperties();

            var columnHeader = new List<KeyValuePair<int, string>>();
            var columns = new List<string>();

            foreach (var propertyInfo in properties)
            {
                var attr = (ConsoleTableColumnAttributes)propertyInfo.GetCustomAttributes(typeof(ConsoleTableColumnAttributes), true).FirstOrDefault();
                if (attr != null)
                {
                    columns.Add(propertyInfo.Name);

                    columnHeader.Add(new KeyValuePair<int, string>(attr.Order, string.IsNullOrWhiteSpace(attr.Name) ? propertyInfo.Name : attr.Name));
                }
            }

            builder.AddColumn(columnHeader.OrderBy(x => x.Key).ThenBy(x => x.Value).Select(x => x.Value).ToList(), true);
            
            var source = values.Select(value => columns.Select(column => GetValue<T>(value, column)));
            foreach (var propertyValues in source)
                builder.AddRow(propertyValues.ToArray());

            return builder;
        }

        internal List<int> ColumnLengths()
        {
            var columnLengths = new List<int>();

            for (var i = 0; i < Columns.Count; i++)
            {
                var maxRow = Rows.Select(x => x[i]).Max(x => x.ToString().Length);
                var lColumn = Columns[i].ToString().Length;
                columnLengths.Add(maxRow >= lColumn ? maxRow : lColumn);
            }

            if (!columnLengths.Any())
            {
                throw new Exception("Table has no column");
            }

            return columnLengths;
        }

        internal string Format(List<int> columnLengths)
        {
            return Format(columnLengths, '|');
        }

        internal string Format(List<int> columnLengths, char delimiter)
        {
            var delimiterStr = delimiter == char.MinValue ? string.Empty : delimiter.ToString();
            var format = (Enumerable.Range(0, Columns.Count)
                              .Select(i => " " + delimiterStr + " {" + i + ",-" + columnLengths[i] + "}")
                              .Aggregate((s, a) => s + a) + " " + delimiterStr).Trim();
            return format;
        }

        private static object GetValue<T>(object target, string column)
        {
            return typeof(T).GetProperty(column).GetValue(target, null);
        }
    }
}
