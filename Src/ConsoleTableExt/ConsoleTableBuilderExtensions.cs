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
                throw new ArgumentNullException(nameof(rowValues));

            builder.Rows.Add(new List<object>(rowValues));

            return builder;
        }

        public static ConsoleTableBuilder AddRow(this ConsoleTableBuilder builder, List<object> row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            builder.Rows.Add(row);

            return builder;
        }

        public static ConsoleTableBuilder AddRow(this ConsoleTableBuilder builder, List<List<object>> rows)
        {
            if (rows == null)
                throw new ArgumentNullException(nameof(rows));

            builder.Rows.AddRange(rows);
            return builder;
        }

        public static ConsoleTableBuilder AddRow(this ConsoleTableBuilder builder, DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            builder.Rows.Add(new List<object>(row.ItemArray));
            return builder;
        }

        public static ConsoleTableBuilder WithFormat(this ConsoleTableBuilder builder, ConsoleTableBuilderFormat format)
        {
            builder.TableFormat = format;
            return builder;
        }

        public static ConsoleTableBuilder WithOptions(this ConsoleTableBuilder builder, ConsoleTableBuilderOption options)
        {
            builder.Options = options;
            return builder;
        }

        public static StringBuilder Export(this ConsoleTableBuilder builder)
        {
            if (!builder.Rows.Any())
            {
                throw new Exception("Table has no rows");
            }

            var numberOfColumns = builder.Rows.Max(x => x.Count);

            if (numberOfColumns < builder.Column.Count)
            {
                numberOfColumns = builder.Column.Count;
            }

            for (int i = 0; i < 1; i++)
            {
                if (builder.Column.Count < numberOfColumns)
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

            switch (builder.TableFormat)
            {
                case ConsoleTableBuilderFormat.Default:
                    return CreateTableForDefaultFormat(builder);
                case ConsoleTableBuilderFormat.Minimal:
                    builder.Options.Delimiter = string.Empty;
                    return CreateTableForMarkdownFormat(builder);
                case ConsoleTableBuilderFormat.Alternative:
                    return CreateTableForAlternativeFormat(builder);
                case ConsoleTableBuilderFormat.MarkDown:
                    return CreateTableForMarkdownFormat(builder);
                default:
                    return CreateTableForDefaultFormat(builder);
            }
        }

        public static void ExportAndWrite(this ConsoleTableBuilder builder)
        {
            Console.Write(builder.Export());
        }

        public static void ExportAndWriteLine(this ConsoleTableBuilder builder)
        {
            Console.WriteLine(builder.Export());
        }

        private static StringBuilder CreateTableForDefaultFormat(ConsoleTableBuilder builder)
        {
            var strBuilder = new StringBuilder();
            if (builder.Options.MetaRowPosition == MetaRowPosition.Top)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }

            // create the string format with padding
            var format = builder.Format(builder.Options.Delimiter);

            if (format == string.Empty)
            {
                return strBuilder;
            }

            // find the longest formatted line
            var maxRowLength = Math.Max(0, builder.Rows.Any() ? builder.Rows.Max(row => string.Format(format, row.ToArray()).Length) : 0);

            // add each row
            var results = builder.Rows.Select(row => string.Format(format, row.ToArray())).ToList();

            // create the divider
            var divider = string.Join("", Enumerable.Repeat(builder.Options.DividerString, maxRowLength).ToArray());

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

            if (builder.Options.MetaRowPosition == MetaRowPosition.Bottom)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }
            return strBuilder;
        }

        private static StringBuilder CreateTableForMarkdownFormat(ConsoleTableBuilder builder)
        {
            var strBuilder = new StringBuilder();
            if (builder.Options.MetaRowPosition == MetaRowPosition.Top)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }

            // create the string format with padding
            var format = builder.Format(builder.Options.Delimiter);

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
            var divider = Regex.Replace(columnHeaders, @"[^|]", builder.Options.DividerString);

            strBuilder.AppendLine(columnHeaders);
            strBuilder.AppendLine(divider);

            // add each row
            var results = builder.Rows.Skip(skipFirstRow ? 1 : 0).Select(row => string.Format(format, row.ToArray())).ToList();
            results.ForEach(row => strBuilder.AppendLine(row));

            if (builder.Options.MetaRowPosition == MetaRowPosition.Bottom)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }

            return strBuilder;
        }

        private static StringBuilder CreateTableForAlternativeFormat(ConsoleTableBuilder builder)
        {
            var strBuilder = new StringBuilder();
            if (builder.Options.MetaRowPosition == MetaRowPosition.Top)
            {
                strBuilder.AppendLine(BuildMetaRowFormat(builder));
            }

            // create the string format with padding
            var format = builder.Format(builder.Options.Delimiter);

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
            var divider = Regex.Replace(columnHeaders, @"[^|]", builder.Options.DividerString);
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

            if (builder.Options.MetaRowPosition == MetaRowPosition.Bottom)
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
    }
}
