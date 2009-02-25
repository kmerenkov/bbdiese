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
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_002()
        {
            string bbcode = "[b]foo[/b]";
            string expected = "<s>foo</s>";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_003()
        {
            string bbcode = "foo[b]bar[/b]zar";
            string expected = "foo<s>bar</s>zar";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_004()
        {
            string bbcode = "foo[b]bar";
            string expected = "foo[b]bar";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_005()
        {
            string bbcode = "[url]foo[/url]";
            string expected = "<a href=\"foo\">foo</a>";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_006()
        {
            string bbcode = "[url src=\"foo\"]bar[/url]";
            string expected = "<a href=\"foo\">bar</a>";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_007()
        {
            string bbcode = "<b>or&z</b>";
            string expected = "&lt;b&gt;or&amp;z&lt;/b&gt;";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_008()
        {
            string bbcode = "[b]foo[i]bar[/b]zar[/i]";
            string expected = "<s>foo<em>barzar</em></s>";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_009()
        {
            string bbcode = null;
            string expected = "";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_010()
        {
            string bbcode = "";
            string expected = "";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_011()
        {
            string bbcode = "   ";
            string expected = "   ";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_012()
        {
            string bbcode = "   [b]aaa[/b]  ";
            string expected = "   <s>aaa</s>  ";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_013()
        {
            string bbcode = "[img]http://foobar[/img]";
            string expected = "<img src=\"http://foobar\"/>";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_014()
        {
            string bbcode = "[img]  http://foobar    [/img]";
            string expected = "<img src=\"http://foobar\"/>";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_015()
        {
            string bbcode = "[img][/img]";
            string expected = "<img src=\"\"/>";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_016()
        {
            string bbcode = "[url src = \"foo\"]bar[/url]";
            string expected = "<a href=\"foo\">bar</a>";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_017()
        {
            string bbcode = "[url src=foo]bar[/url]";
            string expected = "<a href=\"foo\">bar</a>";
            string actual = Parser.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }
    }
}