using IdleGame.Core;
using IdleGame.Core.Module.EventSystem;
using IdleGame.Data;
using IdleGame.Data.Base.Scene;
using IdleGame.Data.Common.Event;
using UnityEngine.DedicatedServer;
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
            Logic_LoadSegmentsSetting();

            GameManager.Scene.Logic_TryChangeScene(eSceneKind.Intro);
        }

        public void Logic_LoadSegmentsSetting()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            string result = null;

            foreach (string arg in args)
            {
                if (arg.StartsWith("-idleData"))
                {
                    result = arg;
                }
            }

            string[] array = result.Split(" .");

            for (int i = 0; i < array.Length; i++)
            {
                string[] seg = array[i].Split(":");
                switch (seg[0])
                {
                    case "user":
                        Global_Data.Editor.userName = seg[1];
                        break;
                    case "initsave":
                        Global_Data.Editor.isInitSave = int.Parse(seg[1]) == 1;
                        break;
                    case "table":
                        Global_Data.Editor.LocalData_Grid = int.Parse(seg[1]);
                        break;
                }
            }
        }

    }
}