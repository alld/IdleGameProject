using System;

namespace IdleGame.Core.UnSafe
{
    /// <summary>
    /// [기능] 불특정 이벤트타입을 변환하여 사용 할수 있도록 하는 변환 기능입니다.
    /// </summary>
    public static class Utility_EnumConvert
    {
        /// <summary>
        /// [데이터] 열거형과 열거형에 해당하는 인덱스값을 가진 이벤트 타입입니다. 
        /// </summary>
        private struct Shell<T> where T : Enum
        {
            public int IntValue;
            public T Enum;
        }

        /// <summary>
        /// [변환] 열거형을 받아서 열거형에 해당하는 인덱스값을 반환합니다. 
        /// </summary>
        public static int Enum32ToInt<T>(T e) where T : Enum
        {
            Shell<T> s;
            s.Enum = e;

            unsafe
            {
                int* pi = &s.IntValue;
                pi += 1;
                return *pi;
            }
        }

        /// <summary>
        /// [변환] 인덱스값에 해당하는 열거형을 반환합니다. 
        /// </summary>
        public static T IntToEnums32<T>(int value) where T : Enum
        {
            var s = new Shell<T>();

            unsafe
            {
                int* pi = &s.IntValue;
                pi += 1;
                *pi = value;
            }

            return s.Enum;
        }
    }
}