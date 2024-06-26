using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Log;
using System;
using UnityEngine;

namespace IdleGame.Core.Unit
{
    public abstract class Character : Base_Unit
    {
        // 1. 능력치 계산시 Global_Data의 정보가 공유되고 합산된 별도의 데이터 기반을 활용한다.
        // 2. Global_Data의 성장 수치는 공용 데이터, 해당 데이터의 캐릭터의 기본수치에 반영.
        // 3. 공격 타입의 다양성을 위해 다양한 공격 패턴을 포함

        // 기능
        // 1. Base_Unit을 상속하여 재정의 형태로 구현       (완)
        // 2. Active로 사용되는 Player 스킬이 아닌, 멀티 샷 같이 능력치 상승으로 인해 재현될 수 있는
        //      추가 공격 타입을 별도로 구현
        // 3. 몬스터와 다르게 승리 후 이동 로직이 정방향으로 이동 되게 구현.
        // 3-1. 게임이 끝나지 않은 상황이라면 맵만 이동되도록 설정.
        // 4. 카메라 시점은 플레이어의 유닛에 초점을 맞춘다.
        // 5. 플레이어 유닛이 적용되게 되면 Global_Data의 설정값도 적절히 변경되어야 한다.
        //      ※ 외부 작동 시에도, 설정기능은 Player에 대응 기능이 포함되면 좋다.

        // 캐릭터에 필요한 기능들
        // 1. 이동
        // 2. 공격 (스킬 / 자동)
        // 3. 방어
        // 4. 특성 (포인트)
        // 5. 동료 (비행기게임의 꼬마비행기 역할)
        // 6. 고유 스킬 (각 캐릭터별 전용)
        // 7. 영웅 (플레이어블 캐릭터를 일컫나?)
        //      ATK | HP | HP Regen | Critical % | Critical Damage |
        //      스킬 획득 % | 레벨 | 공속 | 노멀몹 피해량 | 보스몹 피해량
        // 8. 몬스터
        //      타입 : 보스 / 일반
        // 9. 재화
        //      골드 | 특성 포인트 | 스킬 포인트


        public int Level { get; protected set; }                // 레벨
        public string CharacterName { get; protected set; }     // 캐릭터 명
        public float CurrentHealth { get; protected set; }               // 체력
        public float HealthRegen { get; protected set; }        // 체력 재생
        public float MaxHealth { get; protected set; }            // 최대 체력
        public float AttackDamage { get; protected set; }         // 공격력
        public float AttackSpeed { get; protected set; }        // 공속
        public float CriticalChance { get; protected set; }     // 치명타 확률
        public float CriticalDamage { get; protected set; }     // 치명타 피해량
        
        protected Data_UnitType unitType;
        protected eUnitTpye eUnitState;

        protected Character(eUnitTpye type, int index)
        {
            unitType = new Data_UnitType(type, index);
        }

        public static Character CreateCharacter(eUnitTpye type, int index)
        {
            switch (type)
            {
                case eUnitTpye.Player:
                    return new Player(index);
                case eUnitTpye.Enemy:
                    return new Enemy(index);
                case eUnitTpye.Party:
                    return new Party(index);
                default:
                    Base_Engine.Log.Logic_PutLog(new Data_Log("타입이 없습니다 : " + type));
                    throw new ArgumentException("타입이 없습니다 : " + type);
            }
        }


        public override void Logic_Act_Stun()
        {
            base.Logic_Act_Stun();
        }

        public override void Logic_Act_Damaged()
        {
            base.Logic_Act_Damaged();
        }

        protected override void Logic_Init_Custom()
        {
            base.Logic_Init_Custom();

            Level = 1;
            CurrentHealth = MaxHealth;
            HealthRegen = 0.1f;
            AttackDamage = 1;
            AttackSpeed = 1f;
            CriticalChance = 0.1f;
            CriticalDamage = 1.01f;
        }

        /// <summary>
        /// 데미지를 받을 때 호출
        /// </summary>
        /// <param name="damage"></param>
        public virtual void TakeDamage(int damage)
        {
            int combinedDamage = damage;
            CurrentHealth -= combinedDamage;

            // 현재 체력 모두 소진시
            if(CurrentHealth <= 0)
            {
                Logic_Act_Die();
            }
            // 체력이 남아있을 경우
            else
            {
                Logic_Act_Damaged();
            }

        }

        /// <summary>
        /// 레벨업 기능
        /// (플레이어만 레벨업이 가능하다면 삭제 후 Player스크립트에 작성.)
        /// </summary>
        protected virtual void Logic_LevelUp()
        {
            Level++;
        }

        /// <summary>
        /// 체력 재생 기능
        /// </summary>
        /// <param name="regen"></param>
        protected virtual void Logic_Heal(float regen)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + regen, MaxHealth);
        }

        /// <summary>
        /// 캐릭터 사망 기능
        /// </summary>
        public override void Logic_Act_Die()
        {
            base.Logic_Act_Die();
        }

        /// <summary>
        /// 캐릭터 대기 기능
        /// </summary>
        public override void Logic_Act_Stay()
        {
            base.Logic_Act_Stay();
        }

        public override void Logic_Act_AttackMove()
        {
            base.Logic_Act_AttackMove();
        }


        // eUnitType 오타있다고 나중에 알려주기.
        public void Chracter(eUnitTpye type)
        {
            switch(type)
            {
                case eUnitTpye.Player:
                    break;
                case eUnitTpye.Enemy:
                    break;
                case eUnitTpye.Party:
                    break;
                // 케이스 보스 추가?
            }
        }
    }
}
