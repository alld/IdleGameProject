using IdleGame.Data.Base.BottomPage;
using IdleGame.Data.NSave;

namespace IdleGame.Data
{
    /// <summary>
    /// [데이터] 플레이어에 관련된 동적 데이터 정보들을 담고 있습니다. 
    /// </summary>
    public class Data_Player : Interface_SaveData
    {
        /// <summary>
        /// [데이터] 기본적으로 적용되는 디폴트 이름입니다.
        /// </summary>
        private const string DefaultName = "기본 이름입니다.";


        /// <summary>
        /// [데이터] 플레이어의 이름입니다.
        /// </summary>
        public string nick = DefaultName;

        /// <summary>
        /// [상태] 현재 보여지고 있는 페이지에대한 정보입니다. 
        /// </summary>
        public eUIPage currentPage = eUIPage.AbilityUpgrade;
    }
}