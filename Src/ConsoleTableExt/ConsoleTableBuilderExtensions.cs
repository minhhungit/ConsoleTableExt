using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleTableExt
{
    public static class ConsoleTableBuilderExtensions
    {
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();


        public static ConsoleTableBuilder AddColumn(this ConsoleTableBuilder builder, string columnName)
        {
            builder.Column.Add(columnName);
            return builder;
        }

        public static ConsoleTableBuilder AddColumn(this ConsoleTableBuilder builder, List<string> columnNames)
        {
#if NET35
            columnNames.ForEach(f => builder.Column.Add(f));
#else
            builder.Column.AddRange(columnNames);
#endif
            return builder;
        }

        public static ConsoleTableBuilder AddColumn(this ConsoleTableBuilder builder, params string[] columnNames)
        {
            builder.Column.AddRange(new List<object>(columnNames));
            return builder;
        }

        public static ConsoleTableBuilder WithColumn(this ConsoleTableBuilder builder, List<string> columnNames)
        {
            builder.Column = new List<object>();
#if NET35
            columnNames.ForEach(f => builder.Column.Add(f));
#else
            builder.Column.AddRange(columnNames);
#endif
            return builder;
        }

        public static ConsoleTableBuilder WithColumn(this ConsoleTableBuilder builder, params string[] columnNames)
        {
            builder.Column = new List<object>();
            builder.Column.AddRange(new List<object>(columnNames));
            return builder;
        }

        public static ConsoleTableBuilder AddRow(this ConsoleTableBuilder builder, params object[] rowValues)
        {
            if (rowValues == null)
                return builder;

            builder.Rows.Add(new List<object>(rowValues));

            return builder;
        }

        public static ConsoleTableBuilder WithMetadataRow(this ConsoleTableBuilder builder, MetaRowPositions position, Func<ConsoleTableBuilder, string> contentGenerator)
        {
            switch (position)
            {
                case MetaRowPositions.Top:
                    if (builder.TopMetadataRows == null)
                    {
                        builder.TopMetadataRows = new List<KeyValuePair<MetaRowPositions, Func<ConsoleTableBuilder, string>>>();
                    }

                    builder.TopMetadataRows.Add(new KeyValuePair<MetaRowPositions, Func<ConsoleTableBuilder, string>>(position, contentGenerator));
                    break;
                case MetaRowPositions.Bottom:
                    if (builder.BottomMetadataRows == null)
                    {
                        builder.BottomMetadataRows = new List<KeyValuePair<MetaRowPositions, Func<ConsoleTableBuilder, string>>>();
                    }

                    builder.BottomMetadataRows.Add(new KeyValuePair<MetaRowPositions, Func<ConsoleTableBuilder, string>>(position, contentGenerator));
                    break;

                default:
                    break;
            }

            return builder;
        }

        /// <summary>
        /// Add title row on top of table
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static ConsoleTableBuilder WithTitle(this ConsoleTableBuilder builder, TableTitle tableTitle, TextAligntment titleAligntment = TextAligntment.Center)
        {
            builder.TableTitle = tableTitle;
            builder.TableTitleTextAlignment = titleAligntment;
            return builder;
        }

        public static ConsoleTableBuilder WithPaddingLeft(this ConsoleTableBuilder builder, string paddingLeft)
        {
            builder.PaddingLeft = paddingLeft ?? string.Empty;
            return builder;
        }

        public static ConsoleTableBuilder WithPaddingRight(this ConsoleTableBuilder builder, string paddingRight)
        {
            builder.PaddingRight = paddingRight ?? string.Empty;
            return builder;
        }

        public static ConsoleTableBuilder WithFormatter(this ConsoleTableBuilder builder, int columnIndex, Func<string, string> formatter)
        {
            if (!builder.FormatterStore.ContainsKey(columnIndex))
            {
                builder.FormatterStore.Add(columnIndex, formatter);
            }
            else
            {
                builder.FormatterStore[columnIndex] = formatter;
            }

            return builder;
        }

        public static ConsoleTableBuilder WithColumnFormatter(this ConsoleTableBuilder builder, int columnIndex, Func<string, string> formatter)
        {
            if (!builder.ColumnFormatterStore.ContainsKey(columnIndex))
            {
                builder.ColumnFormatterStore.Add(columnIndex, formatter);
            }
            else
            {
                builder.ColumnFormatterStore[columnIndex] = formatter;
            }

            return builder;
        }

        /// <summary>
        /// Text alignment definition
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="alignmentData"></param>
        /// <returns></returns>
        public static ConsoleTableBuilder WithTextAlignment(this ConsoleTableBuilder builder, Dictionary<int, TextAligntment> alignmentData)
        {
            if (alignmentData != null)
            {
                builder.TextAligmentData = alignmentData;
            }

            return builder;
        }

        public static ConsoleTableBuilder WithMinLength(this ConsoleTableBuilder builder, Dictionary<int, int> minLengthData)
        {
            if (minLengthData != null)
            {
                builder.MinLengthData = minLengthData;
            }

            return builder;
        }

        public static ConsoleTableBuilder TrimColumn(this ConsoleTableBuilder builder, bool canTrimColumn = true)
        {
            builder.CanTrimColumn = canTrimColumn;
            return builder;
        }

        public static ConsoleTableBuilder AddRow(this ConsoleTableBuilder builder, List<object> row)
        {
            if (row == null)
                return builder;

            builder.Rows.Add(row);
            return builder;
        }

        public static ConsoleTableBuilder AddRow(this ConsoleTableBuilder builder, List<List<object>> rows)
        {
            if (rows == null)
                return builder;

            builder.Rows.AddRange(rows);
            return builder;
        }

        public static ConsoleTableBuilder AddRow(this ConsoleTableBuilder builder, DataRow row)
        {
            if (row == null)
                return builder;

            builder.Rows.Add(new List<object>(row.ItemArray));
            return builder;
        }

        public static ConsoleTableBuilder WithFormat(this ConsoleTableBuilder builder, ConsoleTableBuilderFormat format)
        {
            // reset CharMapPositions
            builder.CharMapPositionStore = null;
            builder.TableFormat = format;

            switch (builder.TableFormat)
            {
                case ConsoleTableBuilderFormat.Default:
                    builder.CharMapPositionStore = new Dictionary<CharMapPositions, MapCharItem>
                    {
                        { CharMapPositions.TopLeft, new MapCharItem('-') },
                        { CharMapPositions.TopCenter, new MapCharItem('-') },
                        { CharMapPositions.TopRight, new MapCharItem('-') },
                        { CharMapPositions.MiddleLeft, new MapCharItem('-') },
                        { CharMapPositions.MiddleCenter, new MapCharItem('-') },
                        { CharMapPositions.MiddleRight, new MapCharItem('-') },
                        { CharMapPositions.BottomLeft, new MapCharItem('-') },
                        { CharMapPositions.BottomCenter, new MapCharItem('-') },
                        { CharMapPositions.BottomRight, new MapCharItem('-') },
                        { CharMapPositions.BorderTop, new MapCharItem('-') },
                        { CharMapPositions.BorderLeft, new MapCharItem('|') },
                        { CharMapPositions.BorderRight, new MapCharItem('|') },
                        { CharMapPositions.BorderBottom, new MapCharItem('-') },
                        { CharMapPositions.DividerX, new MapCharItem('-') },
                        { CharMapPositions.DividerY, new MapCharItem('|') },
                    };
                    break;
                case ConsoleTableBuilderFormat.MarkDown:
                    builder.CharMapPositionStore = new Dictionary<CharMapPositions, MapCharItem>
                    {
                        { CharMapPositions.DividerY, new MapCharItem('|') },
                        { CharMapPositions.BorderLeft, new MapCharItem('|') },
                        { CharMapPositions.BorderRight, new MapCharItem('|') },
                    };

                    builder.HeaderCharMapPositionStore = new Dictionary<HeaderCharMapPositions, MapCharItem>
                    {
                        { HeaderCharMapPositions.BorderBottom, new MapCharItem('-') },
                        { HeaderCharMapPositions.BottomLeft, new MapCharItem('|') },
                        { HeaderCharMapPositions.BottomCenter, new MapCharItem('|') },
                        { HeaderCharMapPositions.BottomRight, new MapCharItem('|') },
                        { HeaderCharMapPositions.BorderLeft, new MapCharItem('|') },
                        { HeaderCharMapPositions.BorderRight, new MapCharItem('|') },
                        { HeaderCharMapPositions.Divider, new MapCharItem('|') },
                    };
                    break;
                case ConsoleTableBuilderFormat.Alternative:
                    builder.CharMapPositionStore = new Dictionary<CharMapPositions, MapCharItem>
                    {
                        { CharMapPositions.TopLeft, new MapCharItem('+') },
                        { CharMapPositions.TopCenter, new MapCharItem('+') },
                        { CharMapPositions.TopRight, new MapCharItem('+') },
                        { CharMapPositions.MiddleLeft, new MapCharItem('+') },
                        { CharMapPositions.MiddleCenter, new MapCharItem('+') },
                        { CharMapPositions.MiddleRight, new MapCharItem('+') },
                        { CharMapPositions.BottomLeft, new MapCharItem('+') },
                        { CharMapPositions.BottomCenter, new MapCharItem('+') },
                        { CharMapPositions.BottomRight, new MapCharItem('+') },
                        { CharMapPositions.BorderTop, new MapCharItem('-') },
                        { CharMapPositions.BorderRight, new MapCharItem('|') },
                        { CharMapPositions.BorderBottom, new MapCharItem('-') },
                        { CharMapPositions.BorderLeft, new MapCharItem('|') },
                        { CharMapPositions.DividerX, new MapCharItem('-') },
                        { CharMapPositions.DividerY, new MapCharItem('|') },
                    };
                    break;
                case ConsoleTableBuilderFormat.Minimal:
                    builder.CharMapPositionStore = new Dictionary<CharMapPositions, MapCharItem> { };

                    builder.HeaderCharMapPositionStore = new Dictionary<HeaderCharMapPositions, MapCharItem>
                    {
                        { HeaderCharMapPositions.BorderBottom, new MapCharItem('-') }                        
                    };

                    builder.PaddingLeft = string.Empty;
                    builder.PaddingRight = " ";
                    break;
                default:
                    break;
            }

            return builder;
        }

        public static ConsoleTableBuilder WithCharMapDefinition(this ConsoleTableBuilder builder)
        {
            return builder.WithCharMapDefinition(new Dictionary<CharMapPositions, MapCharItem> { });
        }

        public static ConsoleTableBuilder WithCharMapDefinition(this ConsoleTableBuilder builder, Dictionary<CharMapPositions, MapCharItem> charMapPositions)
        {
            builder.CharMapPositionStore = charMapPositions;
            return builder;
        }

        public static ConsoleTableBuilder WithCharMapDefinition(this ConsoleTableBuilder builder, Dictionary<CharMapPositions, MapCharItem> charMapPositions, Dictionary<HeaderCharMapPositions, MapCharItem> headerCharMapPositions = null)
        {
            builder.CharMapPositionStore = charMapPositions;
            builder.HeaderCharMapPositionStore = headerCharMapPositions;
            return builder;
        }

        public static ConsoleTableBuilder WithHeaderCharMapDefinition(this ConsoleTableBuilder builder, Dictionary<HeaderCharMapPositions, MapCharItem> headerCharMapPositions = null)
        {
            builder.HeaderCharMapPositionStore = headerCharMapPositions;
            return builder;
        }

        public static StringBuilder Export(this ConsoleTableBuilder builder)
        {
            var numberOfColumns = 0;
            if (builder.Rows.Any())
            {
                numberOfColumns = builder.Rows.Max(x => x.Count);
            }
            else
            {
                if (builder.Column != null)
                {
                    numberOfColumns = builder.Column.Count();
                }                
            }

            if (numberOfColumns == 0)
            {
                return new StringBuilder();
            }

            if (builder.Column == null)
            {
                numberOfColumns = 0;
            }
            else
            {
                if (numberOfColumns < builder.Column.Count)
                {
                    numberOfColumns = builder.Column.Count;
                }
            }

            for (int i = 0; i < 1; i++)
            {
                if (builder.Column != null && builder.Column.Count < numberOfColumns)
                {
                    var missCount = numberOfColumns - builder.Column.Count;
                    for (int j = 0; j < missCount; j++)
                    {
                        builder.Column.Add(null);
                    }
                }
            }

            for (int i = 0; i < builder.Rows.Count; i++)
            {
                if (builder.Rows[i].Count < numberOfColumns)
                {
                    var missCount = numberOfColumns - builder.Rows[i].Count;
                    for (int j = 0; j < missCount; j++)
                    {
                        builder.Rows[i].Add(null);
                    }
                }
            }

            return CreateTableForCustomFormat(builder);
        }

        public static void ExportAndWrite(this ConsoleTableBuilder builder, TableAligntment alignment = TableAligntment.Left)
        {
            var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
            if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
            {
                Console.WriteLine("failed to get output console mode");
                Console.ReadKey();
                return;
            }

            outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
            if (!SetConsoleMode(iStdOut, outConsoleMode))
            {
                Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
                Console.ReadKey();
                return;
            }

            var strBuilder = builder.Export();
            var tableLength = builder.ColumnLengths.Sum();

            var lines = strBuilder.ToString().Split('\n');

            var linesCount = lines.Count();
            for (int i = 0; i < linesCount; i++)
            {
                var row = string.Empty;
                
                switch (alignment)
                {
                    case TableAligntment.Left:
                        row = lines[i];
                        break;
                    case TableAligntment.Center:
                        row = String.Format("{0," + ((Console.WindowWidth / 2) + (tableLength / 2)) + "}", lines[i]);
                        break;
                    case TableAligntment.Right:
                        row = String.Format("{0," + Console.WindowWidth + "}", new string(' ', Console.WindowWidth - tableLength) + lines[i]);
                        break;
                }

                if (i == linesCount - 2)
                {
                    if (row.EndsWith('\r'.ToString()))
                    {
                        Console.Write(row.Substring(0, row.Length - 1));
                    }
                    else
                    {
                        Console.Write(row);
                    }
                }
                else
                {
                    if (i == linesCount - 1) // is last line
                    {
                        Console.Write(row);
                    }
                    else
                    {
                        Console.WriteLine(row);
                    }
                }
            }
        }

        public static void ExportAndWriteLine(this ConsoleTableBuilder builder, TableAligntment alignment = TableAligntment.Left)
        {
            builder.ExportAndWrite(alignment);
            Console.Write('\n');
        }
                
        private static StringBuilder CreateTableForCustomFormat(ConsoleTableBuilder builder)
        {
            if (builder.CharMapPositionStore == null)
            {
                builder.WithFormat(ConsoleTableBuilderFormat.Default);
            }

            builder.PopulateFormattedColumnsRows();
            builder.BuildCadidateColumnLengths();

            builder.CenterRowContent();

            var filledMap = FillCharMap(builder.CharMapPositionStore);
            var filledHeaderMap = FillHeaderCharMap(builder.HeaderCharMapPositionStore);

            var strBuilder = new StringBuilder();
            var topMetadataStringBuilder = BuildMetaRowsFormat(builder, MetaRowPositions.Top);
            for (int i = 0; i < topMetadataStringBuilder.Count; i++)
            {
                strBuilder.AppendLine(topMetadataStringBuilder[i]);
            }            

            var tableTopLine = builder.CreateTableTopLine(filledMap);
            var tableRowContentFormat = builder.CreateTableContentLineFormat(filledMap);
            var tableMiddleLine = builder.CreateTableMiddleLine(filledMap);
            var tableBottomLine = builder.CreateTableBottomLine(filledMap);

            var headerTopLine = string.Empty;
            var headerRowContentFormat = string.Empty;
            var headerBottomLine = string.Empty;

            if (filledHeaderMap != null)
            {
                headerTopLine = builder.CreateHeaderTopLine(filledMap, filledHeaderMap);
                headerRowContentFormat = builder.CreateHeaderContentLineFormat(filledMap, filledHeaderMap);
                headerBottomLine = builder.CreateHeaderBottomLine(filledMap, filledHeaderMap);
            }

            // find the longest formatted line
            //var maxRowLength = Math.Max(0, builder.Rows.Any() ? builder.Rows.Max(row => string.Format(tableRowContentFormat, row.ToArray()).Length) : 0);

            var hasHeader = builder.FormattedColumns != null && builder.FormattedColumns.Any() && builder.FormattedColumns.Max(x => (x ?? string.Empty).ToString().Length) > 0 ;
            
            // header
            if (hasHeader)
            {
                if (headerTopLine != null && headerTopLine.Trim().Length > 0)
                {
                    strBuilder.AppendLine(headerTopLine);
                }
                else
                {
                    if (tableTopLine != null && tableTopLine.Trim().Length > 0)
                    {
                        strBuilder.AppendLine(tableTopLine);
                    }                    
                }

                var headerSlices = builder.FormattedColumns.ToArray();
                var formattedHeaderSlice = Enumerable.Range(0, headerSlices.Length).Select(idx => builder.ColumnFormatterStore.ContainsKey(idx) ? builder.ColumnFormatterStore[idx](headerSlices[idx] == null ? string.Empty : headerSlices[idx].ToString()) : headerSlices[idx] == null ? string.Empty : headerSlices[idx].ToString()).ToArray();

                formattedHeaderSlice = builder.CenterColumnContent(formattedHeaderSlice);

                if (headerRowContentFormat != null && headerRowContentFormat.Trim().Length > 0)
                {
                    strBuilder.AppendLine(string.Format(headerRowContentFormat, formattedHeaderSlice));
                }
                else
                {
                    strBuilder.AppendLine(string.Format(tableRowContentFormat, formattedHeaderSlice));
                }
            }
            //else
            //{
            //    if (beginTableFormat.Length > 0) strBuilder.AppendLine(beginTableFormat);
            //    strBuilder.AppendLine(string.Format(rowContentTableFormat, builder.FormattedColumns.ToArray()));
            //}

            // add each row

            //var results = builder.Rows.Select(row => {
            //    var rowSlices = row.ToArray();
            //    return string.Format(tableRowContentFormat, Enumerable.Range(0, rowSlices.Length).Select(idx => builder.FormatterStore.ContainsKey(idx) ? builder.FormatterStore[idx](rowSlices[idx] == null ? string.Empty : rowSlices[idx].ToString()) : rowSlices[idx] == null ? string.Empty : rowSlices[idx].ToString()).ToArray());
            //}).ToList();

            var results = builder.FormattedRows.Select(row => string.Format(tableRowContentFormat, row.ToArray())).ToList();

            var isFirstRow = true;
            foreach (var row in results)
            {
                if (isFirstRow)
                {
                    if (hasHeader)
                    {
                        if ((string.IsNullOrEmpty(headerBottomLine) || headerBottomLine.Length == 0) && tableMiddleLine.Length > 0)
                        {
                            strBuilder.AppendLine(tableMiddleLine);
                        }
                        else
                        {
                            if (headerBottomLine.Length > 0)
                            {
                                strBuilder.AppendLine(headerBottomLine);
                            }
                        }
                    }
                    else
                    {
                        if (tableTopLine.Length > 0)
                        {
                            strBuilder.AppendLine(tableTopLine);
                        }
                    }

                    isFirstRow = false;
                }
                else
                {
                    if (tableMiddleLine.Length > 0)
                    {
                        strBuilder.AppendLine(tableMiddleLine);
                    }
                }

                strBuilder.AppendLine(row);
            }

            if (results.Any())
            {
                if (tableBottomLine.Length > 0)
                {
                    strBuilder.AppendLine(tableBottomLine);
                }
            }
            else
            {
                if ((string.IsNullOrEmpty(headerBottomLine) || headerBottomLine.Length == 0) && tableBottomLine.Length > 0)
                {
                    strBuilder.AppendLine(tableBottomLine);
                }
                else
                {
                    if (headerBottomLine.Length > 0)
                    {
                        strBuilder.AppendLine(headerBottomLine);
                    }
                }
            }

            var bottomMetadataStringBuilder = BuildMetaRowsFormat(builder, MetaRowPositions.Bottom);
            for (int i = 0; i < bottomMetadataStringBuilder.Count; i++)
            {
                strBuilder.AppendLine(bottomMetadataStringBuilder[i]);
            }

            return strBuilder;
        }

        //private static StringBuilder CreateTableForDefaultFormat(ConsoleTableBuilder builder)
        //{
        //    var strBuilder = new StringBuilder();
        //    BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Top);

        //    // create the string format with padding
        //    var format = builder.Format('|');

        //    if (format == string.Empty)
        //    {
        //        return strBuilder;
        //    }

        //    // find the longest formatted line
        //    var maxRowLength = Math.Max(0, builder.Rows.Any() ? builder.Rows.Max(row => string.Format(format, row.ToArray()).Length) : 0);

        //    // add each row
        //    var results = builder.Rows.Select(row => string.Format(format, row.ToArray())).ToList();

        //    // create the divider
        //    var divider = new string('-', maxRowLength);

        //    // header
        //    if (builder.Column != null && builder.Column.Any() && builder.Column.Max(x => (x ?? string.Empty).ToString().Length) > 0)
        //    {
        //        strBuilder.AppendLine(divider);
        //        strBuilder.AppendLine(string.Format(format, builder.Column.ToArray()));
        //    }

        //    foreach (var row in results)
        //    {
        //        strBuilder.AppendLine(divider);
        //        strBuilder.AppendLine(row);
        //    }

        //    strBuilder.AppendLine(divider);

        //    BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Bottom);
        //    return strBuilder;
        //}

        //private static StringBuilder CreateTableForMinimalFormat(ConsoleTableBuilder builder)
        //{
        //    var strBuilder = new StringBuilder();
        //    BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Top);

        //    // create the string format with padding
        //    var format = builder.Format('\0').Trim();

        //    if (format == string.Empty)
        //    {
        //        return strBuilder;
        //    }

        //    var skipFirstRow = false;
        //    var columnHeaders = string.Empty;

        //    if (builder.Column != null && builder.Column.Any() && builder.Column.Max(x => (x ?? string.Empty).ToString().Length) > 0)
        //    {
        //        skipFirstRow = false;
        //        columnHeaders = string.Format(format, builder.Column.ToArray());
        //    }
        //    else
        //    {
        //        skipFirstRow = true;
        //        columnHeaders = string.Format(format, builder.Rows.First().ToArray());
        //    }

        //    // create the divider
        //    var divider = Regex.Replace(columnHeaders, @"[^|]", '-'.ToString());

        //    strBuilder.AppendLine(columnHeaders);
        //    strBuilder.AppendLine(divider);

        //    // add each row
        //    var results = builder.Rows.Skip(skipFirstRow ? 1 : 0).Select(row => string.Format(format, row.ToArray())).ToList();
        //    results.ForEach(row => strBuilder.AppendLine(row));

        //    BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Bottom);

        //    return strBuilder;
        //}

        //private static StringBuilder CreateTableForMarkdownFormat(ConsoleTableBuilder builder)
        //{
        //    var strBuilder = new StringBuilder();
        //    BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Top);

        //    // create the string format with padding
        //    var format = builder.Format('|');

        //    if (format == string.Empty)
        //    {
        //        return strBuilder;
        //    }

        //    var skipFirstRow = false;
        //    var columnHeaders = string.Empty;

        //    if (builder.Column != null && builder.Column.Any() && builder.Column.Max(x => (x ?? string.Empty).ToString().Length) > 0)
        //    {
        //        skipFirstRow = false;
        //        columnHeaders = string.Format(format, builder.Column.ToArray());
        //    }
        //    else
        //    {
        //        skipFirstRow = true;
        //        columnHeaders = string.Format(format, builder.Rows.First().ToArray());
        //    }

        //    // create the divider
        //    var divider = Regex.Replace(columnHeaders, @"[^|]", '-'.ToString());

        //    strBuilder.AppendLine(columnHeaders);
        //    strBuilder.AppendLine(divider);

        //    // add each row
        //    var results = builder.Rows.Skip(skipFirstRow ? 1 : 0).Select(row => string.Format(format, row.ToArray())).ToList();
        //    results.ForEach(row => strBuilder.AppendLine(row));

        //    BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Bottom);

        //    return strBuilder;
        //}

        //private static StringBuilder CreateTableForAlternativeFormat(ConsoleTableBuilder builder)
        //{
        //    var strBuilder = new StringBuilder();
        //    BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Top);

        //    // create the string format with padding
        //    var format = builder.Format('|');

        //    if (format == string.Empty)
        //    {
        //        return strBuilder;
        //    }

        //    var skipFirstRow = false;
        //    var columnHeaders = string.Empty;

        //    if (builder.Column != null && builder.Column.Any() && builder.Column.Max(x => (x ?? string.Empty).ToString().Length) > 0)
        //    {
        //        skipFirstRow = false;
        //        columnHeaders = string.Format(format, builder.Column.ToArray());
        //    }
        //    else
        //    {
        //        skipFirstRow = true;
        //        columnHeaders = string.Format(format, builder.Rows.First().ToArray());
        //    }

        //    // create the divider
        //    var divider = Regex.Replace(columnHeaders, @"[^|]", '-'.ToString());
        //    var dividerPlus = divider.Replace("|", "+");

        //    strBuilder.AppendLine(dividerPlus);
        //    strBuilder.AppendLine(columnHeaders);

        //    // add each row
        //    var results = builder.Rows.Skip(skipFirstRow ? 1 : 0).Select(row => string.Format(format, row.ToArray())).ToList();

        //    foreach (var row in results)
        //    {
        //        strBuilder.AppendLine(dividerPlus);
        //        strBuilder.AppendLine(row);
        //    }
        //    strBuilder.AppendLine(dividerPlus);

        //    BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Bottom);
        //    return strBuilder;
        //}

        private static List<string> BuildMetaRowsFormat(ConsoleTableBuilder builder, MetaRowPositions position)
        {
            var result = new List<string>();
            switch (position)
            {
                case MetaRowPositions.Top:
                    if (builder.TopMetadataRows.Any())
                    {
                        foreach (var item in builder.TopMetadataRows)
                        {
                            if (item.Value != null)
                            {
                                result.Add(item.Value.Invoke(builder));
                            }
                        }
                    }
                    break;
                case MetaRowPositions.Bottom:
                    if (builder.BottomMetadataRows.Any())
                    {
                        foreach (var item in builder.BottomMetadataRows)
                        {
                            if (item.Value != null)
                            {
                                result.Add(item.Value.Invoke(builder));
                            }                                
                        }
                    }
                    break;
                default:
                    break;
            }

            return result;
        }

        private static Dictionary<CharMapPositions, MapCharItem> FillCharMap(Dictionary<CharMapPositions, MapCharItem> definition)
        {
            if (definition == null)
            {
                return new Dictionary<CharMapPositions, MapCharItem>();
            }

            var filledMap = definition;

            foreach (CharMapPositions c in (CharMapPositions[])Enum.GetValues(typeof(CharMapPositions)))
            {
                if (!filledMap.ContainsKey(c))
                {
                    filledMap.Add(c, new MapCharItem('\0'));
                }
            }

            return filledMap;
        }

        private static Dictionary<HeaderCharMapPositions, MapCharItem> FillHeaderCharMap(Dictionary<HeaderCharMapPositions, MapCharItem> definition)
        {
            if (definition == null)
            {
                return null;
            }

            var filledMap = definition;

            foreach (HeaderCharMapPositions c in (HeaderCharMapPositions[])Enum.GetValues(typeof(HeaderCharMapPositions)))
            {
                if (!filledMap.ContainsKey(c))
                {
                    filledMap.Add(c, new MapCharItem('\0'));
                }
            }

            return filledMap;
        }

    }
}
