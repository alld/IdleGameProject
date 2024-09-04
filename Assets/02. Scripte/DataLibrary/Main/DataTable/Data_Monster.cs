using IdleGame.Core.Unit;
using IdleGame.Data.Numeric;

namespace IdleGame.Data
{
    /// <summary>
    /// [데이터] 몬스터를 구성하기위한 데이터 구성을 담고 있습니다. 
    /// </summary>
    public class Data_Monster
    {
        /// <summary>
        /// [데이터] 몬스터 고유 인덱스입니다.
        /// </summary>
        public int idx = -1;

        /// <summary>
        /// [데이터] 몬스터에 할당된 고유 ID입니다.
        /// </summary>
        public int monster_id = -1;

        /// <summary>
        /// [데이터] 몬스터외형 ID입니다.
        /// </summary>
        public int mon_shape_id = -1;

        /// <summary>
        /// [데이터] 배경이 적용되는 ID입니다.
        /// </summary>
        public int background_id = -1;

        /// <summary>
        /// [데이터] 몬스터의 이름입니다.
        /// </summary>
        public string monster_name = string.Empty;

        /// <summary>
        /// [데이터] 몬스터의 타입입니다.
        /// </summary>
        public eMonsterType monster_type = eMonsterType.normal;

        /// <summary>
        /// [데이터] 몬스터의 기본 레벨입니다.
        /// </summary>
        public int level = -1;

        /// <summary>
        /// [데이터] 몬스터의 방어력입니다.
        /// </summary>
        public ExactInt defense = new ExactInt(0);

        /// <summary>
        /// [데이터] 몬스터의 이동속도입니다.
        /// </summary>
        public float speed = -1;

        /// <summary>
        /// [데이터] 몬스터의 최대 체력입니다.
        /// </summary>
        public ExactInt mon_max_hp = new ExactInt(0);

        /// <summary>
        /// [데이터] 공격 범위입니다.
        /// </summary>
        public int attack_range = -1;

        /// <summary>
        /// [데이터] 공격 속도입니다.
        /// </summary>
        public int attack_speed = -1;

        /// <summary>
        /// [데이터] 일반 공격 피해량입니다.
        /// </summary>
        public ExactInt mon_attack = new ExactInt(0);

        /// <summary>
        /// [데이터] 공격 스킬 피해량입니다.
        /// </summary>
        public ExactInt attack_skill = new ExactInt(0);

        /// <summary>
        /// [데이터] 보상 경험치입니다.
        /// </summary>
        public ExactInt experience_reward = new ExactInt(0);

        /// <summary>
        /// [데이터] 보상 재화입니다..
        /// </summary>
        public ExactInt gold_reward = new ExactInt(0);

        /// <summary>
        /// [데이터] 동료 보상유무를 나타냅니다.
        /// </summary>
        public int colleague_reward = -1;

        /// <summary>
        /// [데이터] 몬스터의 속성입니다. 
        /// </summary>
        public eUnitProperty proprty = eUnitProperty.None;
    }
}