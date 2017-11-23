using System;

namespace ConsoleTableExt
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConsoleTableColumnAttributes : Attribute
    {
        public string Name;
        public int Order;

        public ConsoleTableColumnAttributes(int order)
        {
            this.Order = order;
        }

        public ConsoleTableColumnAttributes(int order, string name)
        {
            this.Order = order;
            this.Name = name;
        }
    }
}