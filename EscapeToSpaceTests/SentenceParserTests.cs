using Microsoft.VisualStudio.TestTools.UnitTesting;
using EscapeToSpace.Enums;
namespace EscapeToSpace.Tests
{
    [TestClass()]
    public class SentenceParserTests
    {
        [TestMethod()]
        public void VerifyInvalidSentenceType()
        {
            var parser = new SentenceParser();
            SentenceTypes st;
            var tokens = parser.Parse("This is not a valid sentence", out st);
            Assert.AreEqual(SentenceTypes.Invalid, st);
        }
        [TestMethod()]
        public void VerifyRomanNumberDefinitionSentenceType()
        {
            var parser = new SentenceParser();
            SentenceTypes st;
            var tokens = parser.Parse("Cheese is I", out st);
            Assert.AreEqual(SentenceTypes.RomanNumberMapper, st);
        }
    }
}