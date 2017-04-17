using EscapeToSpace.Enums;
namespace EscapeToSpace
{
    public abstract class Sentence
    {
        protected SentenceTypes type;

        public SentenceTypes Type
        {
            get
            {
                return type;
            }
        }

        public abstract Sentence Parse(string sentence, ParseTableReader reader);
    }
}
