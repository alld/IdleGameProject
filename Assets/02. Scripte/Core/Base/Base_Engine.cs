using IdleGame.Data.Base;
using UnityEngine;


namespace IdleGame.Core.Procedure
{
    /// <summary>
    /// [기능] 게임의 전반적인 진행을 통제하는 로직을 담고 있습니다. 
    /// </summary>
    [DefaultExecutionOrder(-1)]
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
        /// [상태] 지속적인 호출을 제한하기위해서 최초 실행 유무를 판단합니다.
        /// </summary>
        [Tooltip("\nture : 최초 1회 실행되었습니다. \nfalse : 한번도 실행된 적이 없습니다.")]
        private static bool _IsFirstRun = false;


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
    }
}