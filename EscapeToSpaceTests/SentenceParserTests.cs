using Microsoft.VisualStudio.TestTools.UnitTesting;
using EscapeToSpace.Enums;
using EscapeToSpace;
using System;

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
            var t = (st as RomanNumberMapper).GetRomanNumberMapping();
            Assert.AreEqual("icecream", t.Item1);
            Assert.AreEqual('I', t.Item2);
        }
        [TestMethod()]
        public void VerifyCommodityUnitPriceDefinitionSentenceType()
        {
            var st = parser.Parse("VELVET icecream Copper is 78 Credits");
            var t = (st as CommodityValueAssigner).GetCommodityUnitPrice();
            Assert.AreEqual(SentenceTypes.CommodityDefinition, st.Type);
            Assert.AreEqual("copper", t.Item1.Trim().ToLower());
            Assert.AreEqual(13, t.Item2);
        }
        [TestMethod()]
        public void VerifyQueryDefinition()
        {
            var seed = parser.Parse("drum cent platinum is 1500 credits");
            if (seed.Type == SentenceTypes.CommodityDefinition)
            {
                var st = parser.Parse("how much is cent lift platinum ?");
                var t = (st as Query).GetCommodityQueryTerms();
                Assert.AreEqual(SentenceTypes.CommodityValueQuery, st.Type);
                Assert.AreEqual("CL", t.Item1);
                Assert.AreEqual("platinum", t.Item2);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void EvaluateTest()
        {
            var seed = parser.Parse("cenT gold is 2000 credits");
            var query = parser.Parse("how many credits is lift gold?");
            var result = parser.Evaluate(query);
            Assert.AreEqual(1000, result);
        }
        [TestMethod()]
        public void EvaluateIncorrectCommodityQuery ()
        {
            var seed = parser.Parse("cenT gold is 2000 credits");
            var query = parser.Parse("looking forward to spring time?");
            try
            {
                var result = parser.Evaluate(query);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual(1, 1);
            }
        }
        [TestMethod()]
        public void EvaluateIncorrectRomanNumberQuery()
        {
            var query = parser.Parse("how much is cent cent make?"); //invalid roman number
            try
            {
                var result = parser.Evaluate(query);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual(1, 1);
            }
        }
        [TestMethod()]
        public void EvaluateRomanNumberQueryTest1 ()
        {
            var query = parser.Parse("how much is lift?");
            var result = parser.Evaluate(query);
            Assert.AreEqual(50, result);
        }
        [TestMethod()]
        public void EvaluateRomanNumberQueryTest2()
        {
            var query = parser.Parse("how much is make drum lift velvet Icecream?");
            var result = parser.Evaluate(query);
            Assert.AreEqual(1556, result);
        }
    }
}