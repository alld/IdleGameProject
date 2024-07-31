using IdleGame.Data.Base.BottomPage;
using IdleGame.Data.NSave;

namespace IdleGame.Data
{
    /// <summary>
    /// [데이터] 게임이 진행된 상황에대한 데이터를 정의합니다.
    /// </summary>
    public class Data_PlayProgress : Interface_SaveData
    {
        /// <summary>
        /// [상태] 현재 보여지고 있는 페이지에대한 정보입니다. 
        /// </summary>
        public eUIPage currentPage = eUIPage.AbilityUpgrade;

        /// <summary>
        /// [상태] 현재 진행중인 메인 스테이지의 고유 인덱스입니다.
        /// </summary>
        public int stage_curIndex = 1001;

        /// <summary>
        /// [상태] 현재 진행중인 메인 스테이지의 웨이브 단계입니다.
        /// </summary>
        public int stage_curWave = 0;
    }
}