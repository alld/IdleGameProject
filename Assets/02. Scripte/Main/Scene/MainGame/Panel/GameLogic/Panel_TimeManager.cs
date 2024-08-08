using IdleGame.Core;
using IdleGame.Core.Utility;
using IdleGame.Data;
using IdleGame.Data.Common.Event;
using System;
using System.Collections;
using UnityEngine;

namespace IdleGame.Main.GameLogic
{
    /* 정각 마다, 이벤트 발생
     * 지나온 시간을 계산해서, 이벤트를 발생시킴 (출석 보상같은거...)
     * 
     * 
     * 
     * 
     */


    /// <summary>
    /// [기능] 전반적인 시간의 흐름을 관리합니다. 
    /// </summary>
    public class Panel_TimeManager : Base_ManagerPanel
    {
        /// <summary>
        /// [캐시] 작동중인 타이머 코루틴입니다.
        /// </summary>
        private Coroutine _co_Timer;

        /// <summary>
        /// [캐시] 타이머를 1초단위로 진행시키기위한 딜레이값입니다.
        /// </summary>
        private WaitForSeconds _delay_1_00f = Utility_Common.WaitForSeconds(1f);

        /// <summary>
        /// [상태] 타이머 정보를 갱신할지를 판단하는 설정데이터입니다.
        /// </summary>
        private bool _activeTimer = false;

        protected override void Logic_Init_Custom()
        {
            Logic_StartTimer();
        }


        /// <summary>
        /// [기능] 새로운 타이머를 작동시킵니다. 
        /// </summary>
        public void Logic_StartTimer()
        {
            _activeTimer = true;

            if (_co_Timer == null)
                _co_Timer = StartCoroutine(Logic_Timer());
        }

        /// <summary>
        /// [기능] 타이머를 중지시킵니다. 
        /// </summary>
        public void Logic_StopTimer()
        {
            _activeTimer = false;
        }

        /// <summary>
        /// [기능] 작동중인 타이머를 제거합니다.
        /// </summary>
        public void Logic_DeleteTimer()
        {
            Logic_StopTimer();
            if (_co_Timer != null) StopCoroutine(_co_Timer);
            _co_Timer = null;
        }

        private IEnumerator Logic_Timer()
        {
            int updateCount = 5;

            while (true)
            {
                yield return _delay_1_00f;
                if (updateCount-- == 0)
                {
                    if (GameManager.Scene.Logic_ConditionIsInGameScene())
                        GameManager.Save.Logic_Save(true);
                    GameManager.Event.CallEvent(eGlobalEventType.TimeEvent_OnUpdate_5_00f);
                    updateCount = 5;
                }

                if (_activeTimer)
                    Logic_AddCounting_PlayingTime();
            }
        }


        /// <summary>
        /// [기능] 현재 시간을 가산해서 시간정보를 갱신시킵니다.
        /// </summary>
        private void Logic_AddCounting_PlayingTime()
        {
            if (!Global_Data.Main.isUpdateTime)
                Logic_TimeCalculation();


            Global_Data.Main.lastPlayTime = DateTime.Now;
            Global_Data.Main.totalPlyingTime.AddSeconds(1);
        }

        /// <summary>
        /// [기능] 누락된 시간을 계산해서 필요한 결과를 반영시킵니다. 
        /// </summary>
        private void Logic_TimeCalculation()
        {
            Global_Data.Main.isUpdateTime = true;

            TimeSpan distance = Global_Data.Main.lastPlayTime - DateTime.Now;
            if (distance.TotalSeconds >= 600)
            {
                // TODO : 10분 이상 초과된 경우 특별한 무언가를 해줌.. 정산같은거??
            }
        }

    }
}