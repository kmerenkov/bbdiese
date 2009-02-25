using System;
using System.Collections.Generic;
using System.Text;


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
        static public Tag Parse(string text)
        {
            /* XXX? atm '/b' => 'b', but '/ b' => '/ b' */
            if (text == null) {
                return null;
            }
            Dictionary<string, string> attributes = null;
            string tag_name = "";
            bool closing = false;
            text = text.Trim(new char[] {'[', ' ', ']'});
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
            int pos = 0;
            while (true) {
                int eq_sign = text.IndexOf('=', pos);
                if (eq_sign == -1) {
                    break;
                }
                string attr_name = text.Substring(pos, eq_sign-pos).Trim();
                int open_quote = text.IndexOf('\"', eq_sign+1);
                if (open_quote == -1) {
                    break;
                }
                int close_quote = text.IndexOf('\"', open_quote+1);
                if (close_quote == -1) {
                    break;
                }
                string attr_value = text.Substring(open_quote+1, close_quote-open_quote-pos-1);
                attributes.Add(attr_name, attr_value);
                pos = close_quote+1;
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