using System;
using System.Collections.Generic;
using System.Text;


namespace BBDiese
{
    public class Tag
    {
        public string Name {get; set;}
        public Dictionary<string, string> Attributes {get; set;}
        public bool IsClosing {get; set;}
        public string Content {get;set;}

        public Tag(string name, Dictionary<string, string> attributes, bool is_closing)
        {
            this.Name = name;
            this.Attributes = attributes;
            this.IsClosing = is_closing;
        }
    }

    internal static class TagParser
    {
        private static char[] bad_attr_chars = new char[] {'"', ' ', '"'};
        private static char[] bad_tag_chars = new char[] {'[', ' ', ']'};


        static public Tag Parse(string text)
        {
            /* XXX? atm '/b' => 'b', but '/ b' => '/ b' */
            if (text == null) {
                return null;
            }
            Dictionary<string, string> attributes = null;
            string tag_name = "";
            bool closing = false;
            text = text.Trim(bad_tag_chars);
            int separator_idx = text.IndexOf(" ");
            if (separator_idx == -1) {
                separator_idx = text.IndexOf("=");
            }
            if (separator_idx == -1) {
                tag_name = text;
            }
            else {
                tag_name = text.Substring(0, separator_idx);
                string rest = text.Substring(tag_name.Length+1);
                if (rest.Length > 0) {
                    attributes = TagParser.ParseAttributes(rest);
                }
            }
            if ((tag_name.Length > 1) && (tag_name[0] == '/')) {
                tag_name = tag_name.Substring(1);
                closing = true;
            }
            if (attributes == null) {
                attributes = new Dictionary<string, string>();
            }
            return new Tag(tag_name, attributes, closing);
        }

        static public Dictionary<string, string> ParseAttributes(string text)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            if (text == null) {
                return attributes;
            }
            /* In real life, I have never seen a bb tag with few attributes */
            string[] tokens = text.Split(new char[]{'='}, 2);
            string attr_name;
            string attr_value;
            if (tokens.Length < 2) {
                attr_name = "";
                attr_value = tokens[0];
            }
            else {
                attr_name = tokens[0];
                attr_value = tokens[1];
            }
            attributes.Add(attr_name.Trim(bad_attr_chars), attr_value.Trim(bad_attr_chars));
            return attributes;
        }
    }
}