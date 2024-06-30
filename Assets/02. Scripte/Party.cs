using IdleGame.Core.Unit;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace IdleGame.Core.Unit
{
    public class Party : Base_Unit
    {
        // 1. 플레이어와 동료는 적 타겟팅 기능이 있어야한다.
        // 2. 몬스터는 플레이어만 타겟팅 할 수 있어야한다.

        [SerializeField] private float _atk;       // 공격력
        [SerializeField] private float _atkSpeed;  // 공격 속도
        [SerializeField] private float _atkDistance; // 공격 사거리
        //[SerializeField] private float _moveSpeed; // 이동 속도

        [SerializeField] private float _timer;

        public float Atk { get => _atk; set => _atk = value; }
        public float AtkSpeed { get => _atkSpeed; set => _atkSpeed = value; }
        public float AtkDistance { get => _atkDistance; set => _atkDistance = value; }
        
        private Enemy _enemy;
        protected override void Logic_Init_Custom()
        {
            base.Logic_Init_Custom();

            Debug.Log("1번만 호출하기로 약속☆");

            _atk = 10;
            _atkSpeed = 1f;
            _atkDistance = 15f;

            _state = new Data_UnitState();
            _state.cur = eUnitState.Idle;
            Logic_SetAction(_state.cur);

            _enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();
            _target = GameObject.FindWithTag("Enemy").GetComponent<Base_Unit>();
        }

        private void Start()
        {
            Logic_Init_Custom();
        }

        public override void Logic_Act_Die()
        {
            base.Logic_Act_Die();

            gameObject.SetActive(false);
        }

        /// <summary>
        /// 공격속도 계산 함수
        /// </summary>
        public void AttackSpeed()
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                Debug.Log("공격!");
                Attack();

                _timer = _atkSpeed;
            }
        }

        public void Logic()
        {
            float distance = Vector3.Distance(transform.position, _target.gameObject.transform.position);
            //Debug.Log("거리 : " + distance);

            // 범위안에 들어오면 공격속도 로직 실행
            if (distance <= _atkDistance)
            {
                AttackSpeed();
            }
        }

        /// <summary>
        /// 공격 실행
        /// </summary>
        public void Attack()
        {
            Debug.Log("플레이어가 공격을 실행합니다.");
            //StartCoroutine(Logic_Action_Attack());      // 나중에는 여기서 작업을 할 것.

            //_enemy.Logic_Act_Damaged();
            _state.cur = eUnitState.Attack;
            Logic_SetAction(_state.cur);
        }

        private void Update()
        {
            Logic();
        }


    }
}
