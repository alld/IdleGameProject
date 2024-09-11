using IdleGame.Core.Popup;
using IdleGame.Data.Common.Event;
using IdleGame.Main.Scene.Main;

namespace IdleGame.Main.UI
{
    /// <summary>
    /// [기능] 게임에서 졌을때 뜨는 팝업창입니다.
    /// <br> 다음 진행 처리 방식을 결정합니다. </br>
    /// </summary>
    public class Panel_GameFailPopup : Base_AnimationPopup
    {
        protected override void Logic_RegisterEvent_Custom()
        {
            Panel_MainGameScene.Event.RegisterEvent(eSceneEventType_MainGame.On_GameFail, Logic_OpenPopup);
        }


        public void Logic_OpenPopup()
        {
            Logic_Open_Base();
        }


        #region 콜백함수
        /// <summary>
        /// [버튼콜백] 게임 재시작 버튼이 누른경우 호출됩니다.
        /// </summary>
        public void OnClickReTryGame()
        {
            GameManager.Main.stage.Logic_ReStartStage();
            Logic_Close_Base();
        }

        #endregion
    }
}