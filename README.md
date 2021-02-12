# ConsoleTableExt [![Build status](https://ci.appveyor.com/api/projects/status/e4ugtobtcrjhk9p4?svg=true)](https://ci.appveyor.com/project/minhhungit/consoletableext) <a href="https://www.nuget.org/packages/ConsoleTableExt/"><img src="https://img.shields.io/nuget/v/ConsoleTableExt.svg?style=flat" /></a>

A library to print out a nicely formatted table in a console application C#

### Nuget
> Install-Package ConsoleTableExt

### How to use:

```csharp
var tableData = new List<List<object>>
{
    new List<object>{ "Sakura Yamamoto", "Support Engineer", "London", 46},
    new List<object>{ "Serge Baldwin", "Data Coordinator", "San Francisco", 28, "something else" },
    new List<object>{ "Shad Decker", "Regional Director", "Edinburgh"},
};

// Simple table
ConsoleTableBuilder
    .From(tableData)
    .ExportAndWriteLine();


// Advanced table 
ConsoleTableBuilder
    .From(tableData)
    .WithTitle("CONTACTS ", ConsoleColor.Yellow, ConsoleColor.DarkGray)
    .WithColumn("Id", "First Name", "Sur Name")
    .WithMinLength(new Dictionary<int, int> {
        { 1, 25 },
        { 2, 25 }
    })
    .WithTextAlignment(new Dictionary<int, TextAligntment>
    {
        {2, TextAligntment.Right }
    })
    .WithCharMapDefinition(new Dictionary<CharMapPositions, char> {
        {CharMapPositions.BottomLeft, '=' },
        {CharMapPositions.BottomCenter, '=' },
        {CharMapPositions.BottomRight, '=' },
        {CharMapPositions.BorderTop, '=' },
        {CharMapPositions.BorderBottom, '=' },
        {CharMapPositions.BorderLeft, '|' },
        {CharMapPositions.BorderRight, '|' },
        {CharMapPositions.DividerY, '|' },
    })
    .WithHeaderCharMapDefinition(new Dictionary<HeaderCharMapPositions, char> {
        {HeaderCharMapPositions.TopLeft, '=' },
        {HeaderCharMapPositions.TopCenter, '=' },
        {HeaderCharMapPositions.TopRight, '=' },
        {HeaderCharMapPositions.BottomLeft, '|' },
        {HeaderCharMapPositions.BottomCenter, '-' },
        {HeaderCharMapPositions.BottomRight, '|' },
        {HeaderCharMapPositions.Divider, '|' },
        {HeaderCharMapPositions.BorderTop, '=' },
        {HeaderCharMapPositions.BorderBottom, '-' },
        {HeaderCharMapPositions.BorderLeft, '|' },
        {HeaderCharMapPositions.BorderRight, '|' },
    })
    .ExportAndWriteLine(TableAligntment.Right);
```

<img src="https://raw.githubusercontent.com/minhhungit/ConsoleTableExt/master/wiki/Images/demo1.png" style="width: 100%;" />

Check more demo here https://github.com/minhhungit/ConsoleTableExt/blob/master/Src/ConsoleTableApp/Program.cs

<img src="https://raw.githubusercontent.com/minhhungit/ConsoleTableExt/master/wiki/Images/demo.png" style="width: 100%;" />

### Char Map Definition

<img src="https://raw.githubusercontent.com/minhhungit/ConsoleTableExt/master/wiki/Images/CharMapPositions.png" style="width: 100%;" />

### Header Char Map

<img src="https://raw.githubusercontent.com/minhhungit/ConsoleTableExt/master/wiki/Images/HeaderCharMapPositions.png" style="width: 100%;" />

<a href='https://ko-fi.com/I2I13GAGL' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://cdn.ko-fi.com/cdn/kofi4.png?v=2' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>

### Inspired by 
- <a href="https://github.com/khalidabuhakmeh/ConsoleTables">khalidabuhakmeh/ConsoleTables</a>
