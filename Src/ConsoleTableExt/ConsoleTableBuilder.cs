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

        public static ConsoleTableBuilder From(List<object[]> rows)
        {
            if (rows == null || !rows.Any())
            {
                throw new Exception("Invail rows");
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
                throw new Exception("Invail rows");
            }

            var builder = new ConsoleTableBuilder();

            foreach (var row in rows)
            {
                builder.Rows.Add(row);
            }

            return builder;
        }

        private List<int> ColumnLengths()
        {
            var columnLengths = new List<int>();

            var numberOfColumns = this.Rows.Max(x => x.Count);

            if (numberOfColumns < this.Column.Count)
            {
                numberOfColumns = this.Column.Count;
            }

            for (var i = 0; i < numberOfColumns; i++)
            {
                var maxRow = this.Rows.Select(x => x[i])
                    .Max(x => x == null ? 0 : x.ToString().Length);

                if (this.Column.ToArray().Length > i && (this.Column[i] ?? string.Empty).ToString().Length > maxRow)
                {
                    maxRow = this.Column[i].ToString().Length;
                }

                columnLengths.Add(maxRow);
            }

            if (!columnLengths.Any())
            {
                throw new Exception("Table has no columns");
            }

            if (this.Options.TrimColumn)
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
                    }
                }
            }

            return columnLengths;
        }

        internal string Format(char delimiter)
        {
            var columnLengths = ColumnLengths();
            var delimiterStr = delimiter == char.MinValue ? string.Empty : delimiter.ToString();
            var format = (Enumerable.Range(0, columnLengths.Count)
                              .Select(i => " " + delimiterStr + " {" + i + ",-" + columnLengths[i] + "}")
                              .Aggregate((s, a) => s + a) + " " + delimiterStr).Trim();
            return format;
        }
    }
}
