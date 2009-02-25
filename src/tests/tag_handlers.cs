using System.Collections.Generic;
using NUnit.Framework;


namespace BBDiese
{
    [TestFixture]
    public class SimpleTagTests
    {
        [Test]
        public void test_001()
        {
            SimpleTag st = new SimpleTag("s");
            string bbcode = "[b]foo[/b]";
            string expected = "<s>foo</s>";
            string actual = Parser.ToHtml(bbcode,
                                          new Dictionary<string, BaseTagHandler> {
                                              {"b", st}
                                          });
            Assert.AreEqual(expected, actual);
        }
    }
}