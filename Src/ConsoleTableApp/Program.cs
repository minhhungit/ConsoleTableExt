using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace ConsoleTableApp
{ 
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleTableBuilder.From(new List<object[]>
            {
                new object[] { "s" }
            })
                .WithTitle("abcdefghlm")
                .ExportAndWriteLine();

            _____________________________PrintDemoDivider();

            Console.WriteLine("From [DataTable] type and Minimal format:");
            ConsoleTableBuilder.From(SampleTableData()).WithFormat(ConsoleTableBuilderFormat.Minimal).ExportAndWriteLine();

            _____________________________PrintDemoDivider();

            var strBuilder01 =
                   ConsoleTableBuilder
                   .From(SampleTableData())
                   .WithPaddingLeft(string.Empty)
                   .WithCharMapDefinition()
                   .Export();
            Console.WriteLine(strBuilder01);

            _____________________________PrintDemoDivider();

            var strBuilder02 =
                   ConsoleTableBuilder
                   .From(SampleTableData())
                   .WithTitle("MARKDOWN WITH TITLE ???")
                   .WithPaddingLeft(string.Empty)
                   .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                   .Export();
            Console.WriteLine(strBuilder02);

            _____________________________PrintDemoDivider();

            Console.WriteLine("Text alignment with table title");
            ConsoleTableBuilder.From(SampleListData)
                //.WithFormat(ConsoleTableBuilderFormat.MarkDown)
                .WithTextAlignment(new Dictionary<int, TextAligntment> {
                    { 1, TextAligntment.Right },
                    { 3, TextAligntment.Right },
                    { 100, TextAligntment.Right }
                })
                .WithMinLength(new Dictionary<int, int> {
                    { 1, 30 }
                })
                .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
                .WithTitle("HELLO I AM TITLE", ConsoleColor.Green, ConsoleColor.DarkGray, TitleAligntment.Right)
                .WithFormatter(1, (text)=> {
                    return text.ToUpper().Replace(" ", "-") + " «";
                })                
                .ExportAndWriteLine(TableAligntment.Center);

            _____________________________PrintDemoDivider();

            Console.WriteLine("Text alignment and column min length");
            ConsoleTableBuilder.From(SampleTableData())
                .WithTextAlignment(new Dictionary<int, TextAligntment> {
                    { 1, TextAligntment.Right },
                    { 3, TextAligntment.Right },
                    { 100, TextAligntment.Right }
                })
                .WithMinLength(new Dictionary<int, int> {
                    {1, 35}
                })
                .WithFormatter(2, (text) => {
                    char[] chars = text.ToCharArray();
                    Array.Reverse(chars);
                    return new String(chars);
                })
                .WithTitle("Hello, everyone! This is the LONGEST TEXT EVER! I was inspired by the various other 'longest texts ever' on the internet, and I wanted to make my own. So here it is!".ToUpper(), ConsoleColor.Yellow, ConsoleColor.DarkMagenta)
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .WithFormatter(3, (text) => {
                    if (string.IsNullOrEmpty(text) || text.Trim().Length == 0)
                    {
                        return "0 $";
                    }
                    else
                    {
                        return text + " $";
                    }
                }, true)
                .ExportAndWriteLine();

            _____________________________PrintDemoDivider();

            Console.WriteLine("Custom format using CharMap");
            ConsoleTableBuilder.From(SampleTableData())
                .WithCharMapDefinition(
                    CharMapDefinition.FramePipDefinition,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                    })
                .ExportAndWriteLine(TableAligntment.Right);

            _____________________________PrintDemoDivider();

            Console.WriteLine("Custom format using CharMap: Header has no divider");
            ConsoleTableBuilder.From(SampleTableData())
                .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
                .WithCharMapDefinition(
                    CharMapDefinition.FramePipDefinition,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '═' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╤' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, ' ' },
                    })
                .ExportAndWriteLine();

            _____________________________PrintDemoDivider();

            Console.WriteLine("No header FramePipDefinition");
            ConsoleTableBuilder.From(SampleListData)
                .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
                .ExportAndWriteLine();

            _____________________________PrintDemoDivider();

            Console.WriteLine("No header FrameDoublePipDefinition:");
            ConsoleTableBuilder.From(SampleListData)
                .WithCharMapDefinition(CharMapDefinition.FrameDoublePipDefinition)
                .ExportAndWriteLine();


            _____________________________PrintDemoDivider();

            Console.WriteLine("From [DataTable] type and Default format:");
            ConsoleTableBuilder.From(SampleTableData()).ExportAndWriteLine();

            Console.WriteLine("From [DataTable] type and Minimal format:");
            ConsoleTableBuilder.From(SampleTableData()).WithFormat(ConsoleTableBuilderFormat.Minimal).ExportAndWriteLine();

            _____________________________PrintDemoDivider();

            Console.WriteLine("From [List] type and Alternative format:");
            ConsoleTableBuilder.From(SampleListData)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();

            _____________________________PrintDemoDivider();

            Console.WriteLine("From [List] type and MarkDown format w/ custom column name:");
            ConsoleTableBuilder.From(SampleListData)
                .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                .WithColumn(new List<string> { "N A M E", "[Position]", "Office", "<Age>", "Something else I don't care" })
                .ExportAndWriteLine();

            _____________________________PrintDemoDivider();

            Console.WriteLine("From [List<T>] (where T:class) type and Minimal format:");
            ConsoleTableBuilder.From(SampleEmployeesList).WithFormat(ConsoleTableBuilderFormat.Minimal).ExportAndWriteLine();

            _____________________________PrintDemoDivider();

            Console.WriteLine("From [List<T>] (where T: !class) type and Alternative format:");
            ConsoleTableBuilder.From(new List<int> { 1, 2, 3, 4, 5, 6 }).WithFormat(ConsoleTableBuilderFormat.Alternative).WithColumn("I'm a custom name").ExportAndWrite();


            _____________________________PrintDemoDivider();

            ConsoleTableBuilder.From(new List<object[]>
            {
                new object[] { "luong", "son", "ba", null, "phim", null, null, null, 2, null},
                new object[] { "chuc", "anh", "dai", "nhac", null, null, null }
            })
                .TrimColumn(true)
                .AddRow(new List<object> { 1, "this", "is", "new", "row", "use", "<List>", null, null, null })
                .AddRow(new object[] { "2", "new row", "use", "array[] values", null, null })
                .WithMetadataRow(MetaRowPositions.Top, b => string.Format("=> First top line <{0}>", "FIRST"))
                .WithMetadataRow(MetaRowPositions.Top, b => string.Format("=> Second top line <{0}>", "SECOND"))
                .WithMetadataRow(
                    MetaRowPositions.Bottom,
                    b => string.Format("=> This table has {3} rows and {2} columns=> [{0}] - [test value {1}]",
                        "test value 1",
                        2,
                        b.NumberOfColumns,
                        b.NumberOfRows
                    )
                )
                .WithMetadataRow(MetaRowPositions.Bottom, b => string.Format("=> Bottom line <{0}>", "HELLO WORLD"))
                .WithColumn(new List<string> { "THIS", "IS", "ADVANCED", "OPTIONS" })
                .WithCharMapDefinition(new Dictionary<CharMapPositions, char> {
                    { CharMapPositions.BorderLeft, '¡' },
                    { CharMapPositions.BorderRight, '¡' },
                    { CharMapPositions.DividerY, '¡' }
                })
                .WithHeaderCharMapDefinition(new Dictionary<HeaderCharMapPositions, char> {
                    { HeaderCharMapPositions.BottomLeft, '»' },
                    { HeaderCharMapPositions.BottomCenter, '»' },
                    { HeaderCharMapPositions.BottomRight, '»' },
                    { HeaderCharMapPositions.Divider, '¡' },
                    { HeaderCharMapPositions.BorderBottom, '»' },
                    { HeaderCharMapPositions.BorderLeft, '¡' },
                    { HeaderCharMapPositions.BorderRight, '¡' }
                })
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

        static List<List<object>> SampleShortListData = new List<List<object>>
        {
            new List<object>{ ""},
            new List<object>{ ""},
            new List<object>{ ""},
            new List<object>{ ""},
        };

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

        private static void _____________________________PrintDemoDivider()
        {
            Console.WriteLine();
            Console.WriteLine();
            // Console.Write(string.Format("\n\n{0}\n", Enumerable.Range(0, Console.WindowWidth).Select(x => "=").Aggregate((s, a) => s + a)));
        }
    }
}
