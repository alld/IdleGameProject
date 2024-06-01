using IdleGame.Data.Common.Event;
using IdleGame.Main.Scene.Intro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.Core.Intro
{
    /// <summary>
    /// [기능] 인트로 페이지는 인트로 화면에 제공되는 스플래쉬 페이지를 의미하며 해당 페이즈들의 전환과정을 관리합니다.
    /// </summary>
    public class Panel_IntroPage : Base_Panel
    {
        /// <summary>
        /// [캐시] 인트로 화면에서 관리되는 페이지 그룹입니다. 
        /// </summary>
        [SerializeField]
        private Graphic_IntroPage[] _pages;

        /// <summary>
        /// [캐시] 스킵을 시도하기위해 화면을 클릭하는 영역에 해당하는 버튼입니다.
        /// </summary>
        [SerializeField]
        private Button b_screenTouch;

        /// <summary>
        /// [상태] 현재 활성화중인 페이지의 인덱스를 나타냅니다. 
        /// </summary>
        private int _currentPageIndex = -1;

        /// <summary>
        /// [캐시] 다음 페이지로 넘어가기 전에 기본적인 대기시간을 적용한 코루틴입니다.
        /// </summary>
        private Coroutine _co_AutoDelayNextPage;

        /// <summary>
        /// [캐시] 다음 페이지로 전환대기까지 기다리는 시간입니다.
        /// </summary>
        private WaitForSeconds _delay_Page = new WaitForSeconds(3f);

        protected override void Logic_Init_Custom()
        {
            b_screenTouch.interactable = true;

            foreach (var page in _pages)
            {
                page.Logic_Init();
            }

            Logic_NextPage();

        }

        protected override void Logic_RegisterEvent_Custom()
        {
            Panel_IntroScene.Event.RegisterEvent(eSceneEventType_Intro.Act_IntroPage_NextPage, Logic_NextPage);
        }

        /// <summary>
        /// [기능] 다음 페이지로 전환합니다.
        /// </summary>
        private void Logic_NextPage()
        {
            _currentPageIndex++;
            if (_co_AutoDelayNextPage != null) StopCoroutine(_co_AutoDelayNextPage);

            // 역할 :: 더이상 넘어갈 페이지가 없는 경우 페이지 전환을 종료합니다.
            if (_currentPageIndex >= _pages.Length)
            {
                Logic_Finish();
                return;
            }

            _co_AutoDelayNextPage = StartCoroutine(Logic_AutoDelayNextPage());
        }

        /// <summary>
        /// [기능] 일정한 지연시간이 적용 된 후에 다음 페이지로 전환을 합니다.
        /// </summary>
        private IEnumerator Logic_AutoDelayNextPage()
        {
            _pages[_currentPageIndex].Logic_FadeIn();

            yield return _delay_Page;


            _pages[_currentPageIndex].Logic_FadeOut();
            Logic_NextPage();
        }


        /// <summary>
        /// [기능] 모든 페이지 전환이 완료된 경우 호출합니다.
        /// <br> 인트로 씬을 호출하여 씬전환을 시도합니다. </br>
        /// </summary>
        private void Logic_Finish()
        {
            Panel_IntroScene.Event.CallEvent(eSceneEventType_Intro.Act_ChangeScene);

            b_screenTouch.interactable = false;
        }

        #region 콜백 함수
        /// <summary>
        /// [버튼콜백] 스크린 버튼이 클릭된 경우 호출됩니다. 다음 페이지로의 전환을 시도합니다. 
        /// </summary>
        public void OnClickScreenTouch()
        {
            if (_currentPageIndex >= _pages.Length)
                return;

            _pages[_currentPageIndex].Logic_TrySkip();
        }
        #endregion
    }
}