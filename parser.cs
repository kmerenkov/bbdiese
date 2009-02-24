using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace BBDiese {
    public static class Parser
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

        static public string EscapeHtml(string text)
        {
            StringBuilder output = new StringBuilder(text);
            foreach (Map item in html_replacement_map) {
                output.Replace(item.Text, item.Replacement);
            }
            return output.ToString();
        }

        static private string FindNextToken(string text, int pos)
        {
            if (pos == text.Length) {
                return null;
            }
            int open_brace_index;
            int close_brace_index;
            if (text[pos] != '[') {
                open_brace_index = text.IndexOf('[', pos+1);
                if (open_brace_index == -1) { // no tags at all
                    int substring_length = text.Length-pos+1;
                    if (pos+substring_length > text.Length) {
                        substring_length -= 1;
                    }
                    return text.Substring(pos, substring_length);
                }
                else {
                    return text.Substring(pos, open_brace_index-pos);
                }
            }
            /* we're @tag */
            open_brace_index = pos;
            close_brace_index = text.IndexOf(']', open_brace_index+1);
            if (close_brace_index == -1) {
                return text.Substring(open_brace_index, text.Length-pos);
            }
            return text.Substring(open_brace_index, close_brace_index-pos+1);
        }

        private static List<Token> Tokenize(string text)
        {
            List<Token> tokens = new List<Token>();
            int pos = 0;
            string str_token;
            while ((str_token = FindNextToken(text, pos)) != null) {
                /* determine token type.
                   yes it is MUCH FASTER than StartsWith and EndsWith */
                bool token_is_text = true;
                if (str_token.Length > 2) {
                    if ((str_token[0] == '[') && (str_token[str_token.Length-1] == ']')) {
                        token_is_text = false;
                    }
                }
                /* create token */
                Token token = new Token(str_token, (token_is_text ? TokenType.Text : TokenType.Tag));
                pos += str_token.Length;
                if (token.Type == TokenType.Tag) {
                    Tag tag = TagParser.Parse(token.RawBody);
                    token.Tag = tag;
                }
                tokens.Add(token);
            }
            return tokens;
        }

        private static Token BuildAST(string text)
        {
            List<Token> tokens = Tokenize(text);
            /* filter unmatching tags */
            /* XXX I bet that the following code is suboptimal! */
            /* Find all tags that close before their open, or open w/o closing in the end,
               and change their type from Tag to Text */
            List<string> open_tags = new List<string>();
            List<int> open_tags_idxs = new List<int>();
            for (int i = 0; i < tokens.Count; i++) {
                Token token = tokens[i];
                /* not a tag, skip */
                if (token.Type != TokenType.Tag) {
                    continue;
                }
                if (!token.Tag.IsClosing) {
                    /* save information about opening tag */
                    open_tags.Add(token.Tag.Name);
                    open_tags_idxs.Add(i);
                }
                else {
                    int open_tag_idx = open_tags.IndexOf(token.Tag.Name);
                    if (open_tag_idx != -1) {
                        /* this tag was opened before */
                        /* j - opening tag, i - closing tag */
                        int j = open_tags_idxs[open_tag_idx];
                        for (int x = j+1; x < i; x++) {
                            /* build a tree, finally */
                            tokens[x].Parent = tokens[j];
                            tokens[j].Children.Add(tokens[x]);
                        }
                        open_tags.RemoveAt(open_tag_idx);
                        open_tags_idxs.RemoveAt(open_tag_idx);
                    }
                    else {
                        /* we haven't met this tag before, even though it is closing */
                        tokens[i].Type = TokenType.Text;
                        tokens[i].Tag = null;
                    }
                }
            }
            /* mark all unclosed tags as text */
            foreach (int open_tag_idx in open_tags_idxs) {
                tokens[open_tag_idx].Type = TokenType.Text;
                tokens[open_tag_idx].Tag = null;
            }

            Token root_token = new Token("",
                                         TokenType.Tag,
                                         new Tag("",
                                                 new Dictionary<string, string>(),
                                                 false));
            /* find all tags that are either text or un(!)closing tags and that have no parent */
            root_token.Children = new List<Token>(tokens.Where(t =>
                                                                (t.Parent == null) &&
                                                                ((t.Type == TokenType.Text) ||
                                                                 (!t.Tag.IsClosing))));
            return root_token;
        }

        public static string ToHtml(string text)
        {
            if (text == null) return "";
            if (text.Length == 0) return "";
            Token root = BuildAST(text);
            Dictionary<string, BaseTag> handlers = new Dictionary<string, BaseTag>();
            handlers.Add("", new RootTag());
            handlers.Add("b", new SimpleTag("s"));
            handlers.Add("i", new SimpleTag("em"));
            handlers.Add("u", new SimpleTag("u"));
            handlers.Add("url", new LinkTag());
            string processing_result = ProcessAST(root, handlers);
            return processing_result == null ? "" : processing_result;
        }

        private static string ProcessAST(Token token, Dictionary<string, BaseTag> handlers)
        {
            if ((token == null) || token.IsProcessed || (handlers == null)) {
                return null;
            }
            token.IsProcessed = true;
            StringBuilder content = new StringBuilder();
            foreach (Token child in token.Children) {
                if (child.Type == TokenType.Text) {
                    if (!child.IsProcessed) {
                        child.IsProcessed = true;
                        content.Append(EscapeHtml(child.RawBody));
                    }
                }
                else {
                    if (!child.IsProcessed) {
                        if (!child.Tag.IsClosing) {
                            string processing_result = ProcessAST(child, handlers);
                            if (processing_result != null) {
                                content.Append(processing_result);
                            }
                        }
                    }
                }
            }
            if (token.Type == TokenType.Tag) {
                token.Content = content.ToString();
                string output = handlers[token.Tag.Name].Process(token);
                return output;
            }
            else {
                return content.ToString();
            }
        }
    }
}