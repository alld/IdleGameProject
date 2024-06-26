namespace IdleGame.Data.Common.Event
{
    /// <summary>
    /// [종류] 어디서든 사용가능한 형태의 이벤트들을 구성하고 있습니다. 
    /// </summary>
    public enum eGlobalEventType
    {
        #region 게임 흐름

        /// <summary> [기능] 게임을 시작시킵니다. (최초) </summary>
        Game_Start,

        #endregion

        #region 공용 팝업 & 팝업 기능
        /// <summary> [공용팝업] 버튼이 한개(확인)인 팝업창을 엽니다. </summary>
        CommonPopup_OneBt_Open,
        /// <summary> [공용팝업] 버튼이 두개(확인, 취소)인 팝업창을 엽니다. </summary>
        CommonPopup_TwoBt_Open,
        /// <summary> [공용팝업] 버튼이 없는 팝업창을 엽니다. </summary>
        CommonPopup_NoBt_Open,

        /// <summary> [공용팝업] 모든 팝업창을 끕니다. </summary>
        Popup_Act_AllClose,
        /// <summary> [공용팝업] 가장 최근에 연 팝업창을 끕니다. </summary>
        Popup_Act_Close,
        #endregion

        #region 시간 이벤트
        /// <summary> [타이머] 5초 간격으로 실행되는 이벤트입니다. </summary>
        TimeEvent_OnUpdate_5_00f,
        #endregion

        #region 데이터 테이블
        /// <summary> [웹응답] 웹서버로부터 데이터 응답이 온경우 호출됩니다. </summary>
        Table_OnResponseData,

        #endregion

        #region 세이브
        /// <summary> 모든 세이브가 완료된 경우 호출됩니다. </summary>
        Save_OnResponseSave,

        /// <summary> 로드 과정에서 단계별 진행과정이 클리어될때마다 호출됩니다. </summary>
        Save_OnResponseStep,

        /// <summary> 모든 데이터 로드가 완료된 경우 호출됩니다. </summary>
        Save_OnResponseLoad,

        #endregion

        #region 옵션
        /// <summary> 언어 설정이 변경된 경우 호출됩니다. </summary>
        Option_OnChangeLanguage,
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

        TestA,
    }
}