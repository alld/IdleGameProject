using IdleGame.Core.Popup;

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

        }


        public void Logic_OpenPopup()
        {

        }


        #region 콜백함수
        /// <summary>
        /// [버튼콜백] 게임 재시작 버튼이 누른경우 호출됩니다.
        /// </summary>
        public void OnClickReTryGame()
        {

        }

        #endregion
    }
}