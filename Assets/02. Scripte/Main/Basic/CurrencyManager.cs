using IdleGame.Data;
using IdleGame.Data.Numeric;
using System.Collections.Generic;
using UnityEngine;


namespace IdleGame.Main
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
    
    public class CurrencyManager : MonoBehaviour
    {
        private int currentFlag = 0;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    
        public Dictionary<eCurrencyType, Data_Currency> currencyList;
        void Awake()
        {
            // Todo : Get currencyList from Player
            // currencyList = Global_Data.Player.currencyList;

            InitializeCurrencyFlags();
        }

        private void InitializeCurrencyFlags()
        {
            // Initialize Flags
            // Todo : Maybe read config file to initliaize 
            currencyList[eCurrencyType.Jewel].Flags = 0x1;
        }

        public bool SetCurrentFlag(int newFlag)
        {
            if (newFlag < 1 || 32 < newFlag) return false;

            currentFlag = newFlag;

            foreach (KeyValuePair<eCurrencyType, Data_Currency> iter in currencyList)
            {
                if (iter.Value.IsFlagSet(currentFlag))
                {
                    // Todo : Turn on widget
                }
                else
                {
                    // Todo : Turn off widget
                }
            }
            

            return true;
        }
    
        public bool AddCurrency(eCurrencyType currencyType, ExactInt addAmount)
        {
            if (currencyList[currencyType].IsFlagSet(currentFlag))
            {
                // Todo : Play Animation
            }
            else
            {
                currencyList[currencyType].Amount += addAmount;
            }
            
            return true;
        }
        
        public bool SubCurrency(eCurrencyType currencyType, ExactInt subAmount)
        {
            if (currencyList[currencyType].Amount < subAmount) return false;
            
            currencyList[currencyType].Amount -= subAmount;
            
            // Todo : currencyWidget Update
            
            return true;
        }
    
        public List<KeyValuePair<eCurrencyType, Data_Currency>> GetCurrencyList(int flag)
        {
            List<KeyValuePair<eCurrencyType, Data_Currency>> result = new List<KeyValuePair<eCurrencyType, Data_Currency>>();
            foreach (KeyValuePair<eCurrencyType, Data_Currency> iter in currencyList)
            {
                if (iter.Value.IsFlagSet(flag))
                {
                    result.Add(iter);
                }
            }
    
            return result;
        }
    }
}