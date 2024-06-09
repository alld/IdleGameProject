using IdleGame.Data.Base.Language;
using IdleGame.Data.NSave;

namespace IdleGame.Data.Option
{
    /// <summary>
    /// [데이터] 플레이어가 지정한 게임의 전반적인 설정을 담고 있습니다.
    /// </summary>
    public class Data_Option : Interface_SaveData
    {
        /// <summary>
        /// [상태] 현재 선택중인 언어를 나타냅니다. 
        /// </summary>
        public eLanguage language = eLanguage.Kr;
    }
}