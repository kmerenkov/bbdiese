namespace BBDiese
{
    public class ColorTag:BaseTagHandler
    {
        public ColorTag() {}

        public override string Process(Tag tag)
        {
            if (tag == null) return "";
            string color;
            /* in color tag, color itself is a tag and an attribute name! */
            if (tag.Attributes.ContainsKey("")) {
                color = tag.Attributes[""];
            }
            else {
                return tag.Content;
            }
            return "<span style=\"color:" + color + ";\">" + tag.Content + "</span>";
        }
    }
}