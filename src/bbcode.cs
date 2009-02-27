using System.Collections.Generic;
using System.Text;
using System.Web;


namespace BBDiese
{
    sealed internal class Map
    {
        public string Text {get;set;}
        public string Replacement {get;set;}

        public Map(string text, string replacement) {
            this.Text = text;
            this.Replacement = replacement;
        }
    }

    public static class BBCode
    {
        private static List<Map> smileys = new List<Map> {
            new Map("[:-)]", "<img src=\"smile.gif\">")
        };

        private static string ProcessSmileys(string text)
        {
            StringBuilder output = new StringBuilder(text);
            foreach (Map m in smileys) {
                output.Replace(m.Text, m.Replacement);
            }
            return output.ToString();
        }

        public static string ToHtml(string text)
        {
            return ToHtml(text, null);
        }

        public static string ToHtml(string text, Dictionary<string, BaseTagHandler> handlers)
        {
            if (text == null) return "";
            if (text.Length == 0) return "";
            /* register tags handlers */
            if (handlers == null) {
                handlers = new Dictionary<string, BaseTagHandler> {
                    {"", new RootTag()},
                    {"b", new SimpleTag("b")},
                    {"s", new SimpleTag("s")},
                    {"i", new SimpleTag("em")},
                    {"u", new SimpleTag("u")},
                    {"code", new SimpleTag("pre")},
                    {"quote", new SimpleTag("blockquote", new SimpleTag("p"))},
                    {"url", new LinkTag()},
                    {"img", new ImageTag()}
                };
            }
            else {
                if (handlers.ContainsKey("")) {
                    handlers[""] = new RootTag();
                }
                else {
                    handlers.Add("", new RootTag());
                }
            }
            if (handlers.Keys.Count > 0) {
                Token root = BBParser.BuildAST(BBParser.Tokenize(text));
                string processing_result = BBParser.ProcessAST(root, handlers);
                return null ?? ProcessSmileys(processing_result);
            }
            else {
                return ProcessSmileys(HttpUtility.HtmlEncode(text));
            }
        }

    }
}