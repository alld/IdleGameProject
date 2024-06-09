using DG.Tweening;
using IdleGame.Core.GameInfo;
using IdleGame.Data.Common.Event;
using IdleGame.Data.DataTable;
using IdleGame.Main;
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
        /// [데이터] 로딩을 실제 진행하는 페이지 번호입니다. 
        /// </summary>
        [Header("로딩 페이지")]
        [SerializeField]
        private int _loadPageIndex = 2;

        /// <summary>
        /// [캐시] 로딩화면에서 노출되는 게임버전입니다.
        /// </summary>
        [SerializeField]
        private Graphic_Text _t_gameVersion;

        /// <summary>
        /// [캐시] 로딩이 완료되면 화면을 클릭하라는 안내 문구가 담긴 텍스트입니다.
        /// </summary>
        [SerializeField]
        private Graphic_Text _t_touchScreen;

        /// <summary>
        /// [캐시] 로딩의 진행상태를 나타내는 로딩바입니다.
        /// </summary>
        [SerializeField]
        private CanvasGroup _cg_LoadingBar;

        /// <summary>
        /// [캐시] 로딩바의 현재 진행상태를 나타내는 게이지입니다.
        /// </summary>
        [SerializeField]
        private Image _i_progressGauge;

        /// <summary>
        /// [캐시] 로딩바의 현재 상태를 표시하는 텍스트입니다.
        /// </summary>
        [SerializeField]
        private Graphic_Text _t_loadingBar;

        /// <summary>
        /// [캐시] 로딩바의 연출용 텍스트입니다.
        /// </summary>
        [SerializeField]
        private Graphic_Text _t_loadingBarPeriod;

        /// <summary>
        /// [상태] 현재 활성화중인 페이지의 인덱스를 나타냅니다. 
        /// </summary>
        private int _currentPageIndex = -1;

        /// <summary>
        /// [상태] 현재 로딩이 총진행된 스텝을 나타냅니다. 
        /// </summary>
        private float _loadingCount = 0;

        private const float _LoadMaxStep = 3 + Library_DataTable.DataTableCount;

        /// <summary>
        /// [캐시] 다음 페이지로 넘어가기 전에 기본적인 대기시간을 적용한 코루틴입니다.
        /// </summary>
        private Coroutine _co_AutoDelayNextPage;

        /// <summary>
        /// [캐시] 다음 페이지로 전환대기까지 기다리는 시간입니다.
        /// </summary>
        private WaitForSeconds _delay_Page = new WaitForSeconds(3f);

        /// <summary>
        /// [캐시] 다음 페이지로 전환대기까지 기다리는 시간입니다.
        /// </summary>
        private WaitForSeconds _delay_LoadPage = new WaitForSeconds(1f);

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
            GameManager.Event.RegisterEvent(eGlobalEventType.Save_OnResponseStep, Logic_AddCountLoadStep);
        }


        private void OnDestroy()
        {
            GameManager.Event.RemoveEvent(eGlobalEventType.Save_OnResponseStep, Logic_AddCountLoadStep);
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

            if (_currentPageIndex != _loadPageIndex)
                _co_AutoDelayNextPage = StartCoroutine(Logic_AutoDelayNextPage());
            else
                _co_AutoDelayNextPage = StartCoroutine(Logic_GameLoadingProcedure());
        }

        #region 게임 데이터 로딩 페이지

        /// <summary>
        /// [기능] 게임 데이터를 불러와서 설정하는 절차를 진행합니다. 
        /// </summary>
        private IEnumerator Logic_GameLoadingProcedure()
        {
            Logic_SetActiveLoadingBar(true);
            Logic_AddCountLoadStep();
            _pages[_currentPageIndex].Logic_FadeIn();
            yield return _delay_LoadPage;
            _t_loadingBar.text = "데이터 로딩중";
            StartCoroutine(Logic_LoadingBarTextEffect());

            GameManager.Table.Logic_TryLoadData(eDataTableType.GameInfo);
            if (GameManager.Save.usedLoadData)
            {
                GameManager.Save.Logic_Load();


                while (_loadingCount < _LoadMaxStep)
                {
                    yield return null;
                }
            }
            else
            {
                _i_progressGauge.DOFillAmount(1, 0.8f);
                yield return new WaitForSeconds(1f);
            }


            Logic_SetActiveLoadingBar(false);
        }

        /// <summary>
        /// [기능] 진행상황에 맞쳐서 로딩바를 움직입니다.
        /// </summary>
        public void Logic_AddCountLoadStep()
        {
            _loadingCount++;

            if (_loadingCount > _LoadMaxStep) return;

            _i_progressGauge.DOKill();
            _i_progressGauge.DOFillAmount(_loadingCount / _LoadMaxStep, 0.5f);

        }

        /// <summary>
        /// [기능] 로딩바 전환 유무를 지정합니다. 
        /// </summary>
        private void Logic_SetActiveLoadingBar(bool m_active)
        {
            if (m_active)
            {
                _t_gameVersion.SetText(Global_GameInfo.version + "v");
                _t_loadingBar.gameObject.SetActive(true);
                _t_loadingBarPeriod.gameObject.SetActive(true);
                _t_touchScreen.gameObject.SetActive(false);
                b_screenTouch.interactable = false;
            }
            else
            {
                _cg_LoadingBar.DOFade(0, 0.5f)
                    .OnComplete(
                    () =>
                    {
                        b_screenTouch.interactable = true;
                        _t_touchScreen.gameObject.SetActive(true);
                        Logic_TouchScreenTextEffect();
                        _t_loadingBar.gameObject.SetActive(false);
                    });
            }
        }

        /// <summary>
        /// [기능] 로딩바 텍스트 문구의 진행상태를 나타냅니다. 
        /// </summary>
        private IEnumerator Logic_LoadingBarTextEffect()
        {
            int count = 0;
            string period = "";
            while (!(_currentPageIndex != _loadPageIndex || !_t_loadingBar.gameObject.activeSelf))
            {
                count++;
                count %= 3;
                period = "";
                for (int i = 0; i <= count; i++)
                {
                    period += ".";
                }
                _t_loadingBarPeriod.text = period;
                yield return new WaitForSeconds(0.5f);
            }
        }

        /// <summary>
        /// [기능] 스크린 터치 안내 문구에 연출 효과를 적용합니다.
        /// </summary>
        private void Logic_TouchScreenTextEffect(bool m_show = true)
        {
            if (_currentPageIndex != _loadPageIndex) return;

            _t_touchScreen.GetText().DOFade(m_show ? 1 : 0, 1)
                .OnComplete(() => { Logic_TouchScreenTextEffect(!m_show); })
                .SetDelay(0.3f);
        }

        #endregion

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