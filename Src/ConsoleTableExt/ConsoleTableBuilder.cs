using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConsoleTableExt
{
    public class TableTitle
    {
        public string Value { get; private set; }
        public string Start { get; private set; }
        public string End { get; private set; }

        public TableTitle()
        {

        }

        public TableTitle(string value)
        {
            Value = value;
        }

        public TableTitle(string value, string start, string end)
        {
            Value = value;
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", Start, Value, End);
        }
    }

    public class ConsoleTableBuilder
    {
        internal List<object> Column { get; set; }
        internal List<string> FormattedColumns { get; set; }

        internal List<List<object>> Rows { get; set; }
        internal List<List<object>> FormattedRows { get; set; }

        internal ConsoleTableBuilderFormat TableFormat { get; set; }
        internal Dictionary<CharMapPositions, MapCharItem> CharMapPositionStore { get; set; } = null;
        internal Dictionary<HeaderCharMapPositions, MapCharItem> HeaderCharMapPositionStore { get; set; } = null;
        internal List<KeyValuePair<MetaRowPositions, Func<ConsoleTableBuilder, string>>> TopMetadataRows = new List<KeyValuePair<MetaRowPositions, Func<ConsoleTableBuilder, string>>>();
        internal List<KeyValuePair<MetaRowPositions, Func<ConsoleTableBuilder, string>>> BottomMetadataRows = new List<KeyValuePair<MetaRowPositions, Func<ConsoleTableBuilder, string>>>();
        internal Dictionary<int, TextAligntment> TextAligmentData = new Dictionary<int, TextAligntment>();
        internal Dictionary<int, int> MinLengthData = new Dictionary<int, int>();

        internal bool CanTrimColumn = false;
        internal TableTitle TableTitle = new TableTitle();
        internal TextAligntment TableTitleTextAlignment = TextAligntment.Center;

        internal string PaddingLeft = " ";
        internal string PaddingRight = " ";

        internal Dictionary<int, Func<string, string>> FormatterStore = new Dictionary<int, Func<string, string>>();
        internal Dictionary<int, Func<string, string>> ColumnFormatterStore = new Dictionary<int, Func<string, string>>();

        public List<int> ColumnLengths { get; set; }

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

        internal void PopulateFormattedColumnsRows()
        {
            FormattedColumns = Enumerable.Range(0, Column.Count)
                .Select(idx =>
                {
                    if (ColumnFormatterStore.ContainsKey(idx))
                    {
                        return ColumnFormatterStore[idx](Column[idx] == null ? string.Empty : Column[idx].ToString());
                    }
                    else
                    {
                        return Column[idx] == null ? string.Empty : Column[idx].ToString();
                    }
                }).ToList();


            FormattedRows = new List<List<object>>();
            for (int i = 0; i < Rows.Count; i++)
            {
                FormattedRows.Add(
                    Enumerable.Range(0, Rows[i].Count)
                    .Select(idx => {
                        if (FormatterStore.ContainsKey(idx))
                        {
                            return FormatterStore[idx](Rows[i][idx] == null ? string.Empty : Rows[i][idx].ToString());
                        }
                        else
                        {
                            return Rows[i][idx];
                        }
                    }).ToList());
            }
        }

        internal void CenterRowContent()
        {
            for (int i = 0; i < FormattedRows.Count; i++)
            {
                for (int j = 0; j < FormattedRows[i].Count; j++)
                {
                    if (TextAligmentData.ContainsKey(j) && TextAligmentData[j] == TextAligntment.Center)
                    {
                        FormattedRows[i][j] = CenteredString(FormattedRows[i][j], ColumnLengths[j]);
                    }                    
                }
            }
        }

        internal string[] CenterColumnContent(string[] columnSlices)
        {
            for (int i = 0; i < columnSlices.Length; i++)
            {
                if (TextAligmentData.ContainsKey(i) && TextAligmentData[i] == TextAligntment.Center)
                {
                    columnSlices[i] = CenteredString(columnSlices[i], ColumnLengths[i]);
                }
            }

            return columnSlices;
        }

        private string CenteredString(object s, int width)
        {
            if (s == null)
            {
                return null;
            }

            if (s.ToString().Length >= width)
            {
                return s.ToString();
            }

            int leftPadding = (width - s.ToString().Length) / 2;
            int rightPadding = width - s.ToString().Length - leftPadding;

            return new string(' ', leftPadding) + s + new string(' ', rightPadding);
        }

        internal void BuildCadidateColumnLengths()
        {
            var columnLengths = new List<int>();

            var numberOfColumns = 0;
            if (FormattedRows.Any())
            {
                numberOfColumns = FormattedRows.Max(x => x.Count);
            }
            else
            {
                if (FormattedColumns != null)
                {
                    numberOfColumns = FormattedColumns.Count;
                }                
            }

            if (numberOfColumns == 0)
            {
                ColumnLengths = new List<int>();
            }

            if (numberOfColumns < FormattedColumns.Count)
            {
                numberOfColumns = FormattedColumns.Count;
            }

            for (var i = 0; i < numberOfColumns; i++)
            {
                var maxRow = 0;
                if (FormattedRows.Any())
                {
                    maxRow = FormattedRows
                        .Where(x => i < x.Count)
                        .Select(x => x[i]) // list cells of column i
                        .Max(x => x == null ? 0 : x.ToString().Length);
                }

                if (FormattedColumns.ToArray().Length > i && (FormattedColumns[i] ?? string.Empty).ToString().Length > maxRow)
                {
                    maxRow = FormattedColumns[i].ToString().Length;
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

            this.ColumnLengths = columnLengths;
        }

        public int NumberOfColumns { get { return ColumnLengths.Count; } }
        public int NumberOfRows { get { return this.Rows.Count; } }

        //internal string Format(char delimiter)
        //{
        //    string delim = delimiter == '\0' ? string.Empty : delimiter.ToString();

        //    var columnLengths = GetCadidateColumnLengths();

        //    // | {0,-14} | {1,-29} | {2,-13} | {3,-3} | {4,-22} |
        //    if (columnLengths.Count > 0)
        //    {
        //        var format = Enumerable.Range(0, columnLengths.Count)
        //                    .Select(i => PaddingLeft + "{" + i + "," + (TextAligmentData == null ? "-" : (TextAligmentData.ContainsKey(i) ? TextAligmentData[i].ToString() : "-")) + columnLengths[i] + "}" + PaddingRight)
        //                    .Aggregate((s, a) => s + delim + a);

        //        return delim + format + delim;
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        private string RepeatString(string input, int count)
        {
            if (!string.IsNullOrEmpty(input))
            {
                StringBuilder builder = new StringBuilder(input.Length * count);

                for (int i = 0; i < count; i++) builder.Append(input);

                return builder.ToString();
            }

            return string.Empty;
        }

        private string EmbedTitle(string line)
        {
            return line;
            //var originalTitleLength = TableTitle.Length;

            //if (!string.IsNullOrEmpty(TableTitle) && TableTitle.Trim().Length > 0) // !IsNullOrWhiteSpace
            //{
            //    if (TableTitle.Length > line.Length - 4)
            //    {
            //        TableTitle = TableTitle.Substring(0, line.Length - 4);

            //        if (originalTitleLength != TableTitle.Length && TableTitle.Length > 3)
            //        {
            //            TableTitle = TableTitle.Substring(0, TableTitle.Length - 3) + "...";
            //        }
            //    }

            //    TableTitle = TableTitle.Trim();
            //    TableTitle = " " + TableTitle + " ";

            //    var startPoint = 0;
            //    switch (TableTitleTextAlignment)
            //    {
            //        case TextAligntment.Left:
            //            startPoint = 1;
            //            break;
            //        case TextAligntment.Right:
            //            startPoint = line.Length - 1 - TableTitle.Length;
            //            break;
            //        case TextAligntment.Center:
            //            startPoint = (line.Length - TableTitle.Length) / 2;
            //            break;
            //        default:
            //            break;
            //    }

            //    TitlePositionStartAt = startPoint;
            //    var newBeginTableFormat = line.Substring(0, startPoint);
            //    newBeginTableFormat += TableTitle;
            //    TitlePositionLength = TableTitle.Length;
            //    newBeginTableFormat += line.Substring(newBeginTableFormat.Length, line.Length - newBeginTableFormat.Length);

            //    line = newBeginTableFormat;
            //    line = line.Replace("\0", " ");
            //}

            //return line;
        }
        #region Table lines

        internal string CreateTableTopLine(Dictionary<CharMapPositions, MapCharItem> definition)
        {
            var borderTop = definition[CharMapPositions.BorderTop];
            var topLeft = definition[CharMapPositions.TopLeft];
            var topCenter = definition[CharMapPositions.TopCenter];
            var topRight = definition[CharMapPositions.TopRight];


            if (ColumnLengths.Count > 0)
            {
                string line = string.Empty;
                
                if (TableTitle != null && TableTitle.ToString().Length > 0)
                {
                    // @TODO IMPLEMENT TITLE
                    var result = Enumerable.Range(0, ColumnLengths.Count)
                           .Select(i => RepeatString(borderTop.ToString(), ColumnLengths[i] + (PaddingLeft + PaddingRight).Length))
                           .Aggregate((s, a) => s + (CanRemoveDividerY() ? string.Empty : topCenter.ToString()) + a);

                    line = (CanRemoveBorderLeft() ? string.Empty : topLeft.ToString()) + result + (CanRemoveBorderRight() ? string.Empty : topRight.ToString());
                }
                else
                {
                    var result = Enumerable.Range(0, ColumnLengths.Count)
                            .Select(i => RepeatString(borderTop.ToString(), ColumnLengths[i] + (PaddingLeft + PaddingRight).Length))
                            .Aggregate((s, a) => s + (CanRemoveDividerY() ? string.Empty : topCenter.ToString()) + a);

                    line = (CanRemoveBorderLeft() ? string.Empty : topLeft.ToString()) + result + (CanRemoveBorderRight() ? string.Empty : topRight.ToString());
                }

                if (line.Trim('\0').Length == 0)
                {
                    line = string.Empty;
                }

                return line;
            }
            else
            {
                return string.Empty;
            }
        }

        internal string CreateTableContentLineFormat(Dictionary<CharMapPositions, MapCharItem> definition)
        {
            var borderLeft = definition[CharMapPositions.BorderLeft];
            var divider = definition[CharMapPositions.DividerY];
            var borderRight = definition[CharMapPositions.BorderRight];

            if (ColumnLengths.Count > 0)
            {
                var result = Enumerable.Range(0, ColumnLengths.Count)
                            .Select(i =>
                            {
                                var alignmentChar = string.Empty;
                                if (TextAligmentData == null || !TextAligmentData.ContainsKey(i) || TextAligmentData[i] == TextAligntment.Left)
                                {
                                    alignmentChar = "-";
                                }
                                return PaddingLeft + "{" + i + "," + alignmentChar + ColumnLengths[i] + "}" + PaddingRight;
                            })
                            .Aggregate((s, a) => s + (CanRemoveDividerY() ? string.Empty : divider.ToString()) + a);

                var line = (CanRemoveBorderLeft() ? string.Empty : borderLeft.ToString()) + result + (CanRemoveBorderRight() ? string.Empty : borderRight.ToString());

                return line;
            }
            else
            {
                return string.Empty;
            }
        }

        internal string CreateTableMiddleLine(Dictionary<CharMapPositions, MapCharItem> definition)
        {
            var dividerX = definition[CharMapPositions.DividerX];
            var middleLeft = definition[CharMapPositions.MiddleLeft];
            var middleCenter = definition[CharMapPositions.MiddleCenter];
            var middleRight = definition[CharMapPositions.MiddleRight];

            if (ColumnLengths.Count > 0)
            {
                var result = Enumerable.Range(0, ColumnLengths.Count)
                            .Select(i => RepeatString(dividerX.ToString(), ColumnLengths[i] + (PaddingLeft + PaddingRight).Length))
                            .Aggregate((s, a) => s + (CanRemoveDividerY() ? string.Empty : middleCenter.ToString()) + a);

                var line = (CanRemoveBorderLeft() ? string.Empty : middleLeft.ToString()) + result + (CanRemoveBorderRight() ? string.Empty : middleRight.ToString());

                if (line.Trim('\0').Length == 0)
                {
                    line = string.Empty;
                }

                return line;
            }
            else
            {
                return string.Empty;
            }
        }


        internal string CreateTableBottomLine(Dictionary<CharMapPositions, MapCharItem> definition)
        {
            var borderBottom = definition[CharMapPositions.BorderBottom];
            var bottomLeft = definition[CharMapPositions.BottomLeft];
            var bottomCenter = definition[CharMapPositions.BottomCenter];
            var bottomRight = definition[CharMapPositions.BottomRight];

            if (ColumnLengths.Count > 0)
            {
                var result = Enumerable.Range(0, ColumnLengths.Count)
                            .Select(i => RepeatString(borderBottom.ToString(), ColumnLengths[i] + (PaddingLeft + PaddingRight).Length))
                            .Aggregate((s, a) => s + (CanRemoveDividerY() ? string.Empty : bottomCenter.ToString()) + a);

                var line = (CanRemoveBorderLeft() ? string.Empty : bottomLeft.ToString()) + result + (CanRemoveBorderRight() ? string.Empty : bottomRight.ToString());

                if (line.Trim('\0').Length == 0)
                {
                    line = string.Empty;
                }

                return line;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion


        #region Header lines

        internal string CreateHeaderTopLine(Dictionary<CharMapPositions, MapCharItem> definition, Dictionary<HeaderCharMapPositions, MapCharItem> headerDefinition)
        {
            var borderTop = headerDefinition != null && headerDefinition.ContainsKey(HeaderCharMapPositions.BorderTop) ? headerDefinition[HeaderCharMapPositions.BorderTop] : definition[CharMapPositions.BorderTop];
            var topLeft = headerDefinition != null && headerDefinition.ContainsKey(HeaderCharMapPositions.TopLeft) ? headerDefinition[HeaderCharMapPositions.TopLeft] : definition[CharMapPositions.TopLeft];
            var topCenter = headerDefinition != null && headerDefinition.ContainsKey(HeaderCharMapPositions.TopCenter) ? headerDefinition[HeaderCharMapPositions.TopCenter] : definition[CharMapPositions.TopCenter];
            var topRight = headerDefinition != null && headerDefinition.ContainsKey(HeaderCharMapPositions.TopRight) ? headerDefinition[HeaderCharMapPositions.TopRight] : definition[CharMapPositions.TopRight];

            if (ColumnLengths.Count > 0)
            {
                var result = Enumerable.Range(0, ColumnLengths.Count)
                            .Select(i => RepeatString(borderTop.ToString(), ColumnLengths[i] + (PaddingLeft + PaddingRight).Length))
                            .Aggregate((s, a) => s + (CanRemoveDividerY() ? string.Empty : topCenter.ToString()) + a);

                var line = (CanRemoveBorderLeft() ? string.Empty : topLeft.ToString()) + result + (CanRemoveBorderRight() ? string.Empty : topRight.ToString());

                line = EmbedTitle(line);

                if (line.Trim('\0').Length == 0)
                {
                    line = string.Empty;
                }

                return line;
            }
            else
            {
                return string.Empty;
            }
        }

        internal string CreateHeaderContentLineFormat(Dictionary<CharMapPositions, MapCharItem> definition, Dictionary<HeaderCharMapPositions, MapCharItem> headerDefinition)
        {
            var borderLeft = headerDefinition != null && headerDefinition.ContainsKey(HeaderCharMapPositions.BorderLeft) ? headerDefinition[HeaderCharMapPositions.BorderLeft] : definition[CharMapPositions.BorderLeft];
            var divider = headerDefinition != null && headerDefinition.ContainsKey(HeaderCharMapPositions.Divider) ? headerDefinition[HeaderCharMapPositions.Divider] : definition[CharMapPositions.DividerY];
            var borderRight = headerDefinition != null && headerDefinition.ContainsKey(HeaderCharMapPositions.BorderRight) ? headerDefinition[HeaderCharMapPositions.BorderRight] : definition[CharMapPositions.BorderRight];

            if (ColumnLengths.Count > 0)
            {
                var result = Enumerable.Range(0, ColumnLengths.Count)
                            .Select(i => {
                                var alignmentChar = string.Empty;
                                if (TextAligmentData == null || !TextAligmentData.ContainsKey(i) || TextAligmentData[i] == TextAligntment.Left)
                                {
                                    alignmentChar = "-";
                                }

                                return PaddingLeft + "{" + i + "," + alignmentChar + ColumnLengths[i] + "}" + PaddingRight;
                            })
                            .Aggregate((s, a) => s + (CanRemoveDividerY() ? string.Empty : divider.ToString()) + a);

                var line = (CanRemoveBorderLeft() ? string.Empty : borderLeft.ToString()) + result + (CanRemoveBorderRight() ? string.Empty : borderRight.ToString());

                return line;
            }
            else
            {
                return string.Empty;
            }
        }

        internal string CreateHeaderBottomLine(Dictionary<CharMapPositions, MapCharItem> definition, Dictionary<HeaderCharMapPositions, MapCharItem> headerDefinition)
        {
            var borderBottom = headerDefinition != null && headerDefinition.ContainsKey(HeaderCharMapPositions.BorderBottom) ? headerDefinition[HeaderCharMapPositions.BorderBottom] : definition[CharMapPositions.DividerX];
            var bottomLeft = headerDefinition != null && headerDefinition.ContainsKey(HeaderCharMapPositions.BottomLeft) ? headerDefinition[HeaderCharMapPositions.BottomLeft] : definition[CharMapPositions.MiddleLeft];
            var bottomCenter = headerDefinition != null && headerDefinition.ContainsKey(HeaderCharMapPositions.BottomCenter) ? headerDefinition[HeaderCharMapPositions.BottomCenter] : definition[CharMapPositions.MiddleRight];
            var bottomRight = headerDefinition != null && headerDefinition.ContainsKey(HeaderCharMapPositions.BottomRight) ? headerDefinition[HeaderCharMapPositions.BottomRight] : definition[CharMapPositions.MiddleCenter];

            if (ColumnLengths.Count > 0)
            {
                var result = Enumerable.Range(0, ColumnLengths.Count)
                            .Select(i => RepeatString(borderBottom.ToString(), ColumnLengths[i] + (PaddingLeft + PaddingRight).Length))
                            .Aggregate((s, a) => s + (CanRemoveDividerY() ? string.Empty : bottomCenter.ToString()) + a);

                var line = (CanRemoveBorderLeft() ? string.Empty : bottomLeft.ToString()) + result + (CanRemoveBorderRight() ? string.Empty : bottomRight.ToString());

                if (line.Trim('\0').Length == 0)
                {
                    line = string.Empty;
                }

                return line;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        internal bool CanRemoveBorderLeft()
        {
            if (HeaderCharMapPositionStore == null)
            {
                return new List<MapCharItem> {
                    CharMapPositionStore[CharMapPositions.TopLeft],
                    CharMapPositionStore[CharMapPositions.MiddleLeft],
                    CharMapPositionStore[CharMapPositions.BottomLeft],
                    CharMapPositionStore[CharMapPositions.BorderLeft]
                }
                .Select(x => x.ToString())
                .Aggregate((s, a) => s + a)
                .Replace("\0", string.Empty)
                .Trim().Length == 0;
            }
            else
            {
                var data = new List<MapCharItem> { };
                data.Add(HeaderCharMapPositionStore.ContainsKey(HeaderCharMapPositions.TopLeft) ? 
                    HeaderCharMapPositionStore[HeaderCharMapPositions.TopLeft] : CharMapPositionStore[CharMapPositions.TopLeft]);

                data.Add(HeaderCharMapPositionStore.ContainsKey(HeaderCharMapPositions.BorderLeft) ?
                    HeaderCharMapPositionStore[HeaderCharMapPositions.BorderLeft] : CharMapPositionStore[CharMapPositions.BorderLeft]);

                data.Add(HeaderCharMapPositionStore.ContainsKey(HeaderCharMapPositions.BottomLeft) ?
                    HeaderCharMapPositionStore[HeaderCharMapPositions.BottomLeft] : CharMapPositionStore[CharMapPositions.MiddleLeft]);

                data.Add(CharMapPositionStore[CharMapPositions.MiddleLeft]);
                data.Add(CharMapPositionStore[CharMapPositions.BorderLeft]);
                data.Add(CharMapPositionStore[CharMapPositions.BottomLeft]);

                return 
                    data
                        .Select(x => x.ToString())
                        .Aggregate((s, a) => s + a)
                        .Replace("\0", string.Empty)
                        .Trim().Length == 0;
            }
        }

        internal bool CanRemoveBorderRight()
        {
            if (HeaderCharMapPositionStore == null)
            {
                return new List<MapCharItem> {
                    CharMapPositionStore[CharMapPositions.TopRight],
                    CharMapPositionStore[CharMapPositions.MiddleRight],
                    CharMapPositionStore[CharMapPositions.BottomRight],
                    CharMapPositionStore[CharMapPositions.BorderRight]
                }
                .Select(x => x.ToString())
                .Aggregate((s, a) => s + a)
                .Replace("\0", string.Empty)
                .Trim().Length == 0;
            }
            else
            {
                var data = new List<MapCharItem> { };
                data.Add(HeaderCharMapPositionStore.ContainsKey(HeaderCharMapPositions.TopRight) ?
                    HeaderCharMapPositionStore[HeaderCharMapPositions.TopRight] : CharMapPositionStore[CharMapPositions.TopRight]);

                data.Add(HeaderCharMapPositionStore.ContainsKey(HeaderCharMapPositions.BorderRight) ?
                    HeaderCharMapPositionStore[HeaderCharMapPositions.BorderRight] : CharMapPositionStore[CharMapPositions.BorderRight]);

                data.Add(HeaderCharMapPositionStore.ContainsKey(HeaderCharMapPositions.BottomRight) ?
                    HeaderCharMapPositionStore[HeaderCharMapPositions.BottomRight] : CharMapPositionStore[CharMapPositions.MiddleRight]);

                data.Add(CharMapPositionStore[CharMapPositions.MiddleRight]);
                data.Add(CharMapPositionStore[CharMapPositions.BorderRight]);
                data.Add(CharMapPositionStore[CharMapPositions.BottomRight]);

                return
                    data
                        .Select(x => x.ToString())
                        .Aggregate((s, a) => s + a)
                        .Replace("\0", string.Empty)
                        .Trim().Length == 0;
            }
        }

        internal bool CanRemoveDividerY()
        {
            if (HeaderCharMapPositionStore == null)
            {
                return new List<MapCharItem> {
                    CharMapPositionStore[CharMapPositions.TopCenter],
                    CharMapPositionStore[CharMapPositions.MiddleCenter],
                    CharMapPositionStore[CharMapPositions.BottomCenter],
                    CharMapPositionStore[CharMapPositions.DividerY]
                }
                .Select(x => x.ToString())
                .Aggregate((s, a) => s + a)
                .Replace("\0", string.Empty)
                .Trim().Length == 0;
            }
            else
            {
                var data = new List<MapCharItem> { };
                data.Add(HeaderCharMapPositionStore.ContainsKey(HeaderCharMapPositions.TopCenter) ?
                    HeaderCharMapPositionStore[HeaderCharMapPositions.TopCenter] : CharMapPositionStore[CharMapPositions.TopCenter]);

                data.Add(HeaderCharMapPositionStore.ContainsKey(HeaderCharMapPositions.Divider) ?
                    HeaderCharMapPositionStore[HeaderCharMapPositions.Divider] : CharMapPositionStore[CharMapPositions.DividerY]);

                data.Add(HeaderCharMapPositionStore.ContainsKey(HeaderCharMapPositions.BottomCenter) ?
                    HeaderCharMapPositionStore[HeaderCharMapPositions.BottomCenter] : CharMapPositionStore[CharMapPositions.MiddleCenter]);

                data.Add(CharMapPositionStore[CharMapPositions.MiddleCenter]);
                data.Add(CharMapPositionStore[CharMapPositions.DividerY]);
                data.Add(CharMapPositionStore[CharMapPositions.BottomCenter]);

                return
                    data
                        .Select(x => x.ToString())
                        .Aggregate((s, a) => s + a)
                        .Replace("\0", string.Empty)
                        .Trim().Length == 0;
            }
        }
    }
}
