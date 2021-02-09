using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleTableExt
{
    public static class ConsoleTableBuilderExtensions
    {
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
                        builder.TopMetadataRows = new List<KeyValuePair<MetaRowPositions, string>>();
                    }

                    builder.TopMetadataRows.Add(new KeyValuePair<MetaRowPositions, string>(position, contentGenerator.Invoke(builder)));
                    break;
                case MetaRowPositions.Bottom:
                    if (builder.BottomMetadataRows == null)
                    {
                        builder.BottomMetadataRows = new List<KeyValuePair<MetaRowPositions, string>>();
                    }

                    builder.BottomMetadataRows.Add(new KeyValuePair<MetaRowPositions, string>(position, contentGenerator.Invoke(builder)));
                    break;

                default:
                    break;
            }

            return builder;
        }

        /// <summary>
        /// Add title row on top of table (just available for Custom Format - using WithCharMapDefinition method)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static ConsoleTableBuilder WithTitle(this ConsoleTableBuilder builder, string title)
        {
            builder.TableTitle = title;
            return builder;
        }

        /// <summary>
        /// Add title row on top of table (just available for Custom Format - using WithCharMapDefinition method)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="title"></param>
        /// <param name="foregroundColor">text color</param>
        /// <returns></returns>
        public static ConsoleTableBuilder WithTitle(this ConsoleTableBuilder builder, string title, ConsoleColor foregroundColor)
        {
            builder.TableTitle = title;
            builder.TableTitleColor = new ConsoleColorNullable(foregroundColor);
            return builder;
        }

        /// <summary>
        /// Add title row on top of table (just available for Custom Format - using WithCharMapDefinition method)
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="title"></param>
        /// <param name="foregroundColor">text color</param>
        /// <param name="backgroundColor">background color</param>
        /// <returns></returns>
        public static ConsoleTableBuilder WithTitle(this ConsoleTableBuilder builder, string title, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            builder.TableTitle = title;
            builder.TableTitleColor = new ConsoleColorNullable(foregroundColor, backgroundColor);
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
                builder.TextAligmentData = new Dictionary<int, string>();

                foreach (var item in alignmentData)
                {
                    builder.TextAligmentData.Add(item.Key, item.Value == TextAligntment.Left ? "-" : string.Empty);
                }
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
            builder.TableFormat = format;
            return builder;
        }

        public static ConsoleTableBuilder WithCharMapDefinition(this ConsoleTableBuilder builder, Dictionary<CharMapPositions, char> charMapPositions)
        {
            builder.CharMapPositions = charMapPositions;
            return builder;
        }

        public static ConsoleTableBuilder WithCharMapDefinition(this ConsoleTableBuilder builder, Dictionary<CharMapPositions, char> charMapPositions, Dictionary<HeaderCharMapPositions, char> headerCharMapPositions = null)
        {
            builder.CharMapPositions = charMapPositions;
            builder.HeaderCharMapPositions = headerCharMapPositions;
            return builder;
        }

        public static ConsoleTableBuilder WithHeaderCharMapDefinition(this ConsoleTableBuilder builder, Dictionary<HeaderCharMapPositions, char> headerCharMapPositions = null)
        {
            builder.HeaderCharMapPositions = headerCharMapPositions;
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

            if (builder.CharMapPositions != null)
            {
                return CreateTableForCustomFormat(builder, builder.CharMapPositions, builder.HeaderCharMapPositions);
            }
            else
            {
                switch (builder.TableFormat)
                {
                    case ConsoleTableBuilderFormat.Default:
                        return CreateTableForDefaultFormat(builder);
                    case ConsoleTableBuilderFormat.Minimal:
                        return CreateTableForMinimalFormat(builder);
                    case ConsoleTableBuilderFormat.Alternative:
                        return CreateTableForAlternativeFormat(builder);
                    case ConsoleTableBuilderFormat.MarkDown:
                        return CreateTableForMarkdownFormat(builder);
                    default:
                        return CreateTableForDefaultFormat(builder);
                }
            }
           
        }

        public static void ExportAndWrite(this ConsoleTableBuilder builder, TableAligntment alignment = TableAligntment.Left)
        {
            var strBuilder = builder.Export();

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
                        row = String.Format("{0," + ((Console.WindowWidth / 2) + (lines[i].Length / 2)) + "}", lines[i]);
                        break;
                    case TableAligntment.Right:
                        row = String.Format("{0," + Console.WindowWidth + "}", new string(' ', Console.WindowWidth - lines[i].Length) + lines[i]);
                        break;
                }

                if (i == 0 && !string.IsNullOrEmpty(builder.TableTitle) && builder.TableTitle.Trim().Length != 0 && !builder.TableTitleColor.IsForegroundColorNull)
                {
                    var textRange = row.Split('\0');
                    if (textRange.Count() == 3)
                    {
                        Console.Write(textRange[0]);

                        Console.ForegroundColor = builder.TableTitleColor.ForegroundColor;
                        if (!builder.TableTitleColor.IsBackgroundColorNull)
                        {
                            Console.BackgroundColor = builder.TableTitleColor.BackgroundColor;
                        }

                        Console.Write(string.Format(" {0} ", textRange[1]));
                        Console.ResetColor();
                        Console.Write(textRange[2]);
                        Console.Write('\n');
                    }
                    else
                    {
                        if (i == linesCount - 1)
                        {
                            Console.Write(row);
                        }
                        else
                        {
                            Console.WriteLine(row);
                        }
                    }
                }
                else
                {
                    if (i == linesCount - 1)
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
            var strBuilder = builder.Export();

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
                        row = String.Format("{0," + ((Console.WindowWidth / 2) + (lines[i].Length / 2)) + "}", lines[i]);
                        break;
                    case TableAligntment.Right:
                        row = String.Format("{0," + Console.WindowWidth + "}", new string(' ', Console.WindowWidth - lines[i].Length) + lines[i]);
                        break;
                }

                if (i == 0 && !string.IsNullOrEmpty(builder.TableTitle) && builder.TableTitle.Trim().Length != 0 && !builder.TableTitleColor.IsForegroundColorNull)
                {
                    var textRange = row.Split('\0');
                    if (textRange.Count() == 3)
                    {
                        Console.Write(textRange[0]);

                        Console.ForegroundColor = builder.TableTitleColor.ForegroundColor;
                        if (!builder.TableTitleColor.IsBackgroundColorNull)
                        {
                            Console.BackgroundColor = builder.TableTitleColor.BackgroundColor;
                        }
                        
                        Console.Write(string.Format(" {0} ", textRange[1]));
                        Console.ResetColor();
                        Console.Write(textRange[2]);
                        Console.Write('\n');
                    }
                    else
                    {
                        Console.WriteLine(row);
                    }
                }
                else
                {
                    Console.WriteLine(row);
                }                
            }
        }


        private static StringBuilder CreateTableForDefaultFormat(ConsoleTableBuilder builder)
        {
            var strBuilder = new StringBuilder();
            BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Top);

            // create the string format with padding
            var format = builder.Format('|');

            if (format == string.Empty)
            {
                return strBuilder;
            }

            // find the longest formatted line
            var maxRowLength = Math.Max(0, builder.Rows.Any() ? builder.Rows.Max(row => string.Format(format, row.ToArray()).Length) : 0);

            // add each row
            var results = builder.Rows.Select(row => string.Format(format, row.ToArray())).ToList();

            // create the divider
            var divider = new string('-', maxRowLength);

            // header
            if (builder.Column != null && builder.Column.Any() && builder.Column.Max(x => (x ?? string.Empty).ToString().Length) > 0)
            {
                strBuilder.AppendLine(divider);
                strBuilder.AppendLine(string.Format(format, builder.Column.ToArray()));
            }

            foreach (var row in results)
            {
                strBuilder.AppendLine(divider);
                strBuilder.AppendLine(row);
            }

            strBuilder.AppendLine(divider);

            BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Bottom);
            return strBuilder;
        }

        private static StringBuilder CreateTableForCustomFormat(ConsoleTableBuilder builder, Dictionary<CharMapPositions, char> charMapDefinition, Dictionary<HeaderCharMapPositions, char> headerCharMapDefinition = null)
        {
            var filledMap = FillCharMap(charMapDefinition);
            var filledHeaderMap = FillHeaderCharMap(headerCharMapDefinition);

            var strBuilder = new StringBuilder();
            BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Top);

            // create the string format with padding
            char delim = 'x';

            //for example: | {0,-14} | {1,-29} | {2,-13} | {3,-3} | {4,-22} |
            string format = builder.Format(delim);

            if (format == string.Empty)
            {
                return strBuilder;
            }

            // find the longest formatted line
            var maxRowLength = Math.Max(0, builder.Rows.Any() ? builder.Rows.Max(row => string.Format(format, row.ToArray()).Length) : 0);

            string formatWithoutContent = builder.FormatWithoutContent(delim);

            var beginTableFormat = formatWithoutContent;
            beginTableFormat = filledMap[CharMapPositions.TopLeft] + beginTableFormat.Substring(1);
            beginTableFormat = beginTableFormat.Substring(0, beginTableFormat.Length - 1) + filledMap[CharMapPositions.TopRight];
            beginTableFormat = beginTableFormat.Replace(' ', filledMap[CharMapPositions.BorderX]).Replace(delim, filledMap[CharMapPositions.TopCenter]);

            if (builder.TableTitle.Length > beginTableFormat.Length)
            {
                if (beginTableFormat.Length < 10)
                {
                    builder.TableTitle = builder.TableTitle.Substring(0, beginTableFormat.Length - 4);
                }
                else
                {
                    if (beginTableFormat.Length < 20)
                    {
                        builder.TableTitle = builder.TableTitle.Substring(0, beginTableFormat.Length - 7);
                    }
                    else
                    {
                        builder.TableTitle = builder.TableTitle.Substring(0, beginTableFormat.Length - 7) + "...";
                    }
                }
            }

            if (!string.IsNullOrEmpty(builder.TableTitle) && builder.TableTitle.Trim().Length > 0) // !IsNullOrWhiteSpace
            {
                var newBeginTableFormat = beginTableFormat.Substring(0, (beginTableFormat.Length - builder.TableTitle.Length) / 2 - 1) + '\0';
                //newBeginTableFormat = string.Format("{0}\0{1}\0 ", newBeginTableFormat, builder.TableTitle);
                newBeginTableFormat += builder.TableTitle + '\0';
                newBeginTableFormat += beginTableFormat.Substring(newBeginTableFormat.Length, beginTableFormat.Length - newBeginTableFormat.Length);

                beginTableFormat = newBeginTableFormat;
            }

            if (beginTableFormat.Trim('\0').Length == 0)
            {
                beginTableFormat = string.Empty;
            }

            var rowContentTableFormat = format;
            rowContentTableFormat = filledMap[CharMapPositions.BorderY] + rowContentTableFormat.Substring(1);
            rowContentTableFormat = rowContentTableFormat.Substring(0, rowContentTableFormat.Length - 1) + filledMap[CharMapPositions.BorderY];
            rowContentTableFormat = rowContentTableFormat.Replace(delim, filledMap[CharMapPositions.DividerY]);

            var dividerTableFormat = formatWithoutContent;
            dividerTableFormat = filledMap[CharMapPositions.MiddleLeft] + dividerTableFormat.Substring(1);
            dividerTableFormat = dividerTableFormat.Substring(0, dividerTableFormat.Length - 1) + filledMap[CharMapPositions.MiddleRight];
            dividerTableFormat = dividerTableFormat.Replace(' ', filledMap[CharMapPositions.DividerX]).Replace(delim, filledMap[CharMapPositions.MiddleCenter]);

            if (dividerTableFormat.Trim('\0').Length == 0)
            {
                dividerTableFormat = string.Empty;
            }

            var endTableFormat = formatWithoutContent;
            endTableFormat = filledMap[CharMapPositions.BottomLeft] + endTableFormat.Substring(1);
            endTableFormat = endTableFormat.Substring(0, endTableFormat.Length - 1) + filledMap[CharMapPositions.BottomRight];
            endTableFormat = endTableFormat.Replace(' ', filledMap[CharMapPositions.BorderX]).Replace(delim, filledMap[CharMapPositions.BottomCenter]);

            if (endTableFormat.Trim('\0').Length == 0)
            {
                endTableFormat = string.Empty;
            }

            // header formats
            var beginHeaderFormat = string.Empty;
            var rowContentHeaderFormat = string.Empty;
            var endHeaderFormat = string.Empty;

            var hasHeader = builder.Column != null && builder.Column.Any() && builder.Column.Max(x => (x ?? string.Empty).ToString().Length) > 0 ;
            if (hasHeader)
            {
                if (filledHeaderMap != null)
                {
                    beginHeaderFormat = formatWithoutContent;
                    beginHeaderFormat = filledHeaderMap[HeaderCharMapPositions.TopLeft] + beginHeaderFormat.Substring(1);
                    beginHeaderFormat = beginHeaderFormat.Substring(0, beginHeaderFormat.Length - 1) + filledHeaderMap[HeaderCharMapPositions.TopRight];
                    beginHeaderFormat = beginHeaderFormat.Replace(' ', filledHeaderMap[HeaderCharMapPositions.BorderXTop]).Replace(delim, filledHeaderMap[HeaderCharMapPositions.TopCenter]);

                    if (builder.TableTitle.Length > beginHeaderFormat.Length)
                    {
                        if (beginHeaderFormat.Length < 10)
                        {
                            builder.TableTitle = builder.TableTitle.Substring(0, beginHeaderFormat.Length - 4);
                        }
                        else
                        {
                            if (beginHeaderFormat.Length < 20)
                            {
                                builder.TableTitle = builder.TableTitle.Substring(0, beginHeaderFormat.Length - 7);
                            }
                            else
                            {
                                builder.TableTitle = builder.TableTitle.Substring(0, beginHeaderFormat.Length - 7) + "...";
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(builder.TableTitle) && builder.TableTitle.Trim().Length > 0) // !IsNullOrWhiteSpace
                    {
                        var newBeginHeaderFormat = beginHeaderFormat.Substring(0, (beginHeaderFormat.Length - builder.TableTitle.Length) / 2 - 1) + ' ';
                        newBeginHeaderFormat += builder.TableTitle + ' ';
                        newBeginHeaderFormat += beginHeaderFormat.Substring(newBeginHeaderFormat.Length, beginHeaderFormat.Length - newBeginHeaderFormat.Length);

                        beginHeaderFormat = newBeginHeaderFormat;
                    }

                    if (beginHeaderFormat.Trim('\0').Length == 0)
                    {
                        beginHeaderFormat = string.Empty;
                    }

                    rowContentHeaderFormat = format;
                    rowContentHeaderFormat = filledHeaderMap[HeaderCharMapPositions.BorderY] + rowContentHeaderFormat.Substring(1);
                    rowContentHeaderFormat = rowContentHeaderFormat.Substring(0, rowContentHeaderFormat.Length - 1) + filledHeaderMap[HeaderCharMapPositions.BorderY];
                    rowContentHeaderFormat = rowContentHeaderFormat.Replace(delim, filledHeaderMap[HeaderCharMapPositions.Divider]);

                    endHeaderFormat = formatWithoutContent;
                    endHeaderFormat = filledHeaderMap[HeaderCharMapPositions.BottomLeft] + endHeaderFormat.Substring(1);
                    endHeaderFormat = endHeaderFormat.Substring(0, endHeaderFormat.Length - 1) + filledHeaderMap[HeaderCharMapPositions.BottomRight];
                    endHeaderFormat = endHeaderFormat.Replace(' ', filledHeaderMap[HeaderCharMapPositions.BorderXBottom]).Replace(delim, filledHeaderMap[HeaderCharMapPositions.BottomCenter]);

                    if (endHeaderFormat.Trim('\0').Length == 0)
                    {
                        endHeaderFormat = string.Empty;
                    }
                }
            }

            // add each row
            var results = builder.Rows.Select(row => string.Format(rowContentTableFormat, row.ToArray())).ToList();

            // header
            if (hasHeader)
            {
                if (filledHeaderMap != null)
                {
                    if (beginHeaderFormat.Length > 0)
                    {
                        strBuilder.AppendLine(beginHeaderFormat);
                    }
                    
                    strBuilder.AppendLine(string.Format(rowContentHeaderFormat, builder.Column.ToArray()));
                }
                else
                {
                    if (beginTableFormat.Length > 0)
                    {
                        strBuilder.AppendLine(beginTableFormat);
                    }
                    
                    strBuilder.AppendLine(string.Format(rowContentTableFormat, builder.Column.ToArray()));
                }
            }
            //else
            //{
            //    if (beginTableFormat.Length > 0) strBuilder.AppendLine(beginTableFormat);
            //    strBuilder.AppendLine(string.Format(rowContentTableFormat, builder.Column.ToArray()));
            //}

            var isFirstRow = true;
            foreach (var row in results)
            {
                if (isFirstRow)
                {
                    if (hasHeader)
                    {
                        if ((string.IsNullOrEmpty(endHeaderFormat) || endHeaderFormat.Length == 0) && dividerTableFormat.Length > 0)
                        {
                            strBuilder.AppendLine(dividerTableFormat);
                        }
                        else
                        {
                            if (endHeaderFormat.Length > 0)
                            {
                                strBuilder.AppendLine(endHeaderFormat);
                            }                            
                        }
                    }
                    else
                    {
                        if (beginTableFormat.Length > 0)
                        {
                            strBuilder.AppendLine(beginTableFormat);
                        }                        
                    }                    

                    isFirstRow = false;
                }
                else
                {
                    if (dividerTableFormat.Length > 0)
                    {
                        strBuilder.AppendLine(dividerTableFormat);
                    }                    
                }

                strBuilder.AppendLine(row);
            }

            if (results.Any())
            {
                if (endTableFormat.Length > 0)
                {
                    strBuilder.AppendLine(endTableFormat);
                }
            }
            else
            {
                if ((string.IsNullOrEmpty(endHeaderFormat) || endHeaderFormat.Length == 0) && endTableFormat.Length > 0)
                {
                    strBuilder.AppendLine(endTableFormat);
                }
                else
                {
                    if (endHeaderFormat.Length > 0)
                    {
                        strBuilder.AppendLine(endHeaderFormat);
                    }
                }
            }

            BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Bottom);
            return strBuilder;
        }

        private static StringBuilder CreateTableForMinimalFormat(ConsoleTableBuilder builder)
        {
            var strBuilder = new StringBuilder();
            BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Top);

            // create the string format with padding
            var format = builder.Format('\0');

            if (format == string.Empty)
            {
                return strBuilder;
            }

            var skipFirstRow = false;
            var columnHeaders = string.Empty;

            if (builder.Column != null && builder.Column.Any() && builder.Column.Max(x => (x ?? string.Empty).ToString().Length) > 0)
            {
                skipFirstRow = false;
                columnHeaders = string.Format(format, builder.Column.ToArray());
            }
            else
            {
                skipFirstRow = true;
                columnHeaders = string.Format(format, builder.Rows.First().ToArray());
            }

            // create the divider
            var divider = Regex.Replace(columnHeaders, @"[^|]", '-'.ToString());

            strBuilder.AppendLine(columnHeaders);
            strBuilder.AppendLine(divider);

            // add each row
            var results = builder.Rows.Skip(skipFirstRow ? 1 : 0).Select(row => string.Format(format, row.ToArray())).ToList();
            results.ForEach(row => strBuilder.AppendLine(row));

            BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Bottom);

            return strBuilder;
        }

        private static StringBuilder CreateTableForMarkdownFormat(ConsoleTableBuilder builder)
        {
            var strBuilder = new StringBuilder();
            BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Top);

            // create the string format with padding
            var format = builder.Format('|');

            if (format == string.Empty)
            {
                return strBuilder;
            }

            var skipFirstRow = false;
            var columnHeaders = string.Empty;

            if (builder.Column != null && builder.Column.Any() && builder.Column.Max(x => (x ?? string.Empty).ToString().Length) > 0)
            {
                skipFirstRow = false;
                columnHeaders = string.Format(format, builder.Column.ToArray());
            }
            else
            {
                skipFirstRow = true;
                columnHeaders = string.Format(format, builder.Rows.First().ToArray());
            }

            // create the divider
            var divider = Regex.Replace(columnHeaders, @"[^|]", '-'.ToString());

            strBuilder.AppendLine(columnHeaders);
            strBuilder.AppendLine(divider);

            // add each row
            var results = builder.Rows.Skip(skipFirstRow ? 1 : 0).Select(row => string.Format(format, row.ToArray())).ToList();
            results.ForEach(row => strBuilder.AppendLine(row));

            BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Bottom);

            return strBuilder;
        }

        private static StringBuilder CreateTableForAlternativeFormat(ConsoleTableBuilder builder)
        {
            var strBuilder = new StringBuilder();
            BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Top);

            // create the string format with padding
            var format = builder.Format('|');

            if (format == string.Empty)
            {
                return strBuilder;
            }

            var skipFirstRow = false;
            var columnHeaders = string.Empty;

            if (builder.Column != null && builder.Column.Any() && builder.Column.Max(x => (x ?? string.Empty).ToString().Length) > 0)
            {
                skipFirstRow = false;
                columnHeaders = string.Format(format, builder.Column.ToArray());
            }
            else
            {
                skipFirstRow = true;
                columnHeaders = string.Format(format, builder.Rows.First().ToArray());
            }

            // create the divider
            var divider = Regex.Replace(columnHeaders, @"[^|]", '-'.ToString());
            var dividerPlus = divider.Replace("|", "+");

            strBuilder.AppendLine(dividerPlus);
            strBuilder.AppendLine(columnHeaders);

            // add each row
            var results = builder.Rows.Skip(skipFirstRow ? 1 : 0).Select(row => string.Format(format, row.ToArray())).ToList();

            foreach (var row in results)
            {
                strBuilder.AppendLine(dividerPlus);
                strBuilder.AppendLine(row);
            }
            strBuilder.AppendLine(dividerPlus);

            BuildMetaRowsFormat(builder, strBuilder, MetaRowPositions.Bottom);
            return strBuilder;
        }

        private static StringBuilder BuildMetaRowsFormat(ConsoleTableBuilder builder, StringBuilder stringBuilder, MetaRowPositions position)
        {
            switch (position)
            {
                case MetaRowPositions.Top:
                    if (builder.TopMetadataRows.Any())
                    {
                        foreach (var item in builder.TopMetadataRows)
                        {
                            stringBuilder.AppendLine(item.Value);
                        }
                    }
                    break;
                case MetaRowPositions.Bottom:
                    if (builder.BottomMetadataRows.Any())
                    {
                        foreach (var item in builder.BottomMetadataRows)
                        {
                            stringBuilder.AppendLine(item.Value);
                        }
                    }
                    break;
                default:
                    break;
            }


            return stringBuilder;
        }

        private static Dictionary<CharMapPositions, char> FillCharMap(Dictionary<CharMapPositions, char> definition)
        {
            if (definition == null)
            {
                return new Dictionary<CharMapPositions, char>();
            }

            var filledMap = definition;

            foreach (CharMapPositions c in (CharMapPositions[])Enum.GetValues(typeof(CharMapPositions)))
            {
                if (!filledMap.ContainsKey(c))
                {
                    filledMap.Add(c, '\0');
                }
            }

            return filledMap;
        }

        private static Dictionary<HeaderCharMapPositions, char> FillHeaderCharMap(Dictionary<HeaderCharMapPositions, char> definition)
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
                    filledMap.Add(c, '\0');
                }
            }

            return filledMap;
        }

    }
}
