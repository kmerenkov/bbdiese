using System.Collections.Generic;
using NUnit.Framework;


namespace BBDiese
{
    [TestFixture]
    public class SimpleTagTests
    {
        [Test]
        public void test_normal()
        {
            SimpleTag st = new SimpleTag("s");
            string bbcode = "[b]foo[/b]";
            string expected = "<s>foo</s>";
            string actual = BBCode.ToHtml(bbcode,
                                          new Dictionary<string, BaseTagHandler> {
                                              {"b", st}
                                          });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_normal2()
        {
            SimpleTag st = new SimpleTag("x");
            string bbcode = "[z]foo[/z]";
            string expected = "<x>foo</x>";
            string actual = BBCode.ToHtml(bbcode,
                                          new Dictionary<string, BaseTagHandler> {
                                              {"z", st}
                                          });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_nested()
        {
            SimpleTag st = new SimpleTag("x", new SimpleTag("y"));
            string bbcode = "[z]foo[/z]";
            string expected = "<x><y>foo</y></x>";
            string actual = BBCode.ToHtml(bbcode,
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
        public void test_normal()
        {
            LinkTag lt = new LinkTag();
            string bbcode = "[url]http://click me[/url]";
            string expected = "<a href=\"http://click%20me\">http://click me</a>";
            string actual = BBCode.ToHtml(bbcode,
                                          new Dictionary<string, BaseTagHandler> {
                                              {"url", lt}
                                          });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_space_in_src_value()
        {
            LinkTag lt = new LinkTag();
            string bbcode = "[url src=\"http://some url\"]Click me[/url]";
            string expected = "<a href=\"http://some%20url\">Click me</a>";
            string actual = BBCode.ToHtml(bbcode,
                                          new Dictionary<string, BaseTagHandler> {
                                              {"url", lt}
                                          });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_redundant_attribute()
        {
            LinkTag lt = new LinkTag();
            string bbcode = "[url src=\"http://some.url\" xxx=yyy]Click me[/url]";
            string expected = "<a href=\"http://some.url\">Click me</a>";
            string actual = BBCode.ToHtml(bbcode,
                                          new Dictionary<string, BaseTagHandler> {
                                              {"url", lt}
                                          });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_empty_body()
        {
            LinkTag lt = new LinkTag();
            string bbcode = "[url src=somewhere][/url]";
            string expected = "<a href=\"somewhere\">somewhere</a>";
            string actual = BBCode.ToHtml(bbcode,
                                          new Dictionary<string, BaseTagHandler> {
                                              {"url", lt}
                                          });
            Assert.AreEqual(expected, actual);
        }
    }

    [TestFixture]
    public class ImageTagTests
    {
        [Test]
        public void test_normal()
        {
            ImageTag it = new ImageTag();
            string bbcode = "[img]http://some.url[/img]";
            string expected = "<img src=\"http://some.url\">";
            string actual = BBCode.ToHtml(bbcode,
                                          new Dictionary<string, BaseTagHandler> {
                                              {"img", it}
                                          });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_empty_body()
        {
            ImageTag it = new ImageTag();
            string bbcode = "[img][/img]";
            string expected = "";
            string actual = BBCode.ToHtml(bbcode,
                                          new Dictionary<string, BaseTagHandler> {
                                              {"img", it}
                                          });
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void test_body_with_whitespaces_around()
        {
            ImageTag it = new ImageTag();
            string bbcode = "[img]  http://foobar    [/img]";
            string expected = "<img src=\"http://foobar\">";
            string actual = BBCode.ToHtml(bbcode,
                                          new Dictionary<string, BaseTagHandler> {
                                              {"img", it}
                                          });
            Assert.AreEqual(expected, actual);
        }


    }
}