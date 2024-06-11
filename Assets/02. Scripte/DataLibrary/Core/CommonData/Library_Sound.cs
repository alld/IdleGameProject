using UnityEngine;

namespace IdleGame.Data.Common
{
    /// <summary>
    /// [종류] 사운드의 형태를 구분합니다. 
    /// </summary>
    public enum eSoundType
    {
        None = 0,
        BGM,
        SFX,
        HPE,
        Voice,
    }

    /// <summary>
    /// [종류] 실제 사운드가 사용되는 종류를 구분합니다.
    /// </summary>
    public enum eSoundKind
    {
        None = 0,

        #region 공용 UI
        Mouse_Click,
        Mouse_Click_Toggle,
        #endregion

        #region 팝업
        Popup_Open,
        Popup_Close,
        #endregion
    }

    /// <summary>
    /// [데이터] 사운드 매니저에서 관리되는 오디오 그룹 컴포넌트입니다. 
    /// </summary>
    [System.Serializable]
    public struct Data_AudioSourceComponent
    {
        /// <summary>
        /// [캐시] 배경음을 전담하여 재생시키는 오디오입니다.
        /// </summary>
        [Header("BGM")]
        public AudioSource[] bgm;

        /// <summary>
        /// [캐시] 일반적인 사운드등을 재생시키는 오디오입니다.
        /// </summary>
        [Header("SFX")]
        public AudioSource[] sfx;

        /// <summary>
        /// [캐시] 특수 관리처리가 가능한 사운드들을 재생시키는 오디오입니다.
        /// </summary>
        [Header("HPE")]
        public AudioSource[] hpe;

        /// <summary>
        /// [캐시] 음성만 별도로 재생하는 오디오입니다. 
        /// </summary>
        [Header("Voice")]
        public AudioSource[] voice;
    }


    /// <summary>
    /// [데이터] 루프 타입을 통제할 수있는 사운드 데이터입니다.
    /// </summary>
    public struct Data_HPE
    {
        /// <summary>
        /// [상태] 현재 재생중인 사운드 플레이어 인덱스입니다. 
        /// </summary>
        private int _playIndex;

        /// <summary>
        /// [상태] 현재 데이터가 사용된 데이터인지를 나타냅니다.
        /// </summary>
        private bool _isUsedData;

        /// <summary>
        /// [생성자] 해당 생성로직은 사운드 매니저에서 사용됩니다.
        /// </summary>
        public Data_HPE(int m_index)
        {
            _playIndex = m_index;
            _isUsedData = true;
        }

        /// <summary>
        /// [기능] 해당 사운드 데이터가 사용된 경우 호출 처리됩니다. 
        /// <br> 사운드 매니저 이외에 공간에서 해당 함수를 호출 처리할 경우 예기치 못한 문제가 발생 할 수 있습니다. </br>
        /// </summary>
        public void Logic_SetUseSound() => _isUsedData = false;

        /// <summary>
        /// [기능] 해당 사운드의 플레이어 인덱스를 반환합니다. 
        /// </summary>
        public int Logic_GetPlayIndex() => _playIndex;

        /// <summary>
        /// [기능] 해당 사운드가 사용된 데이터인지를 구분합니다. 
        /// </summary>
        public bool Logic_GetUseSound() => _isUsedData;
    }
}