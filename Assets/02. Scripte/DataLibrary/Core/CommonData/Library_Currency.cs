using System;

namespace IdleGame.Data.Common
{

    /// <summary>
    /// [종류] 재화 타입을 나타냅니다. 
    /// </summary>
    [Flags]
    public enum eCurrencyType
    {
        None = 0,

        /// <summary> [종류] 일반 재화 </summary>
        Gold = 1 << 0,
        /// <summary> [종류] 능력치 강화 포인트 </summary>
        Ability = 1 << 1,
    }

    public class Library_Currency
    {
    }
}