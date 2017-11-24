using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ConsoleTableApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(ConsoleTableBuilder.From(new List<object[]>{ new[] { "1", null } , new []{ null, "4"}}).Export(new ConsoleTableExportOption{ExportFormat = ConsoleTableFormat.Alternative}).ToString());

            var optionDefault = new ConsoleTableExportOption { IncludeRowCount = IncludeRowCountType.Top };
            var optionAlternative = new ConsoleTableExportOption { ExportFormat = ConsoleTableFormat.Alternative };
            var optionMarkDown = new ConsoleTableExportOption { ExportFormat = ConsoleTableFormat.MarkDown };
            var optionMinimal = new ConsoleTableExportOption { ExportFormat = ConsoleTableFormat.Minimal, IncludeRowCount = IncludeRowCountType.Bottom };

            var result0 = ConsoleTableBuilder.From(GetTable()).Export().ToString();
            var result1 = ConsoleTableBuilder.From(GetTable()).Export(optionDefault).ToString();
            var result2 = ConsoleTableBuilder.From(GetTable()).Export(optionAlternative).ToString();
            var result3 = ConsoleTableBuilder.From(GetTable()).Export(optionMarkDown).ToString();
            var result4 = ConsoleTableBuilder.From(GetTable()).Export(optionMinimal).ToString();

            Console.Write(result0);
            Console.Write(Environment.NewLine);
            Console.Write(result1);
            Console.Write(Environment.NewLine);
            Console.Write(result2);
            Console.Write(Environment.NewLine);
            Console.Write(result3);
            Console.Write(Environment.NewLine);
            Console.Write(result4);

            var rows = Enumerable.Repeat(new Something(null), 200).ToList();
            rows.AddRange(Enumerable.Repeat(new Something("GetCustr ecognized", null), 200).ToList());
            rows.AddRange(Enumerable.Repeat(new Something("Sending email  Gmail"), 200).ToList());
            rows.AddRange(Enumerable.Repeat(new Something("Be sure to use Se deprecated System.Web.Mail"), 200).ToList());
            rows = rows.OrderBy(elem => Guid.NewGuid()).ToList();
            var x = ConsoleTableBuilder.From(rows).Export().ToString();

            var result5 = ConsoleTableBuilder.From(rows)
                .AddColumn(new List<string> {"A", "B", "C"}, true)
                .AddRow("1", "2", "3")
                .AddRow("11", "22", "33")
                .AddRow(new List<object[]> {new[] {"a", "b", "c"}, new[] {"a", "a", "ccc"}})
                .AddRow("111", "222", "333")
                .AddColumn(new List<string> {"ColA", "ColB", "ColC"}, true)
                .Export().ToString();

            Console.Write(Environment.NewLine);
            Console.Write(result5);
            Console.Write(Environment.NewLine);

            Console.ReadKey();
        }

        static DataTable GetTable()
        {
            // Here we create a DataTable with four columns.
            DataTable table = new DataTable();
            table.Columns.Add("Dosage", typeof(int));
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

    public class Something : IConsoleTableDataStore
    {
        public Something(string name)
        {
            Id = Guid.NewGuid().ToString("N");
            Name = name;
            Date = DateTime.Now;
        }

        public Something(string name, DateTime? date)
        {
            Id = Guid.NewGuid().ToString("N");
            Name = name;
            Date = date;
        }

        [ConsoleTableColumnAttributes(1, "Name")]
        public string Name { get; set; }

        [ConsoleTableColumnAttributes(3, "Dxxxx")]
        public string Id { get; set; }

        [ConsoleTableColumnAttributes(2)]
        public DateTime? Date { get; set; }
    }
}
