using EscapeToSpace.Enums;
namespace EscapeToSpace
{
    /// <summary>
    /// base class for all sentence types
    /// </summary>
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
        /// <summary>
        /// Parse sentence
        /// </summary>
        /// <param name="sentence">sentence to be parsed</param>
        /// <param name="reader">reader object for existing parse tables</param>
        /// <returns></returns>
        public abstract Sentence Parse(string sentence, ParseTableReader reader);
    }
}
