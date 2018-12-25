using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ConsoleTableExt
{
    public class ConsoleTableBuilder
    {
        internal List<object> Column { get; set; }
        internal List<List<object>> Rows { get; set; }
        internal ConsoleTableBuilderOption Options { get; set; }
        internal ConsoleTableBuilderFormat TableFormat { get; set; }

        private ConsoleTableBuilder()
        {
            Column = new List<object>();
            Rows = new List<List<object>>();
            TableFormat = ConsoleTableBuilderFormat.Default;
            Options = new ConsoleTableBuilderOption();
        }

        public static ConsoleTableBuilder From(DataTable dt)
        {
            var builder = new ConsoleTableBuilder();

            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(x => x.ColumnName)
                .ToList();

            builder.Column = new List<object>(columnNames);

            foreach (DataRow row in dt.Rows)
            {
                builder.Rows.Add(new List<object>(row.ItemArray));
            }

            return builder;
        }

        public static ConsoleTableBuilder From<T>(List<T> list)
        {
            if (list == null || !list.Any())
            {
                throw new ArgumentException($"{nameof(list)} cannot be null or empty");
            }

            var builder = new ConsoleTableBuilder();
            var isClass = typeof(T).IsClass;
            var props = list.First().GetType().GetProperties();
            List<object> columnNames;
            if (isClass)
            {
                columnNames = props.Select(p => p.Name as object).ToList();
            }
            else
            {
                columnNames = new List<object> { "Value" };
            }


            builder.Column = columnNames;

            foreach (var item in list)
            {
                if (isClass == true)
                {
                    var itemPropValues = new List<object>();

                    foreach (var prop in props)
                    {
                        var objValue = prop.GetValue(item);
                        itemPropValues.Add(objValue);
                    }

                    builder.Rows.Add(itemPropValues);
                }
                else
                {
                    builder.Rows.Add(new List<object>{item });
                }
            }

            return builder;
        }

        public static ConsoleTableBuilder From(List<object[]> rows)
        {
            if (rows == null || !rows.Any())
            {
                throw new ArgumentException($"{nameof(rows)} cannot be null or empty");
            }

            var builder = new ConsoleTableBuilder();

            foreach (var row in rows)
            {
                builder.Rows.Add(new List<object>(row));
            }

            return builder;
        }

        public static ConsoleTableBuilder From(List<List<object>> rows)
        {
            if (rows == null || !rows.Any())
            {
                throw new ArgumentException($"{nameof(rows)} cannot be null or empty");
            }

            var builder = new ConsoleTableBuilder();

            foreach (var row in rows)
            {
                builder.Rows.Add(row);
            }

            return builder;
        }

        internal List<int> GetCadidateColumnLengths()
        {
            var columnLengths = new List<int>();

            var numberOfColumns = Rows.Max(x => x.Count);

            if (numberOfColumns < Column.Count)
            {
                numberOfColumns = Column.Count;
            }

            for (var i = 0; i < numberOfColumns; i++)
            {
                var maxRow = Rows.Select(x => x[i])
                    .Max(x => x == null ? 0 : x.ToString().Length);

                if (Column.ToArray().Length > i && (Column[i] ?? string.Empty).ToString().Length > maxRow)
                {
                    maxRow = Column[i].ToString().Length;
                }

                columnLengths.Add(maxRow);
            }

            if (!columnLengths.Any())
            {
                throw new Exception("Table has no columns");
            }

            if (Options.TrimColumn)
            {
                if (columnLengths.Any())
                {
                    var temp = columnLengths;
                    for (int i = temp.Count - 1; i >= 0; i--)
                    {
                        if (temp[i] == 0)
                        {
                            columnLengths.RemoveAt(i);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return columnLengths;
        }

        internal string Format(string delimiter)
        {
            var columnLengths = GetCadidateColumnLengths();

            if (columnLengths.Count > 0)
            {
                return (Enumerable.Range(0, columnLengths.Count)
                            .Select(i => " " + delimiter + " {" + i + ",-" + columnLengths[i] + "}")
                            .Aggregate((s, a) => s + a) + " " + delimiter).Trim();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
