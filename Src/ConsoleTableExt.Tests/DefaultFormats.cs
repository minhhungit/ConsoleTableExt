using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Linq;

namespace ConsoleTableExt.Tests
{
    [TestFixture]
    public  class DefaultFormats
    {
        public DefaultFormats()
        {
            //set CultureInfo default en-US
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
        }

        [Test]
        public void DefaultFormatWithDataTable()
        {
            var strBuilder =
                ConsoleTableBuilder
                .From(SampleData.SampleTableData)
                .Export();

            TestContext.Out.WriteLine(strBuilder.ToString());
            Assert.IsTrue(strBuilder.ToString() == @"
-------------------------------------------------------------------------------------------------
| Name           | Position                      | Office        | Age | Start Date             |
-------------------------------------------------------------------------------------------------
| Airi Satou     | Accountant                    | Tokyo         | 33  | 5/9/2017 12:00:00 AM   |
-------------------------------------------------------------------------------------------------
| Angelica Ramos | Chief Executive Officer (CEO) | New York      | 47  | 1/12/2017 12:00:00 AM  |
-------------------------------------------------------------------------------------------------
| Ashton Cox     | Junior Technical Author       | London        | 46  | 4/2/2017 12:00:00 AM   |
-------------------------------------------------------------------------------------------------
| Bradley Greer  | Software Engineer             | San Francisco | 28  | 11/15/2017 12:00:00 AM |
-------------------------------------------------------------------------------------------------
".TrimStart());

            var lines = strBuilder.ToString().Split('\n');
            Assert.IsTrue(lines.Length == 12);
        }

        [Test]
        public void MinimalFormatWithDataTable()
        {
            var strBuilder =
                ConsoleTableBuilder
                .From(SampleData.SampleTableData)
                .WithFormat(ConsoleTableBuilderFormat.Minimal)
                .Export();

            Assert.IsTrue(strBuilder.ToString() == @"Name           Position                      Office        Age Start Date             
--------------------------------------------------------------------------------------
Airi Satou     Accountant                    Tokyo         33  5/9/2017 12:00:00 AM   
Angelica Ramos Chief Executive Officer (CEO) New York      47  1/12/2017 12:00:00 AM  
Ashton Cox     Junior Technical Author       London        46  4/2/2017 12:00:00 AM   
Bradley Greer  Software Engineer             San Francisco 28  11/15/2017 12:00:00 AM 
");

            var lines = strBuilder.ToString().Split('\n');
            Assert.IsTrue(lines.Length == 7);
        }

        [Test]
        public void AlternativeFormatWithDataTable()
        {
            var strBuilder =
                ConsoleTableBuilder
                .From(SampleData.SampleTableData)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .Export();

            Assert.IsTrue(strBuilder.ToString() == @"
+----------------+-------------------------------+---------------+-----+------------------------+
| Name           | Position                      | Office        | Age | Start Date             |
+----------------+-------------------------------+---------------+-----+------------------------+
| Airi Satou     | Accountant                    | Tokyo         | 33  | 5/9/2017 12:00:00 AM   |
+----------------+-------------------------------+---------------+-----+------------------------+
| Angelica Ramos | Chief Executive Officer (CEO) | New York      | 47  | 1/12/2017 12:00:00 AM  |
+----------------+-------------------------------+---------------+-----+------------------------+
| Ashton Cox     | Junior Technical Author       | London        | 46  | 4/2/2017 12:00:00 AM   |
+----------------+-------------------------------+---------------+-----+------------------------+
| Bradley Greer  | Software Engineer             | San Francisco | 28  | 11/15/2017 12:00:00 AM |
+----------------+-------------------------------+---------------+-----+------------------------+
".TrimStart());

            var lines = strBuilder.ToString().Split('\n');
            Assert.IsTrue(lines.Length == 12);
        }

        [Test]
        public void MarkDownFormatWithDataTable()
        {
            var strBuilder =
                ConsoleTableBuilder
                .From(SampleData.SampleTableData)
                .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                .Export();

            Assert.IsTrue(strBuilder.ToString() == @"
| Name           | Position                      | Office        | Age | Start Date             |
|----------------|-------------------------------|---------------|-----|------------------------|
| Airi Satou     | Accountant                    | Tokyo         | 33  | 5/9/2017 12:00:00 AM   |
| Angelica Ramos | Chief Executive Officer (CEO) | New York      | 47  | 1/12/2017 12:00:00 AM  |
| Ashton Cox     | Junior Technical Author       | London        | 46  | 4/2/2017 12:00:00 AM   |
| Bradley Greer  | Software Engineer             | San Francisco | 28  | 11/15/2017 12:00:00 AM |
".TrimStart());

            var lines = strBuilder.ToString().Split('\n');
            Assert.IsTrue(lines.Length == 7);
        }

        [Test]
        public void AlternativeFormatWithUtf8CharactersDataTable()
        {
            var strBuilder =
               ConsoleTableBuilder
               .From<dynamic>(SampleData.SampleListWithUtf8Characters)
               .WithFormat(ConsoleTableBuilderFormat.Alternative)
               .Export();

            Assert.IsTrue(strBuilder.ToString() == @"
+-----+-----------------+-----------+------+---------+
| Id  | Name            | Host      | Port | status  |
+-----+-----------------+-----------+------+---------+
| xxx | tab其它语言test | 127.0.0.1 | 80   | success |
+-----+-----------------+-----------+------+---------+
".TrimStart());

            var lines = strBuilder.ToString().Split('\n');
            Assert.IsTrue(lines.Length == 6);
        }
    }
}
