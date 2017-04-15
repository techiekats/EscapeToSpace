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
                //check max repetitions of I,X, C, M
                valid = !(romanDigits.Count(c=> c=='I') > 3 || romanDigits.Count(c => c == 'X') > 3 || romanDigits.Count(c => c == 'C') > 3 || romanDigits.Count(c => c == 'M') > 3);
                reasonIfInvalid = "Characters I,X,C,M can appear only thrice in a continuous sequence";
            }
            if (valid)
            {
                //check sequence
                int[] digits = GetIntForEachCharacter(romanDigits);
                for (int i = 0; i < digits.Length - 1 && valid; i++)
                {
                    int current = digits[i];
                    int next = digits[i + 1];
                    int previous = (i == 0 ? int.MaxValue : digits[i-1]);
                    if (next > current)
                    {
                        if (current == Symbols.V || current == Symbols.L || current == Symbols.D)
                        {
                            valid = false; //V, L and D can never be subtracted
                        }
                        /*"I" can be subtracted from "V" and "X" only. "X" can be subtracted from "L" and "C" only. "C" can be subtracted from
"D" and "M" only. "V", "L", and "D" can never be subtracted*/
                        else if (previous <= current && current < next)
                        {
                            valid = false;
                        }
                        else if (current == Symbols.I && !(next == Symbols.V || next == Symbols.X))
                        {
                            valid = false;
                        }
                        else if (current == Symbols.X && !(next == Symbols.L || next == Symbols.C))
                        {
                            valid = false;
                        }
                        else if (current == Symbols.C && !(next == Symbols.D || next == Symbols.M))
                        {
                            valid = false;
                        }
                    }
                }
                reasonIfInvalid = "Invalid number sequence";
            }
            reasonIfInvalid = valid ? string.Empty : reasonIfInvalid;
            return valid;
        }
        private static int GetIntegerValue (string romanDigits)
        {
            int arabicNumber = 0;
            int[] digits = GetIntForEachCharacter(romanDigits);
            for (int i=0; i< digits.Length; i++)
            {
                int current = digits[i];
                int next = (i == digits.Length - 1 ? int.MinValue : digits[i + 1]);
                if (next > current)
                {
                    arabicNumber -= digits[i];
                }
                else
                {
                    arabicNumber += digits[i];
                }
            }
            return arabicNumber;
        }
        private static int[] GetIntForEachCharacter (string romanDigits)
        {
            int[] numbers = new int[romanDigits.Length];
            for (int i = 0; i < romanDigits.Length; i++)
            {
                char c = romanDigits[i];
                switch (c)
                {
                    case 'I':
                        numbers[i] = Symbols.I;
                        break;
                    case 'V':
                        numbers[i] = Symbols.V;
                        break;
                    case 'X':
                        numbers[i] = Symbols.X;
                        break;
                    case 'L':
                        numbers[i] = Symbols.L;
                        break;
                    case 'C':
                        numbers[i] = Symbols.C;
                        break;
                    case 'D':
                        numbers[i] = Symbols.D;
                        break;
                    case 'M':
                        numbers[i] = Symbols.M;
                        break;
                    default:
                        throw new InvalidCastException();                
                }
            }
            return numbers;
        }
    }
}
