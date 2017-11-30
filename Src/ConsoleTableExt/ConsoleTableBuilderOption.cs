namespace ConsoleTableExt
{
    public class ConsoleTableBuilderOption
    {
        public ConsoleTableBuilderOption()
        {
            Delimiter = "|";
            DividerString = "-";
            TrimColumn = false;
            IncludeRowInfo = IncludeRowInfoType.None;
            RowInfoFormat = string.Empty;
            RowInfoParams = new[] {""};
        }

        public IncludeRowInfoType IncludeRowInfo { get; set; }
        public string RowInfoFormat { get; set; }
        public object[] RowInfoParams { get; set; }

        public string Delimiter { get; set; }
        public string DividerString { get; set; }

        /// <summary>
        /// Trim empty columns on right side
        /// </summary>
        public bool TrimColumn { get; set; }
    }

    public enum IncludeRowInfoType
    {
        None = 0,
        Top = 1,
        Bottom = 2
    }
}
