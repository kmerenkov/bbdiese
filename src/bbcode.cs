using System.Collections.Generic;
using System.Web;


namespace BBDiese
{
    public static class BBCode
    {
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
                    {"b", new SimpleTag("s")},
                    {"i", new SimpleTag("em")},
                    {"u", new SimpleTag("u")},
                    {"code", new SimpleTag("pre")},
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
                return null ?? processing_result;
            }
            else {
                return HttpUtility.HtmlEncode(text);
            }
        }

    }
}