using System.Web;


namespace BBDiese
{
    public class LinkTag:BaseTagHandler
    {
        private string attributes;

        public LinkTag():this(null) {}

        public LinkTag(string attributes) {
            this.attributes = attributes ?? "";
        }

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
            string prefix;
            if (this.attributes.Length > 0) {
                prefix = "<a href=\"" + src + "\" " + this.attributes + ">";
            }
            else {
                prefix = "<a href=\"" + src + "\">";
            }
            return prefix + content + "</a>";
        }
    }
}