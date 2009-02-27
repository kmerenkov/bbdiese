using System.Text;


namespace BBDiese
{
    public class QuoteTag:BaseTagHandler
    {
        public QuoteTag() {}

        public override string Process(Tag tag)
        {
            if (tag == null) return "";
            string quoted_by = "";
            if (tag.Attributes.ContainsKey("")) {
                quoted_by = tag.Attributes[""];
            }
            StringBuilder output = new StringBuilder();
            if (quoted_by.Length > 0) {
                output.Append("<p>" + quoted_by + " wrote:</p>");
            }
            output.Append("<blockquote><p>" + tag.Content + "</p></blockquote>");
            return output.ToString();
        }
    }
}