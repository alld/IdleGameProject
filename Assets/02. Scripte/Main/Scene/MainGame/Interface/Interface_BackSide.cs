using IdleGame.Data.Common.Event;

namespace IdleGame.Main.Scene.Main
{
    /// <summary>
    /// [인터페이스] 백사이드로 화면 전환하는 기능을 담습니다. 
    /// </summary>
    public interface Interface_BackSide
    {
        public void ILogic_RegisterEvent()
        {
            Panel_MainGameScene.Event.RegisterEvent(eSceneEventType_MainGame.BackSide_On, ILogic_BackSideOn);
            Panel_MainGameScene.Event.RegisterEvent(eSceneEventType_MainGame.BackSide_On, ILogic_BackSideOff);
        }

        /// <summary>
        /// [기능] 백사이드가 활성화된 경우에대한 대응이 담깁니다.
        /// </summary>
        public void ILogic_BackSideOn();

        /// <summary>
        /// [기능] 백사이드가 비활성화된 경우에대한 대응이 담깁니다.
        /// </summary>
        public void ILogic_BackSideOff();
    }
}