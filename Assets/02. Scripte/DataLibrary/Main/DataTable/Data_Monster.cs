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
        public int index = -1;

        /// <summary>
        /// [데이터] 몬스터에 할당된 고유 ID입니다.
        /// </summary>
        public int monster_id = -1;

        /// <summary>
        /// [데이터] 몬스터의 이름입니다.
        /// </summary>
        public string monster_name = "";

        /// <summary>
        /// [데이터] 몬스터의 타입입니다.
        /// </summary>
        public int monster_type = -1;

        /// <summary>
        /// [데이터] 몬스터의 기본 레벨입니다.
        /// </summary>
        public int level = -1;

        /// <summary>
        /// [데이터] 몬스터의 이동속도입니다.
        /// </summary>
        public float speed = -1;

        /// <summary>
        /// [데이터] 몬스터의 최대 체력입니다.
        /// </summary>
        public int mon_max_hp = -1;

        /// <summary>
        /// [데이터] 공격 범위입니다.
        /// </summary>
        public int attack_range = -1;

        /// <summary>
        /// [데이터] 일반 공격 피해량입니다.
        /// </summary>
        public float mon_attack = -1;

        /// <summary>
        /// [데이터] 공격 스킬 피해량입니다.
        /// </summary>
        public float attack_skill = -1;

        /// <summary>
        /// [데이터] 보상 경험치입니다.
        /// </summary>
        public long experience_reward = -1;

        /// <summary>
        /// [데이터] 보상 재화입니다..
        /// </summary>
        public long gold_reward = -1;

        /// <summary>
        /// [데이터] 동료 보상유무를 나타냅니다.
        /// </summary>
        public int colleague_reward = -1;

        /// <summary>
        /// [데이터] 몬스터의 속성입니다. 
        /// </summary>
        public int proprty = -1;
    }
}