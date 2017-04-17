using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EscapeToSpace
{
    public class RomanNumberMapper : Sentence
    {
        private readonly string numberDefinitionPattern = @"(\w+) (is) (I|i|V|v|X|x|L|l|C|c|D|d|M|m)";
        private string aliendigit = string.Empty;
        private char romanDigit;
        public RomanNumberMapper()
        {
            type = Enums.SentenceTypes.RomanNumberMapper;
        }
        public override Sentence Parse(string sentence, ParseTableReader reader)
        {
            original = sentence;
            string[] tokenStrings = Regex.Split(sentence, numberDefinitionPattern).Where(s => s != "").ToArray<string>();
            if (tokenStrings.Length != 3)
            {
                throw new FormatException();
            }
            else
            {
                aliendigit = tokenStrings[0];
                romanDigit = tokenStrings[2].Trim().ToUpper()[0];
            }
            return this;        
        }
        public Tuple<string, char> GetRomanNumberMapping ()
        {
            return new Tuple<string, char>(aliendigit, romanDigit);
        }
    }
}
