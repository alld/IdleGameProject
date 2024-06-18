using DG.DemiEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace IdleGame.Data.Numeric
{
    static class DataLimit
    {
        public const int Int = 10000;
    }

    [Serializable]
    public struct ExactInt
    {
        public List<int> Value;
        public int Scale;
        public bool IsPositive;

        // Value 값에 IntLimit 이상의 값이 들어가지 않는 것을 원칙으로 한다.
        public ExactInt (int value, bool isPositive = true, int scale = 0)
        {
            Value = new List<int>(new int[scale+1]);
            Scale = scale;
            IsPositive = isPositive;
            Value[scale] = value;
        }

        public ExactInt(List<int> value, bool isPositive = true, int scale = 0)
        {
            Value = value;
            Scale = scale;
            IsPositive = isPositive;
        }

        public override string ToString ()
        {
            string result = IsPositive ? "" : "-";

            result += Value.Last().ToString();

            if (Value.Count > 1)
            {
                string decimalString = Value[Value.Count - 2].ToString();
                if (decimalString.Length > 2)
                {
                    result += '.';
                    decimalString = decimalString.Length > 3 ? decimalString : '0' + decimalString;
                    result += decimalString[1] == '0' ? decimalString[0] : decimalString.Substring(0, 2);
                }
            }

            result += ScaleToAlphabet(Scale);

            return result;
        }

        public static string ScaleToAlphabet(int scale)
        {
            string result = string.Empty;
            while (scale > 0)
            {
                scale--; // 1을 빼서 0부터 시작하도록 조정
                int remainder = scale % 26;
                result = (char)(remainder + 'a') + result;
                scale /= 26;
            }
            return result;
        }

        private void Normalize()
        {
            for (int i = 0; i < Value.Count; ++i)
            {
                if (Value[i] < DataLimit.Int) continue;

                if (i == Value.Count - 1)
                {
                    Value.Add(0);
                }

                Value[i + 1] += Value[i] / DataLimit.Int;
                Value[i] %= DataLimit.Int;
            }

            Scale = Value.Count - 1;
        }

        private static int CompareAbsoluteValues(ExactInt a, ExactInt b)
        {
            if (a.Scale != b.Scale)
            {
                return a.Scale > b.Scale ? 1 : -1;
            }

            for (int i = a.Value.Count - 1; i >= 0; --i)
            {
                if (a.Value[i] != b.Value[i])
                {
                    return a.Value[i] > b.Value[i] ? 1 : -1;
                }
            }

            return 0;
        }

        private static void AlignScales(ref ExactInt a, ref ExactInt b)
        {
            int scaleDiff = Math.Abs(a.Scale - b.Scale);
            if (a.Scale > b.Scale)
            {
                for (int i = 0; i < scaleDiff; ++i)
                {
                    b.Value.Add(0);
                }
            }
            else if (b.Scale > a.Scale)
            {
                for (int i = 0; i < scaleDiff; ++i)
                {
                    a.Value.Add(0);
                }
            }
        }
        
        public static ExactInt operator + (ExactInt a, ExactInt b)
        {
            if (a.IsPositive != b.IsPositive)
            {
                b.IsPositive = !b.IsPositive;
                return a - b;
            }
            
            AlignScales(ref a, ref b);
            for (int i = 0; i < a.Value.Count; ++i)
            {
                a.Value[i] += b.Value[i];
            }
            
            a.Normalize();
            
            return a;
        }

        public static ExactInt operator -(ExactInt a, ExactInt b)
        {
            if (a.IsPositive != b.IsPositive)
            {
                b.IsPositive = !b.IsPositive;

                return a + b;
            }

            int comparison = CompareAbsoluteValues(a, b);

            if (comparison == 0)
            {
                return new ExactInt(0);
            }
            
            AlignScales(ref a, ref b);
            a.IsPositive = comparison > 0 ? a.IsPositive : !a.IsPositive;

            if (comparison > 0)
            {
                for (int i = 0; i < a.Value.Count; ++i)
                {
                    a.Value[i] -= b.Value[i];
                }
            }
            else
            {
                for (int i = 0; i < a.Value.Count; ++i)
                {
                    a.Value[i] = b.Value[i] - a.Value[i];
                }
            }

            for (int i = a.Value.Count - 1; i > 0; --i)
            {
                if (a.Value[i - 1] < 0)
                {
                    a.Value[i - 1] += DataLimit.Int;
                    if (--a.Value[i] == 0 && i == a.Value.Count - 1)
                    {
                        a.Value.RemoveAt(i);
                    }
                }
            }
            
            a.Normalize();

            return a;
        }

        public static ExactInt operator *(ExactInt a, ExactInt b)
        {
            List<int> resultValue = new List<int>(new int[a.Value.Count + b.Value.Count - 1]);
            for (int i = 0; i < a.Value.Count; ++i)
            {
                for (int j = 0; j < b.Value.Count; ++j)
                {
                    resultValue[i + j] += a.Value[i] * b.Value[j];
                }
            }

            bool resultIsPositive = a.IsPositive == b.IsPositive;
            ExactInt result = new ExactInt(resultValue, resultIsPositive, a.Scale + b.Scale);
            result.Normalize();
            return result;
        }

        public static ExactInt operator /(ExactInt a, ExactInt b)
        {
            if (b.Value.Last() == 0)
            {
                throw new DivideByZeroException();
            }

            a.Scale -= b.Scale;
            a.IsPositive = a.IsPositive == b.IsPositive;

            int temp = 0;
            for (int i = a.Value.Count - 1; i >= 0; --i)
            {
                temp = temp * DataLimit.Int + a.Value[i];
                a.Value[i] = temp / b.Value.Last();
                if (i > 0 && a.Value[i] == 0 && i == a.Value.Count - 1)
                {
                    a.Value.RemoveAt(i);
                }
                temp %= b.Value.Last();
            }
            
            a.Normalize();

            return a;
        }
    }
}