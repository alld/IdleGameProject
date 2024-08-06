using IdleGame.Data;
using IdleGame.Data.Numeric;

namespace IdleGame.Core.Unit
{
    /// <summary>
    /// [데이터] 유닛의 기본 스텟 구조를 정의합니다. 
    /// </summary>
    public struct Data_UnitAbility
    {
        /// <summary>
        /// [데이터] 현재 체력
        /// </summary>
        public ExactInt hp;

        /// <summary>
        /// [데이터] 피해량
        /// </summary>
        public ExactInt damage;

        /// <summary>
        /// [데이터] 유닛의 아이디입니다. 
        /// </summary>
        public int Id;

        /// <summary>
        /// [데이터] 유닛의 공격 사거리입니다. 
        /// </summary>
        public float attackRange;

        public static explicit operator Data_UnitAbility(Data_Monster m_data) => new Data_UnitAbility(m_data);
        public static explicit operator Data_UnitAbility(Data_Character m_data) => new Data_UnitAbility(m_data);

        /// <summary>
        /// [생성자] 몬스터 데이터를 기본형으로 변환시킵니다.
        /// </summary>
        public Data_UnitAbility(Data_Monster m_data)
        {
            attackRange = m_data.attack_range;
            hp = (ExactInt)m_data.mon_max_hp;
            damage = (ExactInt)m_data.mon_attack;

            Id = m_data.monster_id;
        }

        /// <summary>
        /// [생성자] 몬스터 데이터를 기본형으로 변환시킵니다.
        /// </summary>
        public Data_UnitAbility(Data_Character m_data)
        {
            attackRange = m_data.attack_range;
            hp = (ExactInt)0;
            damage = (ExactInt)0;

            Id = m_data.character_id;
        }
    }
}