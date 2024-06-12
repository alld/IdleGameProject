using IdleGame.Core.Procedure;
using IdleGame.Main.GameLogic;
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
        private Panel_StageManager _stage;

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
                Logic_ManagerInit();
            }
            else
            {
                Destroy(gameObject);
            }
            #endregion
        }

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