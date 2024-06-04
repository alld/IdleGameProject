using IdleGame.Core.Module;
using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Event;
using System.Collections.Generic;
using System.Linq;

namespace IdleGame.Core.Popup
{
    /// <summary>
    /// [기능] 현재 열려 있는 팝업들을 관리하며 팝업들간의 우선순위를 할당해줍니다. 
    /// </summary>
    public class Module_PopupManager : Base_Module
    {
        /// <summary>
        /// [캐시] 활성화된 팝업창들을 고유 번호를 할당하여 보관합니다. 
        /// </summary>
        private Dictionary<int, Base_Popup> _activePopupList = new Dictionary<int, Base_Popup>();

        private List<int> _indexList = new List<int>();

        private Stack<int> _availableIndexes = new Stack<int>();

        public override void Logic_Init()
        {
            base.Logic_Init();
        }

        /// <summary>
        /// [설정] 팝업 매니저에 필요한 이벤트들을 등록시킵니다. 
        /// </summary>
        private void Logic_RegisterEvent()
        {
            Base_Engine.Event.RegisterEvent(eGlobalEventType.Popup_Act_Close, Logic_CloseLastPopup);
            Base_Engine.Event.RegisterEvent(eGlobalEventType.Popup_Act_AllClose, Logic_AllClosePopup);
        }

        /// <summary>
        /// [초기화] 팝업 매니저에서 사용되어지는 모든 데이터를 지웁니다. 
        /// </summary>
        private void Logic_Clear()
        {
            _activePopupList.Clear();
            _indexList.Clear();
            _availableIndexes.Clear();
        }

        /// <summary>
        /// [기능] 팝업창이 가장 최근에 열린 팝업창들을 우선적으로 비활성화시킵니다. 
        /// </summary>
        private void Logic_CloseLastPopup()
        {
            Logic_ClosePopup(_indexList.Last());
        }

        /// <summary>
        /// [기능] 특정 팝업창을 닫습니다. 
        /// </summary>
        private void Logic_ClosePopup(int m_index)
        {
            _activePopupList[m_index].Module_ClosePopup();
        }

        /// <summary>
        /// [기능] 모든 팝업창을 닫습니다. 
        /// </summary>
        private void Logic_AllClosePopup()
        {
            int count = _indexList.Count;
            for (int i = 0; i < count; i++)
            {
                Logic_CloseLastPopup();
            }
        }

        /// <summary>
        /// [설정] 팝업 매니저에 특정 팝업을 등록시킵니다.
        /// </summary>
        public int Logic_ReigsterPopup(Base_Popup m_popup)
        {
            int index = -1;
            if (_availableIndexes.Count != 0)
                index = _availableIndexes.Pop();
            else
                index = _indexList.Count;

            _activePopupList.Add(index, m_popup);
            _indexList.Add(index);

            return index;
        }

        /// <summary>
        /// [설정] 등록되어 있는 특정 팝업을 제거합니다. 
        /// </summary>
        public void Logic_RemovePopup(int m_index)
        {
            _availableIndexes.Push(m_index);

            _activePopupList.Remove(m_index);
            _indexList.Remove(m_index);

            if (_activePopupList.Count == 0)
                Logic_Clear();
        }
    }
}