using Microsoft.VisualStudio.TestTools.UnitTesting;
using EscapeToSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}