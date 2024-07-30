using DG.DemiEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IdleGame.Data.Numeric
{
    static class DataLimit
    {
        public const int Int = 1000;
    }

    /// <summary>
    /// [데이터] 단위 환산식 수식에 사용되는 커스텀 숫자입니다. 
    /// </summary>
    [Serializable]
    public struct ExactInt
    {
        /// <summary>
        /// [데이터] 단위별 해당하는 값입니다.
        /// </summary>
        public List<int> Value;
        /// <summary>
        /// [데이터] 현재 숫자의 단위 값입니다.
        /// </summary>
        public int Scale;
        /// <summary>
        /// [데이터] 표현된 값이 음수인지, 양수인지를 나타냅니다. 
        /// </summary>
        public bool IsPositive;

        /// <summary>
        /// [캐시] 변환에 사용되는 스트링빌더입니다. 입력받은 스트링중에 숫자만을 담습니다.
        /// </summary>
        private static StringBuilder _Sb_digit = new StringBuilder(10);
        /// <summary>
        /// [캐시] 변환에 사용되는 스트링빌더입니다. 입력받은 스트링중에 문자만을 담습니다. 
        /// </summary>
        private static StringBuilder _Sb_letter = new StringBuilder(10);

        // Value 값에 IntLimit 이상의 값이 들어가지 않는 것을 원칙으로 한다.
        public ExactInt(int value, bool isPositive = true, int scale = 0)
        {
            Value = new List<int>(new int[scale + 1]);
            Scale = scale;
            IsPositive = isPositive;
            Value[scale] = value;
        }

        public ExactInt(int value, int scale)
        {
            Value = new List<int>(new int[scale + 1]);
            Scale = scale;
            IsPositive = value >= 0;
            Value[scale] = value;
        }

        public ExactInt(int value)
        {
            Value = ConvertNumber(value);
            Scale = Value.Count - 1;
            IsPositive = value >= 0;
        }

        public ExactInt(long value)
        {
            Value = ConvertNumber(value);
            Scale = Value.Count - 1;
            IsPositive = value >= 0;
        }

        public ExactInt(List<int> value, bool isPositive = true, int scale = 0)
        {
            Value = value;
            Scale = scale;
            IsPositive = isPositive;
        }

        public static bool CompareZero(ExactInt m_value)
        {
            if (m_value.Scale != 0)
                return false;

            if (m_value.Value[0] != 0)
                return false;

            return true;
        }

        public static List<int> ConvertNumber(int m_value)
        {
            return ConvertNumber((long)m_value);
        }

        public static List<int> ConvertNumber(long m_value)
        {
            List<int> result = new List<int>();
            m_value = Math.Abs(m_value);

            while (true)
            {
                if (m_value < 10000)
                {
                    result.Add((int)m_value);
                    break;
                }

                m_value /= 10000;
            }

            return result;
        }

        public override string ToString()
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


        public static ExactInt operator +(ExactInt a, ExactInt b)
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

        public static ExactInt operator +(ExactInt a, int b)
        {
            ExactInt num_b = new ExactInt(b);

            return a + num_b;
        }

        public static ExactInt operator +(ExactInt a, long b)
        {
            ExactInt num_b = new ExactInt(b);

            return a + num_b;
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

        public static ExactInt operator -(ExactInt a, int b)
        {
            ExactInt num_b = new ExactInt(b);

            return a - num_b;
        }

        public static ExactInt operator -(ExactInt a, long b)
        {
            ExactInt num_b = new ExactInt(b);

            return a - num_b;
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

        public static ExactInt operator *(ExactInt a, int b)
        {
            ExactInt num_b = new ExactInt(b);

            return a * num_b;
        }

        public static ExactInt operator *(ExactInt a, long b)
        {
            ExactInt num_b = new ExactInt(b);

            return a * num_b;
        }

        public static ExactInt operator /(ExactInt a, ExactInt b)
        {
            if (b.Value.Last() == 0)
            {
                throw new DivideByZeroException();
            }

            if (a.Scale < b.Scale || (a.Scale == b.Scale && a.Value[1] < b.Value[1])) return new ExactInt(0);

            a.Scale -= b.Scale;
            a.IsPositive = a.IsPositive == b.IsPositive;

            int devideValue = b.Value.Last() * DataLimit.Int;
            if (b.Value.Count() > 1)
            {
                devideValue += b.Value[b.Value.Count() - 2];
            }
            int temp = 0;
            for (int i = a.Value.Count - 1; i > 0; --i)
            {
                temp = (temp * DataLimit.Int + a.Value[i]) * DataLimit.Int + a.Value[i - 1];
                a.Value[i] = temp / devideValue;
                if (i > 0 && a.Value[i] == 0 && i == a.Value.Count - 1)
                {
                    a.Value.RemoveAt(i);
                }
                temp %= devideValue;
            }
            a.Value[0] = (temp * DataLimit.Int + a.Value[0]) / b.Value.Last();

            a.Normalize();

            return a;
        }

        public static ExactInt operator /(ExactInt a, int b)
        {
            ExactInt num_b = new ExactInt(b);

            return a / num_b;
        }

        public static ExactInt operator /(ExactInt a, long b)
        {
            ExactInt num_b = new ExactInt(b);

            return a / num_b;
        }

        public static bool operator <(ExactInt a, ExactInt b)
        {
            if (a.IsPositive != b.IsPositive)
            {
                return !a.IsPositive;
            }

            if (a.Scale != b.Scale)
            {
                return a.Scale < b.Scale;
            }

            for (int i = a.Value.Count - 1; i >= 0; --i)
            {
                if (a.Value[i] == b.Value[i]) continue;
                return a.Value[i] < b.Value[i];
            }

            return false;
        }
        public static bool operator >(ExactInt a, ExactInt b)
        {
            if (a.IsPositive != b.IsPositive)
            {
                return a.IsPositive;
            }

            if (a.Scale != b.Scale)
            {
                return a.Scale > b.Scale;
            }

            for (int i = a.Value.Count - 1; i >= 0; --i)
            {
                if (a.Value[i] == b.Value[i]) continue;
                return a.Value[i] > b.Value[i];
            }

            return false;
        }

        public static bool operator ==(ExactInt a, ExactInt b)
        {
            if (CompareZero(a) == CompareZero(b)) return true;

            if (a.IsPositive != b.IsPositive || a.Scale != b.Scale) return false;

            for (int i = a.Value.Count - 1; i >= 0; --i)
            {
                if (a.Value[i] == b.Value[i]) continue;
                return false;
            }

            return true;
        }

        public static bool operator ==(ExactInt a, int b)
        {
            ExactInt num_b = new ExactInt(b);

            return a == num_b;
        }

        public static bool operator ==(ExactInt a, long b)
        {
            ExactInt num_b = new ExactInt(b);

            return a == num_b;
        }

        public static bool operator !=(ExactInt a, ExactInt b)
        {
            if (a.IsPositive != b.IsPositive || a.Scale != b.Scale) return true;

            for (int i = a.Value.Count - 1; i >= 0; --i)
            {
                if (a.Value[i] == b.Value[i]) continue;
                return true;
            }

            return false;
        }

        public static bool operator !=(ExactInt a, int b)
        {
            ExactInt num_b = new ExactInt(b);

            return a != num_b;
        }

        public static bool operator !=(ExactInt a, long b)
        {
            ExactInt num_b = new ExactInt(b);

            return a != num_b;
        }

        public static bool operator <=(ExactInt a, ExactInt b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(ExactInt a, ExactInt b)
        {
            return a > b || a == b;
        }

        public static ExactInt Parse(string m_value)
        {
            _Sb_digit.Clear();
            _Sb_letter.Clear();

            for (int i = 0; i < m_value.Length; i++)
            {
                if (char.IsDigit(m_value[i]))
                    _Sb_digit.Append(m_value[i]);
                else
                    _Sb_letter.Append(m_value[i]);
            }

            return new ExactInt(_Sb_digit.Length == 0 ? 0 : int.Parse(_Sb_digit.ToString()), Convert_CharToScale(_Sb_letter.ToString()));
        }

        /// <summary>
        /// [변환] 입력받은 문자를 토대로 스케일값을 반환합니다. 
        /// </summary>
        public static int Convert_CharToScale(string m_value)
        {
            if (m_value.IsNullOrEmpty())
                return 0;

            m_value = m_value.ToUpper();

            int changeNumber = 0;

            for (int i = 0; i < m_value.Length; i++)
            {
                if (Char.IsDigit(m_value[i]))
                    continue;

                int charDigit = m_value[i];
                if (charDigit >= 65 && charDigit <= 90)
                    changeNumber += (charDigit - 64) * (int)Math.Pow(26, i);
            }

            return changeNumber;
        }

        /// <summary>
        /// [사용하지않음]
        /// </summary>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        /// <summary>
        /// [사용하지않음]
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [Serializable]
    public struct SimpleInt
    {
        public List<int> Value;
        public int Scale;
        public bool IsPositive;

        public SimpleInt(int value, bool isPositive = true, int scale = 0)
        {
            Value = new List<int>(new int[2]);
            Value[1] = value;
            IsPositive = isPositive;
            Scale = scale;
        }

        public SimpleInt(List<int> value, bool isPositive = true, int scale = 0)
        {
            Value = value;
            IsPositive = isPositive;
            Scale = scale;
        }

        public override string ToString()
        {
            string result = IsPositive ? "" : "-";

            result += Value.Last().ToString();

            string decimalString = Value[0].ToString();
            if (decimalString.Length > 2)
            {
                result += '.';
                decimalString = decimalString.Length > 3 ? decimalString : '0' + decimalString;
                result += decimalString[1] == '0' ? decimalString[0] : decimalString.Substring(0, 2);
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

        void Normalize()
        {
            for (int i = 0; i < Value.Count(); ++i)
            {
                if (Value[i] >= DataLimit.Int)
                {
                    if (i == Value.Count() - 1)
                    {
                        Value.Add(0);
                        Scale++;
                    }

                    Value[i + 1] += Value[i] / DataLimit.Int;
                    Value[i] %= DataLimit.Int;
                }
            }

            Value = Value.Skip(Value.Count() - 2).ToList();
        }

        public static SimpleInt operator +(SimpleInt a, SimpleInt b)
        {
            if (a.IsPositive != b.IsPositive)
            {
                b.IsPositive = !b.IsPositive;
                return a - b;
            }

            int scaleDiff = a.Scale - b.Scale;
            if (Math.Abs(scaleDiff) > 1)
            {
                // 단위가 2 이상 차이가 나는 경우
                if (scaleDiff > 0)
                {
                    a.Value[0]++;
                }
                else
                {
                    b.Value[0]++;
                    a.Value = b.Value;
                    a.Scale = b.Scale;
                }
            }
            else if (Math.Abs(scaleDiff) == 1)
            {
                // 단위가 차이가 1 인 경우
                if (scaleDiff > 0)
                {
                    a.Value[0] += b.Value[1];
                }
                else
                {
                    b.Value[0] += a.Value[1];
                    a.Value = b.Value;
                    a.Scale = b.Scale;
                }
            }
            else
            {
                // 단위가 같은 경우
                a.Value[0] += b.Value[0];
                a.Value[1] += b.Value[1];
            }

            a.Normalize();
            return a;
        }

        public static SimpleInt operator -(SimpleInt a, SimpleInt b)
        {
            if (a.IsPositive != b.IsPositive)
            {
                b.IsPositive = !b.IsPositive;

                return a + b;
            }

            int scaleDiff = a.Scale - b.Scale;
            if (Math.Abs(scaleDiff) > 1)
            {
                if (scaleDiff > 0)
                {
                    a.Value[0]--;
                }
                else
                {
                    a.Value[1] = b.Value[1];
                    a.Value[0] = b.Value[0] - 1;
                    a.IsPositive = !a.IsPositive;
                    a.Scale = b.Scale;
                }
            }
            else if (Math.Abs(scaleDiff) == 1)
            {
                if (scaleDiff > 0)
                {
                    a.Value[0] -= b.Value[1];
                }
                else
                {
                    a.Value[1] = b.Value[1];
                    a.Value[0] = b.Value[0] - a.Value[1];
                    a.IsPositive = !a.IsPositive;
                    a.Scale = b.Scale;
                }
            }
            else
            {
                a.Value[1] -= b.Value[1];
                if (a.Value[1] < 0)
                {
                    a.Value[1] *= -1;
                    a.IsPositive = !a.IsPositive;
                }
                a.Value[0] -= b.Value[0];
            }

            for (int i = a.Value.Count - 1; i > 0; --i)
            {
                if (a.Value[i - 1] < 0)
                {
                    a.Value[i]--;
                    a.Value[i - 1] += DataLimit.Int;
                }
            }

            if (a.Value[1] == 0 && a.Value[0] != 0)
            {
                a.Value[1] = a.Value[0];
                a.Value[0] = 0;
                a.Scale = a.Scale > 0 ? a.Scale-- : 0;
            }

            a.Normalize();
            return a;
        }

        public static SimpleInt operator *(SimpleInt a, SimpleInt b)
        {
            if (a.Value[1] == 0 || b.Value[1] == 0) return new SimpleInt(0);
            List<int> resultValue = new List<int>(new int[a.Value.Count + b.Value.Count - 1]);
            for (int i = 0; i < a.Value.Count; ++i)
            {
                for (int j = 0; j < b.Value.Count; ++j)
                {
                    resultValue[i + j] += a.Value[i] * b.Value[j];
                }
            }

            SimpleInt result = new SimpleInt(resultValue, a.IsPositive == b.IsPositive, a.Scale + b.Scale);
            result.Normalize();
            return result;
        }

        public static SimpleInt operator /(SimpleInt a, SimpleInt b)
        {
            if (b.Value.Last() == 0)
            {
                throw new DirectoryNotFoundException();
            }

            if (a.Scale < b.Scale || (a.Scale == b.Scale && a.Value[1] < b.Value[1])) return new SimpleInt(0);

            a.Scale -= b.Scale;
            a.IsPositive = a.IsPositive == b.IsPositive;

            int devideValue = b.Value[1] * DataLimit.Int + b.Value[0];
            int temp = (a.Value[1] * DataLimit.Int) + a.Value[0];
            a.Value[1] = temp / devideValue;
            temp = temp % devideValue;
            a.Value[0] = (temp * DataLimit.Int + a.Value[0]) / b.Value[1];

            if (a.Value[1] == 0 && a.Value[0] != 0)
            {
                a.Value[1] = a.Value[0];
                a.Value[0] = 0;
                a.Scale = a.Scale > 0 ? a.Scale-- : 0;
            }

            a.Normalize();
            return a;
        }

        public static bool operator <(SimpleInt a, SimpleInt b)
        {
            if (a.IsPositive != b.IsPositive)
            {
                return !a.IsPositive;
            }

            if (a.Scale != b.Scale)
            {
                return a.Scale < b.Scale;
            }

            for (int i = a.Value.Count - 1; i >= 0; --i)
            {
                if (a.Value[i] == b.Value[i]) continue;
                return a.Value[i] < b.Value[i];
            }

            return false;
        }
        public static bool operator >(SimpleInt a, SimpleInt b)
        {
            if (a.IsPositive != b.IsPositive)
            {
                return a.IsPositive;
            }

            if (a.Scale != b.Scale)
            {
                return a.Scale > b.Scale;
            }

            for (int i = a.Value.Count - 1; i >= 0; --i)
            {
                if (a.Value[i] == b.Value[i]) continue;
                return a.Value[i] > b.Value[i];
            }

            return false;
        }

        public static bool operator ==(SimpleInt a, SimpleInt b)
        {
            if (a.IsPositive != b.IsPositive || a.Scale != b.Scale) return false;

            for (int i = a.Value.Count - 1; i >= 0; --i)
            {
                if (a.Value[i] == b.Value[i]) continue;
                return false;
            }

            return true;
        }

        public static bool operator !=(SimpleInt a, SimpleInt b)
        {
            if (a.IsPositive != b.IsPositive || a.Scale != b.Scale) return true;

            for (int i = a.Value.Count - 1; i >= 0; --i)
            {
                if (a.Value[i] == b.Value[i]) continue;
                return true;
            }

            return false;
        }

        public static bool operator <=(SimpleInt a, SimpleInt b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(SimpleInt a, SimpleInt b)
        {
            return a > b || a == b;
        }
        /// <summary>
        /// [사용하지않음]
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// [사용하지않음]
        /// </summary>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}