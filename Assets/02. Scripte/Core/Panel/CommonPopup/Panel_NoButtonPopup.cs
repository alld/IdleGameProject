using IdleGame.Core.Procedure;
using IdleGame.Core.Utility;
using IdleGame.Data.Common.Event;
using IdleGame.Data.Popup;
using System.Collections;
using UnityEngine;

namespace IdleGame.Core.Popup
{
    // TODO :: 팝업이 중복으로 호출되어 닫히는 과정에서 추가 호출되는 상황에대한 대비는 안되어있음. 
    /// <summary>
    /// [기능] 버튼이 없는 공용 팝업입니다.
    /// </summary>
    public class Panel_NoButtonPopup : Base_CommonPopup
    {
        [Header("NoButtonPopup")]
        [SerializeField] private Graphic_Text _t_title;

        [SerializeField] private Graphic_Text _t_context;

        [SerializeField] private float _autoCloseTimer = 3f;

        /// <summary>
        /// [상태] 팝업창이 활성화 중인 상태에서 새로운 팝업창이 열린 경우에대한 처리 과정을 확인합니다.
        /// </summary>
        private bool _isOverridePopup = false;

        /// <summary>
        /// [데이터] 팝업창을 덮어 씌울 데이터를 임시 보관합니다.
        /// </summary>
        private Data_Popup _overrideData;

        /// <summary>
        /// [캐시] 일정 시간이 경과 한후 자동으로 닫히게 하는 코루틴입니다. 
        /// </summary>
        private Coroutine _co_autoClose;

        protected override void Logic_Init_Base()
        {
            base.Logic_Init_Base();
            Logic_PopupInit();
        }

        protected override void Logic_RegisterEvent_Custom()
        {
            Base_Engine.Event.RegisterEvent<Data_Popup, float>(eGlobalEventType.CommonPopup_NoBt_Open, Logic_OpenNoButtonPopup);
        }

        protected override void Logic_PopupInit()
        {
            _isOverridePopup = false;
            if (_co_autoClose != null) StopCoroutine(_co_autoClose);
            _co_autoClose = null;

            _t_title.text = null;
            _t_context.text = null;
            _callback_OK = null;
        }

        /// <summary>
        /// [기능] 버튼이 자동으로 닫히는 시간을 반영해서 팝업창을 엽니다.
        /// </summary>
        private void Logic_OpenNoButtonPopup(Data_Popup m_data, float m_timer)
        {
            if (m_timer <= 0)
                _autoCloseTimer = m_timer;

            if (_isShowPopup)
            {
                _overrideData = m_data;
                _isOverridePopup = true;
                if (_co_autoClose != null) StopCoroutine(_co_autoClose);
                Logic_Close_Callback();
                return;
            }

            Logic_SetPopupData(m_data);

            Logic_Open_Base();

            StartCoroutine(Logic_AutoClosePopup());
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

            Logic_OpenNoButtonPopup(_overrideData, 0);
        }


        private IEnumerator Logic_AutoClosePopup()
        {
            yield return Utility_Common.WaitForSeconds(_autoCloseTimer);

            Logic_Close_Callback();

            _co_autoClose = null;
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