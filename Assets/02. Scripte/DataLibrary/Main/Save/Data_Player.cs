using IdleGame.Data.NSave;
using IdleGame.Data.Numeric;

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
        /// [데이터] 골드 재화 보유량입니다.
        /// </summary>
        public ExactInt cc_Gold = new ExactInt(0);

        /// <summary>
        /// [데이터] 특성 포인트 보유량입니다. 
        /// </summary>
        public ExactInt cc_Ability = new ExactInt(0);
    }
}