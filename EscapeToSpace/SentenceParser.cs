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
        private readonly string queryPattern = @"(how much|how many credits|how many Credits) (is) (\w+)";
        private readonly ParseTables parseTables;
        public SentenceParser ()
        {
            parseTables = new ParseTables();
        }

        public Sentence Parse(string sentence)
        {
            Sentence parsedSentence;
            if (Regex.IsMatch(sentence, numberDefinitionPattern) && !sentence.EndsWith("?"))
            {   try
                {
                    parsedSentence = new RomanNumberMapper().Parse(sentence, parseTables.GetReader());
                    UpdateRomanNumberMap((parsedSentence as RomanNumberMapper).GetRomanNumberMapping());
                }
                catch (FormatException ex)
                {
                    parsedSentence = new InvalidSentence();
                }
            }
            else if (Regex.IsMatch(sentence, commodityDefinitionPattern))
            {
                try
                {
                    parsedSentence = new CommodityValueAssigner().Parse(sentence, parseTables.GetReader());
                    UpdateCommodityUnitPriceMap((parsedSentence as CommodityValueAssigner).GetCommodityUnitPrice());
                }
                catch (FormatException ex)
                {
                    parsedSentence = new InvalidSentence();
                }
            }
            else if (Regex.IsMatch(sentence, queryPattern) && sentence.EndsWith("?"))
            {
                try
                {
                    parsedSentence = new Query().Parse(sentence, parseTables.GetReader());
                }
                catch (FormatException ex)
                {
                    parsedSentence = new InvalidSentence();
                }
            }
            else
            {
                parsedSentence = new InvalidSentence();
            }
            return parsedSentence;
        }

        public int Evaluate (Sentence query)
        {
            if (query.Type == SentenceTypes.Invalid)
            {
                return int.MinValue;
            }
            var t = (query as Query).GetQueryTerms();
            int numberOfItems = RomanNumber.Parse(t.Item1);
            int unitPrice = parseTables.GetReader().GetCommodityUnitPrice(t.Item2);
            return numberOfItems * unitPrice;
        }
        private void UpdateRomanNumberMap(Tuple<string, char> tuple)
        {
            parseTables.UpdateRomanNumberMap(tuple);
        }
        private void UpdateCommodityUnitPriceMap (Tuple<string, int> tuple)
        {
            parseTables.UpdateCommodityUnitPriceMap(tuple);
        }
    }
    public class ParseTables
    {
        private Dictionary<string, char> romanNumberTranslation;
        private Dictionary<string, int> commodityUnitPrice;
        public ParseTables ()
        {
            romanNumberTranslation = new Dictionary<string, char>(7);
            commodityUnitPrice = new Dictionary<string, int>();
        }
        public ParseTableReader GetReader ()
        {
            return new ParseTableReader(this.romanNumberTranslation, this.commodityUnitPrice);
        }
        public void UpdateRomanNumberMap(Tuple<string, char> tuple)
        {
            string key = tuple.Item1.Trim().ToLower();
            if (romanNumberTranslation.ContainsKey(key))
            {
                romanNumberTranslation[key] = tuple.Item2;
            }
            else
            {
                romanNumberTranslation.Add(key, tuple.Item2);
            }
        }
        public void UpdateCommodityUnitPriceMap(Tuple<string, int> tuple)
        {
            string key = tuple.Item1.Trim().ToLower();
            if (commodityUnitPrice.ContainsKey(key))
            {
                commodityUnitPrice[key] = tuple.Item2;
            }
            else
            {
                commodityUnitPrice.Add(key, tuple.Item2);
            }
        }
    }
    public class ParseTableReader
    {
        private Dictionary<string, char> romanNumberTranslation;
        private Dictionary<string, int> commodityUnitPrice;
        public ParseTableReader (Dictionary<string, char> romanNumberTranslation, Dictionary<string, int> commodityUnitPrice)
        {
            this.romanNumberTranslation = romanNumberTranslation;
            this.commodityUnitPrice = commodityUnitPrice;
        }
        public char GetTranslationForAlienDigit (string key)
        {
            key = key.Trim().ToLower();
            if (romanNumberTranslation.ContainsKey(key))
                return romanNumberTranslation[key];
            return Char.MinValue;
        }
        public bool IsValidCommodity (string key)
        {
            return commodityUnitPrice.ContainsKey(key.Trim().ToLower());
        }
        public int GetCommodityUnitPrice (string key)
        {
            key = key.Trim().ToLower();
            if (commodityUnitPrice.ContainsKey(key))
            {
                return commodityUnitPrice[key];
            }
            throw new KeyNotFoundException();
        }
    }
}
