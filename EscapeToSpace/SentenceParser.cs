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
        private readonly string numberDefinitionPattern = @"(\w+) (is) (I|i|V|v|X|x|L|l|C|c|D|d|M|m)";
        private readonly string commodityDefinitionPattern = @"(\w+)+ (is) (\d+) (Credits|credits)";
        private readonly string queryPattern = @"(how much|how many) (Credits|credits) (is) (\w+)";
        private Dictionary<string, char> romanNumberTranslation;
        private Dictionary<string, int> commodityUnitPrice;
        public SentenceParser ()
        {
            romanNumberTranslation = new Dictionary<string, char>(7);
            commodityUnitPrice = new Dictionary<string, int>();
        }

        public Token[] Parse(string sentence, out SentenceTypes type)
        {
            Token[] tokens = null;
            type = SentenceTypes.Invalid;
            if (Regex.IsMatch(sentence, numberDefinitionPattern))
            {
                tokens = TokenizeRomanNumberDefinition(sentence, out type);
            }
            else if (Regex.IsMatch(sentence, commodityDefinitionPattern))
            {
                tokens = TokenizeCommodityDefinition(sentence, out type);
            }
            else if (Regex.IsMatch(sentence, queryPattern))
            {
                tokens = TokenizeQueryDefinition(sentence, out type);
            }
            return tokens;
        }
        private Token[] TokenizeRomanNumberDefinition (string sentence, out SentenceTypes type)
        {
            string[] tokenStrings = Regex.Split(sentence, numberDefinitionPattern).Where(s=> s!="").ToArray<string>();
            if (RomanNumber.Parse(tokenStrings[2]) > 0)
            {
                if (romanNumberTranslation.ContainsKey(tokenStrings[0]))
                {
                    romanNumberTranslation[tokenStrings[0].ToLower()] = tokenStrings[2].ToCharArray()[0];
                }
                else
                {
                    romanNumberTranslation.Add(tokenStrings[0].ToLower(), tokenStrings[2].ToCharArray()[0]);
                }
                type = SentenceTypes.RomanNumberMapper;
            }
            else
            {
                type = SentenceTypes.Invalid;
            }
            return new Token[] { new Token(tokenStrings[0], TokenTypes.AlienDigit), new Token(tokenStrings[1], TokenTypes.Equality), new Token(tokenStrings[2], TokenTypes.RomanDigit) };
        }
        private Token[] TokenizeCommodityDefinition (string sentence, out SentenceTypes type)
        {

            string[] tokenStrings = sentence.Split(new char[] {' '}).Where(s => s != "").ToArray<string>();
         
            List<Token> parsedTokens = new List<Token>();
            string commodity = string.Empty;
            type = SentenceTypes.CommodityDefinition;
            for (int i=0; i < tokenStrings.Length; i++)
            {
                int result;
                if (romanNumberTranslation.ContainsKey(tokenStrings[i].Trim().ToLower()))
                {
                    if (i==0 || parsedTokens.Last().Type == TokenTypes.AlienDigit)
                    {
                        parsedTokens.Add(new Token(tokenStrings[i], TokenTypes.AlienDigit));
                    }
                    else
                    {
                        type = SentenceTypes.Invalid;
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
                        commodityUnitPrice.Add(commodity.Trim().ToLower(), result);
                    }
                    else
                    {
                        type = SentenceTypes.Invalid;
                        throw new FormatException();
                    }
                }
                else if (parsedTokens.Last()?.Type == TokenTypes.AlienDigit)
                {
                    parsedTokens.Add(new Token(tokenStrings[i], TokenTypes.Commodity));
                    commodity = tokenStrings[i];
                }
            }
            return parsedTokens.ToArray<Token>();
        }
        private Token[] TokenizeQueryDefinition (string sentence, out SentenceTypes type)
        {
            type = SentenceTypes.Query;
            return null;
        }
    }
}
