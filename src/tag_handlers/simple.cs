namespace BBDiese
{
    public class SimpleTag:BaseTagHandler
    {
        public string HtmlTag {get;set;}
        private string attributes;
        private BaseTagHandler nested_tag;

        public SimpleTag(string html_tag):this(html_tag, "", null)
        {}

        public SimpleTag(string html_tag, string attributes):this(html_tag, attributes, null)
        {}

        public SimpleTag(string html_tag, SimpleTag nested_tag):this(html_tag, "", nested_tag)
        {}

        public SimpleTag(string html_tag, string attributes, SimpleTag nested_tag) {
            this.HtmlTag = html_tag;
            this.attributes = attributes ?? "";
            if ((nested_tag != null) && (nested_tag.HtmlTag != null) && (nested_tag.HtmlTag.Length > 0)) {
                this.nested_tag = nested_tag;
            }
        }

        public override string Process(Tag tag)
        {
            if (tag == null) return "";
            string prefix;
            if (this.attributes.Length > 0) {
                prefix = "<" + this.HtmlTag + " " + this.attributes + ">";
            }
            else {
                prefix = "<" + this.HtmlTag + ">";
            }
            if (this.nested_tag != null) {
                return prefix + this.nested_tag.Process(tag) + "</" + this.HtmlTag + ">";
            }
            else {
                return prefix + tag.Content + "</" + this.HtmlTag + ">";
            }
        }
    }
}