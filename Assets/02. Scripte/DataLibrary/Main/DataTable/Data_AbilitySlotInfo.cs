using IdleGame.Data.Numeric;

namespace IdleGame.Data
{
    /// <summary>
    /// [데이터] 능력치 상승에 관한 레벨 데이터입니다. 
    /// </summary>
    public struct Data_AbilitySlotInfo
    {
        /// <summary>
        /// [데이터] 해당 레벨 수치입니다. 
        /// </summary>
        public int level;

        public ExactInt value_e;

        public float value_f;

        public int value_i;

        /// <summary>
        /// [데이터] 레벨에 해당하는 가격입니다.
        /// </summary>
        public ExactInt price;
    }
}