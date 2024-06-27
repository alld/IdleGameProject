using IdleGame.Core.Unit;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : Base_Unit
{
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

    private Player _player;

    protected override void Logic_Init_Custom()
    {
        base.Logic_Init_Custom();

        Debug.Log("1번만 호출하기로 약속☆");

        _atk = 10;
        _atkSpeed = 1f;
        _atkDistance = 2f;
        _maxHealth = 50;
        _health = _maxHealth;
        _def = 1f;
        _moveSpeed = 5f;

        _state = new Data_UnitState();
        _state.cur = eUnitState.Idle;

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _target = GameObject.FindWithTag("Player").GetComponent<Base_Unit>();
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

    public override void Logic_Act_Damaged()
    {
        base.Logic_Act_Damaged();

        _health -= _player.Atk - _def;
        Debug.Log("적이 피해를 입었습니다.");

        if (_health <= 0)
        {
            Debug.Log("적 : 깨꼬닥");
            Logic_Act_Die();
        }
    }

    /// <summary>
    /// 공격속도 계산 함수
    /// </summary>
    public void AttackSpeed()
    {
        _timer -= Time.deltaTime;    

        if(_timer <= 0)
        {
            Attack();

            _timer = _atkSpeed;
        }
    }

    /// <summary>
    /// 공격 실행
    /// </summary>
    public void Attack()
    {
        Debug.Log("적이 공격을 실행합니다.");
        //StartCoroutine(Logic_Action_Attack());      // 나중에는 여기서 수치 계산 

        Logic_SetAction(eUnitState.Attack);
    }


    public void Logic()
    {
        // 범위안에 들어오면 공격속도 로직 실행
        if (Mathf.Abs(transform.position.x + _atkDistance) >= Mathf.Abs(_target.gameObject.transform.position.x))
        {
            AttackSpeed();
        }
    }

    private void Update()
    {
        Logic();
    }
}
