using IdleGame.Data.DataTable;
using IdleGame.Data.Numeric;
using System;
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

        private static System.Random random = new System.Random();


        /// <summary>
        /// [기능] 데미지를 계산하여 최종 데미지량을 반영합니다.
        /// </summary>
        public static ExactInt Logic_Calculator(Data_UnitAbility m_attacker, Data_UnitAbility m_target, ExactInt m_damage)
        {
            // 역할 :: 기본 데미지 계산
            ExactInt result = new ExactInt(0);
            ExactInt cri = m_damage;
            ExactInt basic = m_damage - m_target.defense;
            ExactInt pro = m_damage;
            result += basic;


            // 역할 :: 치명타 계산
            LastData_IsCritical = Logic_TryCriticalCalculator(m_attacker.critical_chance);
            if (LastData_IsCritical)
                cri.SetPercent(m_attacker.critical_strike_rate);
            result += cri;

            // 역할 :: 추가 계산 방지
            LastData_IsZeroDamage = result < 0;
            if (LastData_IsZeroDamage)
            {
                LastData_IsCritical = false;
                return new ExactInt(0);
            }

            // 역할 :: 속성 계산
            pro.SetPercent(Library_DataTable.property[(m_attacker.property, m_target.property)]);
            result += pro;

            return result;
        }

        /// <summary>
        /// [기능] 치명타 확률을 계산합니다. 
        /// </summary>
        public static bool Logic_TryCriticalCalculator(float m_chance)
        {
            return (float)random.NextDouble() < m_chance;
        }
    }
}