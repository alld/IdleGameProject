using System.Collections;
using UnityEngine;


namespace IdleGame.Core.Unit
{
    public class Player : Base_Unit
    {
        // 1. 플레이어와 동료는 적 타겟팅 기능이 있어야한다.
        // 2. 몬스터는 플레이어만 타겟팅 할 수 있어야한다.

        [SerializeField] private float _atk;       // 공격력
        [SerializeField] private float _atkSpeed;  // 공격 속도
        [SerializeField] private float _atkDistance; // 공격 사거리
        [SerializeField] private float _health;    // 현재 체력
        [SerializeField] private float _maxHealth; // 최대 체력
        [SerializeField] private float _def;       // 방어력
        [SerializeField] private float _moveSpeed; // 이동 속도

        [SerializeField] private float _timer;

        public float Atk { get => _atk; set => _atk = value; }
        public float AtkSpeed { get => _atkSpeed; set => _atkSpeed = value; }
        public float AtkDistance { get => _atkDistance; set => _atkDistance = value; }
        public float Health { get => _health; set => _health = value; }
        public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
        public float Def { get => _def; set => _def = value; }
        public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }


        private Enemy _enemy;
        protected override void Logic_Init_Custom()
        {
            base.Logic_Init_Custom();

            Debug.Log("Player에서 1번만 호출하기로 약속☆");

            _atk = 10;
            _atkSpeed = 1f;
            _atkDistance = 15f;
            _maxHealth = 100;
            _health = _maxHealth;
            _def = 5f;
            _moveSpeed = 5f;

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

        public override void Logic_Act_Damaged()
        {
            base.Logic_Act_Damaged();

            _health -= _enemy.Atk - _def;
            Debug.Log("플레이어가 피해를 입었습니다. 현재 체력 : " + _health);

            if (_health <= 0)
            {
                Debug.Log("플레이어 : 깨꼬닥");
                Logic_Action_Die();
            }
        }

        public override void Logic_Act_Die()
        {
            base.Logic_Act_Die();

            _state.cur = eUnitState.Die;
            Logic_SetAction(_state.cur);
            
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
                Attack();

                _timer = _atkSpeed;
            }
        }


        public void Logic()
        {
            //
            float atkdistance = Vector3.Distance(transform.position, _target.gameObject.transform.position);
            //Debug.Log("거리 : " + atkdistance);

            // 범위안에 들어오면 공격속도 로직 실행
            if (atkdistance <= _atkDistance)
            {
                Logic_SearchTarget_Base();
                AttackSpeed();
            }
            else
            {
                Move();
            }
        }

        /// <summary>
        /// 공격 실행
        /// </summary>
        public void Attack()
        {
            Debug.Log("플레이어가 공격을 실행합니다.");
            //StartCoroutine(Logic_Action_Attack());      // 나중에는 여기서 작업을 할 것.

            _state.cur = eUnitState.Attack;
            //Logic_SetAction(_state.cur);
            Logic_Act_AttackMove();
            _enemy.Logic_Act_Damaged();
        }

        public void Move()
        {
            Debug.Log("플레이어가 이동합니다.");
            _state.cur = eUnitState.Move;
            Logic_Action_Move();
        }

        private void Update()
        {
            Logic();
        }



    }
}
