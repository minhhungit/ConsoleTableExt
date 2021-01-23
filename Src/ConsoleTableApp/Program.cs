using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace ConsoleTableApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Has Column A:");
            var tableBuilder = ConsoleTableBuilder.From(SampleEmployeesList);
            tableBuilder
                .WithCharMapDefination(CharMapDefinations.FramePipDefination)
                .ExportAndWriteLine(TableAligntment.Center);

            Console.WriteLine("Has Column B:");
            tableBuilder = ConsoleTableBuilder.From(SampleTableData());
            tableBuilder
                .WithCharMapDefination(CharMapDefinations.FrameDoublePipDefination)
                .ExportAndWriteLine();

            Console.WriteLine("Has Column C:");
            tableBuilder = ConsoleTableBuilder.From(SampleTableData());
            tableBuilder
                .WithCharMapDefination(
                    CharMapDefinations.FramePipDefination,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.A1, '╒' },
                        {HeaderCharMapPositions.B1, '╤' },
                        {HeaderCharMapPositions.C1, '╕' },
                        {HeaderCharMapPositions.A2, '╞' },
                        {HeaderCharMapPositions.B2, '╪' },
                        {HeaderCharMapPositions.C2, '╡' },
                        {HeaderCharMapPositions.BorderX, '═' },
                        {HeaderCharMapPositions.BorderY, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                    })
                .ExportAndWriteLine(TableAligntment.Right);

            Console.WriteLine("Has Column D:");
            tableBuilder = ConsoleTableBuilder.From(SampleTableData());
            tableBuilder
                .WithCharMapDefination(
                    CharMapDefinations.FramePipDefination,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.A1, '╒' },
                        {HeaderCharMapPositions.B1, '═' },
                        {HeaderCharMapPositions.C1, '╕' },
                        {HeaderCharMapPositions.A2, '╞' },
                        {HeaderCharMapPositions.B2, '╤' },
                        {HeaderCharMapPositions.C2, '╡' },
                        {HeaderCharMapPositions.BorderX, '═' },
                        {HeaderCharMapPositions.BorderY, '│' },
                        {HeaderCharMapPositions.Divider, '\0' },
                    })
                .ExportAndWriteLine();

            // --------------------------

            Console.WriteLine();
            Console.WriteLine("Has NO Column A:");
            tableBuilder = ConsoleTableBuilder.From(SampleListData);
            tableBuilder
                .WithCharMapDefination(CharMapDefinations.FramePipDefination)
                .ExportAndWriteLine();

            Console.WriteLine("Has NO Column B:");
            tableBuilder = ConsoleTableBuilder.From(SampleListData);
            tableBuilder
                .WithCharMapDefination(CharMapDefinations.FrameDoublePipDefination)
                .ExportAndWriteLine();

            Console.WriteLine("Has NO Column C:");
            tableBuilder = ConsoleTableBuilder.From(SampleListData);
            tableBuilder
                .WithCharMapDefination(
                    CharMapDefinations.FramePipDefination,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.A1, '╒' },
                        {HeaderCharMapPositions.B1, '╤' },
                        {HeaderCharMapPositions.C1, '╕' },
                        {HeaderCharMapPositions.A2, '╞' },
                        {HeaderCharMapPositions.B2, '╪' },
                        {HeaderCharMapPositions.C2, '╡' },
                        {HeaderCharMapPositions.BorderX, '═' },
                        {HeaderCharMapPositions.BorderY, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                    })
                .ExportAndWriteLine();

            Console.WriteLine("Has NO Column D:");
            tableBuilder = ConsoleTableBuilder.From(SampleListData);
            tableBuilder
                .WithCharMapDefination(
                    CharMapDefinations.FramePipDefination,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.A1, '╒' },
                        {HeaderCharMapPositions.B1, '═' },
                        {HeaderCharMapPositions.C1, '╕' },
                        {HeaderCharMapPositions.A2, '╞' },
                        {HeaderCharMapPositions.B2, '╤' },
                        {HeaderCharMapPositions.C2, '╡' },
                        {HeaderCharMapPositions.BorderX, '═' },
                        {HeaderCharMapPositions.BorderY, '│' },
                        {HeaderCharMapPositions.Divider, '\0' },
                    })
                .ExportAndWriteLine();

            // ------------

            Console.WriteLine();
            Console.WriteLine("From [DataTable] type and Default format:");
            tableBuilder = ConsoleTableBuilder.From(SampleTableData());
            tableBuilder.ExportAndWriteLine();

            Console.WriteLine("From [DataTable] type and Minimal format:");
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
                .WithColumn(new List<string> { "N A M E", "[Position]", "Office", "<Age>", "Something else I don't care" })
                .ExportAndWriteLine();

            Console.WriteLine();

            Console.WriteLine("From [List<T>] (where T:class) type and Minimal format:");
            ConsoleTableBuilder.From(SampleEmployeesList).WithFormat(ConsoleTableBuilderFormat.Minimal).ExportAndWriteLine();
            Console.WriteLine();

            Console.WriteLine("From [List<T>] (where T: !class) type and Alternative format:");
            ConsoleTableBuilder.From(new List<int> { 1, 2, 3, 4, 5, 6 }).WithFormat(ConsoleTableBuilderFormat.Alternative).WithColumn("I'm a custom name, my default name is 'Value'").ExportAndWrite();
            Console.WriteLine();

            var arrayBuilder = ConsoleTableBuilder.From(new List<object[]>
            {
                new object[] {"luong", "son", "ba", null, "phim", null, null, null, 2, null},
                new object[] {"chuc", "anh", "dai", "nhac", null, null, null }
            });

            arrayBuilder
                .AddRow(new List<object> { 1, "this", "is", "new", "row", "use", "<List>", null, null, null })
                .AddRow(new object[] { "2", "new row", "use", "array[] values", null, null })
                .WithOptions(new ConsoleTableBuilderOption
                {
                    MetaRowPosition = ConsoleTableBuilderOption.MetaRowPositions.Bottom,
                    MetaRowFormat = "\n=> This table has {3} rows and {2} columns\n=> [{0}] - [test value {1}]",
                    MetaRowParams = new object[]
                    {
                        "test value 1",
                        2,
                        AppConstants.MetaRow.COLUMN_COUNT,
                        AppConstants.MetaRow.ROW_COUNT
                    },
                    TrimColumn = true,
                    DelimiterChar = '¡',
                    DividerChar = '»',
                })
                .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                .WithColumn(new List<string> { "THIS", "IS", "ADVANCED", "OPTIONS" })
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


        static List<Employee> SampleEmployeesList = new List<Employee>
        {
            new Employee("Airi Satou", "Accountant", "Tokyo", 33, new DateTime(2017, 05, 09)),
            new Employee("Angelica Ramos", "Chief Executive Officer (CEO)", "New York", 47, new DateTime(2017, 01, 12)),
            new Employee("Ashton Cox", "Junior Technical Author", "London", 46, new DateTime(2017, 04, 02)),
            new Employee("Bradley Greer", "Software Engineer", "San Francisco", 28, new DateTime(2017, 11, 15))
        };

        private class Employee
        {
            public Employee(string name, string position, string office, int age, DateTime startDate)
            {
                Name = name;
                Position = position;
                Office = office;
                Age = age;
                StartDate = startDate;
            }

            [Description("N - A - M - E")]
            public string Name { get; set; }
            public string Position { get; set; }
            public string Office { get; set; }
            public int Age { get; set; }
            public DateTime StartDate { get; set; }
        }
    }
}
