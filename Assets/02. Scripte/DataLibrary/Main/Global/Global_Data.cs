using IdleGame.Data.Option;
using IdleGame.Data.Save;

namespace IdleGame.Data
{
    /// <summary>
    /// [기능] 게임이 진행되면서 발생하는 모든 동적 데이터를 관리하고 접근가능하게 합니다. 
    /// </summary>
    public static class Global_Data
    {
        /// <summary>
        /// [데이터] 세이브데이터의 가장 기본적인 데이터 구성들에대한 정보들을 담고 있습니다.
        /// </summary>
        public static Data_MainSave Main = new Data_MainSave();

        /// <summary>
        /// [데이터] 플레이어에 관련된 기본적인 정보들을 담고 있습니다. 대표적인것이 재화들이 있습니다.
        /// </summary>
        public static Data_Player Player = new Data_Player();

        /// <summary>
        /// [데이터] 플레이어가 지정한 게임에 관한 전반적인 설정입니다. 
        /// </summary>
        public static Data_Option Option = new Data_Option();
    }
}