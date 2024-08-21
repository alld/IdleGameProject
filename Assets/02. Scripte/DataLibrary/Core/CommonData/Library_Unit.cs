using IdleGame.Data;
using IdleGame.Data.Numeric;
using UnityEngine;

namespace IdleGame.Core.Unit
{
    /// <summary>
    /// [캐시] 유닛이 공통적으로 사용되는 기본 구성요소입니다.
    /// </summary>
    [System.Serializable]
    public struct Data_UnitComponent
    {

    }

    /// <summary>
    /// [종류] 유닛의 속성을 정의합니다.
    /// </summary>
    public enum eUnitProperty
    {
        None,
        a1,
        a2,
        a3,
        a4,
        a5,
        a6,
        a7
    }

    /// <summary>
    /// [데이터] 저장되는 형태의 강화 데이터입니다.
    /// </summary>
    public class Data_AbilitySlot
    {
        /// <summary>
        /// [데이터] 현재 레벨을 나타냅니다.
        /// </summary>
        public int level;

        /// <summary>
        /// [데이터] 현재 적용된 추가 능력치입니다.
        /// </summary>
        public ExactInt value;

        /// <summary>
        /// [데이터] 현재 적용된 1회 비용입니다.
        /// </summary>
        public ExactInt price;

        /// <summary>
        /// [데이터] 능력 타입입니다.
        /// </summary>
        public eAbilityType type;

        /// <summary>
        /// [초기화] 데이터를 초기화시킵니다.
        /// </summary>
        public Data_AbilitySlot Init(eAbilityType m_type)
        {
            type = m_type;
            level = 1;
            price = new ExactInt(0);
            value = new ExactInt(0);

            return this;
        }

        /// <summary>
        /// [기능] 정해진 레벨만큼 레벨업을 시킵니다. 
        /// </summary>
        public void LevelUp(int m_level = 1)
        {
            level += m_level;
            Global_Data.Player.Update_UnitAb(type);
        }
    }

    /// <summary>
    /// [종류] 능력치 타입입니다. 
    /// </summary>
    public enum eAbilityType
    {
        None,
        /// <summary> 체력 </summary>
        Hp,
        /// <summary> 피해량 </summary>
        Damage,
        /// <summary> 체력 회복 </summary>
        HpRegen,
        /// <summary> 치명타 배수</summary>
        CriticalMultiplier,
        /// <summary> 치명타 확률</summary>
        CriticalChance,
    }

    /// <summary>
    /// [종류] 유닛의 형태를 정의합니다.
    /// </summary>
    public enum eUnitTpye
    {
        None = 0,
        /// <summary> [종류] 플레이어 유닛 </summary>
        Player,
        /// <summary> [종류] 적 </summary>
        Enemy,
        /// <summary> [종료] 동료 </summary>
        Party
    }

    /// <summary>
    /// [종류] 유닛의 현재 상태를 정의합니다.
    /// </summary>
    public enum eUnitState
    {
        None = 0,
        /// <summary> 기본 </summary>
        Idle,
        /// <summary> 죽음 </summary>
        Die,
        /// <summary> 공격 </summary>
        Attack,
        /// <summary> 이동 </summary>
        Move,
        /// <summary> 등장 </summary>
        Appear,

        /// <summary>
        /// [기능] 상태값을 초기화할때 사용됩니다.
        /// </summary>
        Clear,
    }

    /// <summary>
    /// [상태] 유닛의 현재 상태값을 나타냅니다.
    /// </summary>
    public struct Data_UnitState
    {

        /// <summary>
        /// [상태] 유닛의 현재 상태를 나타냅니다.
        /// </summary>
        public eUnitState cur;

        /// <summary>
        /// [상태] 이전 진행되었던 유닛의 상태입니다.
        /// </summary>
        public eUnitState prev;

        /// <summary>
        /// [상태] 다음 진행될 유닛의 상태입니다.
        /// </summary>
        public eUnitState next;

        /// <summary>
        /// [초기화] 모든 데이터를 지웁니다.
        /// </summary>
        public void Clear()
        {
            cur = eUnitState.None;
            prev = eUnitState.None;
            next = eUnitState.None;
        }
    }

    /// <summary>
    /// [데이터] 유닛의 특정 타입을 지정할때 사용합니다. 
    /// </summary>
    public struct Data_UnitType
    {
        /// <summary>
        /// [데이터] 라이브러리를 구분하는 타입입니다.
        /// </summary>
        public eUnitTpye type;

        /// <summary>
        /// [데이터] 타입을 기점으로 라이브러리에 해당하는 고유 인덱스번호를 나타냅니다.
        /// </summary>
        public int index;


        /// <summary>
        /// [생성자] 타입과 인덱스를 기반으로 데이터를 할당 받습니다. 
        /// <br> TODO :: 추후 라이브러리에서 관리되는 데이터를 할당받을 수 있어서 생성자로 관리됨</br>
        /// </summary>
        public Data_UnitType(eUnitTpye m_type, int m_index)
        {
            type = m_type;
            index = m_index;
        }

    }

    /// <summary>
    /// [데이터] 유닛이 행동을 하는데 있어서 필요한 구성 데이터입니다. 
    /// </summary>
    public struct Data_UnitDynamicData
    {
        /// <summary>
        /// [데이터] 움직여야할 포인트입니다. 
        /// <br> 최종적으로 해당 위치로 이동하지만 움직여야할 방향의 역할에 더 가깝습니다. </br>
        /// </summary>
        public Vector3 target_movePoint;

        /// <summary>
        /// [데이터] 처음 시작하는 위치입니다.
        /// </summary>
        public Vector3 startPosition;

        /// <summary>
        /// [데이터] 특정 상황을 재 판단하는데 걸리는 시간입니다. 
        /// </summary>
        public WaitForSeconds sycleTime;

        /// <summary>
        /// [데이터] 다음 공격까지 걸리는 시간입니다. 
        /// </summary>
        public WaitForSeconds attackDelay;

        /// <summary>
        /// [데이터] 플레이어 유닛인지를 판단합니다. 동료 유닛도 포함됩니다.
        /// </summary>
        public bool isPlayerUnit;

        /// <summary>
        /// [초기화] 모든 데이터를 지웁니다.
        /// </summary>
        public void Clear()
        {
            target_movePoint = Vector3.zero;
            startPosition = Vector3.zero;
            sycleTime = null;
            attackDelay = null;
            isPlayerUnit = false;
        }
    }
}