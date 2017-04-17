using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeToSpace
{
    public class InvalidSentence : Sentence
    {
        public InvalidSentence (string sentence)
        {
            type = Enums.SentenceTypes.Invalid;
            original = sentence;
        }

        public override Sentence Parse(string sentence, ParseTableReader reader)
        {
            original = sentence;
            throw new NotImplementedException();
        }
    }
}
