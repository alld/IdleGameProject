using IdleGame.Core.Procedure;
using System.Collections;
using UnityEngine;

namespace IdleGame.Core
{
    /// <summary>
    /// [기능] 씬마다 필수적으로 할당되는 씬 전용 메인 패널입니다. 씬 전용 게임매니저의 역할을 합니다.
    /// <br> 씬내에서만 사용되어지는 기반 기능들을 담고 있습니다. 씬의 전환과정에서 필요한 설정을 담당하기도 합니다. </br>
    /// </summary>
    [DefaultExecutionOrder(-3)]
    public class Base_ScenePanel : Base_Panel
    {
        /// <summary>
        /// [기능] 베이스 패널의 기능을 제한하고 씬 패널은 별도로 관리되어야 하기때문에 기존 기능을 상속하지 않습니다.
        /// </summary>
        protected sealed override void Awake()
        {
            System_Init();
        }

        /// <summary>
        /// [초기화] GameMaster가 설정이 완료될때까지 대기한후 자신을 등록합니다.
        /// </summary>
        private void System_Init()
        {
            Base_Engine.Logic_SetPanel(this);


            // Todo :: 임시로 들어감..

            StartCoroutine(System_Setting());
        }

        /// <summary>
        /// [초기화] 해당 씬에서 필요한 초기화가 진행됩니다.
        /// </summary>
        internal IEnumerator System_Setting()
        {
            Logic_Init_Custom();

            Logic_RegisterEvent_Custom();

            yield break;
        }

        /// <summary>
        /// [기능] 해당 씬에서 사용된 로직들을 정리합니다. 
        /// </summary>
        internal IEnumerator System_Clear()
        {
            Logic_Clear_Custom();

            yield break;
        }

        /// <summary>
        /// [기능] 해당 씬에서 사용된 기능들을 정리하는 로직을 시작합니다. 
        /// </summary>
        protected virtual void Logic_Clear_Custom() { }
    }
}