using DG.DemiEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IdleGame.Data.Numeric
{
    /// <summary>
    /// [데이터] 단위 환산식 수식에 사용되는 커스텀 숫자입니다. 
    /// </summary>
    [Serializable]
    public struct ExactInt
    {
        /// <summary>
        /// [캐시] 데이터를 구성하는 단위입니다.
        /// </summary>
        private const int LimitInt = 1000;

        /// <summary>
        /// [캐시] 변환에 사용되는 스트링빌더입니다. 입력받은 스트링중에 숫자만을 담습니다.
        /// </summary>
        private static StringBuilder _Sb_digit = new StringBuilder(10);

        /// <summary>
        /// [캐시] 변환에 사용되는 스트링빌더입니다. 입력받은 스트링중에 문자만을 담습니다. 
        /// </summary>
        private static StringBuilder _Sb_letter = new StringBuilder(10);

        #region 구성
        /// <summary>
        /// [데이터] 단위별 해당하는 값입니다.
        /// </summary>
        public int[] value;
        /// <summary>
        /// [데이터] 현재 숫자의 단위 값입니다.
        /// </summary>
        public int scale;
        /// <summary>
        /// [데이터] 표현된 값이 음수인지, 양수인지를 나타냅니다. 
        /// <br> true : 양수</br>
        /// <br> false : 음수 </br>
        /// </summary>
        public bool isPositive;
        #endregion


        #region 생성자
        /// <summary>
        /// [생성자] 모든 구성을 일일이 지정하여 생성합니다. 
        /// </summary> 
        public ExactInt(int m_value, bool m_isPositive = true, int m_scale = 0)
        {
            value = new int[m_scale + 1];
            scale = m_scale;
            isPositive = m_isPositive;
            value[m_scale] = m_value;
        }

        /// <summary>
        /// [생성자] 값과 스케일을 지정하여 생성합니다.
        /// </summary>
        public ExactInt(int m_value, int m_scale)
        {
            value = new int[m_scale + 1];
            scale = m_scale;
            isPositive = m_value >= 0;
            value[m_scale] = m_value;
        }

        /// <summary>
        /// [생성자] 인트를 가지고 
        /// </summary>
        public ExactInt(int m_value)
        {
            value = ConvertNumber(m_value);
            scale = value.Length - 1;
            isPositive = m_value >= 0;
        }

        public ExactInt(long m_value)
        {
            value = ConvertNumber(m_value);
            scale = value.Length - 1;
            isPositive = m_value >= 0;
        }

        public ExactInt(int[] m_value, bool m_isPositive = true, int m_scale = 0)
        {
            value = m_value;
            scale = m_scale;
            isPositive = m_isPositive;
        }

        #region 형변환
        public static explicit operator ExactInt(int b) => new ExactInt(b);
        public static explicit operator ExactInt(long b) => new ExactInt(b);


        public override string ToString()
        {
            string result = isPositive ? "" : "-";

            result += value.Last().ToString();

            if (value.Length > 1)
            {
                string decimalString = value[value.Length - 2].ToString();
                if (decimalString.Length > 2)
                {
                    result += '.';
                    decimalString = decimalString.Length > 3 ? decimalString : '0' + decimalString;
                    result += decimalString[1] == '0' ? decimalString[0] : decimalString.Substring(0, 2);
                }
            }

            result += ScaleToAlphabet(scale);

            return result;
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

        public static int[] ConvertNumber(int m_value)
        {
            return ConvertNumber((long)m_value);
        }

        public static int[] ConvertNumber(long m_value)
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

            return result.ToArray();
        }
        #endregion
        #endregion


        /// <summary>
        /// [기능] 퍼센트를 적용합니다. 
        /// </summary>
        public void SetPercent(int m_value)
        {
            this = this * m_value / 100;
        }


        /// <summary>
        /// [기능] 퍼센트를 적용합니다. 
        /// </summary>
        public void SetPercent(float m_value)
        {
            m_value *= 100;
            SetPercent((int)m_value);
        }

        /// <summary>
        /// [기능] 해당 값이 0인지를 판단합니다. 
        /// </summary>
        public static bool CompareZero(ExactInt m_value)
        {
            if (m_value.scale != 0)
                return false;

            if (m_value.value[0] != 0)
                return false;

            return true;
        }

        public static void AddValue(ref int[] m_value, int m_target)
        {
            int[] newArray = new int[m_value.Length + 1];
            for (int i = 0; i < m_value.Length; i++)
            {
                newArray[i] = m_value[i];
            }
            newArray[m_value.Length] = m_target;
            m_value = newArray;
        }

        public static void RemoveAtValue(ref int[] m_value, int m_index)
        {
            int[] newArray = new int[m_value.Length - 1];
            for (int i = 0, j = 0; i < m_value.Length; i++)
            {
                if (i != m_index)
                {
                    newArray[j++] = m_value[i];
                }
            }
            m_value = newArray;
        }

        public static void RemoveValue(ref int[] m_value, int m_target)
        {
            int count = 0;
            for (int i = 0; i < m_value.Length; i++)
            {
                if (m_value[i] != m_target)
                {
                    count++;
                }
            }

            int[] newArray = new int[count];
            for (int i = 0, j = 0; i < m_value.Length; i++)
            {
                if (m_value[i] != m_target)
                {
                    newArray[j++] = m_value[i];
                }
            }
            m_value = newArray;
        }


        #region 연산자 재정의
        #region 사칙연산

        #region 더하기
        public static ExactInt operator +(ExactInt a, ExactInt b)
        {
            // 조건 :: 
            if (a.isPositive != b.isPositive)
                return a - b;

            AlignScales(ref a, ref b);
            for (int i = 0; i < a.value.Length; ++i)
            {
                a.value[i] += b.value[i];
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

        public static ExactInt operator -(ExactInt a)
        {
            a.isPositive = false;
            return a;
        }
        #endregion
        #region 빼기
        public static ExactInt operator -(ExactInt a, ExactInt b)
        {
            b.isPositive = !b.isPositive;

            if (a.isPositive != b.isPositive)
                return a + b;


            int comparison = CompareAbsoluteValues(a, b);

            if (comparison == 0)
            {
                return new ExactInt(0);
            }

            AlignScales(ref a, ref b);
            a.isPositive = comparison > 0 ? a.isPositive : !a.isPositive;

            if (comparison > 0)
            {
                for (int i = 0; i < a.value.Length; ++i)
                {
                    a.value[i] -= b.value[i];
                }
            }
            else
            {
                for (int i = 0; i < a.value.Length; ++i)
                {
                    a.value[i] = b.value[i] - a.value[i];
                }
            }

            for (int i = a.value.Length - 1; i > 0; --i)
            {
                if (a.value[i - 1] < 0)
                {
                    a.value[i - 1] += LimitInt;
                    if (--a.value[i] == 0 && i == a.value.Length - 1)
                    {
                        RemoveAtValue(ref a.value, i);
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
        #endregion
        #region 곱하기
        public static ExactInt operator *(ExactInt a, ExactInt b)
        {
            int[] resultValue = new int[a.value.Length + b.value.Length - 1];
            for (int i = 0; i < a.value.Length; ++i)
            {
                for (int j = 0; j < b.value.Length; ++j)
                {
                    resultValue[i + j] += a.value[i] * b.value[j];
                }
            }

            bool resultIsPositive = a.isPositive == b.isPositive;
            ExactInt result = new ExactInt(resultValue, resultIsPositive, a.scale + b.scale);
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

        #endregion
        #region 나누기

        public static ExactInt operator /(ExactInt a, ExactInt b)
        {
            if (CompareZero(a) || CompareZero(b))
            {
                return new ExactInt(0);
            }

            if (a.scale < b.scale || (a.scale == b.scale && a.value[a.scale] < b.value[b.scale])) return new ExactInt(0);

            a.scale -= b.scale;
            a.isPositive = a.isPositive == b.isPositive;

            int devideValue = b.value.Last() * LimitInt;
            if (b.value.Count() > 1)
            {
                devideValue += b.value[b.value.Count() - 2];
            }
            int temp = 0;
            for (int i = a.value.Length - 1; i > 0; --i)
            {
                temp = (temp * LimitInt + a.value[i]) * LimitInt + a.value[i - 1];
                a.value[i] = temp / devideValue;
                if (i > 0 && a.value[i] == 0 && i == a.value.Length - 1)
                {
                    RemoveAtValue(ref a.value, i);
                }
                temp %= devideValue;
            }
            a.value[0] = (temp * LimitInt + a.value[0]) / b.value.Last();

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

        #endregion

        #endregion

        #region 비교 연산자

        public static bool operator <(ExactInt a, ExactInt b)
        {
            if (a.isPositive != b.isPositive)
            {
                return !a.isPositive;
            }

            if (a.scale != b.scale)
            {
                return a.scale < b.scale;
            }

            for (int i = a.value.Length - 1; i >= 0; --i)
            {
                if (a.value[i] == b.value[i]) continue;
                return a.value[i] < b.value[i];
            }

            return false;
        }

        public static bool operator >(ExactInt a, int b)
        {
            return a > new ExactInt(b);
        }
        public static bool operator >(ExactInt a, long b)
        {
            return a > new ExactInt(b);
        }

        public static bool operator >(ExactInt a, ExactInt b)
        {
            if (a.isPositive != b.isPositive)
            {
                return a.isPositive;
            }

            if (a.scale != b.scale)
            {
                return a.scale > b.scale;
            }

            for (int i = a.value.Length - 1; i >= 0; --i)
            {
                if (a.value[i] == b.value[i]) continue;
                return a.value[i] > b.value[i];
            }

            return false;
        }

        public static bool operator <(ExactInt a, int b)
        {
            return a < new ExactInt(b);
        }
        public static bool operator <(ExactInt a, long b)
        {
            return a < new ExactInt(b);
        }

        public static bool operator ==(ExactInt a, ExactInt b)
        {
            if (CompareZero(a) == CompareZero(b)) return true;

            if (a.isPositive != b.isPositive || a.scale != b.scale) return false;

            for (int i = a.value.Length - 1; i >= 0; --i)
            {
                if (a.value[i] == b.value[i]) continue;
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
            if (a.isPositive != b.isPositive || a.scale != b.scale) return true;

            for (int i = a.value.Length - 1; i >= 0; --i)
            {
                if (a.value[i] == b.value[i]) continue;
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

        public static bool operator <=(ExactInt a, int b)
        {
            ExactInt num_b = new ExactInt(b);

            return a <= num_b;
        }

        public static bool operator <=(ExactInt a, long b)
        {
            ExactInt num_b = new ExactInt(b);

            return a <= num_b;
        }

        public static bool operator >=(ExactInt a, ExactInt b)
        {
            return a > b || a == b;
        }

        public static bool operator >=(ExactInt a, int b)
        {
            ExactInt num_b = new ExactInt(b);

            return a >= num_b;
        }

        public static bool operator >=(ExactInt a, long b)
        {
            ExactInt num_b = new ExactInt(b);

            return a >= num_b;
        }

        #endregion

        #endregion


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

        /// <summary>
        /// [기능] 기본 규격에 맞쳐서 값을 재정렬시킵니다. 
        /// </summary>
        private void Normalize()
        {
            for (int i = 0; i < value.Length; ++i)
            {
                if (value[i] < LimitInt) continue;

                if (i == value.Length - 1)
                {
                    AddValue(ref value, 0);
                }

                value[i + 1] += value[i] / LimitInt;
                value[i] %= LimitInt;
            }

            scale = value.Length - 1;
        }

        private static int CompareAbsoluteValues(ExactInt a, ExactInt b)
        {
            if (a.scale != b.scale)
            {
                return a.scale > b.scale ? 1 : -1;
            }

            for (int i = a.value.Length - 1; i >= 0; --i)
            {
                if (a.value[i] != b.value[i])
                {
                    return a.value[i] > b.value[i] ? 1 : -1;
                }
            }

            return 0;
        }

        private static void AlignScales(ref ExactInt a, ref ExactInt b)
        {
            int scaleDiff = Math.Abs(a.scale - b.scale);
            if (a.scale > b.scale)
            {
                for (int i = 0; i < scaleDiff; ++i)
                {
                    AddValue(ref a.value, 0);
                }
            }
            else if (b.scale > a.scale)
            {
                for (int i = 0; i < scaleDiff; ++i)
                {
                    AddValue(ref a.value, 0);
                }
            }
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
        private const int LimitInt = 1000;

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
                if (Value[i] >= LimitInt)
                {
                    if (i == Value.Count() - 1)
                    {
                        Value.Add(0);
                        Scale++;
                    }

                    Value[i + 1] += Value[i] / LimitInt;
                    Value[i] %= LimitInt;
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
                    a.Value[i - 1] += LimitInt;
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

            int devideValue = b.Value[1] * LimitInt + b.Value[0];
            int temp = (a.Value[1] * LimitInt) + a.Value[0];
            a.Value[1] = temp / devideValue;
            temp = temp % devideValue;
            a.Value[0] = (temp * LimitInt + a.Value[0]) / b.Value[1];

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