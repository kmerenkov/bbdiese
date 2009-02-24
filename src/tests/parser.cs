using NUnit.Framework;


namespace BBDiese
{
    [TestFixture]
    public class ToHtmlTest
    {
        [Test]
        public void test_001()
        {
            string bbcode = "foo";
            string expected = "foo";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void test_002()
        {
            string bbcode = "[b]foo[/b]";
            string expected = "<s>foo</s>";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void test_003()
        {
            string bbcode = "foo[b]bar[/b]zar";
            string expected = "foo<s>bar</s>zar";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void test_004()
        {
            string bbcode = "foo[b]bar";
            string expected = "foo[b]bar";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void test_005()
        {
            string bbcode = "[url]foo[/url]";
            string expected = "<a href=\"foo\">foo</a>";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void test_006()
        {
            string bbcode = "[url src=\"foo\"]bar[/url]";
            string expected = "<a href=\"foo\">bar</a>";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void test_007()
        {
            string bbcode = "<b>or&z</b>";
            string expected = "&lt;b&gt;or&amp;z&lt;/b&gt;";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(actual, expected);
        }
    }
}