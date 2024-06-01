namespace IdleGame.Data.Common.Event
{
    /// <summary>
    /// [종류] 어디서든 사용가능한 형태의 이벤트들을 구성하고 있습니다. 
    /// </summary>
    public enum eGlobalEventType
    {
        #region 공용 팝업 & 팝업 기능
        /// <summary> [공용팝업] 버튼이 한개(확인)인 팝업창을 엽니다. </summary>
        CommonPopup_OneBt_Open,
        /// <summary> [공용팝업] 버튼이 두개(확인, 취소)인 팝업창을 엽니다. </summary>
        CommonPopup_TwoBt_Open,
        /// <summary> [공용팝업] 버튼이 없는 팝업창을 엽니다. </summary>
        CommonPopup_NoBt_Open,

        /// <summary> [타이머] 5초 간격으로 실행되는 이벤트입니다. </summary>
        UpdateTime_5_00f,


        /// <summary> [공용팝업] 모든 팝업창을 끕니다. </summary>
        Act_AllClosePopup,
        /// <summary> [공용팝업] 가장 최근에 연 팝업창을 끕니다. </summary>
        Act_ClosePopup,
        #endregion
    }

    /// <summary>
    /// [종류] 특정 씬에서만 사용가능한 이벤트타입들을 정의합니다.
    /// <br> 해당 이벤트 타입은 <br>로드</br> 씬에서만 사용가능합니다. </br>
    /// </summary>
    public enum eSceneEventType_Load
    {
    }

    /// <summary>
    /// [종류] 특정 씬에서만 사용가능한 이벤트타입들을 정의합니다.
    /// <br> 해당 이벤트 타입은 <br>인트로</br> 씬에서만 사용가능합니다. </br>
    /// </summary>
    public enum eSceneEventType_Intro
    {
        /// <summary> [인트로페이지] 다음 페이지로 즉시 전환을 시도합니다. </summary>
        Act_IntroPage_NextPage,

        /// <summary> [인트로씬] 다음 씬으로 전환을 요청합니다. </summary>
        Act_ChangeScene,
    }

    /// <summary>
    /// [종류] 특정 씬에서만 사용가능한 이벤트타입들을 정의합니다.
    /// <br> 해당 이벤트 타입은 <br>MainGame</br> 씬에서만 사용가능합니다. </br>
    /// </summary>
    public enum eSceneEventType_MainGame
    {
        /// <summary> [메인게임] 멈쳐 있는 게임을 진행시킵니다. </summary>
        Act_GameStart,
        /// <summary> [메인게임] 게임의 진행을 중단시킵니다. </summary>
        Act_GameStop,

        /// <summary> [백사이드] 화면 연출을 중지하여 밧데리 소모를 감소시킵니다. </summary>
        BackSide_On,
        /// <summary> [백사이드] 일반적인 게임 화면으로 다시 전환합니다. </summary>
        BackSide_Off,
    }
}