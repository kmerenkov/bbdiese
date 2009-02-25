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

        [Test]
        public void test_002()
        {
            SimpleTag st = new SimpleTag("x");
            string bbcode = "[z]foo[/z]";
            string expected = "<x>foo</x>";
            string actual = Parser.ToHtml(bbcode,
                                          new Dictionary<string, BaseTagHandler> {
                                              {"z", st}
                                          });
            Assert.AreEqual(expected, actual);
        }
    }

    [TestFixture]
    public class LinkTagTests
    {
        [Test]
        public void test_001()
        {
            LinkTag lt = new LinkTag();
            string bbcode = "[url]http://click me[/url]";
            string expected = "<a href=\"http://click%20me\">http://click me</a>";
            string actual = Parser.ToHtml(bbcode,
                                          new Dictionary<string, BaseTagHandler> {
                                              {"url", lt}
                                          });
            Assert.AreEqual(expected, actual);
        }
    }
}