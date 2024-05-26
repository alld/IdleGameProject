using IdleGame.Core;
using IdleGame.Core.Module.EventSystem;
using IdleGame.Data.Common.Event;

namespace IdleGame.Main.Scene.Intro
{
    public class Panel_IntroScene : Base_ScenePanel
    {
        /// <summary>
        /// [기능] 씬내에서만 사용되어지는 이벤트 시스템입니다. 
        /// </summary>
        public static Module_EventSystem<eSceneEventType_Intro> Event = new Module_EventSystem<eSceneEventType_Intro>();
    }
}