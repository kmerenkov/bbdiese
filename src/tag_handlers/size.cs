namespace BBDiese
{
    public class SizeTag:BaseTagHandler
    {
        public SizeTag() {}

        public override string Process(Tag tag)
        {
            if (tag == null) return "";
            float size = 0.0f;
            bool do_not_process = false;
            if (tag.Attributes.ContainsKey("")) {
                try {
                    size = float.Parse(tag.Attributes[""]);
                }
                catch (System.FormatException) {
                    do_not_process = true;
                }
            }
            else {
                do_not_process = true;
            }
            if (size == 0.0) {
                do_not_process = true;
            }
            if (do_not_process == true) {
                return tag.Content;
            }
            else {
                return "<span style=\"font-size:" + size + "pt;\">" + tag.Content + "</span>";
            }
        }
    }
}