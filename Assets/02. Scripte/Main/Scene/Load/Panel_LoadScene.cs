using IdleGame.Core;
using IdleGame.Core.Module.EventSystem;
using IdleGame.Data.Common.Event;
namespace IdleGame.Main.Scene.Load
{
    public class Panel_LoadScene : Base_ScenePanel
    {
        /// <summary>
        /// [기능] 씬내에서만 사용되어지는 이벤트 시스템입니다. 
        /// </summary>
        public static Module_EventSystem<eSceneEventType_Load> Event = new Module_EventSystem<eSceneEventType_Load>();

        protected override void Logic_Init_Custom()
        {
            //GameManager.Scene.Logic_TryChangeScene(eSceneKind.Intro);
        }

        private void Start()
        {

            GameManager.Log.Logic_PutLog(new Data.Common.Log.Data_Log("test Log 디스코드 전송"));
        }
    }
}