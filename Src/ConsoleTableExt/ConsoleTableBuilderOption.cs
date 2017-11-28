using System;

namespace ConsoleTableExt
{
    public class ConsoleTableBuilderOption
    {
        public ConsoleTableBuilderOption()
        {
            Delimiter = "|";
            DividerString = "-";
            TrimColumn = false;
            IncludeRowInfo = IncludeRowCountType.None;
            RowInfoFormat = string.Empty;
            RowInfoParams = new[] {""};
        }

        public IncludeRowCountType IncludeRowInfo { get; set; }
        public string RowInfoFormat { get; set; }
        public object[] RowInfoParams { get; set; }

        public string Delimiter { get; set; }
        public string DividerString { get; set; }

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
