using IdleGame.Core;
using IdleGame.Core.Module.EventSystem;
using IdleGame.Data;
using IdleGame.Data.Base.Scene;
using IdleGame.Data.Common.Event;

namespace IdleGame.Main.Scene.Intro
{
    public class Panel_IntroScene : Base_ScenePanel
    {
        /// <summary>
        /// [기능] 씬내에서만 사용되어지는 이벤트 시스템입니다. 
        /// </summary>
        public static Module_EventSystem<eSceneEventType_Intro> Event = new Module_EventSystem<eSceneEventType_Intro>();

        protected override void Logic_RegisterEvent_Custom()
        {
            Event.RegisterEvent(eSceneEventType_Intro.Act_ChangeScene, Logic_NextScene);
        }

        protected override void Logic_Init_Custom()
        {
            if (Global_Data.Editor.isInitSave == false)
                GameManager.Save.Editor_DeleteSave();
        }

        /// <summary>
        /// [기능] 인게임 씬으로 전환을 시도합니다.
        /// </summary>
        public void Logic_NextScene()
        {
            GameManager.Scene.Logic_TryChangeScene(eSceneKind.MainGame);
        }
    }
}