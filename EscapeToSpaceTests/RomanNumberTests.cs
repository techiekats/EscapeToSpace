using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace EscapeToSpace.Tests
{
    [TestClass()]
    public class RomanNumberTests
    {
        [TestMethod()]
        public void ParseTestForInvalidCharacter()
        {
            List<string> invalidNumbers = new List<string>();
            invalidNumbers.Add("In"); //invalid character 'n'
            foreach (var t in invalidNumbers)
            {
                try
                {
                    RomanNumber.Parse(t);
                    Assert.Fail(); //execution reaching this line implies successful parse.
                }
                catch (Exception exception)
                {
                    Assert.AreEqual(typeof(FormatException), exception.GetType());
                    Assert.AreEqual("Characters other than I, V, X, C, D, M are not allowed", exception.Message);
                }
            }
        }
        [TestMethod()]
        public void ParseTestForInvalidCharacterRepeated()
        {
            List<string> invalidNumbers = new List<string>();
            invalidNumbers.Add("DDC"); //D can never be repeated
            invalidNumbers.Add("LLVM"); //L can never be repeated
            invalidNumbers.Add("VIV"); //V must never be repeated
            foreach (var t in invalidNumbers)
            {
                try
                {
                    RomanNumber.Parse(t);
                    Assert.Fail(); //execution reaching this line implies successful parse.
                }
                catch (Exception exception)
                {
                    Assert.AreEqual(typeof(FormatException), exception.GetType());
                    Assert.AreEqual("Characters D,L,V must not be repeated", exception.Message);
                }
            }
        }
        
        [TestMethod()]
        public void ParseTestForInvalidCharacterRepetitions()
        {
            List<string> invalidNumbers = new List<string>();
            invalidNumbers.Add("XXXXV");
            invalidNumbers.Add("MCCCCD");
            invalidNumbers.Add("MMMMV");
            invalidNumbers.Add("IIII"); //I repeated 4 times
            foreach (var t in invalidNumbers)
            {
                try
                {
                    RomanNumber.Parse(t);
                    Assert.Fail(); //execution reaching this line implies successful parse.
                }
                catch (Exception exception)
                {
                    Assert.AreEqual(typeof(FormatException), exception.GetType());
                    Assert.AreEqual("Characters I,X,C,M can appear only thrice in a continuous sequence", exception.Message);
                }
            }
        }
        [TestMethod()]
        public void ParseTestForInvalidCharacterSequence()
        {//"I" can be subtracted from "V" and "X" only. "X" can be subtracted from "L" and "C" only. "C" can be subtracted from
          //  "D" and "M" only. "V", "L", and "D" can never be subtracted.
            List<string> invalidNumbers = new List<string>();
            invalidNumbers.Add("IC");//I can be subtracted only from V and X
            invalidNumbers.Add("IM"); //I can be subtracted only from V and X
            invalidNumbers.Add("ID");
            invalidNumbers.Add("IL");
            invalidNumbers.Add("MXM"); //X can be subtracted from L and C only; 
            invalidNumbers.Add("MVX"); //V must never be subtracted
            invalidNumbers.Add("MVC");
            invalidNumbers.Add("MVM");
            invalidNumbers.Add("MLC"); //L must never be subtracted
            invalidNumbers.Add("MLM");
            invalidNumbers.Add("MDM"); //D must never be subtracted
            invalidNumbers.Add("IIV"); //only 1 small value must be subtracted
            invalidNumbers.Add("IVX");
            foreach (var t in invalidNumbers)
            {
                try
                {
                    RomanNumber.Parse(t);
                    Assert.Fail(); //execution reaching this line implies successful parse.
                }
                catch (Exception exception)
                {
                    Assert.AreEqual(typeof(FormatException), exception.GetType());
                    Assert.AreEqual("Invalid number sequence", exception.Message);
                }
            }
        }
        [TestMethod()]
        public void ParseValidNumbers ()
        {
            List<Tuple<string, int>> validRomanNumbers = new List<Tuple<string, int>>();
            validRomanNumbers.Add(new Tuple<string, int>("mmvi", 2006));
            validRomanNumbers.Add(new Tuple<string, int>("iv", 4));
            validRomanNumbers.Add(new Tuple<string, int>("x", 10));
            validRomanNumbers.Add(new Tuple<string, int>("cm", 900));
            validRomanNumbers.Add(new Tuple<string, int>("mm", 2000));
            validRomanNumbers.Add(new Tuple<string, int>("mcmxcix", 1999));
            validRomanNumbers.Add(new Tuple<string, int>("cxlviii", 148));
            validRomanNumbers.Add(new Tuple<string, int>("III", 3));
            validRomanNumbers.Add(new Tuple<string, int>("clxxxvi", 186));
            foreach (var t in validRomanNumbers)
            {
                Assert.AreEqual(t.Item2, RomanNumber.Parse(t.Item1));
            }
        }
    }
}