using System.Collections.Generic;
using ConsoleTableExtNet5.Enums;

namespace ConsoleTableExtNet5
{
    public class CharMapDefinition
    {
        public static Dictionary<CharMapPositions, char> FramePipDefinition = new Dictionary<CharMapPositions, char>
        {
            { CharMapPositions.TopLeft, '┌' },
            { CharMapPositions.TopCenter, '┬' },
            { CharMapPositions.TopRight, '┐' },
            { CharMapPositions.MiddleLeft, '├' },
            { CharMapPositions.MiddleCenter, '┼' },
            { CharMapPositions.MiddleRight, '┤' },
            { CharMapPositions.BottomLeft, '└' },
            { CharMapPositions.BottomCenter, '┴' },
            { CharMapPositions.BottomRight, '┘' },
            { CharMapPositions.BorderLeft, '│' },
            { CharMapPositions.BorderRight, '│' },
            { CharMapPositions.BorderTop, '─' },
            { CharMapPositions.BorderBottom, '─' },
            { CharMapPositions.DividerY, '│' },
            { CharMapPositions.DividerX, '─' },
        };

        public static Dictionary<CharMapPositions, char> FrameDoublePipDefinition = new Dictionary<CharMapPositions, char>
        {
            { CharMapPositions.TopLeft, '╔' },
            { CharMapPositions.TopCenter, '╤' },
            { CharMapPositions.TopRight, '╗' },
            { CharMapPositions.MiddleLeft, '╟' },
            { CharMapPositions.MiddleCenter, '┼' },
            { CharMapPositions.MiddleRight, '╢' },
            { CharMapPositions.BottomLeft, '╚' },
            { CharMapPositions.BottomCenter, '╧' },
            { CharMapPositions.BottomRight, '╝' },
            { CharMapPositions.BorderLeft, '║' },
            { CharMapPositions.BorderRight, '║' },
            { CharMapPositions.BorderTop, '═' },
            { CharMapPositions.BorderBottom, '═' },
            { CharMapPositions.DividerY, '│' },
            { CharMapPositions.DividerX, '─' }
        };
    }
}