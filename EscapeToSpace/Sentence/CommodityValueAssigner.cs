﻿using System;
using System.Linq;
using System.Collections.Generic;
using EscapeToSpace.Enums;

namespace EscapeToSpace
{
    class CommodityValueAssigner : Sentence
    {
        private string commodityName = string.Empty;
        private int unitPrice = 0;
        public override Sentence Parse(string sentence, ParseTableReader reader)
        {
            string[] tokenStrings = sentence.Split(new char[] { ' ' }).Where(s => s != "").ToArray<string>();

            List<Token> parsedTokens = new List<Token>();
            string commodity = string.Empty;
            type = SentenceTypes.CommodityDefinition;
            for (int i = 0; i < tokenStrings.Length; i++)
            {
                int result;
                if (reader.GetTranslationForAlienDigit(tokenStrings[i].Trim().ToLower()) != Char.MinValue)
                {
                    if (i == 0 || parsedTokens.Last().Type == TokenTypes.AlienDigit)
                    {
                        parsedTokens.Add(new Token(tokenStrings[i], TokenTypes.AlienDigit));
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }

                else if (tokenStrings[i].Equals("is", StringComparison.CurrentCultureIgnoreCase))
                {
                    parsedTokens.Add(new Token(tokenStrings[i], TokenTypes.Equality));
                }
                else if (tokenStrings[i].Equals("Credits", StringComparison.CurrentCultureIgnoreCase))
                {
                    parsedTokens.Add(new Token(tokenStrings[i], TokenTypes.Unit));
                }
                else if (int.TryParse(tokenStrings[i], out result))
                {
                    if (parsedTokens.Last().Type == TokenTypes.Equality)
                    {
                        parsedTokens.Add(new Token(tokenStrings[i], TokenTypes.UnitPrice));
                        unitPrice = int.Parse(tokenStrings[i]);
                    }
                    else
                    {
                        throw new FormatException();
                    }
                }
                else if (parsedTokens.Last()?.Type == TokenTypes.AlienDigit)
                {
                    parsedTokens.Add(new Token(tokenStrings[i], TokenTypes.Commodity));
                    commodity = tokenStrings[i];
                }
            }
            return this;
        }
        public Tuple<string,int> GetCommodityUnitPrice ()
        {
            return new Tuple<string, int>(commodityName, unitPrice);
        }
    }
}