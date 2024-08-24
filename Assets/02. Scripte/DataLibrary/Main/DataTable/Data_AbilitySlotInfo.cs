using IdleGame.Data.Numeric;

namespace IdleGame.Data
{
    /// <summary>
    /// [데이터] 능력치 상승에 관한 레벨 데이터입니다. 
    /// </summary>
    public struct Data_AbilitySlotInfo
    {
        /// <summary>
        /// [데이터] 테이블 고유 번호입니다.
        /// </summary>
        public int index;

        /// <summary>
        /// [데이터] 해당 레벨 수치입니다. 
        /// </summary>
        public int level;

        /// <summary>
        /// [데이터] 단위환산형 값입니다.
        /// </summary>
        public ExactInt value_e;

        /// <summary>
        /// [데이터] 배율을 표현한 값입니다.
        /// </summary>
        public float value_f;

        /// <summary>
        /// [데이터] 레벨에 해당하는 가격입니다.
        /// </summary>
        public ExactInt price;
    }
}