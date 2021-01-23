
namespace ConsoleTableExt
{
    public class ConsoleTableBuilderOption
    {
        #region Properties
        public MetaRowPositions MetaRowPosition { get; set; }
        public string MetaRowFormat { get; set; }
        public object[] MetaRowParams { get; set; }
        public char DelimiterChar { get; set; }
        public char DividerChar { get; set; }
        /// <summary>
        /// Trim empty columns on right side
        /// </summary>
        public bool TrimColumn { get; set; }
        #endregion

        #region Enums
        public enum MetaRowPositions
        {
            None = 0,
            Top = 1,
            Bottom = 2
        }

        #endregion

        #region Constructor
        public ConsoleTableBuilderOption()
        {
            this.DelimiterChar = '|';
            this.DividerChar = '-';
            this.TrimColumn = false;
            this.MetaRowPosition = MetaRowPositions.None;
            this.MetaRowFormat = string.Empty;
            this.MetaRowParams = new[] { "" };
        }
        #endregion
    }
}
