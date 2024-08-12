using DG.Tweening;
using IdleGame.Core;
using IdleGame.Core.Unit;
using IdleGame.Core.Utility;
using IdleGame.Data;
using IdleGame.Data.Common.Event;
using IdleGame.Data.DataTable;
using System.Collections;
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

        /// <summary>
        /// [상태] 테이블 데이터가 얼마나 불렸는지 카운터합니다. 
        /// <br> 개발 환경에서만 사용되어지는 값입니다.  </br>
        /// </summary>
        private int _step = 0;


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
            // 조건 :: 개발 환경에서 인게임씬으로 바로 진입한 경우
            if (GameManager.Scene.Logic_ConditionIsEntryScene())
            {
                StartCoroutine(Logic_DevelopEntryStart());
                return;
            }

            Global_Data.FirstSetting();
            // TODO:: 듀토리얼 같은거..넣어야함..
            Logic_FadeOutScreen();
        }

        /// <summary>
        /// [기능] 개발환경에서 게임을 바로 시작할경우 초기 필요한 기능들을 설정해줍니다.
        /// <br> 개발환경에서만 사용되어집니다. </br>
        /// </summary>
        private IEnumerator Logic_DevelopEntryStart()
        {
            GameManager.Save.Logic_Load();

            GameManager.Event.RegisterEvent(eGlobalEventType.Save_OnResponseStep, Logic_LoadCount);
            GameManager.Table.Logic_TryLoadData(eDataTableType.GameInfo);

            while (_step < Library_DataTable.DataTableCount)
                yield return Utility_Common.WaitForSeconds(0.5f);

            Global_Data.FirstSetting();
            Logic_FadeOutScreen();
        }

        /// <summary>
        /// [기능] 테이블로드가 완료되었는지 판단합니다. 
        /// <br> 개발환경에서만 사용되어집니다. </b>
        /// </summary>
        private void Logic_LoadCount()
        {
            _step++;
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

            GameManager.Main.Logic_GameStart();



            Panel_MainGameScene.Event.CallEvent(eSceneEventType_MainGame.Act_GameStart);
        }
    }
}