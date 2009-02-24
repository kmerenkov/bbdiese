using System;
using System.Collections.Generic;
using System.Text;


namespace BBDiese {
    [Serializable]
    internal enum TokenType:int {
        Tag = 0,
        Text
    }

    sealed internal class Token
    {
        public TokenType Type {get; set;}
        public string RawBody {get; set;}
        public string Content {get; set;}
        public Tag Tag {get; set;}
        /* AST-related fields */
        public Token Pair {get;set;} /* i.e. closing token */
        public bool IsProcessed {get; set;}
        public Token Parent {get; set;}
        public List<Token> Children {get; set;}

        public Token(string raw_body, TokenType type, Tag tag)
        {
            this.RawBody = raw_body;
            this.Type = type;
            this.Tag = tag;
            this.Children = new List<Token>();
            this.IsProcessed = false;
        }

        public Token(string raw_body, TokenType type):this(raw_body, type, null)
        {}
    }
}