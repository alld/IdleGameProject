using UnityEngine;

namespace IdleGame.Core.Unit
{
    public class Character : Base_Unit
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

        public void Start()
        {
            Logic_Init();       // 유닛 초기화
        }

        protected override void Logic_Init_Custom()
        {
            base.Logic_Init_Custom();
        }

        public override void Logic_Act_Die()
        {
            base.Logic_Act_Die();
        }

        public override void Logic_Act_Stay()
        {
            base.Logic_Act_Stay();
        }

        public override void Logic_Act_AttackMove()
        {
            base.Logic_Act_AttackMove();
        }

        public override void Pool_Return()
        {
            base.Pool_Return();
        }
    }
}