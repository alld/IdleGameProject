using IdleGame.Data.NSave;
using IdleGame.Data.Numeric;
using System.Collections.Generic;
using UnityEngine;

namespace IdleGame.Data
{
    /// <summary>
    /// [데이터] 재화의 종류입니다.
    /// </summary>
    public enum eCurrencyType
    {
        None = -1,
        Jewel = 0,
        Money = 1,
    }
    public class Data_Currency
    {
        public void SetFlag(int bitPosition)
        {
            if (bitPosition < 1 || bitPosition > 32)
            {
                // Todo : Log or Exception
                return;
            }

            Flags |= (1 << (bitPosition - 1));
        }
        
        public void ClearFlag(int bitPosition)
        {
            if (bitPosition < 1 || bitPosition > 32)
            {
                // Todo : Log or Exception
                return;
            }

            Flags &= ~(1 << (bitPosition - 1));
        }

        public bool IsFlagSet(int bitPosition)
        {
            if (bitPosition < 1 || bitPosition > 32)
            {
                // Todo : Log or Exception
                return false;
            }

            return (Flags & (1 << (bitPosition - 1))) != 0;
        }
        
        public ExactInt Amount = new ExactInt(0);
        public int Flags = 0;
    }
    /// <summary>
    /// [데이터] 플레이어에 관련된 동적 데이터 정보들을 담고 있습니다. 
    /// </summary>
    public class Data_Player : Interface_SaveData
    {
        /// <summary>
        /// [데이터] 기본적으로 적용되는 디폴트 이름입니다.
        /// </summary>
        private const string DefaultName = "기본 이름입니다.";


        /// <summary>
        /// [데이터] 플레이어의 이름입니다.
        /// </summary>
        public string nick = DefaultName;
        
        /// <summary>
        /// [데이터] 재화의 종류에 따른 재화 데이터입니다.
        /// </summary>
        
        public Dictionary<eCurrencyType, Data_Currency> currencyList = new Dictionary<eCurrencyType, Data_Currency>();
    }
}