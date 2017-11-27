using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Data;

namespace ConsoleTableApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable table = new DataTable();
            table.Columns.Add(null, typeof(int));
            table.Columns.Add("Drug", typeof(string));
            table.Columns.Add("Patient", typeof(string));
            table.Columns.Add("Date", typeof(string));

            DataRow row = table.NewRow();
            row[0] = 999;
            row[1] = "y";
            row[2] = "str";
            row[3] = "x";

            ConsoleTableBuilder
                .From(GetTable())
                .WithColumn(new List<string> {"1", null, null})
                .AddRow(null, null, "3", null, "5", "6", "7")
                .AddRow(new List<object>{ null, 1, "2" })
                .AddRow(row)
                .AddRow(new List<List<object>>
                {
                    new List<object>{"1",2,3,4},
                    new List<object>{"x", "y", 999}
                })
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithOptions(new ConsoleTableBuilderOption {TrimColumn = false})
                .ExportAndWriteLine();

            ConsoleTableBuilder.From(new List<object[]>
                {
                    new object[] {"1", "2", 3, null},
                    new object[] {"1", null, 2},
                    new object[] {"122"}
                })
                .WithColumn(new List<string> {null, "1", null})
                .AddColumn("helo")
                .AddColumn("helo 1")
                .AddColumn("helo 2")
                .AddColumn("1","2","3","4","5")
                .WithColumn("hello", "co", "ba")
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithOptions(new ConsoleTableBuilderOption{TrimColumn = false })
                .ExportAndWrite();

            Console.WriteLine();

            ConsoleTableBuilder.From(new List<List<object>>
                {
                    new List<object> {"1", 2, null, 4},
                    new List<object> {null, "luong son ba chuc", "anh dai"},
                    new List<object> {"133 f afa faf as",}
                })
                .WithFormat(ConsoleTableBuilderFormat.Minimal)
                .ExportAndWriteLine();

            ConsoleTableBuilder
                .From(new List<object[]> {new[] {"1", null}, new[] {null, "4"}})
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();

            ConsoleTableBuilder
                .From(new List<object[]> { new object[] { null, null }, new object[] { null, null } })
                .WithFormat(ConsoleTableBuilderFormat.Default)
                .WithOptions(new ConsoleTableBuilderOption { TrimColumn = true })
                .ExportAndWriteLine();

            ConsoleTableBuilder
                .From(new List<object[]> { new object[] { null, null }, new object[] { null, null } })
                .WithFormat(ConsoleTableBuilderFormat.Default)
                .ExportAndWriteLine();

            var builder = ConsoleTableBuilder.From(GetTable());
            builder.Export().ToString();
            builder.WithFormat(ConsoleTableBuilderFormat.Default).ExportAndWriteLine();
            builder.WithFormat(ConsoleTableBuilderFormat.Alternative).ExportAndWriteLine();
            builder.WithFormat(ConsoleTableBuilderFormat.MarkDown).ExportAndWriteLine();
            builder.WithFormat(ConsoleTableBuilderFormat.Minimal).ExportAndWriteLine();

            Console.ReadKey();
        }

        static DataTable GetTable()
        {
            // Here we create a DataTable with four columns.
            DataTable table = new DataTable();
            table.Columns.Add(null, typeof(int));
            table.Columns.Add("Drug", typeof(string));
            table.Columns.Add("Patient", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            // Here we add five DataRows.
            table.Rows.Add(null, "Indocin", "David", DateTime.Now);
            table.Rows.Add(50, null, "Sam", DateTime.Now);
            table.Rows.Add(10, "Hydralazine", null, DateTime.Now);
            table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            table.Rows.Add(100, "Dilantin", "Melanie", null);
            return table;
        }
    }
}
