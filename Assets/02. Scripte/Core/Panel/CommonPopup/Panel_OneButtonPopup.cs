using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Event;
using IdleGame.Data.Popup;
using UnityEngine;

namespace IdleGame.Core.Popup
{
    /// <summary>
    /// [기능] 버튼이 한개인 공용팝업입니다.
    /// </summary>
    public class Panel_OneButtonPopup : Base_CommonPopup
    {
        [Header("OneButtonPopup")]
        [SerializeField] private Graphic_Text _t_title;

        [SerializeField] private Graphic_Text _t_context;

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
            Base_Engine.Event.RegisterEvent<Data_Popup>(eGlobalEventType.CommonPopup_OneBt_Open, Logic_OpenOneButtonPopup);
        }

        protected override void Logic_PopupInit()
        {
            _isOverridePopup = false;

            _t_title.text = null;
            _t_context.text = null;
            _callback_OK = null;
        }

        /// <summary>
        /// [기능] 버튼이 한개인 팝업창을 설정하여 엽니다. 
        /// </summary>
        private void Logic_OpenOneButtonPopup(Data_Popup m_data)
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

            Logic_OpenOneButtonPopup(_overrideData);
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