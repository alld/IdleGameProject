using IdleGame.Data;
using IdleGame.Data.Numeric;
using System.Collections.Generic;
using UnityEngine;


namespace IdleGame.Main 
{
    public class CurrencyManager : MonoBehaviour
    {
        private int currentFlag = 0;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    
        public Dictionary<Data.eCurrencyType, Data.Data_Currency> currencyList;
        void Awake()
        {
            currencyList = Global_Data.Player.currencyList;
            InitializeCurrencyFlags();
        }

        private void InitializeCurrencyFlags()
        {
            // Initialize Flags
            // Todo : Maybe read config file to initliaize 
            currencyList[Data.eCurrencyType.Jewel].Flags = 0x1;
        }

        public bool SetCurrentFlag(int newFlag)
        {
            if (newFlag < 1 || 32 < newFlag) return false;

            currentFlag = newFlag;

            foreach (KeyValuePair<Data.eCurrencyType, Data.Data_Currency> iter in currencyList)
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
    
        public bool AddCurrency(Data.eCurrencyType currencyType, ExactInt addAmount)
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
        
        public bool SubCurrency(Data.eCurrencyType currencyType, ExactInt subAmount)
        {
            if (currencyList[currencyType].Amount < subAmount) return false;
            
            currencyList[currencyType].Amount -= subAmount;
            
            // Todo : currencyWidget Update
            
            return true;
        }
    
        public List<KeyValuePair<Data.eCurrencyType, Data.Data_Currency>> GetCurrencyList(int flag)
        {
            List<KeyValuePair<Data.eCurrencyType, Data.Data_Currency>> result = new List<KeyValuePair<Data.eCurrencyType, Data.Data_Currency>>();
            foreach (KeyValuePair<Data.eCurrencyType, Data.Data_Currency> iter in currencyList)
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