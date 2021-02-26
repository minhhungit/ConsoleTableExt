using System.Collections.Generic;

namespace ConsoleTableExt
{
    public class MapCharItem
    {
        public char Value { get; private set; }
        public string Start { get; private set; }
        public string End { get; private set; }

        public MapCharItem(char value)
        {
            Value = value;
        }

        public MapCharItem(char value, string start, string end)
        {
            Value = value;
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}", Start, Value, End);
        }
    }

    public class CharMapDefinition
    {
        public static Dictionary<CharMapPositions, MapCharItem> FramePipDefinition = new Dictionary<CharMapPositions, MapCharItem>
        {
            { CharMapPositions.TopLeft, new MapCharItem('┌', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.TopCenter, new MapCharItem('┬', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.TopRight, new MapCharItem('┐', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.MiddleLeft, new MapCharItem('├', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.MiddleCenter, new MapCharItem('┼', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.MiddleRight, new MapCharItem('┤', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.BottomLeft, new MapCharItem('└', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.BottomCenter, new MapCharItem('┴', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.BottomRight, new MapCharItem('┘', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.BorderLeft, new MapCharItem('│', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.BorderRight, new MapCharItem('│', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.BorderTop, new MapCharItem('─', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.BorderBottom, new MapCharItem('─', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.DividerY, new MapCharItem('│', "\u001b[31m", "\u001b[0m") },
            { CharMapPositions.DividerX, new MapCharItem('─', "\u001b[31m", "\u001b[0m") },
        };

        public static Dictionary<CharMapPositions, MapCharItem> FrameDoublePipDefinition = new Dictionary<CharMapPositions, MapCharItem>
        {
            { CharMapPositions.TopLeft, new MapCharItem('╔') },
            { CharMapPositions.TopCenter, new MapCharItem('╤') },
            { CharMapPositions.TopRight, new MapCharItem('╗') },
            { CharMapPositions.MiddleLeft, new MapCharItem('╟') },
            { CharMapPositions.MiddleCenter, new MapCharItem('┼') },
            { CharMapPositions.MiddleRight, new MapCharItem('╢') },
            { CharMapPositions.BottomLeft, new MapCharItem('╚') },
            { CharMapPositions.BottomCenter, new MapCharItem('╧') },
            { CharMapPositions.BottomRight, new MapCharItem('╝') },
            { CharMapPositions.BorderLeft, new MapCharItem('║') },
            { CharMapPositions.BorderRight, new MapCharItem('║') },
            { CharMapPositions.BorderTop, new MapCharItem('═') },
            { CharMapPositions.BorderBottom, new MapCharItem('═') },
            { CharMapPositions.DividerY, new MapCharItem('│') },
            { CharMapPositions.DividerX, new MapCharItem('─') }
        };
    }
}
