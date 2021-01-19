using System;
using System.Linq;
using System.Diagnostics;
using System.Data;
using System.Collections.Generic;
using NUnit.Framework;
using ConsoleTableExt;
using System.Text;

namespace ConsolteTableExtTests
{
    public class CTE_Tests
    {
        private DataTable dt;
        [SetUp]
        public void SetUp()
        {
            dt = new DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Position", typeof(string));
            dt.Columns.Add("Office", typeof(string));
            dt.Columns.Add("Age", typeof(int));
            dt.Columns.Add("Start Date", typeof(DateTime));

            DataRow r = dt.NewRow();
            r[0] = "1";
            r[1] = "Airi Satou";
            r[2] = "Accountant";
            r[3] = "Tokyo";
            r[4] = "33";
            r[5] = "2017-09-05 12:00:00";
            dt.Rows.Add(r);

            r = dt.NewRow();
            r[0] = "2";
            r[1] = "Angelica Ramos";
            r[2] = "Chief Executive Officer (CEO)";
            r[3] = "New York";
            r[4] = "47";
            r[5] = "2017-12-01 12:00:00";
            dt.Rows.Add(r);

            r = dt.NewRow();
            r[0] = "3";
            r[1] = "Ashton Cox";
            r[2] = "Junior Technical Author";
            r[3] = "London";
            r[4] = "46";
            r[5] = "2017-02-04 12:00:00";
            dt.Rows.Add(r);

            r = dt.NewRow();
            r[0] = "4";
            r[1] = "Bradley Greer";
            r[2] = "Software Engineer";
            r[3] = "San Francisco";
            r[4] = "28";
            r[5] = "2017-11-15 12:00:00";
            dt.Rows.Add(r);
        }

        [Test(Description = "Build from DataTable")]
        [TestCase(Category = "From DataTable", Description = "Standard")]
        public void BuildFromDataTable()
        {
            StringBuilder b = ConsoleTableBuilder.From(dt).Export();
            string[] strSplit = b.ToString().Split('\n');
            Assert.AreEqual(12, strSplit.Length);
            Assert.AreEqual(99, strSplit[0].Trim().Length);
            Assert.AreEqual(new string('-',99), strSplit[0].Trim());
        }

        [Test(Description = "Minimal")]
        [TestCase(Category = "From DataTable", Description = "Minimal")]
        public void Minimal()
        {
            StringBuilder b = ConsoleTableBuilder.From(dt).WithFormat(ConsoleTableBuilderFormat.Minimal).Export();
            string[] strSplit = b.ToString().Split('\n');
            Assert.AreEqual(7, strSplit.Length);
            Assert.IsFalse(strSplit[0].Contains('-'));
            Assert.AreEqual(new string('-', 90), strSplit[1].Trim());
        }

        [Test(Description = "Frame Style -> DoublePipe")]
        [TestCase(Category = "From DataTable", Description = "DoublePipe")]
        public void FrameStyle_DoublePipe()
        {
            StringBuilder b = ConsoleTableBuilder.From(dt).WithOptions(new ConsoleTableBuilderOption() { FrameStyle = ConsoleTableBuilderOption.FrameStyles.DoublePipe }).Export();
            string[] strSplit = b.ToString().Split('\n');
            Assert.AreEqual(12, strSplit.Length);
            Assert.AreEqual(99, strSplit[0].Trim().Length);
            Assert.AreEqual(FrameChars.PipeSE + new string(FrameChars.PipeHorizontal, 97) + FrameChars.PipeSW, strSplit[0].Trim());
            Assert.AreEqual(FrameChars.PipeNE + new string(FrameChars.PipeHorizontal, 97) + FrameChars.PipeNW, strSplit[strSplit.Length-2].Trim());
        }

        [Test(Description = "Frame Style -> Pipe")]
        [TestCase(Category = "From DataTable", Description = "Pipe")]
        public void FrameStyle_Pipe()
        {
            StringBuilder b = ConsoleTableBuilder.From(dt).WithOptions(new ConsoleTableBuilderOption() { FrameStyle = ConsoleTableBuilderOption.FrameStyles.Pipe }).Export();
            string[] strSplit = b.ToString().Split('\n');
            Assert.AreEqual(12, strSplit.Length);
            Assert.AreEqual(99, strSplit[0].Trim().Length);
            Assert.AreEqual(FrameChars.BoxSE + new string(FrameChars.BoxHorizontal, 97) + FrameChars.BoxSW, strSplit[0].Trim());
            Assert.AreEqual(FrameChars.BoxNE + new string(FrameChars.BoxHorizontal, 97) + FrameChars.BoxNW, strSplit[strSplit.Length - 2].Trim());
        }

        [Test(Description = "Build from DataTable")]
        [TestCase(Category = "No Row Table", Description = "Standard")]
        public void BuildFromEmptzDataTable()
        {
            StringBuilder b = ConsoleTableBuilder.From(new DataTable()).WithOptions(new ConsoleTableBuilderOption() { FrameStyle = ConsoleTableBuilderOption.FrameStyles.Pipe }).Export();

            string str = b.ToString();
            Debug.Print(str);
            string[] strSplit = b.ToString().Split('\n');
            Assert.AreEqual(12, strSplit.Length);
            Assert.AreEqual(99, strSplit[0].Trim().Length);
            Assert.AreEqual(FrameChars.BoxSE + new string(FrameChars.BoxHorizontal, 97) + FrameChars.BoxSW, strSplit[0].Trim());
            Assert.AreEqual(FrameChars.BoxNE + new string(FrameChars.BoxHorizontal, 97) + FrameChars.BoxNW, strSplit[strSplit.Length - 2].Trim());
        }

        [TearDown]
        public void TearDown()
        {
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }
}