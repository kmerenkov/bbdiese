using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace BBDiese
{
    sealed internal class Tag
    {
        public string Name {get; set;}
        public Dictionary<string, string> Attributes {get; set;}
        public bool IsClosing {get; set;}

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

    internal abstract class BaseTag
    {
        protected BaseTag() {}

        public abstract string Process(Token token);
    }

    sealed internal class RootTag:BaseTag
    {
        public RootTag() {}

        public override string Process(Token token)
        {
            return token == null ? "" : token.Content;
        }
    }

    sealed internal class SimpleTag:BaseTag
    {
        private string html_tag;

        public SimpleTag(string html_tag) {
            this.html_tag = html_tag;
        }

        public override string Process(Token token)
        {
            if (token == null) return "";
            if (token.Tag == null) return "";
            return "<" + this.html_tag + ">" + token.Content + "</" + this.html_tag + ">";
        }
    }

    sealed internal class LinkTag:BaseTag
    {
        public LinkTag() {}

        public override string Process(Token token)
        {
            if (token == null) return "";
            if (token.Tag == null) return "";

            string content = token.Content.Trim();
            string src = "";

            /* no content and no src */
            if (content.Length == 0) {
                if (!token.Tag.Attributes.ContainsKey("src")) {
                    return "";
                }
            }
            /* no src but content */
            if (!token.Tag.Attributes.ContainsKey("src")) {
                src = content;
            }
            /* no content but src */
            else {
                if (content.Length == 0) content = src;
                src = token.Tag.Attributes["src"];
            }
            return "<a href=\"" + src + "\">" + content + "</a>";
        }
    }

    sealed internal class ImageTag:BaseTag
    {
        public ImageTag() {}

        public override string Process(Token token)
        {
            if (token == null) return "";
            if (token.Tag == null) return "";

            string src = token.Content.Trim();
            return "<img src=\"" + src + "\"/>";
        }
    }
}