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
            parser.Parse("icecream is i", out t);
            parser.Parse("velvet is v", out t);
            parser.Parse("xmen ix X", out t);
            parser.Parse("lift is L", out t);
            parser.Parse("CENt is c", out t);
            parser.Parse("Drum is D", out t);
            parser.Parse("Make is m", out t);
        }

        [TestMethod()]
        public void VerifyInvalidSentenceType()
        {
            SentenceTypes st;
            var tokens = parser.Parse("Magic is K", out st); //invalid because K is not a valid roman number
            Assert.AreEqual(SentenceTypes.Invalid, st);
        }
        [TestMethod()]
        public void VerifyRomanNumberDefinitionSentenceType()
        {
            SentenceTypes st;
            var tokens = parser.Parse("icecream is I", out st);
            Assert.AreEqual(SentenceTypes.RomanNumberMapper, st);
            Assert.AreEqual(3, tokens.Length);
        }
        [TestMethod()]
        public void VerifyComodityUnitPriceDefinitionSentenceType()
        {
            SentenceTypes st;
            var tokens = parser.Parse("VELVET icecream Copper is 80 Credits", out st);
            Assert.AreEqual(SentenceTypes.CommodityDefinition, st);
            Assert.AreEqual(6, tokens.Length);
        }
    }
}