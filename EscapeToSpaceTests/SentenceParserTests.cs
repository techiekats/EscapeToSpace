using Microsoft.VisualStudio.TestTools.UnitTesting;
using EscapeToSpace.Enums;
namespace EscapeToSpace.Tests
{
    [TestClass()]
    public class SentenceParserTests
    {
        private SentenceParser parser;
        [TestInitialize]
        public void Initialize()
        {
            parser = new SentenceParser();
            //Load roman numbers
            SentenceTypes t;
            parser.Parse("icecream is i");
            parser.Parse("velvet is v");
            parser.Parse("xmen ix X");
            parser.Parse("lift is L");
            parser.Parse("CENt is c");
            parser.Parse("Drum is D");
            parser.Parse("Make is m");
        }

        [TestMethod()]
        public void VerifyInvalidSentenceType()
        {
            var st = parser.Parse("Magic is K"); //invalid because K is not a valid roman number
            Assert.AreEqual(SentenceTypes.Invalid, st.Type);
        }
        [TestMethod()]
        public void VerifyRomanNumberDefinitionSentenceType()
        {
            var st = parser.Parse("icecream is I");
            Assert.AreEqual(SentenceTypes.RomanNumberMapper, st.Type);
        }
        [TestMethod()]
        public void VerifyComodityUnitPriceDefinitionSentenceType()
        {
            var st = parser.Parse("VELVET icecream Copper is 80 Credits");
            Assert.AreEqual(SentenceTypes.CommodityDefinition, st.Type);
        }
    }
}