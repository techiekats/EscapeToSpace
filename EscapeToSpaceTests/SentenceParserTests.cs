﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using EscapeToSpace.Enums;
using EscapeToSpace;
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
                var t = (st as Query).GetQueryTerms();
                Assert.AreEqual(SentenceTypes.Query, st.Type);
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
        public void EvaluateIncorrectQuery ()
        {
            var seed = parser.Parse("cenT gold is 2000 credits");
            var query = parser.Parse("okay when does this end?");
            var result = parser.Evaluate(query);
            Assert.AreEqual(int.MinValue, result);
        }
    }
}