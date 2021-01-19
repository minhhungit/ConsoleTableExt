using System.Text;

namespace ConsoleTableExt
{
    internal class FrameChars
    {
        #region Pipes
        /// <summary>
        /// Pipe South East: ╔ ALT + 457; ALT + 201
        /// </summary>
        public static char PipeSE { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x95, 0x94 }).ToCharArray()[0];
        /// <summary>
        /// Pipe South West: ╗ ALT + 187
        /// </summary>
        public static char PipeSW { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x95, 0x97 }).ToCharArray()[0];
        /// <summary>
        /// Pipe North East: ╚ ALT + 456; ALT + 200
        /// </summary>
        public static char PipeNE { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x95, 0x9A }).ToCharArray()[0];
        /// <summary>
        /// Pipe North West: ╝ ALT + 188
        /// </summary>
        public static char PipeNW { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x95, 0x9D }).ToCharArray()[0];
        /// <summary>
        /// Pipe Horizontal: ═ ALT + 461; ALT + 205
        /// </summary>
        public static char PipeHorizontal { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x95, 0x90 }).ToCharArray()[0];
        /// <summary>
        /// Pipe Vertical: ║ ALT + 186; ALT + 442
        /// </summary>
        public static char PipeVertical { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x95, 0x91 }).ToCharArray()[0];
        /// <summary>
        /// Pipe Vertical to Right: ╠ ALT + 460; ALT + 204
        /// </summary>
        public static char PipeVerticaltoRight { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x95, 0xA0 }).ToCharArray()[0];
        /// <summary>
        /// Pipe Vertical to Left: ╣ ALT + 185; ALT + 441
        /// </summary>
        public static char PipeVerticaltoLeft { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x95, 0xA3 }).ToCharArray()[0];
        /// <summary>
        /// Pipe Cross: ╬ ALT + 462; ALT + 206
        /// </summary>
        public static char PipeCross { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x95, 0xAC }).ToCharArray()[0];
        /// <summary>
        /// Pipe Horizontal Down: ╦ ALT + 459; ALT + 203
        /// </summary>
        public static char PipeHorizontalDown { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x95, 0xA6 }).ToCharArray()[0];
        /// <summary>
        /// Pipe Horizental Up: ╩ ALT + 458; ALT + 202
        /// </summary>
        public static char PipeHorizontalUp { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x95, 0xA9 }).ToCharArray()[0];
        #endregion

        #region Boxes
        /// <summary>
        /// Box Vertical: │
        /// </summary>
        public static char BoxVertical { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x94, 0x82 }).ToCharArray()[0];
        /// <summary>
        /// Box Horizontal: ─
        /// </summary>
        public static char BoxHorizontal { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x94, 0x80 }).ToCharArray()[0];
        /// <summary>
        /// Box SE: ┌
        /// </summary>
        public static char BoxSE { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x94, 0x8C }).ToCharArray()[0];
        /// <summary>
        /// Box SW: ┐
        /// </summary>
        public static char BoxSW { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x94, 0x90 }).ToCharArray()[0];
        /// <summary>
        /// Box NE: └
        /// </summary>
        public static char BoxNE { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x94, 0x94 }).ToCharArray()[0];
        /// <summary>
        /// Box NW: ┘
        /// </summary>
        public static char BoxNW { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x94, 0x98 }).ToCharArray()[0];
        /// <summary>
        /// Box Vertical to Right: ├
        /// </summary>
        public static char BoxVerticaltoRight { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x94, 0x9C }).ToCharArray()[0];
        /// <summary>
        /// Box Vertical to Left: ┤
        /// </summary>
        public static char BoxVerticaltoLeft { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x94, 0xA4 }).ToCharArray()[0];
        /// <summary>
        /// Box Cross: ┼
        /// </summary>
        public static char BoxCross { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x94, 0xBC }).ToCharArray()[0];
        /// <summary>
        /// Box Horizontal Down: ┬
        /// </summary>
        public static char BoxHorizontalDown { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x94, 0xAC }).ToCharArray()[0];
        /// <summary>
        /// Box Horizental Up: ┴
        /// </summary>
        public static char BoxHorizontalUp { get; } = Encoding.UTF8.GetString(new byte[] { 0xE2, 0x94, 0xB4 }).ToCharArray()[0];
        #endregion
    }
}
