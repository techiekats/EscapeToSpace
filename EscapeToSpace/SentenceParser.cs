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
        private readonly string numberDefinitionPattern = "";
        private readonly string commodityDefinitionPattern = "";
        private readonly string queryPattern = "";

        public Token[] Parse(string sentence, out SentenceTypes type)
        {
            Token[] tokens;
            if (Regex.IsMatch(sentence,numberDefinitionPattern))
            {
                type = SentenceTypes.RomanNumberMapper;
                tokens = TokenizeRomanNumberDefinition(sentence);
            }
            else if (Regex.IsMatch(sentence, commodityDefinitionPattern))
            {
                type = SentenceTypes.CommodityDefinition;
                tokens = TokenizeCommodityDefinition(sentence);
            }
            else if (Regex.IsMatch(sentence, queryPattern))
            {
                type = SentenceTypes.Query;
                tokens = TokenizeQueryDefinition(sentence);
            }
            else
            {
                type = SentenceTypes.Invalid;
                tokens = null;
            }
            return tokens;
        }
        private Token[] TokenizeRomanNumberDefinition (string sentence)
        {
            return null;
        }
        private Token[] TokenizeCommodityDefinition (string sentence)
        {
            return null;
        }
        private Token[] TokenizeQueryDefinition (string sentence)
        {
            return null;
        }
    }
}
