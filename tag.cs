using System;
using System.Collections.Generic;
using System.Text;


namespace BBDiese
{
    public class Tag
    {
        private string name;
        private Dictionary<string, string> attributes;
        private bool closing;

        public string Name
        {
            get {
                return this.name;
            }
            set {
                this.name = value;
            }
        }

        public Dictionary<string, string> Attributes
        {
            get {
                return this.attributes;
            }
            set {
                this.attributes = value;
            }
        }

        public bool IsClosing
        {
            get {
                return this.closing;
            }
            set {
                this.closing = value;
            }
        }

        public Tag(string name, Dictionary<string, string> attributes, bool closing) {
            this.Name = name;
            this.Attributes = attributes;
            this.IsClosing = closing;
        }

        static public Tag Parse(string text)
        {
            if (text == null) {
                return null;
            }
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            string tag = "";
            bool closing = false;
            text = text.Trim(new char[] {'[', ' ', ']'});
            int space_idx = text.IndexOf(" ");
            if (space_idx == -1) {
                tag = text;
            }
            else {
                tag = text.Substring(0, space_idx);
                string rest = text.Substring(tag.Length).Trim();
                if (rest.Length > 0) {
                    attributes = Tag.ParseAttributes(rest);
                }
            }
            if (tag.StartsWith("/")) {
                tag = tag.Substring(1);
                closing = true;
            }
            return new Tag(tag, attributes, closing);
        }

        static public Dictionary<string, string> ParseAttributes(string text)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();
            if (text == null) {
                return attributes;
            }
            int pos = 0;
            while (true) {
                int eq_sign = text.IndexOf("=", pos);
                if (eq_sign == -1) {
                    break;
                }
                string attr_name = text.Substring(pos, eq_sign-pos);
                int open_quote = text.IndexOf("\"", eq_sign+1);
                if (open_quote == -1) {
                    break;
                }
                int close_quote = text.IndexOf("\"", open_quote+1);
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
}