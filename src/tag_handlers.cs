using System.Web;


namespace BBDiese
{

    public abstract class BaseTagHandler
    {
        protected BaseTagHandler() {}

        public abstract string Process(Tag tag);
    }

    public class RootTag:BaseTagHandler
    {
        public RootTag() {}

        public override string Process(Tag tag)
        {
            return tag == null ? "" : tag.Content;
        }
    }

    public class SimpleTag:BaseTagHandler
    {
        private string html_tag;

        public SimpleTag(string html_tag) {
            this.html_tag = html_tag;
        }

        public override string Process(Tag tag)
        {
            if (tag == null) return "";
            return "<" + this.html_tag + ">" + tag.Content + "</" + this.html_tag + ">";
        }
    }

    public class LinkTag:BaseTagHandler
    {
        public LinkTag() {}

        public override string Process(Tag tag)
        {
            if (tag == null) return "";

            string content = tag.Content.Trim();
            string src = "";

            /* no content and no src */
            if ((content.Length == 0) && (!tag.Attributes.ContainsKey("src"))) {
                return "";
            }
            /* no src but content */
            if (!tag.Attributes.ContainsKey("src")) {
                src = content;
            }
            /* no content but src */
            else {
                src = tag.Attributes["src"];
                if (content.Length == 0) {
                    content = src;
                }
            }
            src = HttpUtility.UrlPathEncode(src);
            return "<a href=\"" + src + "\">" + content + "</a>";
        }
    }

    public class ImageTag:BaseTagHandler
    {
        public ImageTag() {}

        public override string Process(Tag tag)
        {
            if (tag == null) return "";

            string src = HttpUtility.UrlPathEncode( tag.Content.Trim() );
            return "<img src=\"" + src + "\">";
        }
    }
}