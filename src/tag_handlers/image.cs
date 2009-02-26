using System.Web;


namespace BBDiese
{
    public class ImageTag:BaseTagHandler
    {
        private string attributes;
        public ImageTag():this(null) {}

        public ImageTag(string attributes) {
            this.attributes = attributes ?? "";
        }

        public override string Process(Tag tag)
        {
            if (tag == null) return "";

            string src = tag.Content.Trim();
            if (src.Length == 0) {
                return src;
            }
            src = HttpUtility.UrlPathEncode(src);
            if (this.attributes.Length > 0) {
                return "<img src=\"" + src + "\" " + this.attributes + ">";
            }
            else {
                return "<img src=\"" + src + "\">";
            }
        }
    }
}