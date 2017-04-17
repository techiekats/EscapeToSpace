using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeToSpace
{
    class InvalidSentence : Sentence
    {
        public InvalidSentence ()
        {
            type = Enums.SentenceTypes.Invalid; 
        }

        public override Sentence Parse(string sentence, ParseTableReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
