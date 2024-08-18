using IdleGame.Data.Numeric;
using UnityEngine;

namespace IdleGame.Core.Unit
{
    /// <summary>
    /// [기능] 데미지를 계산해서 그 결과값을 반환하는 로직입니다.
    /// </summary>
    public static class Global_DamageEngine
    {
        /// <summary>
        /// [데이터] 마지막 데이터 정보가 치명타가 적용되었는지 확인합니다.
        /// </summary>
        public static bool LastData_IsCritical;
        /// <summary>
        /// [데이터] 마지막 데이터가 0이하의 데미지인지를 확인합니다. 
        /// </summary>
        public static bool LastData_IsZeroDamage;

        /// <summary>
        /// [기능] 데미지를 계산하여 최종 데미지량을 반영합니다.
        /// </summary>
        public static ExactInt Logic_Calculator(Data_UnitAbility m_attacker, Data_UnitAbility m_target, ExactInt m_damage)
        {
            ExactInt result = new ExactInt(0);
            result += m_damage;
            result -= m_target.defense;

            LastData_IsZeroDamage = result < 0;
            if (LastData_IsZeroDamage)
            {
                LastData_IsCritical = false;
                return new ExactInt(0);
            }

            //LastData_IsCritical = Logic_TryCriticalCalculator(m_attacker.critical_chance);
            //if (LastData_IsCritical)
            //    result.SetPercent(m_attacker.critical_strike_rate);
            //result.SetPercent(Library_DataTable.property[(m_attacker.property, m_target.property)]);

            return result;
        }

        /// <summary>
        /// [기능] 치명타 확률을 계산합니다. 
        /// </summary>
        public static bool Logic_TryCriticalCalculator(float m_chance)
        {
            return Random.value < m_chance;
        }
    }
}