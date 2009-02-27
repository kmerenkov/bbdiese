using NUnit.Framework;


namespace BBDiese
{
    [TestFixture]
    public class SmileysTests
    {
        [Test]
        public void test_normal()
        {
            string bbcode = "[:-)]";
            string expected = "<img src=\"smile.gif\">";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_surrounging_text()
        {
            string bbcode = "foo[:-)]bar";
            string expected = "foo<img src=\"smile.gif\">bar";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_inside_tag()
        {
            string bbcode = "[b]foo[:-)]bar[/b]";
            string expected = "<b>foo<img src=\"smile.gif\">bar</b>";
            string actual = BBCode.ToHtml(bbcode);
            Assert.AreEqual(expected, actual);
        }
    }
}