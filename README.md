# ConsoleTableExt [![Build status](https://ci.appveyor.com/api/projects/status/e4ugtobtcrjhk9p4?svg=true)](https://ci.appveyor.com/project/minhhungit/consoletableext) <a href="https://www.nuget.org/packages/ConsoleTableExt/"><img src="https://img.shields.io/nuget/v/ConsoleTableExt.svg?style=flat" /></a>
A library to print out a nicely formatted table in a console application C# rewrite based on <a href="https://github.com/khalidabuhakmeh/ConsoleTables">khalidabuhakmeh/ConsoleTables</a>

### Nuget
> Install-Package ConsoleTableExt

### Demo
https://github.com/minhhungit/ConsoleTableExt/tree/master/Src/ConsoleTableApp

<img src="https://raw.githubusercontent.com/minhhungit/ConsoleTableExt/master/wiki/Images/demo.png" style="width: 100%;" />

### How to use:
From [DataTable] type and Default format:
```csharp
ConsoleTableBuilder
   .From(GetSampleTableData())
   .ExportAndWriteLine();
```
From [DataTable] type and Minimal format:
```csharp
ConsoleTableBuilder
   .From(GetSampleTableData())
   .WithFormat(ConsoleTableBuilderFormat.Minimal)
   .ExportAndWriteLine();
```

From [List] type and Alternative format:
```csharp
ConsoleTableBuilder
   .From(GetSampleListData())
   .WithFormat(ConsoleTableBuilderFormat.Alternative)
   .ExportAndWriteLine();
```

From [List] type and MarkDown format w/ custom column name:
```csharp
ConsoleTableBuilder
   .From(GetSampleListData())
   .WithFormat(ConsoleTableBuilderFormat.MarkDown)
   .WithColumn(new List<string>{ "N A M E" , "[Position]", "Office", "<Age>", "Something else I don't care"})
   .ExportAndWriteLine();
```

Custom:
```csharp
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
       MetaRowPosition = MetaRowPosition.Bottom,
       MetaRowFormat = "\n=> This table has {3} rows and {2} columns\n=> [{0}] - [test value {1}]",
       MetaRowParams = new object[]
       {
           "test value 1",
           2,
           AppConstants.MetaRow.COLUMN_COUNT,
           AppConstants.MetaRow.ROW_COUNT 
       },
       TrimColumn = true,
       Delimiter = "¡",
       DividerString = "»",
   })
   .WithFormat(ConsoleTableBuilderFormat.MarkDown)
   .WithColumn(new List<string> {"THIS", "IS", "ADVANCED", "OPTIONS"})
   .ExportAndWriteLine();
```
Sample data:
```csharp
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
```
