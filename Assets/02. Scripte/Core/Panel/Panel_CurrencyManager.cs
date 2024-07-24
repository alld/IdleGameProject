using IdleGame.Data;
using IdleGame.Data.Numeric;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace IdleGame.Core.Panel
{
    /// <summary>
    /// [기능] 현재 상태값을 기록합니다. 
    /// </summary>
    public class Panel_CurrencyManager : Base_ManagerPanel
    {
        [System.Serializable]
        public struct Data_DisplayComponent
        {
            [SerializeField]
            private Graphic_Text t_display;

        }

    }
}