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
            var tokens = parser.Parse("Cheese is I", out st);
            Assert.AreEqual(SentenceTypes.RomanNumberMapper, st);
            Assert.AreEqual(3, tokens.Length);
        }
        [TestMethod()]
        public void VerifyComodityUnitPriceDefinitionSentenceType()
        {
            SentenceTypes st;
            var tokens = parser.Parse("Cheese Cheese Copper is 80 Credits", out st);
            Assert.AreEqual(SentenceTypes.CommodityDefinition, st);
            Assert.AreEqual(6, tokens.Length);
        }
    }
}