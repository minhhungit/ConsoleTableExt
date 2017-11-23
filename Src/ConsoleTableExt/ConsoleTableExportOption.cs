namespace ConsoleTableExt
{
    public class ConsoleTableExportOption
    {
        public ConsoleTableExportOption()
        {
            ExportFormat = ConsoleTableFormat.Default;
            Delimiter = '|';
            IncludeRowCount = IncludeRowCountType.None;
        }

        public IncludeRowCountType IncludeRowCount { get; set; }
        public ConsoleTableFormat ExportFormat { get; set; }
        public char Delimiter { get; set; }
    }

    public enum IncludeRowCountType
    {
        None = 0,
        Top = 1,
        Bottom = 2
    }
}
