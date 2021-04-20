using ConsoleTableExtNet5.Enums;
using NUnit.Framework;

namespace ConsoleTableExtNet5.Tests
{
    [TestFixture]
    public class DefaultFormats
    {
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
    }
}
