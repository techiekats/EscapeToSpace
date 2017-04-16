using EscapeToSpace.Enums;

namespace EscapeToSpace
{
    public class Token
    {
        private string tokenText;
        private TokenTypes type;
        public Token (string tokenText, TokenTypes type)
        {
            this.tokenText = tokenText;
            this.type = type;
        }

        public TokenTypes Type
        {
            get
            {
                return type;
            }
        }
    }
}
