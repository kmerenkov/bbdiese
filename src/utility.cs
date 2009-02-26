using System.Collections.Generic;
using System.Text;


namespace BBDiese
{
    public static class Utility
    {
        private class Map
        {
            public string Text {get;set;}
            public string Replacement {get;set;}

            public Map(string text, string replacement)
            {
                this.Text = text;
                this.Replacement = replacement;
            }
        }

        static private List<Map> html_replacement_map = new List<Map> {
            new Map("&", "&amp;"),
            new Map("<", "&lt;"),
            new Map(">", "&gt;")
        };

        public static string EscapeHtml(string text)
        {
            StringBuilder output = new StringBuilder(text);
            foreach (Map item in html_replacement_map) {
                output.Replace(item.Text, item.Replacement);
            }
            return output.ToString();
        }
    }
}