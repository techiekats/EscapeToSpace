using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeToSpace
{
    public class RomanNumber
    {
        private struct Symbols
        {
            public const int I = 1;
            public const int V = 5;
            public const int X = 10;
            public const int L = 50;
            public const int C = 100;
            public const int D = 500;
            public const int M = 1000;
        }
        private RomanNumber ()
        {
            //never allow direct instantiation so that invalid instances never exist
        }
        public static int Parse (string romanDigits)
        {
            romanDigits = romanDigits.ToUpper();
            string reasonIfInvalid;
            if (Valid(romanDigits, out reasonIfInvalid))
            {
                return GetIntegerValue(romanDigits);
            }
            else
            {
                throw new FormatException(reasonIfInvalid);
            }
        }
        private static bool Valid (string romanDigits, out string reasonIfInvalid)
        {
            bool valid = true;
            reasonIfInvalid = "Characters other than I, V, X, C, D, M are not allowed";
            //test if any character is incorrect
            valid = romanDigits.All(c => (c == 'I' || c == 'V' || c == 'X' || c == 'L' || c == 'C' || c == 'D' || c == 'M'));
            if (valid)
            {
                //check max occurrences of DLV
                valid = !(romanDigits.Count(c=> c == 'D') > 1 || romanDigits.Count(c => c == 'L') > 1 || romanDigits.Count(c => c== 'V') > 1);
                reasonIfInvalid = "Characters D,L,V must not be repeated";
            }
            if (valid)
            {
                //check max repetitiona of I,X, C, M
                valid = !(romanDigits.Count(c=> c=='I') > 3 || romanDigits.Count(c => c == 'X') > 3 || romanDigits.Count(c => c == 'C') > 3 || romanDigits.Count(c => c == 'M') > 3);
                reasonIfInvalid = "Characters I,X,C,M can appear only thrice in a continuous sequence";
            }
            if (valid)
            {
                //check sequence
            }
            reasonIfInvalid = valid ? string.Empty : reasonIfInvalid;
            return valid;
        }
        private static int GetIntegerValue (string romanDigits)
        {
            int arabicNumber = 0;
            return arabicNumber;
        }
    }
}
