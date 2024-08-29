using DG.DemiEditor;
using IdleGame.Core.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace IdleGame.Data.Numeric
{
    /// <summary>
    /// [데이터] 참조형 래핑된 커스텀 숫자입니다.
    /// </summary>
    public class RefExactInt
    {
        /// <summary>
        /// [데이터] 커스텀 숫자입니다.
        /// </summary>
        public ExactInt value = new ExactInt(0);
    }

    /// <summary>
    /// [데이터] 단위 환산식 수식에 사용되는 커스텀 숫자입니다. 
    /// </summary>
    [Serializable]
    public struct ExactInt
    {
        /// <summary>
        /// [데이터] 데이터를 구성하는 단위입니다.
        /// </summary>
        private const int LimitInt = 1000;

        /// <summary>
        /// [데이터] 데이터가 계산되는 환산 단위입니다.
        /// </summary>
        private const int UnitScale = 10000;

        /// <summary>
        /// [캐시] 변환에 사용되는 스트링빌더입니다. 입력받은 스트링중에 숫자만을 담습니다.
        /// </summary>
        private static StringBuilder _Sb_digit = new StringBuilder(10);

        /// <summary>
        /// [캐시] 변환에 사용되는 스트링빌더입니다. 텍스트로 변환할때 사용됩니다.
        /// </summary>
        private static StringBuilder _Sb_string = new StringBuilder(10);

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
        // 겟터 셋터를 이용해서 관리함. 
        /// <summary>
        /// [데이터] 최근에 수정된 상태인지를 확인합니다. 
        /// </summary>
        private bool _isUpdated;

        /// <summary>
        /// [데이터] 해당 커스텀숫자는 배열을 풀링되어 관리되고 있습니다.
        /// <br> 사용 직후 적절한 조치를 취해야합니다. </br>
        /// </summary>
        private bool _isRentData;
        #endregion


        #region 생성자
        /// <summary>
        /// [생성자] 모든 구성을 일일이 지정하여 생성합니다. 
        /// </summary> 
        public ExactInt(int m_value, bool m_isPositive = true, int m_scale = 0, bool m_isRent = false)
        {
            value = m_isRent ? Utility_Common.mPool_int.Rent(m_scale + 1) : new int[m_scale + 1];
            scale = m_scale;
            isPositive = m_isPositive;
            value[m_scale] = m_value;
            _isUpdated = true;
            _isRentData = m_isRent;
        }

        /// <summary>
        /// [생성자] 클론과 같이 동일한 값으로 복사합니다. 계산식에서만 사용됩니다.
        /// </summary>
        private ExactInt(ExactInt m_copy)
        {
            value = Utility_Common.mPool_int.Rent(m_copy.value.Length);
            Array.Copy(m_copy.value, value, m_copy.value.Length);
            scale = m_copy.scale;
            isPositive = m_copy.isPositive;
            _isUpdated = m_copy._isUpdated;
            _isRentData = true;
        }

        /// <summary>
        /// [생성자] 값과 스케일을 지정하여 생성합니다.
        /// </summary>
        public ExactInt(int m_value, int m_scale, bool m_isRent = false)
        {
            value = m_isRent ? Utility_Common.mPool_int.Rent(m_scale + 1) : new int[m_scale + 1];
            scale = m_scale;
            isPositive = m_value >= 0;
            value[m_scale] = m_value;
            _isUpdated = true;
            _isRentData = m_isRent;
        }

        /// <summary>
        /// [생성자] 정수를 기반으로 커스텀숫자를 생성합니다.
        /// </summary>
        public ExactInt(int m_value)
        {
            value = Convert_Number(m_value);
            scale = value.Length - 1;
            isPositive = m_value >= 0;
            _isUpdated = true;
            _isRentData = false;
        }

        /// <summary>
        /// [생성자] long형을 기반으로 커스텀숫자를 생성합니다.
        /// </summary>
        public ExactInt(long m_value)
        {
            value = Convert_Number(m_value);
            scale = value.Length - 1;
            isPositive = m_value >= 0;
            _isUpdated = true;
            _isRentData = false;
        }

        public ExactInt(int[] m_value, bool m_isPositive = true, int m_scale = 0)
        {
            value = (int[])m_value.Clone();
            scale = m_scale;
            isPositive = m_isPositive;
            _isUpdated = true;
            _isRentData = false;
        }

        /// <summary>
        /// [기능] 데이터를 지워서 반환합니다.
        /// </summary>
        public void Clear()
        {
            if (!_isRentData)
                return;

            Utility_Common.mPool_int.Return(value, true);

            _isRentData = false;
            value = null;
            scale = 0;
            isPositive = true;
            _isUpdated = true;
        }

        #region 형변환
        public static explicit operator ExactInt(int b) => new ExactInt(b);
        public static explicit operator ExactInt(long b) => new ExactInt(b);

        public static ExactInt operator -(ExactInt a)
        {
            a.isPositive = false;
            return a;
        }

        /// <summary>
        /// [변환] 커스텀 숫자를 문자열로 변환합니다.
        /// <br> 가장 높은수부터 입력한 단위만큼 표시합니다. </br>
        /// <br> 0을 입력한 경우 모든 단위를 표시합니다. </br>
        /// <br> 해당 포멧팅 방식은 소수점을 표시하지않습니다. </br>
        /// </summary>
        public string ToString(int m_format, bool m_space = false)
        {
            _Sb_string.Clear();
            _Sb_string.Append(isPositive ? "" : "-");

            if (m_format == 0)
                m_format = scale + 1;

            for (int i = m_format; i >= 0; i--)
            {
                if (value[i] == 0)
                    continue;

                _Sb_string.Append(value[i].ToString());
                _Sb_string.Append(Convert_ScaleToString(i));

                if (m_space && i - 1 >= 0)
                    _Sb_string.Append(" ");
            }

            return _Sb_string.ToString();
        }

        /// <summary>
        /// [변환] 커스텀 숫자를 문자열로 변환합니다.
        /// <br> 가장 큰 수의 단위를 표시하며 10의 자릿수 이하인경우 소수점 둘째자리 까지 표시합니다.</br>
        /// </summary>
        public override string ToString()
        {
            _Sb_string.Clear();

            _Sb_string.Append(isPositive ? "" : "-");

            _Sb_string.Append(value[scale].ToString());

            // 조건 :: 스케일값이 존재하면서, 현재 표시 단위가 십의자릿수 이하임(00xx, 000x)
            if (scale >= 1 && value[scale] < 100)
            {
                if (value[scale] < 100 && value[scale - 1] >= 100) // a(00xx) b(xx00)
                {
                    _Sb_string.Append(".");
                    _Sb_string.Append(value[scale - 1] / 100);
                }
            }

            _Sb_string.Append(Convert_ScaleToString(scale));
            return _Sb_string.ToString();
        }


        public static ExactInt Parse(string m_value)
        {
            _Sb_digit.Clear();
            _Sb_letter.Clear();

            // 역할 :: 정수 변환 시도
            if (long.TryParse(m_value, out long tryValue))
                return new ExactInt(tryValue);

            ExactInt result = new ExactInt(0);
            bool checkUnit = false;
            bool isFirst = true;


            for (int i = 0; i < m_value.Length; i++)
            {
                if (char.IsWhiteSpace(m_value[i]))
                    continue;

                if (char.IsDigit(m_value[i]))
                {
                    checkUnit = false;
                    _Sb_digit.Append(m_value[i]);
                }
                else
                {
                    checkUnit = true;
                    _Sb_letter.Append(m_value[i]);
                }

                if ((m_value.Length <= i + 1) || (checkUnit && char.IsDigit(m_value[i + 1])))
                {
                    int scale = Convert_CharToScale(_Sb_letter.ToString());
                    int value = int.Parse(_Sb_digit.ToString());

                    if (isFirst)
                    {
                        isFirst = false;
                        result = new ExactInt(value, scale);
                    }
                    else
                    {
                        result.value[scale] = value;
                    }

                    checkUnit = true;
                    _Sb_digit.Clear();
                    _Sb_letter.Clear();
                }
            }

            return result;
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
        /// [변환] 스케일을 스트링으로 변환합니다.
        /// </summary>
        public static string Convert_ScaleToString(int m_scale)
        {
            _Sb_digit.Clear();

            while (m_scale > 0)
            {
                int remainder = m_scale % 26;
                _Sb_digit.Append((char)(remainder + 'a'));
                m_scale -= 26;
            }
            return _Sb_digit.ToString();
        }

        /// <summary>
        /// [변환] 정수를 커스텀 숫자에서 사용되는 배열로 변환합니다.
        /// </summary>
        public static int[] Convert_Number(int m_value)
        {
            return Convert_Number((long)m_value);
        }

        /// <summary>
        /// [변환] 숫자(Long)를 커스텀숫자에 사용되는 배열로 변환합니다. 
        /// </summary>
        public static int[] Convert_Number(long m_value)
        {
            List<int> result = new List<int>();
            m_value = Math.Abs(m_value);

            while (true)
            {
                if (m_value < UnitScale)
                {
                    result.Add((int)m_value);
                    break;
                }

                result.Add((int)(m_value % UnitScale));
                m_value /= UnitScale;
            }

            return result.ToArray();
        }
        #endregion
        #endregion


        /// <summary>
        /// [기능] 퍼센트를 적용합니다.  [100]
        /// </summary>
        public void SetPercent(int m_value)
        {
            if (m_value == 0)
            {
                this = new ExactInt(0);
                return;
            }
            SetPercent(m_value / 100);
        }



        /// <summary>
        /// [기능] 퍼센트를 적용합니다. 
        /// </summary>
        public void SetPercent(double m_value)
        {
            if (m_value == 0)
            {
                Clear();
                this = new ExactInt(0);
                return;
            }
            else
            {
                m_value = Math.Truncate(m_value * 1000000) / 1000000;
            }
            // TODO :: 음수 양수 변환...

            int percentRange = GetDecimalCount(m_value);
            int multiple = 1;
            for (int i = 0; i < percentRange; i++)
            {
                multiple *= 10;
            }

            this *= (int)(m_value * multiple);
            DecreaseDigits(percentRange);
        }

        /// <summary>
        /// [기능] 자릿수를 늘립니다.
        /// </summary>
        public void IncreaseDigits(int m_count = 1)
        {
            int[] multiple10 = { 1, 10, 100, 1000, 10000 };

            int addDigit = (m_count % 4) + 1;
            int addUnit = (m_count + (4 - GetDigitCount(value[scale]))) / 4;
            int[] result = new int[addUnit + scale];
            for (int i = 0; i < value.Length; i++)
            {
                result[addUnit + i] = ((value[i] * multiple10[addDigit]) % multiple10[addDigit + 1]) + i == 0 ? 0 : (value[i - 1] / multiple10[addDigit + 1]);
            }

            value = result;
            scale = result.Length - 1;
        }

        /// <summary>
        /// [기능] 자릿수를 줄입니다.
        /// </summary>
        public void DecreaseDigits(int m_count = 1)
        {
            int[] multiple10 = { 1, 10, 100, 1000, 10000 };

            int addDigit = (m_count % 4);
            int addUnit = (m_count + (4 - GetDigitCount(value[scale]))) / 4;
            int originSize = scale - addUnit;
            int[] result = new int[originSize + 1];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = ((i + 1) <= scale ? ((value[i + 1] % multiple10[addDigit]) * multiple10[4 - addDigit]) : 0) + (value[i] / multiple10[addDigit]);
            }

            value = result;
            scale = result.Length - 1;
        }

        private int GetDigitCount(int number)
        {
            if (number == 0) return 0;

            return (int)Math.Floor(Math.Log10(Math.Abs(number))) + 1;
        }

        /// <summary>
        /// [기능] 소수점 자릿수를 구합니다.
        /// </summary>
        private int GetDecimalCount(double m_value)
        {
            int count = 0;

            while (m_value != Math.Truncate(m_value))
            {
                m_value *= 10;
                count++;
            }

            return count;
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
            // 역할 :: 결과적으로 덧셈연산이 아닌, 뺄셈인 경우 뺄셈으로 넘김
            if (a.isPositive != b.isPositive)
            {
                b.isPositive = !b.isPositive;
                return a - b;
            }


            bool isOverFlow = false;
            ExactInt result = new ExactInt(0, Mathf.Max(a.scale, b.scale));

            ExactInt numA = new ExactInt(a);
            ExactInt numB = new ExactInt(b);

            // 역할 :: result에 두 값을 더하여 반영함
            for (int i = 0; i <= result.scale; ++i)
            {
                if (isOverFlow)
                {
                    isOverFlow = false;
                    numA.value[i] += 1;
                }

                if (numA.scale < i)
                    result.value[i] = numB.value[i];
                else if (numB.scale < i)
                    result.value[i] = numA.value[i];
                else
                    result.value[i] = numA.value[i] + numB.value[i];

                if (result.value[i] >= UnitScale)
                {
                    isOverFlow = true;
                    result.value[i] -= UnitScale;
                }
            }

            // 역할 :: 최종적으로 넘침이 발생한 경우 단위를 늘려줘야함
            if (isOverFlow)
            {
                ExactInt tempValue = new ExactInt(0, result.scale + 1);

                for (int i = 0; i <= result.scale; i++)
                {
                    tempValue.value[i] = result.value[i];
                }
                tempValue.value[tempValue.scale] += 1;

                result = tempValue;
            }

            // 역할 :: 최종적으로 계산된 값에 부호를 변경해준다.
            result.isPositive = numA.isPositive;

            numA.Clear();
            numB.Clear();
            return result;
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

        #endregion
        #region 빼기
        public static ExactInt operator -(ExactInt a, ExactInt b)
        {
            // 역할 :: 결과적으로 뺄셈 연산이 아닌 덧셈인 경우 덧셈으로 넘김
            if (a.isPositive != b.isPositive)
            {
                b.isPositive = !b.isPositive;
                return a + b;
            }

            int availableIndex = 0;
            ExactInt result = new ExactInt(0);
            bool isPositive = a >= b;
            a.isPositive = true;
            b.isPositive = true;

            // 역할 :: 높은수에서 단순 뺄셈이 계산이 간결해지기에 높은수를 구분함
            ExactInt higherNumber, lowerNumber;
            if (a >= b)
            {
                higherNumber = new ExactInt(a);
                lowerNumber = new ExactInt(b);
            }
            else
            {
                higherNumber = new ExactInt(b);
                lowerNumber = new ExactInt(a);
            }

            // 역할 :: 높은수를 기반으로 낮은수를 뺍니다.
            for (int i = higherNumber.scale; i >= 0; i--)
            {
                if (lowerNumber.scale >= i)
                {
                    // 역할 :: 현재 뺄 값이 낮은수보다 낮은 경우, 앞에서 계산된 값을 가져옵니다.
                    if (higherNumber.value[i] < lowerNumber.value[i])
                    {
                        higherNumber.value[availableIndex]--;
                        for (int j = availableIndex; j > i; j--)
                            higherNumber.value[j] += UnitScale - 1;

                        higherNumber.value[i] += UnitScale;
                        availableIndex = 0;
                    }

                    // 역할 :: 값을 빼는 부분 
                    higherNumber.value[i] -= lowerNumber.value[i];
                }

                // 역할 :: 여유가 있는 수치가 어디부터인지 기록하는 값
                if (availableIndex == 0 && higherNumber.value[i] > 0)
                    availableIndex = i;
            }

            // 역할 :: 결손이 발생한 경우, 스케일이 변경될수 있기때문에 값을 재조정합니다.
            for (int i = higherNumber.scale; i >= 0; i--)
            {
                if (higherNumber.value[i] == 0)
                    continue;

                result = new ExactInt(0, i);
                break;
            }

            // 역할 :: 최종적으로 값을 결과값에 옮깁니다.
            for (int i = 0; i <= result.scale; i++)
            {
                result.value[i] = higherNumber.value[i];
            }

            result.isPositive = isPositive;
            higherNumber.Clear();
            lowerNumber.Clear();
            return result;
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
            ExactInt result = new ExactInt(0, a.scale + b.scale);

            for (int i = 0; i <= a.scale; ++i)
            {
                for (int j = 0; j <= b.scale; ++j)
                {
                    result.value[i + j] += a.value[i] * b.value[j];
                }
            }

            int overflow = 0;
            for (int i = 0; i <= result.scale; i++)
            {
                result.value[i] += overflow;
                overflow = result.value[i] / UnitScale;
                result.value[i] -= overflow;
            }

            result.isPositive = a.isPositive == b.isPositive;
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
            for (int i = a.scale; i > 0; --i)
            {
                temp = (temp * LimitInt + a.value[i]) * LimitInt + a.value[i - 1];
                a.value[i] = temp / devideValue;
                if (i > 0 && a.value[i] == 0 && i == a.scale)
                {
                    RemoveAtValue(ref a.value, i);
                }
                temp %= devideValue;
            }
            a.value[0] = (temp * LimitInt + a.value[0]) / b.value.Last();


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

            if (a.isPositive)
                for (int i = a.scale; i >= 0; --i)
                {
                    if (a.value[i] == b.value[i]) continue;
                    return a.value[i] < b.value[i];
                }
            else
                for (int i = a.scale - 1; i >= 0; --i)
                {
                    if (a.value[i] == b.value[i]) continue;
                    return a.value[i] > b.value[i];
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

            if (a.isPositive)
                for (int i = a.scale; i >= 0; --i)
                {
                    if (a.value[i] == b.value[i]) continue;
                    return a.value[i] > b.value[i];
                }
            else
                for (int i = a.scale; i >= 0; --i)
                {
                    if (a.value[i] == b.value[i]) continue;
                    return a.value[i] < b.value[i];
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
            if (CompareZero(a) && CompareZero(b)) return true;

            if (a.isPositive != b.isPositive || a.scale != b.scale) return false;

            for (int i = a.scale; i >= 0; --i)
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

            for (int i = a.scale; i >= 0; --i)
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