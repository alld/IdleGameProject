using System;

namespace IdleGame.Data.Save
{
    /// <summary>
    /// [데이터] 세이브 데이터의 기본 구성을 담고 있습니다.
    /// </summary>
    public class Data_MainSave
    {
        /// <summary>
        /// [데이터] 마지막으로 플레이한 시간 정보입니다.
        /// </summary>
        public DateTime lastPlayTime;

        /// <summary>
        /// [데이터] 실제 게임을 플레이한 시간입니다..
        /// </summary>
        public DateTime totalPlyingTime;

        /// <summary>
        /// [상태] 최신에 갱신된 시간인지를 확인하는 상태값입니다.
        /// </summary>
        public bool isUpdateTime = false;


        /// <summary>
        /// [데이터] 게임에 최초 접속하는 유저인지를 판단합니다. 
        /// </summary>
        public bool isFirstPlaying = false;

    }
}