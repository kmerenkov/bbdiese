using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


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
        private static Regex re_attr = new Regex(@"(?<n>[^=]*)\s*=\s*(?<v>[^\ ]*)[\ ]?",
                                                 RegexOptions.ExplicitCapture |
                                                   RegexOptions.Compiled);

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
            int space_idx = text.IndexOf(" ");
            if (space_idx == -1) {
                tag_name = text;
            }
            else {
                tag_name = text.Substring(0, space_idx);
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
            /* dirtiness */
            Match m = re_attr.Match(text);
            for (int i = 0; i < m.Groups["n"].Captures.Count; i++) {
                string key = m.Groups["n"].Captures[i].Value.Trim(bad_attr_chars).ToLower();
                string value = m.Groups["v"].Captures[i].Value.Trim(bad_attr_chars).ToLower();
                attributes.Add(key, value);
            }
            return attributes;
        }
    }
}