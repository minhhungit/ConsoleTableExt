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

        public static ConsoleTableBuilder WithCharMapDefination(this ConsoleTableBuilder builder, Dictionary<CharMapPositions, char> charMapPositions)
        {
            builder.CharMapPositions = charMapPositions;
            return builder;
        }

        public static ConsoleTableBuilder WithCharMapDefination(this ConsoleTableBuilder builder, Dictionary<CharMapPositions, char> charMapPositions, Dictionary<HeaderCharMapPositions, char> headerCharMapPositions = null)
        {
            builder.CharMapPositions = charMapPositions;
            builder.HeaderCharMapPositions = headerCharMapPositions;
            return builder;
        }

        public static ConsoleTableBuilder WithOptions(this ConsoleTableBuilder builder, ConsoleTableBuilderOption options)
        {
            builder.Options = options;
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
                return CreateTableForTestNeFormat(builder, builder.CharMapPositions, builder.HeaderCharMapPositions);
            }
            else
            {
                switch (builder.TableFormat)
                {
                    case ConsoleTableBuilderFormat.Default:
                        return CreateTableForDefaultFormat(builder);
                    case ConsoleTableBuilderFormat.Minimal:
                        builder.Options.DelimiterChar = '\0';
                        return CreateTableForMarkdownFormat(builder);
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

            if (alignment != TableAligntment.Left)
            {
                var lines = strBuilder.ToString().Split('\n');

                var linesCount = lines.Count();
                for (int i = 0; i < linesCount; i++)
                {
                    switch (alignment)
                    {
                        case TableAligntment.Center:
                            if (i == linesCount - 1)
                            {
                                Console.Write(String.Format("{0," + ((Console.WindowWidth / 2) + (lines[i].Length / 2)) + "}", lines[i]));
                            }
                            else
                            {
                                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (lines[i].Length / 2)) + "}", lines[i]));
                            }
                            break;
                        case TableAligntment.Right:
                            if (i == linesCount - 1)
                            {
                                Console.Write(String.Format("{0," + Console.WindowWidth + "}", new string(' ', Console.WindowWidth - lines[i].Length) + lines[i]));
                            }
                            else
                            {
                                Console.WriteLine(String.Format("{0," + Console.WindowWidth + "}", new string(' ', Console.WindowWidth - lines[i].Length) + lines[i]));
                            }
                            break;
                    }
                }
            }
            else
            {
                Console.Write(strBuilder);
            }
        }

        public static void ExportAndWriteLine(this ConsoleTableBuilder builder, TableAligntment alignment = TableAligntment.Left)
        {
            var strBuilder = builder.Export();

            if (alignment != TableAligntment.Left)
            {
                var lines = strBuilder.ToString().Split('\n');

                var linesCount = lines.Count();
                for (int i = 0; i < linesCount; i++)
                {
                    switch (alignment)
                    {
                        case TableAligntment.Center:
                            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (lines[i].Length / 2)) + "}", lines[i]));
                            break;
                        case TableAligntment.Right:
                            Console.WriteLine(String.Format("{0," + Console.WindowWidth + "}", new string(' ', Console.WindowWidth - lines[i].Length) + lines[i]));
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine(strBuilder);
            }
        }

        private static StringBuilder CreateTableForDefaultFormat(ConsoleTableBuilder builder)
        {
            var strBuilder = new StringBuilder();
            if (builder.Options.MetaRowPosition == ConsoleTableBuilderOption.MetaRowPositions.Top)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }

            // create the string format with padding
            var format = builder.Format(builder.Options.DelimiterChar);

            if (format == string.Empty)
            {
                return strBuilder;
            }

            // find the longest formatted line
            var maxRowLength = Math.Max(0, builder.Rows.Any() ? builder.Rows.Max(row => string.Format(format, row.ToArray()).Length) : 0);

            // add each row
            var results = builder.Rows.Select(row => string.Format(format, row.ToArray())).ToList();

            // create the divider
            var divider = new string(builder.Options.DividerChar, maxRowLength);

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

            if (builder.Options.MetaRowPosition == ConsoleTableBuilderOption.MetaRowPositions.Bottom)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }
            return strBuilder;
        }

        private static StringBuilder CreateTableForTestNeFormat(ConsoleTableBuilder builder, Dictionary<CharMapPositions, char> charMapDefination, Dictionary<HeaderCharMapPositions, char> headerCharMapDefination = null)
        {
            var filledMap = FillCharMap(charMapDefination);
            var filledHeaderMap = FillHeaderCharMap(headerCharMapDefination);

            var strBuilder = new StringBuilder();
            if (builder.Options.MetaRowPosition == ConsoleTableBuilderOption.MetaRowPositions.Top)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }

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
            beginTableFormat = filledMap[CharMapPositions.A1] + beginTableFormat.Substring(1);
            beginTableFormat = beginTableFormat.Substring(0, beginTableFormat.Length - 1) + filledMap[CharMapPositions.C1];
            beginTableFormat = beginTableFormat.Replace(' ', filledMap[CharMapPositions.BorderY]).Replace(delim, filledMap[CharMapPositions.B1]);

            var rowContentTableFormat = format;
            rowContentTableFormat = filledMap[CharMapPositions.BorderX] + rowContentTableFormat.Substring(1);
            rowContentTableFormat = rowContentTableFormat.Substring(0, rowContentTableFormat.Length - 1) + filledMap[CharMapPositions.BorderX];
            rowContentTableFormat = rowContentTableFormat.Replace(delim, filledMap[CharMapPositions.DividerX]);

            var dividerTableFormat = formatWithoutContent;
            dividerTableFormat = filledMap[CharMapPositions.A2] + dividerTableFormat.Substring(1);
            dividerTableFormat = dividerTableFormat.Substring(0, dividerTableFormat.Length - 1) + filledMap[CharMapPositions.C2];
            dividerTableFormat = dividerTableFormat.Replace(' ', filledMap[CharMapPositions.DividerY]).Replace(delim, filledMap[CharMapPositions.B2]);

            var endTableFormat = formatWithoutContent;
            endTableFormat = filledMap[CharMapPositions.A3] + endTableFormat.Substring(1);
            endTableFormat = endTableFormat.Substring(0, endTableFormat.Length - 1) + filledMap[CharMapPositions.C3];
            endTableFormat = endTableFormat.Replace(' ', filledMap[CharMapPositions.BorderY]).Replace(delim, filledMap[CharMapPositions.B3]);


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
                    beginHeaderFormat = filledHeaderMap[HeaderCharMapPositions.A1] + beginHeaderFormat.Substring(1);
                    beginHeaderFormat = beginHeaderFormat.Substring(0, beginHeaderFormat.Length - 1) + filledHeaderMap[HeaderCharMapPositions.C1];
                    beginHeaderFormat = beginHeaderFormat.Replace(' ', filledHeaderMap[HeaderCharMapPositions.BorderX]).Replace(delim, filledHeaderMap[HeaderCharMapPositions.B1]);

                    rowContentHeaderFormat = format;
                    rowContentHeaderFormat = filledHeaderMap[HeaderCharMapPositions.BorderY] + rowContentHeaderFormat.Substring(1);
                    rowContentHeaderFormat = rowContentHeaderFormat.Substring(0, rowContentHeaderFormat.Length - 1) + filledHeaderMap[HeaderCharMapPositions.BorderY];
                    rowContentHeaderFormat = rowContentHeaderFormat.Replace(delim, filledHeaderMap[HeaderCharMapPositions.Divider]);

                    endHeaderFormat = formatWithoutContent;
                    endHeaderFormat = filledHeaderMap[HeaderCharMapPositions.A2] + endHeaderFormat.Substring(1);
                    endHeaderFormat = endHeaderFormat.Substring(0, endHeaderFormat.Length - 1) + filledHeaderMap[HeaderCharMapPositions.C2];
                    endHeaderFormat = endHeaderFormat.Replace(' ', filledHeaderMap[HeaderCharMapPositions.BorderX]).Replace(delim, filledHeaderMap[HeaderCharMapPositions.B2]);
                }
            }

            // add each row
            var results = builder.Rows.Select(row => string.Format(rowContentTableFormat, row.ToArray())).ToList();

            // header
            if (hasHeader)
            {
                if (filledHeaderMap != null)
                {
                    strBuilder.AppendLine(beginHeaderFormat);
                    strBuilder.AppendLine(string.Format(rowContentHeaderFormat, builder.Column.ToArray()));
                }
                else
                {
                    strBuilder.AppendLine(beginTableFormat);
                    strBuilder.AppendLine(string.Format(rowContentTableFormat, builder.Column.ToArray()));
                }
            }
            //else
            //{
            //    strBuilder.AppendLine(beginTableFormat);
            //    strBuilder.AppendLine(string.Format(rowContentTableFormat, builder.Column.ToArray()));
            //}

            var isFirstRow = true;
            foreach (var row in results)
            {
                if (isFirstRow)
                {
                    if (hasHeader)
                    {
                        strBuilder.AppendLine(string.IsNullOrEmpty(endHeaderFormat) ? dividerTableFormat: endHeaderFormat);
                    }
                    else
                    {
                        strBuilder.AppendLine(beginTableFormat);                        
                    }                    

                    isFirstRow = false;
                }
                else
                {
                    strBuilder.AppendLine(dividerTableFormat);
                }

                strBuilder.AppendLine(row);
            }

            if (results.Any())
            {
                strBuilder.AppendLine(endTableFormat);
            }
            else
            {
                strBuilder.AppendLine(string.IsNullOrEmpty(endHeaderFormat) ? endTableFormat: endHeaderFormat);
            }

            if (builder.Options.MetaRowPosition == ConsoleTableBuilderOption.MetaRowPositions.Bottom)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }
            return strBuilder;
        }

        private static StringBuilder CreateTableForMarkdownFormat(ConsoleTableBuilder builder)
        {
            var strBuilder = new StringBuilder();
            if (builder.Options.MetaRowPosition == ConsoleTableBuilderOption.MetaRowPositions.Top)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }

            // create the string format with padding
            var format = builder.Format(builder.Options.DelimiterChar);

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
            var divider = Regex.Replace(columnHeaders, @"[^|]", builder.Options.DividerChar.ToString());

            strBuilder.AppendLine(columnHeaders);
            strBuilder.AppendLine(divider);

            // add each row
            var results = builder.Rows.Skip(skipFirstRow ? 1 : 0).Select(row => string.Format(format, row.ToArray())).ToList();
            results.ForEach(row => strBuilder.AppendLine(row));

            if (builder.Options.MetaRowPosition == ConsoleTableBuilderOption.MetaRowPositions.Bottom)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }

            return strBuilder;
        }

        private static StringBuilder CreateTableForAlternativeFormat(ConsoleTableBuilder builder)
        {
            var strBuilder = new StringBuilder();
            if (builder.Options.MetaRowPosition == ConsoleTableBuilderOption.MetaRowPositions.Top)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }

            // create the string format with padding
            var format = builder.Format(builder.Options.DelimiterChar);

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
            var divider = Regex.Replace(columnHeaders, @"[^|]", builder.Options.DividerChar.ToString());
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

            if (builder.Options.MetaRowPosition == ConsoleTableBuilderOption.MetaRowPositions.Bottom)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }
            return strBuilder;
        }

        private static string BuildMetaRowFormat(ConsoleTableBuilder builder)
        {
            var result = new StringBuilder().AppendFormat(builder.Options.MetaRowFormat, builder.Options.MetaRowParams).ToString();

            if (result.Contains(AppConstants.MetaRow.ROW_COUNT))
            {
                result = result.Replace(AppConstants.MetaRow.ROW_COUNT, builder.Rows.Count.ToString());
            }

            if (result.Contains(AppConstants.MetaRow.COLUMN_COUNT))
            {
                result = result.Replace(AppConstants.MetaRow.COLUMN_COUNT, builder.GetCadidateColumnLengths().Count.ToString());
            }

            return result;
        }

        private static Dictionary<CharMapPositions, char> FillCharMap(Dictionary<CharMapPositions, char> defination)
        {
            if (defination == null)
            {
                return new Dictionary<CharMapPositions, char>();
            }

            var filledMap = defination;

            foreach (CharMapPositions c in (CharMapPositions[])Enum.GetValues(typeof(CharMapPositions)))
            {
                if (!filledMap.ContainsKey(c))
                {
                    filledMap.Add(c, '\0');
                }
            }

            return filledMap;
        }

        private static Dictionary<HeaderCharMapPositions, char> FillHeaderCharMap(Dictionary<HeaderCharMapPositions, char> defination)
        {
            if (defination == null)
            {
                return null;
            }

            var filledMap = defination;

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
