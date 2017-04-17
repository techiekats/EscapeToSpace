using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EscapeToSpace
{
    public class Query : Sentence
    {
        private string alienDigits = string.Empty;
        private string commodityName = string.Empty;
        public Query ()
        {
            type = Enums.SentenceTypes.RomanNumberTranslationQuery;
        }
        public override Sentence Parse(string sentence, ParseTableReader reader)
        {
            original = sentence;
            string pattern = @"(how much|how many credits|how many Credits) (is)";
            string pattern2 = @"(\w+)+";
            var tokens1 = Regex.Split(sentence, pattern).Where(s=>s!=string.Empty).ToList<string>();
            string combinedTokens = tokens1.Last();
            string[] tokenStrings = Regex.Split(combinedTokens, pattern2).Where(s => s.Trim() != string.Empty && s.Trim() != "?").ToArray<string>();
            List<Token> tokens = new List<Token>();
            for (int i=0; i < tokenStrings.Length; i++)
            {
                if (reader.GetTranslationForAlienDigit(tokenStrings[i]) != Char.MinValue)
                {
                    if (i==0 || tokens.Last().Type == Enums.TokenTypes.AlienDigit)
                    {
                        tokens.Add(new Token(tokenStrings[i], Enums.TokenTypes.AlienDigit));
                        alienDigits += reader.GetTranslationForAlienDigit(tokenStrings[i]);
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                else if (reader.IsValidCommodity(tokenStrings[i]))
                {
                    if (tokens.Last().Type == Enums.TokenTypes.AlienDigit && RomanNumber.Parse(alienDigits) > 0)
                    {
                        if (i == tokenStrings.Length - 1) //if commodity definition, then it has to be the last
                        {
                            commodityName = tokenStrings[i];
                            type = Enums.SentenceTypes.CommodityValueQuery;
                            tokens.Add(new Token(commodityName, Enums.TokenTypes.AlienDigit));
                        }
                        else
                        {
                            throw new FormatException();
                        }
                    }
                }
                else
                {
                    throw new FormatException();
                }
                if (i == tokens.Count - 1)
                {
                    RomanNumber.Parse(alienDigits); //would throw exception if alien digits dont obey the roman number rules
                }
            }
            return this;
        }
        public Tuple<string, string> GetCommodityQueryTerms ()
        {
            return new Tuple<string, string>(alienDigits, commodityName);
        }
        public string GetAlienDigitsForQuery ()
        {
            return alienDigits;
        }
    }
}
