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

        public static ConsoleTableBuilder AddRow(this ConsoleTableBuilder builder, List<object[]> values)
        {
            foreach (var item in values)
            {
                builder.AddRow(item);
            }

            return builder;
        }

        public static ConsoleTableBuilder AddRow(this ConsoleTableBuilder builder, object[] item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (!builder.Columns.Any())
                throw new Exception("Please set the columns first");

            if (builder.Columns.Count != item.Length)
                throw new Exception(
                    $"The number columns in the row ({builder.Columns.Count}) does not match the values ({item.Length}");

            builder.Rows.Add(item);

            return builder;
        }

        public static StringBuilder Export(this ConsoleTableBuilder builder)
        {
            return builder.Export(ConsoleTableFormat.Default);
        }

        public static StringBuilder Export(this ConsoleTableBuilder builder, ConsoleTableFormat tableFormat)
        {
            return Export(builder, tableFormat, '|');
        }

        public static StringBuilder Export(this ConsoleTableBuilder builder, ConsoleTableFormat tableFormat, char delimiter)
        {
            switch (tableFormat)
            {
                case ConsoleTableFormat.Default:
                    return ToDefaultString(builder, delimiter);
                case ConsoleTableFormat.Minimal:
                    return ToMarkDownString(builder, Char.MinValue);
                case ConsoleTableFormat.Alternative:
                    return ToAlternativeString(builder, delimiter);
                case ConsoleTableFormat.MarkDown:
                    return ToMarkDownString(builder, delimiter);
                default:
                    return ToDefaultString(builder, delimiter);
            }
        }

        private static StringBuilder ToDefaultString(ConsoleTableBuilder builder)
        {
            return ToDefaultString(builder, '|');
        }

        private static StringBuilder ToDefaultString(ConsoleTableBuilder builder, char delimiter)
        {
            var strBuilder = new StringBuilder();

            // find the longest column by searching each row
            var columnLengths = builder.ColumnLengths();

            // create the string format with padding
            var format = builder.Format(columnLengths, delimiter);

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

            return strBuilder;
        }

        private static StringBuilder ToMarkDownString(ConsoleTableBuilder builder)
        {
            return ToMarkDownString(builder, '|');
        }

        private static StringBuilder ToMarkDownString(ConsoleTableBuilder builder, char delimiter)
        {
            var strBuilder = new StringBuilder();

            // find the longest column by searching each row
            var columnLengths = builder.ColumnLengths();

            // create the string format with padding
            var format = builder.Format(columnLengths, delimiter);

            // find the longest formatted line
            var columnHeaders = string.Format(format, builder.Columns.ToArray());

            // add each row
            var results = builder.Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            var divider = Regex.Replace(columnHeaders, @"[^|]", "-");

            strBuilder.AppendLine(columnHeaders);
            strBuilder.AppendLine(divider);
            results.ForEach(row => strBuilder.AppendLine(row));

            return strBuilder;
        }

        private static StringBuilder ToAlternativeString(ConsoleTableBuilder builder)
        {
            return ToAlternativeString(builder, '|');
        }

        private static StringBuilder ToAlternativeString(ConsoleTableBuilder builder, char delimiter)
        {
            var stringBuilder = new StringBuilder();

            // find the longest column by searching each row
            var columnLengths = builder.ColumnLengths();

            // create the string format with padding
            var format = builder.Format(columnLengths, delimiter);

            // find the longest formatted line
            var columnHeaders = string.Format(format, builder.Columns.ToArray());

            // add each row
            var results = builder.Rows.Select(row => string.Format(format, row)).ToList();

            // create the divider
            var divider = Regex.Replace(columnHeaders, @"[^|]", "-");
            var dividerPlus = divider.Replace("|", "+");

            stringBuilder.AppendLine(dividerPlus);
            stringBuilder.AppendLine(columnHeaders);

            foreach (var row in results)
            {
                stringBuilder.AppendLine(dividerPlus);
                stringBuilder.AppendLine(row);
            }
            stringBuilder.AppendLine(dividerPlus);

            return stringBuilder;
        }
    }
}
