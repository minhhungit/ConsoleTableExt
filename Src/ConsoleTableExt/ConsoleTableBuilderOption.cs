namespace ConsoleTableExt
{
    public class ConsoleTableBuilderOption
    {
        public ConsoleTableBuilderOption()
        {
            Delimiter = '|';
            IncludeRowCount = IncludeRowCountType.None;
            TrimColumn = false;
        }

        public IncludeRowCountType IncludeRowCount { get; set; }
        public char Delimiter { get; set; }

        /// <summary>
        /// Trim empty columns on right side
        /// </summary>
        public bool TrimColumn { get; set; }
    }

    public enum IncludeRowCountType
    {
        None = 0,
        Top = 1,
        Bottom = 2
    }
}
