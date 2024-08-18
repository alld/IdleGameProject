using IdleGame.Core.Unit;
using IdleGame.Data.DataTable;
using IdleGame.Data.NSave;
using IdleGame.Data.Numeric;
using System.Collections.Generic;
using UnityEngine;

namespace IdleGame.Data
{
    /// <summary>
    /// [데이터] 플레이어에 관련된 동적 데이터 정보들을 담고 있습니다. 
    /// </summary>
    [System.Serializable]
    public class Data_Player : Interface_SaveData
    {
        /// <summary>
        /// [데이터] 기본적으로 적용되는 디폴트 이름입니다.
        /// </summary>
        private const string DefaultName = "기본 이름입니다.";


        /// <summary>
        /// [데이터] 플레이어의 이름입니다.
        /// </summary>
        public string nick = DefaultName;

        /// <summary>
        /// [데이터] 골드 재화 보유량입니다.
        /// </summary>
        public ExactInt cc_Gold = new ExactInt(0);

        /// <summary>
        /// [데이터] 특성 포인트 보유량입니다. 
        /// </summary>
        public ExactInt cc_Ability = new ExactInt(0);

        /// <summary>
        /// [데이터] 현재 사용중인 캐릭터의 인덱스를 반환합니다. 
        /// </summary>
        public int characterID = 1000;

        /// <summary>
        /// [데이터] 플레이어 유닛의 능력치 정보입니다. 
        /// </summary>
        public Data_UnitAbility unit_Ability = new Data_UnitAbility(new Data_Character());

        /// <summary>
        /// [데이터] 현재 업그레이드가 진행된 상태를 나타냅니다. 
        /// </summary>
        [SerializeField]
        public Dictionary<eAbilityType, Data_AbilitySlot> slot_Ability = new Dictionary<eAbilityType, Data_AbilitySlot>();

        /// <summary>
        /// [기능] 플레이어의 데이터가 변경된 경우 호출합니다. 실제 데이터를 반영합니다.
        /// </summary>
        public void Update_UnitAb(eAbilityType m_type)
        {
            switch (m_type)
            {
                case eAbilityType.None:
                    break;
                case eAbilityType.Hp:
                    unit_Ability.hp = Library_DataTable.character[characterID].hp + slot_Ability[m_type].value;
                    break;
                case eAbilityType.Damage:
                    unit_Ability.damage = Library_DataTable.character[characterID].damage + slot_Ability[m_type].value;
                    break;
                case eAbilityType.HpRegen:
                    break;
                case eAbilityType.CriticalMultiplier:
                    break;
                case eAbilityType.CriticalChance:
                    break;
                default:
                    break;
            }
        }
    }
}