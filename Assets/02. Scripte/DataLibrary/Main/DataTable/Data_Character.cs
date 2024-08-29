using IdleGame.Data.Numeric;

namespace IdleGame.Data
{
    /// <summary>
    /// [데이터] 캐릭터 정보를 담고 있습니다. 
    /// </summary>
    public class Data_Character
    {
        /// <summary>
        /// [데이터] 고유 인덱스입니다.
        /// </summary>
        public int index = -1;

        /// <summary>
        /// [데이터] 플레이어 데이터인지를 구분합니다.
        /// </summary>
        public bool player_value = false;

        /// <summary>
        /// [데이터] 유닛의 기본 방어력입니다.
        /// </summary>
        public ExactInt defens = new ExactInt(0);

        /// <summary>
        /// [데이터] 캐릭터의 고유 ID입니다.
        /// </summary>
        public int character_id = -1;

        /// <summary>
        /// [데이터] 캐릭터의 외형 Id입니다.
        /// </summary>
        public int shape_id = -1;

        /// <summary>
        /// [데이터] 해당 캐릭터를 획득 가능한 스테이지 ID입니다
        /// </summary>
        public int stage_id = -1;

        /// <summary>
        /// [데이터] 해당 유닛의 피해량입니다.
        /// </summary>
        public ExactInt damage = new ExactInt(0);

        /// <summary>
        /// [데이터] 치명타 확률입니다.
        /// </summary>
        public float critical_chance = 1;

        /// <summary>
        /// [데이터] 치명타 배률입니다.
        /// </summary>
        public float critical_strike_rate = 1;

        /// <summary>
        /// [데이터] 해당 유닛의 피해량입니다.
        /// </summary>
        public float attack_speed = 1;

        /// <summary>
        /// [데이터] 해당 유닛의 체력입니다.
        /// </summary>
        public ExactInt hp = new ExactInt(0);

        /// <summary>
        /// [데이터] 해당 캐릭터의 이동속도입니다.
        /// </summary>
        public float speed = 1;

        /// <summary>
        /// [데이터] 유닛의 공격 범위입니다.
        /// </summary>
        public float attack_range;

        /// <summary>
        /// [데이터] 해당 유닛의 고유 스킬 ID입니다.
        /// </summary>
        public int skill = -1;

        /// <summary>
        /// [데이터] 해당 동료의 고유 효과 입니다. 
        /// </summary>
        public int effect = -1;
    }
}