using System.Collections.Generic;

namespace ConsoleTableExt
{
    public class CharMapDefinations
    {
        public static Dictionary<CharMapPositions, char> FramePipDefination = new Dictionary<CharMapPositions, char>
        {
            { CharMapPositions.A1, '┌' },
            { CharMapPositions.B1, '┬' },
            { CharMapPositions.C1, '┐' },
            { CharMapPositions.A2, '├' },
            { CharMapPositions.B2, '┼' },
            { CharMapPositions.C2, '┤' },
            { CharMapPositions.A3, '└' },
            { CharMapPositions.B3, '┴' },
            { CharMapPositions.C3, '┘' },
            { CharMapPositions.BorderY, '─' },
            { CharMapPositions.BorderX, '│' },
            { CharMapPositions.DividerY, '─' },
            { CharMapPositions.DividerX, '│' },
        };

        public static Dictionary<CharMapPositions, char> FrameDoublePipDefination = new Dictionary<CharMapPositions, char>
        {
            { CharMapPositions.A1, '╔' },
            { CharMapPositions.B1, '╤' },
            { CharMapPositions.C1, '╗' },
            { CharMapPositions.A2, '╟' },
            { CharMapPositions.B2, '┼' },
            { CharMapPositions.C2, '╢' },
            { CharMapPositions.A3, '╚' },
            { CharMapPositions.B3, '╧' },
            { CharMapPositions.C3, '╝' },
            { CharMapPositions.BorderY, '═' },
            { CharMapPositions.BorderX, '║' },
            { CharMapPositions.DividerY, '─' },
            { CharMapPositions.DividerX, '│' }
        };
    }
}
