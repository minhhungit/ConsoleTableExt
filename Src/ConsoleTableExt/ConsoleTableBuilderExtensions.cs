using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleTableExt
{
    public static class ConsoleTableBuilderExtensions
    {
        public static ConsoleTableBuilder AddColumn(this ConsoleTableBuilder builder, string name)
        {
            builder.Columns.Add(name);
            return builder;
        }

        public static ConsoleTableBuilder AddColumn(this ConsoleTableBuilder builder, List<string> names, bool replaceOldColumns)
        {
            if (replaceOldColumns)
            {
                builder.Columns = new List<object>();
            }

            foreach (var name in names)
                builder.Columns.Add(name);

            return builder;
        }

        public static ConsoleTableBuilder AddRow(this ConsoleTableBuilder builder, params object[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (!builder.Columns.Any())
                throw new Exception("Please set the columns first");

            if (builder.Columns.Count != values.Length)
                throw new Exception(
                    $"The number columns in the row ({builder.Columns.Count}) does not match the values ({values.Length}");

            builder.Rows.Add(values);

            return builder;
        }

        public static StringBuilder Export(this ConsoleTableBuilder builder)
        {
            return builder.Export(new ConsoleTableExportOption());
        }

        public static StringBuilder Export(this ConsoleTableBuilder builder, ConsoleTableExportOption option)
        {
            switch (option.ExportFormat)
            {
                case ConsoleTableFormat.Default:
                    return ToDefaultString(builder, option);
                case ConsoleTableFormat.Minimal:
                    option.Delimiter = Char.MinValue;
                    return ToMarkDownString(builder, option);
                case ConsoleTableFormat.Alternative:
                    return ToAlternativeString(builder, option);
                case ConsoleTableFormat.MarkDown:
                    return ToMarkDownString(builder, option);
                default:
                    return ToDefaultString(builder, option);
            }
        }


        private static StringBuilder ToDefaultString(ConsoleTableBuilder builder, ConsoleTableExportOption option)
        {
            var strBuilder = new StringBuilder();
            if (option.IncludeRowCount == IncludeRowCountType.Top)
            {
                strBuilder.AppendLine($"Count: {builder.Rows.Count}");
            }

            // find the longest column by searching each row
            var columnLengths = builder.ColumnLengths();

            // create the string format with padding
            var format = builder.Format(columnLengths, option.Delimiter);

            // find the longest formatted line
            var maxRowLength = Math.Max(0, builder.Rows.Any() ? builder.Rows.Max(row => string.Format(format, row).Length) : 0);
            var columnHeaders = string.Format(format, builder.Columns.ToArray());

            // longest line is greater of formatted columnHeader and longest row
            var longestLine = Math.Max(maxRowLength, columnHeaders.Length);

            // add each row
            var results = builder.Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            var divider = " " + string.Join("", Enumerable.Repeat("-", longestLine - 1)) + " ";

            strBuilder.AppendLine(divider);
            strBuilder.AppendLine(columnHeaders);

            foreach (var row in results)
            {
                strBuilder.AppendLine(divider);
                strBuilder.AppendLine(row);
            }

            strBuilder.AppendLine(divider);

            if (option.IncludeRowCount == IncludeRowCountType.Bottom)
            {
                strBuilder.AppendLine($"Count: {builder.Rows.Count}");
            }
            return strBuilder;
        }

        private static StringBuilder ToMarkDownString(ConsoleTableBuilder builder, ConsoleTableExportOption option)
        {
            var strBuilder = new StringBuilder();
            if (option.IncludeRowCount == IncludeRowCountType.Top)
            {
                strBuilder.AppendLine($"Count: {builder.Rows.Count}");
            }

            // find the longest column by searching each row
            var columnLengths = builder.ColumnLengths();

            // create the string format with padding
            var format = builder.Format(columnLengths, option.Delimiter);

            // find the longest formatted line
            var columnHeaders = string.Format(format, builder.Columns.ToArray());

            // add each row
            var results = builder.Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            var divider = Regex.Replace(columnHeaders, @"[^|]", "-");

            strBuilder.AppendLine(columnHeaders);
            strBuilder.AppendLine(divider);
            results.ForEach(row => strBuilder.AppendLine(row));

            if (option.IncludeRowCount == IncludeRowCountType.Bottom)
            {
                strBuilder.AppendLine($"Count: {builder.Rows.Count}");
            }

            return strBuilder;
        }

        private static StringBuilder ToAlternativeString(ConsoleTableBuilder builder, ConsoleTableExportOption option)
        {
            var strBuilder = new StringBuilder();
            if (option.IncludeRowCount == IncludeRowCountType.Top)
            {
                strBuilder.AppendLine($"Count: {builder.Rows.Count}");
            }

            // find the longest column by searching each row
            var columnLengths = builder.ColumnLengths();

            // create the string format with padding
            var format = builder.Format(columnLengths, option.Delimiter);

            // find the longest formatted line
            var columnHeaders = string.Format(format, builder.Columns.ToArray());

            // add each row
            var results = builder.Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            var divider = Regex.Replace(columnHeaders, @"[^|]", "-");
            var dividerPlus = divider.Replace("|", "+");

            strBuilder.AppendLine(dividerPlus);
            strBuilder.AppendLine(columnHeaders);

            foreach (var row in results)
            {
                strBuilder.AppendLine(dividerPlus);
                strBuilder.AppendLine(row);
            }
            strBuilder.AppendLine(dividerPlus);

            if (option.IncludeRowCount == IncludeRowCountType.Bottom)
            {
                strBuilder.AppendLine($"Count: {builder.Rows.Count}");
            }
            return strBuilder;
        }
    }
}
