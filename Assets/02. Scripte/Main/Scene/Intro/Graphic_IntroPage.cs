using DG.Tweening;
using IdleGame.Data.Common.Event;
using IdleGame.Main.Scene.Intro;
using UnityEngine;

namespace IdleGame.Core.Intro
{
    /// <summary>
    /// [기능] 인트로 페이지의 전환 연출 로직을 담고 있습니다.
    /// </summary>
    public class Graphic_IntroPage : MonoBehaviour
    {
        /// <summary>
        /// [캐시] 페이지의 내용을 담고 있는 캔버스 그룹입니다. 페이드 연출시 사용됩니다. 
        /// </summary>
        [SerializeField]
        private CanvasGroup _cg_graphic;

        /// <summary>
        /// [상태] 현재 스킵이 눌린 상태인지 확인합니다. 
        /// </summary>
        private bool _isSkip = false;

        /// <summary>
        /// [상태] 현재 두트윈이 적용중인지를 확인합니다.
        /// </summary>
        private bool _isTweening = false;

        /// <summary>
        /// [상태] 마지막 페이지인지를 확인합니다.
        /// </summary>
        [SerializeField]
        private bool _isLastPage = false;

        /// <summary>
        /// [기능] 인트로 페이지를 초기화시킵니다.
        /// </summary>
        public void Logic_Init()
        {
            _cg_graphic.DOKill();

            _isSkip = false;

            _cg_graphic.alpha = 0;

            _cg_graphic.gameObject.SetActive(false);
        }

        /// <summary>
        /// [기능] 페이지 연출 대기를 하지않고 즉각적인 전환을 시도합니다.  
        /// </summary>
        public void Logic_TrySkip()
        {
            if (_isSkip) return;
            _isSkip = true;

            if (!_isTweening)
            {
                Panel_IntroScene.Event.CallEvent(eSceneEventType_Intro.Act_IntroPage_NextPage);
                Logic_FadeOut();
            }

        }

        /// <summary>
        /// [기능] 인트로 페이지를 페이드인 시킵니다.
        /// </summary>
        public void Logic_FadeIn()
        {
            _cg_graphic.gameObject.SetActive(true);
            _isTweening = true;

            _cg_graphic.DOKill();
            _cg_graphic.DOFade(1, 2f).OnComplete(
                () =>
                {
                    _isTweening = false;

                    if (_isSkip)
                    {
                        Panel_IntroScene.Event.CallEvent(eSceneEventType_Intro.Act_IntroPage_NextPage);
                        Logic_FadeOut();
                    }
                });
        }

        /// <summary>
        /// [기능] 인트로 페이지를 페이드 아웃시킵니다. 
        /// </summary>
        public void Logic_FadeOut()
        {
            if (_isLastPage) return;
            _isTweening = true;

            _cg_graphic.DOKill();
            _cg_graphic.DOFade(0, 1.5f).OnComplete(
                () =>
                {
                    _isTweening = false;
                    _cg_graphic.gameObject.SetActive(false);
                });
        }
    }
}