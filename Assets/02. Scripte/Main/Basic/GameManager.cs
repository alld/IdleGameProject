using IdleGame.Core.Procedure;
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