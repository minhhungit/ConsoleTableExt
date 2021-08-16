using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace ConsoleTableExt.Tests
{
    public class SampleData
    {
        public static DataTable SampleTableData
        {
            get
            {
                DataTable table = new DataTable();
                table.Columns.Add("Name", typeof(string));
                table.Columns.Add("Position", typeof(string));
                table.Columns.Add("Office", typeof(string));
                table.Columns.Add("Age", typeof(int));
                table.Columns.Add("Start Date", typeof(DateTime));

                table.Rows.Add("Airi Satou", "Accountant", "Tokyo", 33, new DateTime(2017, 05, 09));
                table.Rows.Add("Angelica Ramos", "Chief Executive Officer (CEO)", "New York", 47, new DateTime(2017, 01, 12));
                table.Rows.Add("Ashton Cox", "Junior Technical Author", "London", 46, new DateTime(2017, 04, 02));
                table.Rows.Add("Bradley Greer", "Software Engineer", "San Francisco", 28, new DateTime(2017, 11, 15));

                return table;
            }
        }

        public static List<List<object>> SampleShortListData = new List<List<object>>
        {
            new List<object>{ ""},
            new List<object>{ ""},
            new List<object>{ ""},
            new List<object>{ ""},
        };

        public static List<List<object>> SampleListData = new List<List<object>>
        {
            new List<object>{ "Sakura Yamamoto", "Support Engineer", "London", 46},
            new List<object>{ "Serge Baldwin", "Data Coordinator", "San Francisco", 28, "something else" },
            new List<object>{ "Shad Decker", "Regional Director", "Edinburgh"},
        };

        public static List<Employee> SampleEmployeesList = new List<Employee>
        {
            new Employee("Airi Satou", "Accountant", "Tokyo", 33, new DateTime(2017, 05, 09)),
            new Employee("Angelica Ramos", "Chief Executive Officer (CEO)", "New York", 47, new DateTime(2017, 01, 12)),
            new Employee("Ashton Cox", "Junior Technical Author", "London", 46, new DateTime(2017, 04, 02)),
            new Employee("Bradley Greer", "Software Engineer", "San Francisco", 28, new DateTime(2017, 11, 15))
        };

        public static List<object> SampleListWithUtf8Characters = new List<object>() {
                new { Id="中午午午午c", Name="tab其它语言test", Host="127.0.0.1", Port=80, status ="success" } ,
                new { Id="a中午b", Name="London语a言aa它xx", Host="127.0.0.1", Port=80, status ="success" },
                new { Id="Airi Satou", Name="Accountant", Host="127.0.0.1", Port=80, status ="Tokyo" },
                new { Id="Ashton Cox", Name="Junior Technical Author", Host="127.0.0.1", Port=80, status ="success" }
            };

        public class Employee
        {
            public Employee(string name, string position, string office, int age, DateTime startDate)
            {
                Name = name;
                Position = position;
                Office = office;
                Age = age;
                StartDate = startDate;
            }

            [Description("N - A - M - E")]
            public string Name { get; set; }
            public string Position { get; set; }
            public string Office { get; set; }
            public int Age { get; set; }
            public DateTime StartDate { get; set; }
        }
    }
}
