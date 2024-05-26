using IdleGame.Core.Module.EventSystem;
using IdleGame.Core.Module.Scene;
using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Event;
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
        /// [기능] 게임매니저가 관리하는 이벤트 시스템입니다. 씬에 구속되지않고 글로벌 단위로 사용되어지는 이벤트타입만 관리됩니다.
        /// </summary>
        public static Module_EventSystem<eGlobalEventType> Event = new Module_EventSystem<eGlobalEventType>();

        /// <summary>
        /// [기능] 씬의 전환을 절차적 과정을 통해서 전환이 되도록 통제하는 씬 매니저입니다. 
        /// </summary>
        public static Module_SceneManager Scene = new Module_SceneManager();

        protected override void Awake()
        {
            base.Awake();

            #region 싱글턴
            if (Main == null)
            {
                Main = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
            #endregion
        }
    }
}