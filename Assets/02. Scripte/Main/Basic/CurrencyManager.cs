using IdleGame.Data;
using IdleGame.Data.Numeric;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Dictionary<Data_Player.eCurrencyType, Data_Currency> currencyList;
    void Awake()
    {
        currencyList = Global_Data.Player.currencyList;
    }

    bool AddCurrency(Data_Player.eCurrencyType currencyType, ExactInt addAmount)
    {
        currencyList[currencyType].Amount += addAmount;
        
        return true;
    }
    
    bool SubCurrency(Data_Player.eCurrencyType currencyType, ExactInt subAmount)
    {
        if (currencyList[currencyType].Amount < subAmount) return false;
        
        currencyList[currencyType].Amount -= subAmount;
        
        // Todo : currencyWidget Update
        
        return true;
    }

    List<KeyValuePair<Data_Player.eCurrencyType, Data_Currency>> GetCurrencyList(int flag)
    {
        List<KeyValuePair<Data_Player.eCurrencyType, Data_Currency>> result = new List<KeyValuePair<Data_Player.eCurrencyType, Data_Currency>>();
        foreach (KeyValuePair<Data_Player.eCurrencyType, Data_Currency> iter in currencyList)
        {
            if (iter.Value.IsFlagSet(flag))
            {
                result.Add(iter);
            }
        }

        return result;
    }
}