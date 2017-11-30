# ConsoleTableExt [![Build status](https://ci.appveyor.com/api/projects/status/e4ugtobtcrjhk9p4?svg=true)](https://ci.appveyor.com/project/minhhungit/consoletableext) <a href="https://www.nuget.org/packages/ConsoleTableExt/"><img src="https://img.shields.io/nuget/v/ConsoleTableExt.svg?style=flat" /></a>
A library to print out a nicely formatted table in a console application C# rewrite based on <a href="https://github.com/khalidabuhakmeh/ConsoleTables">khalidabuhakmeh/ConsoleTables</a>

### Nuget
> Install-Package ConsoleTableExt

### Demo
https://github.com/minhhungit/ConsoleTableExt/tree/master/Src/ConsoleTableApp

### How to use:
From [DataTable] type and Default format:
```csharp
ConsoleTableBuilder
   .From(SampleTableData())
   .ExportAndWriteLine();
```
From [List] type and Minimal format:
```csharp
ConsoleTableBuilder
   .From(SampleTableData())
   .WithFormat(ConsoleTableBuilderFormat.Minimal)
   .ExportAndWriteLine();
```
From [List] type and Alternative format:
```csharp
ConsoleTableBuilder
   .From(SampleListData)
   .WithFormat(ConsoleTableBuilderFormat.Alternative)
   .ExportAndWriteLine();
```

From [List] type and MarkDown format w/ custom column name:
```csharp
ConsoleTableBuilder
   .From(SampleListData)
   .WithFormat(ConsoleTableBuilderFormat.MarkDown)
   .WithColumn(new List<string>{ "N A M E" , "[Position]", "Office", "<Age>", "HELLO"})
   .ExportAndWriteLine();
```

Custom:
```csharp
ConsoleTableBuilder.From(new List<object[]>
   {
   	new object[] {"luong", "son", "ba", null, "phim", null, null, null, 2, null},
   	new object[] {"chuc", "anh", "dai", "nhac", null, null, null}
   })
   .AddRow(new List<object> {1, "this", "is", "new", "row", "use", "<List>", null, null, null})
   .AddRow(new object[] {"2", "new row", "use", "array[] values", null, null})
   .WithOptions(new ConsoleTableBuilderOption
   {
   	IncludeRowInfo = IncludeRowCountType.Bottom,
   	RowInfoFormat = "\n=> This table has {ROW_COUNT} rows and [{0}] - [{1}]",
   	RowInfoParams = new object[] {"value 1", 2},
   	TrimColumn = true,
   	Delimiter = "¡",
   	DividerString = "»",
   })
   .WithFormat(ConsoleTableBuilderFormat.MarkDown)
   .WithColumn(new List<string> {"THIS", "IS", "ADVANCED", "OPTIONS"})
   .ExportAndWriteLine();
```

<img src="https://raw.githubusercontent.com/minhhungit/ConsoleTableExt/master/wiki/Images/demo.png" style="width: 100%;" />
