using NUnit.Framework;


namespace BBDiese
{
    [TestFixture]
    public class ToHtmlTest
    {
        [Test]
        public void test_no_tags()
        {
            string bbcode = "foo";
            string expected = "foo";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_one_tag()
        {
            string bbcode = "[b]foo[/b]";
            string expected = "<b>foo</b>";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_one_tag_with_text_around()
        {
            string bbcode = "foo[b]bar[/b]zar";
            string expected = "foo<b>bar</b>zar";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_unclosed_tag()
        {
            string bbcode = "foo[b]bar";
            string expected = "foo[b]bar";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_html_in_text()
        {
            string bbcode = "<b>or&z</b>";
            string expected = "&lt;b&gt;or&amp;z&lt;/b&gt;";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_overlapping_tags()
        {
            string bbcode = "[b]foo[i]bar[/b]zar[/i]";
            string expected = "<b>foo<em>barzar</em></b>";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_null_bbcode()
        {
            string bbcode = null;
            string expected = "";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_empty_bbcode()
        {
            string bbcode = "";
            string expected = "";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_whitespace_bbcode()
        {
            string bbcode = "   ";
            string expected = "   ";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_tag_with_whitespaces_around()
        {
            string bbcode = "   [b]aaa[/b]  ";
            string expected = "   <b>aaa</b>  ";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_attribute_with_spaces_around_equal_sign()
        {
            string bbcode = "[url src = \"foo\"]bar[/url]";
            string expected = "<a href=\"foo\">bar</a>";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_unquoted_attribute()
        {
            string bbcode = "[url src=foo]bar[/url]";
            string expected = "<a href=\"foo\">bar</a>";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_opening_brace()
        {
            string bbcode = "[foo[b]bar[/b]";
            string expected = "[foo<b>bar</b>";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_closing_brace()
        {
            string bbcode = "f]oo[b]bar[/b]";
            string expected = "f]oo<b>bar</b>";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_attribute_without_value()
        {
            string bbcode = "[url src]bar[/url]";
            string expected = "<a href=\"bar\">bar</a>";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_stress_braces()
        {
            string bbcode = "[[[[[[b]foo[/b]]]]]]";
            string expected = "[[[[[<b>foo</b>]]]]]";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }
    }
}