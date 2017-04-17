using Microsoft.VisualStudio.TestTools.UnitTesting;
using EscapeToSpace.Enums;
using EscapeToSpace;
using System;
using System.Collections.Generic;

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
            parser.Parse("icecream is i");
            parser.Parse("velvet is v");
            parser.Parse("xmen is X");
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
        [TestMethod()]
        public void EvaluateMultipleSentencesTest1 ()
        {
            List<Tuple<string, int>> notes = new List<Tuple<string, int>>();
            notes.Add(new Tuple<string, int>("Xmen soil is 100 Credits", -1));
            notes.Add(new Tuple<string, int>("icecream iceCream iceCream titanium is 3600 credits", -1));
            notes.Add(new Tuple<string, int>("Velvet icecream Aluminium is 1080 credits", -1));
            notes.Add(new Tuple<string, int>("Icecream Iron is 444 credits", -1));
            notes.Add(new Tuple<string, int>("Lift Water is 300 Credits", -1));
            notes.Add(new Tuple<string, int>("How much is xmen icecream velvet soil?", 140));
            notes.Add(new Tuple<string, int>("How many credits is icecream titanium?", 1200));
            notes.Add(new Tuple<string, int>("how many credits is drum xmen iron ?", 444 * 510));
            notes.Add(new Tuple<string, int>("how much is icecream water?",6));
            notes.Add(new Tuple<string, int>("how many credits is make icecream xmen aluminium?", 1009 * 180));
            foreach (var note in notes)
            {
                var st = parser.Parse(note.Item1);
                Assert.AreNotEqual(SentenceTypes.Invalid, st.Type, $"invalid: {note.Item1}");
                if (st.Type == SentenceTypes.CommodityValueQuery || st.Type == SentenceTypes.RomanNumberTranslationQuery)
                {
                    Assert.AreEqual(note.Item2, parser.Evaluate(st));
                }
            }
        }
        [TestMethod()]
        public void EvaluateMultipleSentencesTest2()
        {
            List<Tuple<string, int>> notes = new List<Tuple<string, int>>();
            notes.Add(new Tuple<string, int>(@"glob is I", -1));
            notes.Add(new Tuple<string, int>(@"prok is V", -1));
            notes.Add(new Tuple<string, int>(@"pish is X", -1));
            notes.Add(new Tuple<string, int>(@"tegj is L", -1));
            notes.Add(new Tuple<string, int>(@"glob glob Silver is 34 Credits", -1));
            notes.Add(new Tuple<string, int>(@"glob prok Gold is 57800 Credits", -1));
            notes.Add(new Tuple<string, int>(@"pish pish Iron is 3910 Credits", -1));
            notes.Add(new Tuple<string, int>(@"how much is pish tegj glob glob ?", 42));
            notes.Add(new Tuple<string, int>(@"how many Credits is glob prok Silver ?", 68));
            notes.Add(new Tuple<string, int>(@"how many Credits is glob prok Gold ?", 57800));
            notes.Add(new Tuple<string, int>(@"how many Credits is glob prok Iron ?", 782));
            foreach (var note in notes)
            {
                var st = parser.Parse(note.Item1);
                Assert.AreNotEqual(SentenceTypes.Invalid, st.Type, $"invalid: {note.Item1}");
                if (st.Type == SentenceTypes.CommodityValueQuery || st.Type == SentenceTypes.RomanNumberTranslationQuery)
                {
                    Assert.AreEqual(note.Item2, parser.Evaluate(st));
                }
            }
        }
    }
}