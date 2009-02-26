using System.Collections.Generic;
using System.Text;


namespace BBDiese
{
    public static class Utility
    {
        private struct Map
        {
            public string Text;
            public string Replacement;
        }

        static private List<Map> html_replacement_map = new List<Map> {
            new Map { Text = "&", Replacement = "&amp;" },
            new Map { Text = "<", Replacement = "&lt;" },
            new Map { Text = ">", Replacement = "&gt;" }
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