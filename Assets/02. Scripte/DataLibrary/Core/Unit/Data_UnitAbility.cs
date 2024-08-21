using IdleGame.Data;
using IdleGame.Data.Numeric;

namespace IdleGame.Core.Unit
{
    /// <summary>
    /// [데이터] 유닛의 기본 스텟 구조를 정의합니다. 
    /// </summary>
    public class Data_UnitAbility
    {
        /// <summary>
        /// [데이터] 현재 체력
        /// </summary>
        public ExactInt hp;

        /// <summary>
        /// [데이터] 현재 최대 체력
        /// </summary>
        public ExactInt maxHp;

        /// <summary>
        /// [데이터] 피해량
        /// </summary>
        public ExactInt damage;

        /// <summary>
        /// [데이터] 방어력
        /// </summary>
        public ExactInt defense;

        /// <summary>
        /// [데이터] 유닛의 아이디입니다. 
        /// </summary>
        public int Id;

        /// <summary>
        /// [데이터] 유닛의 이동속도입니다.
        /// </summary>
        public float moveSpeed;

        /// <summary>
        /// [데이터] 유닛의 공격 사거리입니다. 
        /// </summary>
        public float attackRange;

        /// <summary>
        /// [데이터] 치명타 확률입니다.
        /// </summary>
        public float critical_chance;

        /// <summary>
        /// [데이터] 치명타 배율입니다.
        /// </summary>
        public float critical_strike_rate;

        /// <summary>
        /// [데이터] 유닛의 속성입니다. 
        /// </summary>
        public eUnitProperty property;

        public static explicit operator Data_UnitAbility(Data_Monster m_data) => new Data_UnitAbility(m_data);
        public static explicit operator Data_UnitAbility(Data_Character m_data) => new Data_UnitAbility(m_data);

        /// <summary>
        /// [생성자] 몬스터 데이터를 기본형으로 변환시킵니다.
        /// </summary>
        public Data_UnitAbility(Data_Monster m_data)
        {
            attackRange = m_data.attack_range;
            hp = (ExactInt)m_data.mon_max_hp;
            maxHp = hp;
            damage = (ExactInt)m_data.mon_attack;
            moveSpeed = m_data.speed;
            defense = m_data.defense;
            critical_chance = 0;
            critical_strike_rate = 1;
            property = m_data.proprty;
            Id = m_data.monster_id;
        }

        /// <summary>
        /// [생성자] 캐릭터 데이터를 기본형으로 변환시킵니다.
        /// </summary>
        public Data_UnitAbility(Data_Character m_data)
        {
            attackRange = m_data.attack_range;
            hp = (ExactInt)m_data.hp;
            maxHp = hp;
            damage = (ExactInt)m_data.damage;
            moveSpeed = m_data.speed;
            defense = m_data.defens;
            critical_chance = m_data.critical_chance;
            critical_strike_rate = m_data.critical_strike_rate;
            property = eUnitProperty.None;

            Id = m_data.character_id;
        }

    }
}