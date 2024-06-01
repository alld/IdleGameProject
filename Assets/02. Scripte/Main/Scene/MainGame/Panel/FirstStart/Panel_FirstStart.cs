using DG.Tweening;
using IdleGame.Core;
using IdleGame.Data;
using IdleGame.Data.Common.Event;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.Main.Scene.Main
{
    /// <summary>
    /// [기능] 처음 게임에 진입했을때 진행되는 절차를 담당합니다. 
    /// </summary>
    public class Panel_FirstStart : Base_Panel
    {
        /// <summary>
        /// [캐시] 패널에서 관리되고 노출시키는 그래픽 오브젝트입니다.
        /// </summary>
        [Header("BasicComponent")]
        [SerializeField]
        private GameObject _obj_graphic;

        /// <summary>
        /// [캐시] 인트로씬에서 넘어온상태에서 매끄러운 화면 전환을 위해서 화면을 어둡게해주는 암막 이미지입니다.
        /// </summary>
        [SerializeField]
        private Image _i_fade;


        protected override void Logic_Init_Custom()
        {
            _obj_graphic.SetActive(true);

            if (Global_Data.Main.isFirstPlaying) Logic_StartFirstGame();
            else Logic_FadeOutScreen();
        }


        /// <summary>
        /// [기능] 게임을 처음 플레이한 경우 알맞는 연출을 진행시킵니다. 
        /// </summary>
        private void Logic_StartFirstGame()
        {

        }

        /// <summary>
        /// [기능] 가려진 암막 효과를 치우면서 게임진행이 가능한 상태로 전환합니다.
        /// </summary>
        private void Logic_FadeOutScreen()
        {
            _i_fade.DOFade(0, 2f).OnComplete(
                () =>
                {
                    Logic_GameStart();
                });
        }

        /// <summary>
        /// [기능] 실질적으로 게임플레이가 가능한 상태로 전환합니다.
        /// </summary>
        private void Logic_GameStart()
        {
            _obj_graphic.SetActive(false);

            Panel_MainGameScene.Event.CallEvent(eSceneEventType_MainGame.Act_GameStart);
        }
    }
}