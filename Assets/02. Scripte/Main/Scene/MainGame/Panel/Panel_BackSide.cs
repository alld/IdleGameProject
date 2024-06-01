using IdleGame.Core;
using IdleGame.Data.Common.Event;
using UnityEngine;

namespace IdleGame.Main.Scene.Main
{
    /* Todo :: 메인 계산 로직, 흐름 로직을 베이스를 구축함
     * 
     * 1. 인게임용 / 백그라운드용 2가지를 만듬.
     * 
     * 2. 백그라운드로 전환될때 해당 로직 호출 부분의 값을 백그라운드용으로 교체
     * 
     */


    /// <summary>
    /// [기능] 밧데리 소모를 줄이기위해 마련된 백사이드 공간입니다.
    /// </summary>
    public class Panel_BackSide : Base_Panel, Interface_BackSide
    {
        /// <summary>
        /// [캐시] 패널에서 관리되고 노출시키는 그래픽 오브젝트입니다.
        /// </summary>
        [Header("BasicComponent")]
        [SerializeField]
        private GameObject _obj_graphic;


        protected override void Logic_RegisterEvent_Custom()
        {
            Panel_MainGameScene.Event.RegisterEvent(eSceneEventType_MainGame.BackSide_On, ILogic_BackSideOn);
        }

        public void ILogic_BackSideOff()
        {
            Panel_MainGameScene.Event.CallEvent(eSceneEventType_MainGame.BackSide_Off);
            _obj_graphic.SetActive(false);
        }

        public void ILogic_BackSideOn()
        {
            _obj_graphic.SetActive(true);
        }

        #region 콜백 함수
        /// <summary>
        /// [버튼콜백] 백사이드를 종료하고 기존 화면으로 돌아갑니다. 
        /// </summary>
        public void OnClickContinueGame()
        {
            ILogic_BackSideOff();
        }

        #endregion
    }
}