using IdleGame.Core.Module.EventSystem;
using IdleGame.Core.Module.Scene;
using IdleGame.Core.Panel.DataTable;
using IdleGame.Core.Panel.LogCollector;
using IdleGame.Core.Panel.SaveEngine;
using IdleGame.Core.Panel.Sound;
using IdleGame.Core.Popup;
using IdleGame.Data.Base;
using IdleGame.Data.Common.Event;
using UnityEngine;


namespace IdleGame.Core.Procedure
{
    /// <summary>
    /// [기능] 게임의 전반적인 진행을 통제하는 로직을 담고 있습니다. 
    /// </summary>
    [DefaultExecutionOrder(-5)]
    public abstract class Base_Engine : MonoBehaviour
    {
        /// <summary>
        /// [정보] 로직에대한 기초 정보를 담고 있습니다.
        /// </summary>
        [Header("BaseEngine")]
        [SerializeField] private TagInfo _tag;

        /// <summary>
        /// [캐시] 현재 씬에서 활성화 중인 씬 패널입니다.
        /// </summary>
        public static Base_ScenePanel Panel { get { return _Panel; } }
        private static Base_ScenePanel _Panel;

        /// <summary>
        /// [캐시] 매니저에서 관리되는 패널들을 담고있는 오브젝트 그룹입니다.
        /// </summary>
        [SerializeField] private GameObject _obj_logic;

        /// <summary>
        /// [기능] 게임매니저가 관리하는 이벤트 시스템입니다. 씬에 구속되지않고 글로벌 단위로 사용되어지는 이벤트타입만 관리됩니다.
        /// </summary>
        public static Module_EventSystem<eGlobalEventType> Event = new Module_EventSystem<eGlobalEventType>();

        /// <summary>
        /// [기능] 씬의 전환을 절차적 과정을 통해서 전환이 되도록 통제하는 씬 매니저입니다. 
        /// </summary>
        public static Module_SceneManager Scene = new Module_SceneManager();

        /// <summary>
        /// [기능] 활성화된 팝업창들을 관리하는 팝업 매니저입니다. 
        /// </summary>
        public static Module_PopupManager Popup = new Module_PopupManager();

        /// <summary>
        /// [캐시] 로그를 관리하는 기능입니다.
        /// </summary>
        public static Panel_LogCollector Log;

        /// <summary>
        /// [캐시] 동적 데이터들을 저장하거나 저장된 데이터를 불러옵니다.
        /// </summary>
        public static Panel_SaveEngine Save;

        /// <summary>
        /// [캐시] 정적 데이터들을 관리합니다.
        /// </summary>
        public static Panel_DataTableManager Table;

        /// <summary>
        /// [캐시] 모든 사운드를 관리합니다. 
        /// </summary>
        public static Panel_SoundManager Sound;

        /// <summary>
        /// [상태] 지속적인 호출을 제한하기위해서 최초 실행 유무를 판단합니다.
        /// </summary>
        [Tooltip("\nture : 최초 1회 실행되었습니다. \nfalse : 한번도 실행된 적이 없습니다.")]
        private static bool _IsFirstRun = false;


        /// <summary>
        /// [캐시] 매니저가 관리하는 모든 패널을 담습니다. 
        /// </summary>
        private Base_ManagerPanel[] _managerPanels;


        protected virtual void Awake()
        {
            if (_IsFirstRun == false) _IsFirstRun = true;
            else return;

            return;
        }

        /// <summary>
        /// [기능] 게임 매니저에 할당된 씬 패널에 대한 값을 변경합니다. 
        /// </summary>
        public static void Logic_SetPanel(Base_ScenePanel m_panel)
        {
            _Panel = m_panel;
        }

        /// <summary>
        /// [초기화] 게임 매니저가 들고있는 오브젝트들을 탐색해서 적절하게 캐시처리합니다.
        /// </summary>
        public void Logic_RegisterManager()
        {
            Log = _obj_logic.GetComponentInChildren<Panel_LogCollector>();
            Save = _obj_logic.GetComponentInChildren<Panel_SaveEngine>();
            Table = _obj_logic.GetComponentInChildren<Panel_DataTableManager>();
            Sound = _obj_logic.GetComponentInChildren<Panel_SoundManager>();
            _managerPanels = _obj_logic.GetComponentsInChildren<Base_ManagerPanel>();
        }

        /// <summary>
        /// [초기화] 게임 매니저가 관리하는 모든 패널들을 초기화시킵니다. 
        /// </summary>
        protected void Logic_ManagerInit()
        {
            foreach (var panel in _managerPanels)
            {
                panel.Logic_Init();
            }
        }
    }
}