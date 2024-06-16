using IdleGame.Core;
using IdleGame.Core.Module.EventSystem;
using IdleGame.Data.Common.Event;
using IdleGame.Main.Scene.Main.InGame;
using UnityEngine;
namespace IdleGame.Main.Scene.Main
{
    /// <summary>
    /// [기능] 실제 게임이 적용되는 씬을 관리합니다. 
    /// </summary>
    public class Panel_MainGameScene : Base_ScenePanel, Interface_BackSide
    {
        /// <summary>
        /// [캐시] 게임의 메인 배경에 해당하는 오브젝트입니다. 
        /// </summary>
        [SerializeField] protected GameObject _obj_MainGraphic;

        /// <summary>
        /// [캐시] 인게임에대한 기능을 통제합니다. 
        /// </summary>
        public Panel_MainGame mainGamePanel;

        /// <summary>
        /// [기능] 씬내에서만 사용되어지는 이벤트 시스템입니다. 
        /// </summary>
        public static Module_EventSystem<eSceneEventType_MainGame> Event = new Module_EventSystem<eSceneEventType_MainGame>();

        protected override void Logic_Init_Custom()
        {
            if (GameManager.Scene.Logic_ConditionIsEntryScene())
                Logic_EntryGameStart();
        }

        protected override void Logic_RegisterEvent_Custom()
        {
            (this as Interface_BackSide).ILogic_RegisterEvent();
        }

        #region 백사이드 인터페이스
        public void ILogic_BackSideOn()
        {
            _obj_MainGraphic.SetActive(false);
        }

        public void ILogic_BackSideOff()
        {
            _obj_MainGraphic.SetActive(true);
        }
        #endregion

        #region 중도 진입 처리
        /// <summary>
        /// [기능] 정상적인 게임 시작이 아닌 중도 진입인 경우 거기에 알맞는 적절한 처리를 해줍니다.
        /// </summary>
        public void Logic_EntryGameStart()
        {

        }


        #endregion
    }
}