using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


namespace BBDiese {
    public class Parser
    {
        static private string FindNextToken(string text, int pos)
        {
            if (text == null) {
                return null;
            }
            if ((text.Length == 0) || (text == null) || (pos == text.Length)) {
                return null;
            }
            int open_brace_index;
            int close_brace_index;
            if (text[pos] != '[') {
                open_brace_index = text.IndexOf('[', pos);
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
            close_brace_index = text.IndexOf(']', open_brace_index);
            if (close_brace_index == -1) {
                return text.Substring(pos, text.Length-pos);
            }
            return text.Substring(open_brace_index, close_brace_index-pos+1);
        }

        private static List<Token> BuildAST(string text)
        {
            List<Token> tokens = new List<Token>();
            int pos = 0;
            while (true) {
                string str_token = FindNextToken(text, pos);
                if (str_token == null) {
                    break;
                }
                Token token = new Token(str_token, (str_token.StartsWith("[") && str_token.EndsWith("]")) ? TokenType.Tag : TokenType.Text);
                pos += str_token.Length;
                if (token.Type == TokenType.Tag) {
                    Tag tag = Tag.Parse(token.RawBody);
                    token.Tag = tag;
                }
                tokens.Add(token);
            }
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
                    open_tags.Add(token.Tag.Name);
                    open_tags_idxs.Add(i);
                }
                else {
                    int open_tag_idx = open_tags.IndexOf(token.Tag.Name);
                    if (open_tag_idx != -1) {
                        int j = open_tags_idxs[open_tag_idx];
                        for (int x = j+1; x < i; x++) {
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
            /* filter out all closing tags */
            List<Token> tokens2 = new List<Token>(tokens.Where(t => (t.Type == TokenType.Text) || (!t.Tag.IsClosing)));
            /* find all orphaned tokens and join them to a root token */
            Token root_token = new Token("",
                                         TokenType.Tag,
                                         new Tag("",
                                                 new Dictionary<string, string>(),
                                                 false));
            root_token.Children = new List<Token>(tokens2.Where(t => t.Parent == null));
            tokens2.Insert(0, root_token);
            return tokens2;
        }

        public static string ToHtml(string text)
        {
            List<Token> tokens = BuildAST(text);
            Dictionary<string, BaseTag> handlers = new Dictionary<string, BaseTag>();
            handlers.Add("", new RootTag());
            handlers.Add("b", new SimpleTag("s"));
            handlers.Add("i", new SimpleTag("em"));
            handlers.Add("u", new SimpleTag("u"));
            handlers.Add("url", new LinkTag());
            if (tokens.Count > 0) {
                return ProcessAST(tokens[0], handlers);
            }
            return "";
        }

        private static string ProcessAST(Token token, Dictionary<string, BaseTag> handlers)
        {
            if (token == null) return "";
            if (token.IsProcessed) return "";
            if (handlers == null) return "";
            token.IsProcessed = true;
            StringBuilder content = new StringBuilder();
            foreach (Token child in token.Children) {
                if (child.Type == TokenType.Text) {
                    if (!child.IsProcessed) {
                        child.IsProcessed = true;
                        content.Append(child.RawBody);
                    }
                }
                else {
                    if (!child.IsProcessed) {
                        if (!child.Tag.IsClosing) {
                            content.Append(ProcessAST(child, handlers));
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