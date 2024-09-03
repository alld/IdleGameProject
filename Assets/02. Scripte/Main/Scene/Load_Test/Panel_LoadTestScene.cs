using IdleGame.Core;
using IdleGame.Core.Module.EventSystem;
using IdleGame.Data;
using IdleGame.Data.Base.Scene;
using IdleGame.Data.Common.Event;
using IdleGame.Data.Common.Log;

namespace IdleGame.Main.Scene.Load
{
    public class Panel_LoadTestScene : Base_ScenePanel
    {
        /// <summary>
        /// [기능] 씬내에서만 사용되어지는 이벤트 시스템입니다. 
        /// </summary>
        public static Module_EventSystem<eSceneEventType_Load> Event = new Module_EventSystem<eSceneEventType_Load>();


        #region 콜백 함수

        /// <summary>
        /// [버튼콜백] 현재 설정된 상태값을 기반으로 게임을 시작합니다.
        /// </summary>
        public void OnClickGameStart()
        {
            GameManager.Scene.Logic_TryChangeScene(eSceneKind.Intro);
            GameManager.Log.Logic_PutLog(new Data_Log($"테스트용 빌드 시작체크, 시작 타입 : {Global_Data.Editor.LocalData_Grid}"));
        }

        /// <summary>
        /// [버튼콜백] 세이브 데이터를 삭제합니다.
        /// </summary>
        public void OnClickDeleteSave()
        {
            GameManager.Save.Editor_DeleteSave();
        }

        /// <summary>
        /// [드롭다운콜백] 데이터 테이블의 호출되는 타입 형태를 지정합니다.
        /// </summary>
        /// <param name="m_index"></param>
        public void OnClickChangeTableType(int m_index)
        {
            Global_Data.Editor.LocalData_Grid = m_index;
        }

        #endregion
    }
}