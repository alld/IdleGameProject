using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Event;
using IdleGame.Data.Popup;
using TMPro;
using UnityEngine;

namespace IdleGame.Core.Popup
{
    /// <summary>
    /// [기능] 버튼이 2개인 공용 팝업입니다.
    /// </summary>
    public class Panel_TwoButtonPopup : Base_CommonPopup
    {
        [Header("TwoButtonPopup")]
        [SerializeField] private TMP_Text _t_title;

        [SerializeField] private TMP_Text _t_context;

        /// <summary>
        /// [상태] 팝업창이 활성화 중인 상태에서 새로운 팝업창이 열린 경우에대한 처리 과정을 확인합니다.
        /// </summary>
        private bool _isOverridePopup = false;

        /// <summary>
        /// [데이터] 팝업창을 덮어 씌울 데이터를 임시 보관합니다.
        /// </summary>
        private Data_Popup _overrideData;

        protected override void Logic_Init_Base()
        {
            base.Logic_Init_Base();
            Logic_PopupInit();
        }

        protected override void Logic_RegisterEvent_Custom()
        {
            Base_Engine.Event.RegisterEvent<Data_Popup>(eGlobalEventType.CommonPopup_TwoBt_Open, Logic_OpenTwoButtonPopup);
        }

        protected override void Logic_PopupInit()
        {
            _isOverridePopup = false;

            _t_title.text = null;
            _t_context.text = null;
            _callback_OK = null;
        }

        /// <summary>
        /// [기능] 버튼이 두개인 팝업창을 설정하여 엽니다. 
        /// </summary>
        private void Logic_OpenTwoButtonPopup(Data_Popup m_data)
        {
            if (_isShowPopup)
            {
                _overrideData = m_data;
                _isOverridePopup = true;
                Logic_Close_Callback();
                return;
            }

            Logic_SetPopupData(m_data);

            Logic_Open_Base();
        }


        protected override void Logic_CloseComplate_Custom()
        {
            if (!_isOverridePopup)
            {
                Logic_PopupInit();
                return;
            }
            else
                Logic_PopupInit();

            Logic_OpenTwoButtonPopup(_overrideData);
        }

        protected override void Logic_SetPopupData(Data_Popup m_data)
        {
            Logic_PopupInit();

            _t_title.text = m_data.title;
            _t_context.text = m_data.content;
            _callback_OK = m_data.callback_Ok;
        }
    }
}