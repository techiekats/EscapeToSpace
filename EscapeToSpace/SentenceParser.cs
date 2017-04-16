using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EscapeToSpace.Enums;

namespace EscapeToSpace
{
    public class SentenceParser
    {
        private readonly string numberDefinitionPattern = @"(\w+) (is) (I|V|X|L|C|D|M)";
        private readonly string commodityDefinitionPattern = @"(\w)+ (is) (\d) (Credits|credits)";
        private readonly string queryPattern = "";
        private Dictionary<string, char> romanNumberTranslation;
        private Dictionary<string, int> commodityUnitPrice;
        public SentenceParser ()
        {
            romanNumberTranslation = new Dictionary<string, char>(7);
        }

        public Token[] Parse(string sentence, out SentenceTypes type)
        {
            Token[] tokens = null;
            type = SentenceTypes.Invalid;
            if (Regex.IsMatch(sentence, numberDefinitionPattern))
            {
                tokens = TokenizeRomanNumberDefinition(sentence);
                if (tokens.Length > 0)
                {
                    type = SentenceTypes.RomanNumberMapper;
                }
            }
            else if (Regex.IsMatch(sentence, commodityDefinitionPattern))
            {
                tokens = TokenizeCommodityDefinition(sentence);
                if (tokens.Length > 0)
                {
                    type = SentenceTypes.CommodityDefinition;
                }
            }
            else if (Regex.IsMatch(sentence, queryPattern))
            {
                tokens = TokenizeQueryDefinition(sentence);
                if (tokens.Length > 0)
                {
                    type = SentenceTypes.Query;
                }
            }
            return tokens;
        }
        private Token[] TokenizeRomanNumberDefinition (string sentence)
        {
            string[] tokenStrings = Regex.Split(sentence, numberDefinitionPattern).Where(s=> s!="").ToArray<string>();
            if (RomanNumber.Parse(tokenStrings[2]) > 0)
            {
                romanNumberTranslation.Add(tokenStrings[0], tokenStrings[2].ToCharArray()[0]);
            }
            return new Token[] { new Token(tokenStrings[0], TokenTypes.AlienDigit), new Token(tokenStrings[1], TokenTypes.Equality), new Token(tokenStrings[2], TokenTypes.RomanDigit) };
        }
        private Token[] TokenizeCommodityDefinition (string sentence)
        {
            string[] tokenStrings = Regex.Split(sentence, commodityDefinitionPattern).Where(s => s != "").ToArray<string>();
            List<Token> parsedTokens = new List<Token>();
            string commodity = string.Empty;
            for (int i=0; i < tokenStrings.Length; i++)
            {
                int result;
                if (romanNumberTranslation.ContainsKey(tokenStrings[i]))
                {
                    if (i==0 || parsedTokens.Last().Type == TokenTypes.AlienDigit)
                    {
                        parsedTokens.Add(new Token(tokenStrings[i], TokenTypes.AlienDigit));
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }

                else if (tokenStrings[i].Equals("is", StringComparison.CurrentCultureIgnoreCase))
                {
                    parsedTokens.Add(new Token(tokenStrings[i], TokenTypes.Equality));
                }
                else if (tokenStrings[i].Equals("Credits", StringComparison.CurrentCultureIgnoreCase))
                {
                    parsedTokens.Add(new Token(tokenStrings[i], TokenTypes.Unit));
                }
                else if (int.TryParse(tokenStrings[i], out result))
                {
                    if (parsedTokens.Last().Type == TokenTypes.Equality)
                    {
                        parsedTokens.Add(new Token(tokenStrings[i], TokenTypes.UnitPrice));
                        commodityUnitPrice.Add(commodity, result);
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                else if (parsedTokens.Last().Type == TokenTypes.AlienDigit)
                {
                    parsedTokens.Add(new Token(tokenStrings[i], TokenTypes.Commodity));
                    commodity = tokenStrings[i];
                }
            }
            return parsedTokens.ToArray<Token>();
        }
        private Token[] TokenizeQueryDefinition (string sentence)
        {
            return null;
        }
    }
}
