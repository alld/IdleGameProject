using IdleGame.Core.Procedure;
using System.Collections;

namespace IdleGame.Core
{
    /// <summary>
    /// [기능] 씬마다 필수적으로 할당되는 씬 전용 메인 패널입니다. 씬 전용 게임매니저의 역할을 합니다.
    /// <br> 씬내에서만 사용되어지는 기반 기능들을 담고 있습니다. 씬의 전환과정에서 필요한 설정을 담당하기도 합니다. </br>
    /// </summary>
    public class Base_ScenePanel : Base_Panel
    {
        private void Awake()
        {
            System_Init();
        }

        /// <summary>
        /// [초기화] GameMaster가 설정이 완료될때까지 대기한후 자신을 등록합니다.
        /// </summary>
        private void System_Init()
        {
            Base_Engine.Logic_SetPanel(this);
        }

        /// <summary>
        /// [초기화] 해당 씬에서 필요한 초기화가 진행됩니다.
        /// </summary>
        internal IEnumerator System_Setting()
        {
            yield return Logic_InitStart_Custom();
        }

        /// <summary>
        /// [초기화] 해당 씬이 호출된 직후 패널에서 실행시켜야할 기본 로직을 시작합니다. 
        /// </summary>
        protected virtual IEnumerator Logic_InitStart_Custom()
        {
            yield break;
        }

        /// <summary>
        /// [기능] 해당 씬에서 사용된 로직들을 정리합니다. 
        /// </summary>
        internal IEnumerator System_Clear()
        {
            yield return Logic_Clear_Custom();
        }

        /// <summary>
        /// [기능] 해당 씬에서 사용된 기능들을 정리하는 로직을 시작합니다. 
        /// </summary>
        protected virtual IEnumerator Logic_Clear_Custom()
        {
            yield break;
        }
    }
}