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
            Console.WriteLine();
            Console.WriteLine("From [DataTable] type and Default format:");
            var tableBuilder = ConsoleTableBuilder.From(SampleTableData());
            tableBuilder.ExportAndWriteLine();

            Console.WriteLine("From [List] type and Minimal format:");
            tableBuilder.WithFormat(ConsoleTableBuilderFormat.Minimal).ExportAndWriteLine();

            Console.WriteLine();

            var listBuilder = ConsoleTableBuilder.From(SampleListData);
            Console.WriteLine("From [List] type and Alternative format:");
            listBuilder
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();

            Console.WriteLine();

            Console.WriteLine("From [List] type and MarkDown format w/ custom column name:");
            listBuilder
                .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                .WithColumn(new List<string>{ "N A M E" , "[Position]", "Office", "<Age>", "Something else I don't care"})
                .ExportAndWriteLine();

            Console.WriteLine();


            var arrayBuilder = ConsoleTableBuilder.From(new List<object[]>
            {
                new object[] {"luong", "son", "ba", null, "phim", null, null, null, 2, null},
                new object[] {"chuc", "anh", "dai", "nhac", null, null, null }
            });

            arrayBuilder
                .AddRow(new List<object> {1, "this", "is", "new", "row", "use", "<List>", null, null, null})
                .AddRow(new object[] {"2", "new row", "use", "array[] values", null, null})
                .WithOptions(new ConsoleTableBuilderOption
                {
                    IncludeRowInfo = IncludeRowCountType.Bottom,
                    RowInfoFormat = "\n=> This table has {ROW_COUNT} rows and [{0}] - [{1}]",
                    RowInfoParams = new object[] {"value 1",  2},
                    TrimColumn = true,
                    Delimiter = "¡",
                    DividerString = "»",
                })
                .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                .WithColumn(new List<string> {"THIS", "IS", "ADVANCED", "OPTIONS"})
                .ExportAndWriteLine();

            Console.ReadKey();
        }

        static DataTable SampleTableData()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Position", typeof(string));
            table.Columns.Add("Office", typeof(string));
            table.Columns.Add("Age", typeof(int));
            table.Columns.Add("Start Date", typeof(DateTime));

            table.Rows.Add("Airi Satou", "Accountant", "Tokyo", 33, new DateTime(2017, 05, 09));
            table.Rows.Add("Angelica Ramos", "Chief Executive Officer (CEO)", "New York", 47, new DateTime(2017, 01, 12));
            table.Rows.Add("Ashton Cox", "Junior Technical Author", "London", 46, new DateTime(2017, 04, 02));
            table.Rows.Add("Bradley Greer", "Software Engineer", "San Francisco", 28, new DateTime(2017, 11, 15));

            return table;
        }

        static List<List<object>> SampleListData = new List<List<object>>
        {
            new List<object>{ "Sakura Yamamoto", "Support Engineer", "London", 46},
            new List<object>{ "Serge Baldwin", "Data Coordinator", "San Francisco", 28, "something else" },
            new List<object>{ "Shad Decker", "Regional Director", "Edinburgh"},
        };


    }
}
