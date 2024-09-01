using IdleGame.Core.Procedure;
using IdleGame.Data;
using IdleGame.Data.Common.Event;
using IdleGame.Data.DataTable;
using IdleGame.Main.GameLogic;
using IdleGame.Main.Scene.Main.UI;
using UnityEngine;

namespace IdleGame.Main
{
    /// <summary>
    /// [기능] 게임의 전반적인 흐름을 통제하고 관리합니다. 
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class GameManager : Base_Engine
    {
        /// <summary>
        /// [캐시] 게임내에서 작동하는 중추적인 역할과 기능에 접근할 수 있습니다. 
        /// </summary>
        public static GameManager Main = null;

        /// <summary>
        /// [캐시] 스테이지를 구성하고 진행상황을 추적, 저장하는 기능을 하는 스테이지 매니저입니다.
        /// </summary>
        [Header("GameManager")]
        [SerializeField]
        public Panel_StageManager stage;

        /// <summary>
        /// [상태] 게임시작에 제한을 걸기 위해서 적용된 변수입니다. 중복 실행을 방지합니다.
        /// </summary>
        private bool _startLock = false;

        /// <summary>
        /// [캐시] 게임 진행 상황을 기록하고 별도로 측정을 하는 시간 매니저입니다.
        /// </summary>
        [SerializeField]
        private Panel_TimeManager _time;

        protected override void Awake()
        {
            base.Awake();

            #region 싱글턴
            if (Main == null)
            {
                Main = this;
                DontDestroyOnLoad(this);

                Logic_RegisterManager();
                Logic_RegisterEvent();
                Logic_ManagerInit();


                // 역할 :: 데이터 크기 설정을 한번 해주기 위해서 호출함
                Global_Data.GetSaveDatas();
            }
            else
            {
                Destroy(gameObject);
            }
            #endregion
        }

        /// <summary>
        /// [초기화] 이벤트를 등록시킵니다.
        /// </summary>
        private void Logic_RegisterEvent() { }


        /// <summary>
        /// [기능] 본격적인 게임 진행 절차에 들어갑니다. (최초 1회만 선언될것.)
        /// </summary>
        public void Logic_GameStart()
        {
            if (_startLock) return;
            _startLock = true;

            //------ 임시 -------//

            Library_DataTable.stage[Global_Data.PlayProgress.stage_curIndex].currentWave = Global_Data.PlayProgress.stage_curWave;
            stage.Logic_SetStage(Library_DataTable.stage[Global_Data.PlayProgress.stage_curIndex]);
            stage.Logic_StageStart();

            //--------------------//

            Event.CallEvent(eGlobalEventType.Game_Start);
        }


        #region 스테이지
        /// <summary>
        /// [초기화] 씬에 있는 스테이지 보드를 등록시킵니다.
        /// </summary>
        public void Logic_RegisterStageBord(Graphic_StageBord m_bord)
        {
            stage.Logic_RegisterStageBord(m_bord);
        }

        /// <summary>
        /// [기능] 다음 스테이지나 웨이브로 진행을 시도합니다.
        /// </summary>
        public bool Logic_TryNextLevel()
        {
            return stage.Logic_TryNextLevel();
        }
        #endregion

        private void OnApplicationQuit()
        {
            Save.Logic_Save(true);
        }

        private void OnApplicationPause(bool m_pause)
        {
            if (m_pause)
            {
                Save.Logic_Save();
            }
            else
            {
                // TODO :: 방치 보상 계산 // 복귀 시간 계산 
            }
        }
    }
}