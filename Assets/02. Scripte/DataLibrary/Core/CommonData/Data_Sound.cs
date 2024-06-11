using UnityEngine;

namespace IdleGame.Data.Common
{
    /// <summary>
    /// [데이터] 사운드의 형태를 정의합니다. 
    /// </summary>
    [System.Serializable]
    public struct Data_Sound
    {
        /// <summary>
        /// [데이터] 사운드 데이터의 이름입니다.
        /// </summary>
        public string name;

        /// <summary>
        /// [데이터] 사운드의 형태를 규정합니다.
        /// </summary>
        public eSoundType type;

        /// <summary>
        /// [캐시] 사운드 클립입니다. 
        /// </summary>
        public AudioClip clip;

        /// <summary>
        /// [데이터] 사운드의 사용 종류를 규정합니다.
        /// </summary>
        public eSoundKind kind;
    }
}