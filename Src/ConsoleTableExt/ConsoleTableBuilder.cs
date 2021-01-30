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
        internal ConsoleTableBuilderFormat TableFormat { get; set; }
        internal Dictionary<CharMapPositions, char> CharMapPositions { get; set; } = null;
        internal Dictionary<HeaderCharMapPositions, char> HeaderCharMapPositions { get; set; } = null;
        internal List<KeyValuePair<MetaRowPositions, string>> TopMetadataRows = new List<KeyValuePair<MetaRowPositions, string>>();
        internal List<KeyValuePair<MetaRowPositions, string>> BottomMetadataRows = new List<KeyValuePair<MetaRowPositions, string>>();
        internal Dictionary<int, string> TextAligmentData = new Dictionary<int, string>();
        internal Dictionary<int, int> MinLengthData = new Dictionary<int, int>();

        internal bool CanTrimColumn = false;
        internal string TableTitle = string.Empty;

        private ConsoleTableBuilder()
        {
            Column = new List<object>();
            Rows = new List<List<object>>();
            TableFormat = ConsoleTableBuilderFormat.Default;
        }

        public static ConsoleTableBuilder From(DataTable dt)
        {
            var builder = new ConsoleTableBuilder();

            if (dt == null)
            {
                return builder;
            }

            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(x => x.ColumnName)
                .ToList();
#if NET35
            columnNames.ForEach(f => builder.Column.Add(f));
#else

            builder.Column = new List<object>(columnNames);
#endif
            foreach (DataRow row in dt.Rows)
            {
                builder.Rows.Add(new List<object>(row.ItemArray));
            }

            return builder;
        }

        public static ConsoleTableBuilder From<T>(List<T> list)
        {
            var builder = new ConsoleTableBuilder();
            if (list == null)
            {
                return builder;
            }

            var isClass = typeof(T).IsClass;
            var props = new List<System.Reflection.PropertyInfo>();

            if (list.Any())
            {
                props = list.First().GetType().GetProperties().ToList();
            }

            List<object> columnNames;
            if (isClass)
            {
                columnNames = props.Select(p =>
                {
                    object[] attrs = p.GetCustomAttributes(true);
                    foreach (object attr in attrs)
                    {
                        if (attr is System.ComponentModel.DescriptionAttribute)
                        {
                            return ((System.ComponentModel.DescriptionAttribute)attr).Description;
                        }
                    }

                    return p.Name as object;
                }).ToList() ?? new List<object>();
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
#if NET35
                        var objValue = prop.GetValue(item, new object[]{ });
#else
                        var objValue = prop.GetValue(item);
#endif
                        itemPropValues.Add(objValue);
                    }

                    builder.Rows.Add(itemPropValues);
                }
                else
                {
                    builder.Rows.Add(new List<object> { item });
                }
            }

            return builder;
        }

        public static ConsoleTableBuilder From(List<object[]> rows)
        {
            var builder = new ConsoleTableBuilder();

            if (rows == null)
            {
                return builder;
            }            

            foreach (var row in rows)
            {
                builder.Rows.Add(new List<object>(row));
            }

            return builder;
        }

        public static ConsoleTableBuilder From(List<List<object>> rows)
        {
            var builder = new ConsoleTableBuilder();

            if (rows == null)
            {
                return builder;
            }

            foreach (var row in rows)
            {
                builder.Rows.Add(row);
            }

            return builder;
        }

        internal List<int> GetCadidateColumnLengths()
        {
            var columnLengths = new List<int>();

            var numberOfColumns = 0;
            if (Rows.Any())
            {
                numberOfColumns = Rows.Max(x => x.Count);
            }
            else
            {
                if (Column != null)
                {
                    numberOfColumns = Column.Count;
                }                
            }

            if (numberOfColumns == 0)
            {
                return new List<int>();
            }

            if (numberOfColumns < Column.Count)
            {
                numberOfColumns = Column.Count;
            }

            for (var i = 0; i < numberOfColumns; i++)
            {
                var maxRow = 0;
                if (Rows.Any())
                {
                    maxRow = Rows
                        .Where(x => i < x.Count)
                        .Select(x => x[i]) // list cells of column i
                        .Max(x => x == null ? 0 : x.ToString().Length);
                }

                if (Column.ToArray().Length > i && (Column[i] ?? string.Empty).ToString().Length > maxRow)
                {
                    maxRow = Column[i].ToString().Length;
                }

                if (MinLengthData != null && MinLengthData.ContainsKey(i))
                {
                    columnLengths.Add(maxRow > MinLengthData[i] ? maxRow : MinLengthData[i]);
                }
                else
                {
                    columnLengths.Add(maxRow);
                }                
            }

            //if (!columnLengths.Any())
            //{
            //    throw new Exception("Table has no columns");
            //}

            if (this.CanTrimColumn)
            {
                if (columnLengths.Any())
                {
                    var temp = columnLengths;

                    //for (int i = 0; i < temp.Count; i++)
                    //{
                    //    if (temp[i] == 0)
                    //    {
                    //        columnLengths.RemoveAt(0);
                    //    }
                    //    else
                    //    {
                    //        break;
                    //    }
                    //}

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

        public long ColumnLength { get { return this.GetCadidateColumnLengths().Count; } }
        public long RowLength { get { return this.Rows.Count; } }

        internal string Format(char delimiter)
        {
            string delim = delimiter == '\0' ? string.Empty : delimiter.ToString();

            var columnLengths = GetCadidateColumnLengths();

            if (columnLengths.Count > 0)
            {
                return (Enumerable.Range(0, columnLengths.Count)
                            .Select(i => " " + delim + " {" + i + "," + (TextAligmentData == null ? "-" : (TextAligmentData.ContainsKey(i) ? TextAligmentData[i].ToString() : "-")) + columnLengths[i] + "}")
                            .Aggregate((s, a) => s + a) + " " + delim).Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        internal string FormatWithoutContent(char delimiter)
        {
            string delim = delimiter == '\0' ? string.Empty : delimiter.ToString();

            var columnLengths = GetCadidateColumnLengths();

            if (columnLengths.Count > 0)
            {
                return (Enumerable.Range(0, columnLengths.Count)
                            .Select(i => " " + delim + " " + new string(' ', columnLengths[i]))
                            .Aggregate((s, a) => s + a) + " " + delim).Trim();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
