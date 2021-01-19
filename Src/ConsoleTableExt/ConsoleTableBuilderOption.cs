namespace ConsoleTableExt
{
    public class ConsoleTableBuilderOption
    {
        #region Properties
        public MetaRowPositions MetaRowPosition { get; set; }
        public string MetaRowFormat { get; set; }
        public object[] MetaRowParams { get; set; }
        public char Delimiter { get; set; }
        public char DividerChar { get; set; }
        /// <summary>
        /// Trim empty columns on right side
        /// </summary>
        public bool TrimColumn { get; set; }
        public FrameStyles FrameStyle { get; set; } = 0;
        #endregion

        #region Enums
        public enum MetaRowPositions
        {
            None = 0,
            Top = 1,
            Bottom = 2
        }
        public enum FrameStyles
        {
            None = 0,
            Pipe,
            DoublePipe
        }
        #endregion

        #region Constructor
        public ConsoleTableBuilderOption()
        {
            this.Delimiter = '|';
            this.DividerChar = '-';
            this.TrimColumn = false;
            this.MetaRowPosition = MetaRowPositions.None;
            this.MetaRowFormat = string.Empty;
            this.MetaRowParams = new[] { "" };
        }
        #endregion
    }
}
