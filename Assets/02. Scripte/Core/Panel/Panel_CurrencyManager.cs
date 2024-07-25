using IdleGame.Data;
using IdleGame.Data.Common;
using IdleGame.Data.Numeric;
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
            /// <summary>
            /// [캐시] 화면에 표시되는 재화 보유량입니다.
            /// </summary>
            public Graphic_Text t_display;

            /// <summary>
            /// [데이터] 단일 타입만 지정되어야합니다. 해당 박스의 재화종류를 나타냅니다.
            /// </summary>
            public eCurrencyType type;

            /// <summary>
            /// [캐시] 화면에 표시되는 재화 상자입니다. 
            /// </summary>
            public GameObject obj_box;
        }

        /// <summary>
        /// [캐시] 재화 매니저가 관리하는 재화 표시 UI 오브젝트들입니다.
        /// </summary>
        private Data_DisplayComponent[] _dc;

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

            Logic_UpdateCurrencyBox();
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

        /// <summary>
        /// [기능] 표시 단위를 갱신시킵니다. 
        /// </summary>
        private void Logic_UpdateCurrency()
        {
            _dc[0].t_display.SetText(Global_Data.Player.cc_Gold.ToString());
            _dc[1].t_display.SetText(Global_Data.Player.cc_Ability.ToString());
        }

        /// <summary>
        /// [기능] 현재 보여줘야할 재화 상자를 선택된 재화타입에 맞쳐 변경시킵니다. 
        /// </summary>
        private void Logic_UpdateCurrencyBox()
        {
            for (int i = 0; i < _dc.Length; i++)
            {
                if (_currentType.HasFlag(_dc[i].type))
                    _dc[i].obj_box.SetActive(true);
                else
                    _dc[i].obj_box.SetActive(false);
            }

            Logic_UpdateCurrency();
        }

        /// <summary>
        /// [기능] 재화 표시 패널에 데이터를 더하거나 뺍니다. 
        /// </summary>
        public void Logic_SetAddCurrency(eCurrencyType m_type, long m_data)
        {
            Logic_SetAddCurrency(m_type, new ExactInt(m_data));
        }

        /// <summary>
        /// [기능] 재화 표시 패널에 데이터를 더하거나 뺍니다. 
        /// </summary>
        public void Logic_SetAddCurrency(eCurrencyType m_type, ExactInt m_data)
        {
            if (m_data == 0)
                return;

            switch (m_type)
            {
                case eCurrencyType.Gold:
                    Global_Data.Player.cc_Gold += m_data;
                    break;
                case eCurrencyType.Ability:
                    Global_Data.Player.cc_Ability += m_data;
                    break;
            }

            Logic_UpdateCurrency();
        }
    }
}