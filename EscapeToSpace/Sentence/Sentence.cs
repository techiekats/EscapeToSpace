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
        protected string original;
        public string Original
        {
            get
            {
                return original;
            }
        }
        public abstract Sentence Parse(string sentence, ParseTableReader reader);
    }
}
