using IdleGame.Data.Common;
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

        /// <summary>
        /// [상태] 현재 보여지고 있는 타입입니다. 
        /// </summary>
        private eCurrencyType _currentType;

        /// <summary>
        /// [설정] 현재 보여지고 있는 재화 타입을 지정합니다. 
        /// </summary>
        public void Logic_SetCurrencyType(eCurrencyType m_type)
        {
            _currentType = m_type;
        }

        /// <summary>
        /// [설정] 현재 설정된 재화 타입에서 특정 재화 타입을 포함시킵니다.
        /// </summary>
        public void Logic_AddCurrencyType(eCurrencyType m_type)
        {
            Logic_SetCurrencyType(_currentType | m_type);
        }

        /// <summary>
        /// [설정] 현재 설정된 재화 타입에서 특정 재화 타입을 제외시킵니다. 
        /// </summary>
        public void Logic_RemoveCurrencyType(eCurrencyType m_type)
        {
            Logic_SetCurrencyType(_currentType & ~m_type);
        }
    }
}