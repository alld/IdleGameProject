using DG.Tweening;
using IdleGame.Core.Pool;
using IdleGame.Core.Utility;
using IdleGame.Data.Base;
using IdleGame.Data.Numeric;
using System.Collections;
using UnityEngine;

namespace IdleGame.Core.Unit
{
    /// <summary>
    /// [기능] 유닛의 가장 기본적인 구성들을 담고 있습니다. 
    /// <br> 유니티내에서 관리되는 참조리스트에서 유닛들의 할당을 최소화하기 위해서 유니티 콜백함수를 사용하지않습니다. </br>
    /// </summary>
    public abstract class Base_Unit : Base_PoolObject
    {
        /// <summary>
        /// [캐시] 유닛이 공통적으로 사용되어지는 여러 구성요소들을 포함하고 있습니다.
        /// </summary>
        [SerializeField]
        protected Data_UnitComponent _componenet;

        /// <summary>
        /// [데이터] 유닛의 기본 능력치를 나타냅니다. 
        /// </summary>
        public Data_UnitAbility ability;

        /// <summary>
        /// [데이터] 유닛이 판단하는데 필요한 기본 정보들을 담습니다. 
        /// </summary>
        protected Data_UnitDynamicData _dd;

        protected Data_UnitState _state;

        /// <summary>
        /// [상태] 현재 유닛이 죽은 상태인지를 확인합니다.
        /// </summary>
        public bool isDie => _state.cur == eUnitState.Die;

        /// <summary>
        /// [캐시] 상태에 따른 행동 액션을 담고 있습니다.
        /// </summary>
        protected Coroutine _stateAction;
        protected Coroutine StateAction { get; set; }

        /// <summary>
        /// [캐시] 유닛의 목표 대상입니다. 
        /// </summary>
        protected Base_Unit _target;

        /// <summary>
        /// [캐시] 죽었을때 자신을 타겟으로 삼고 있는 유닛들에게 정보를 전달합니다. 
        /// </summary>
        protected Dele_Action _onBroadcastDie;

        /// <summary>
        /// [초기화] 유닛을 특정 설정합니다. 
        /// </summary>
        public void Logic_Init(Data_UnitType m_type)
        {
            Logic_Init();

            Logic_SetModule(m_type.type, m_type.index);
        }

        /// <summary>
        /// [초기화] 유닛을 초기화 시킵니다. 
        /// </summary>
        public void Logic_Init()
        {
            Logic_Clear_Base();

            Logic_Init_Custom();
        }

        /// <summary>
        /// [초기화] 유닛의 모든 내용을 지웁니다. 
        /// </summary>
        protected virtual void Logic_Clear_Base()
        {
            transform.DOKill();

            _dd.Clear();

            Logic_ChangeState(eUnitState.Clear);

            Logic_TargetClear_Base();
        }

        /// <summary>
        /// [초기화] 유닛을 초기화 시킵니다. 
        /// </summary>
        protected virtual void Logic_Init_Custom() { }

        #region 유닛 정보


        #endregion

        #region 생명 주기




        #region 모듈 설정
        /// <summary>
        /// [설정] 유닛의 구성을 재설정합니다.
        /// </summary>
        protected virtual void Logic_SetModule(eUnitTpye m_type, int m_index)
        {
            // TODO :: 구성 설정, 구성에대한 내용이 확정되어야함.. 그럴려면 유닛 기본 디자인 형태같은게 확정되어야함.
        }

        /// <summary>
        /// [설정] 유닛에 설정된 구성요소들을 제거합니다.
        /// </summary>
        protected virtual void Logic_RemoveModule()
        {
            Logic_Clear_Base();

            // TODO :: 구성 제거, 구성에대한 내용이 확정되어야함.. 그럴려면 유닛 기본 디자인 형태같은게 확정되어야함.
        }

        public override void Pool_Clear()
        {
            Logic_RemoveModule();
        }

        /// <summary>
        /// TODO :: 추후 오브젝트 풀이 적용 되었을때 해당 함수 내용을 반영해야함.
        /// </summary>
        public override void Pool_Return_Base()
        {
            base.Pool_Return_Base();
        }
        #endregion
        #endregion

        #region 동작 관련
        #region 행위 패턴

        /// <summary>
        /// [기능] 현재 다음 행위를 취할것이 없는 상태일 경우 해당 함수를 통해서 적절한 행위를 찾아 실행시킵니다.
        /// </summary>
        public IEnumerator Logic_OperatorAct()
        {
            Logic_StopAction();
            if (_state.prev == eUnitState.Move) Sound_StopMove();

            if (_state.next != eUnitState.None)
            {
                Logic_SetAction(_state.next);
                yield break;
            }

            Logic_JudgmentAction_Custom();
        }

        /// <summary>
        /// [기능] 다음 행동할 행동들을 판단하여 명령합니다. 
        /// </summary>
        public virtual void Logic_JudgmentAction_Custom()
        {
            // TODO :: 임시 정의. 상속하여 처리하도록 

            if (_target == null)

                Logic_Act_AttackMove();
        }

        /// <summary>
        /// [기능] 유닛이 등장할때 이루어지는 행동을 정의합니다.
        /// </summary>
        public virtual void Logic_Act_Appear()
        {
            Logic_SetAction(eUnitState.Appear);
        }

        /// <summary>
        /// [기능] 유닛이 대기 상태일때의 행동을 정의합니다.
        /// </summary>
        public virtual void Logic_Act_Stay()
        {
            Logic_SetAction(eUnitState.Idle);
        }

        /// <summary>
        /// [기능] 유닛이 이동하다가 공격상태로 전환하는 행동을 정의합니다.
        /// </summary>
        public virtual void Logic_Act_AttackMove()
        {
            if (_target == null)
                if (!Logic_SearchTarget_Base())
                {
                    Logic_SetAction(eUnitState.Idle);
                    return;
                }

            Logic_ChangeState(eUnitState.Move, eUnitState.Attack);

            Logic_Action_Move();
        }

        /// <summary>
        /// [기능] 유닛이 죽었을떄의 행동을 정의합니다.
        /// </summary>
        public virtual void Logic_Act_Die()
        {
            Logic_SetAction(eUnitState.Die);
        }

        /// <summary>
        /// [기능] 공격받는 행위가 들어오면 피격에대한 동작을 취합니다.
        /// </summary>
        public virtual void Logic_Act_Damaged(Base_Unit m_attacker, ExactInt m_damage)
        {
            ability.hp -= m_damage;

            if (ability.hp <= 0)
            {
                Logic_ChangeState(eUnitState.None, eUnitState.Die);
            }

            Logic_Action_Attacked();
        }

        public virtual void Logic_Act_Stun()
        {
            // TODO 기획에 있다면.. 이건 그냥 아이들 만든다음에, 원래 행동을 다시 실행시키면될듯하네요.
        }

        public virtual void Loigc_Act_Knockback()
        {
            // TODO 얘도 기획에 있다면.. 그냥 우리게임상 방향전환없이 뒤로 밀기만 하면됩니다. 밀고나서, next를 이동으로 넣어두면됩니다.
        }

        #endregion

        #region 행동 정의

        /// <summary>
        /// [기능] 입력된 상태값에 맞쳐서 특정 행위를 실행하도록 합니다. 
        /// </summary>
        protected void Logic_SetAction(eUnitState m_state)
        {
            if (_state.cur == m_state) return;
            Logic_StopAction();

            switch (m_state)
            {
                case eUnitState.Idle:
                    _stateAction = StartCoroutine(Logic_Action_Idle());
                    break;
                case eUnitState.Die:
                    Logic_Action_Die();
                    break;
                case eUnitState.Attack:
                    _stateAction = StartCoroutine(Logic_Action_Attack());
                    break;
                case eUnitState.Move:
                    Logic_Action_Move();
                    break;
                case eUnitState.Appear:
                    _stateAction = StartCoroutine(Logic_Action_Appear());
                    break;
            }
        }


        protected virtual IEnumerator Logic_Action_Appear()
        {
            transform.DOKill();
            Logic_ChangeState(eUnitState.Appear);

            Sound_Appear();
            yield return Utility_Common.WaitForSeconds(2f);

            Logic_ChangeState(eUnitState.None);
            StartCoroutine(Logic_OperatorAct());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m_delayTime"></param>
        /// <returns></returns>
        protected virtual IEnumerator Logic_Action_Idle(float m_delayTime = 0)
        {
            transform.DOKill();
            Logic_StopAction();
            Logic_ChangeState(eUnitState.Idle, m_delayTime == 0 ? eUnitState.None : _state.cur);

            if (m_delayTime == 0) yield break;

            yield return Utility_Common.WaitForSeconds(m_delayTime);

            StartCoroutine(Logic_OperatorAct());
        }

        protected virtual void Logic_Action_Attacked()
        {
            Sound_Damaged();
            // TODO 피격 연출 
            if (isDie || _state.next == eUnitState.Die)
            {
                StartCoroutine(Logic_OperatorAct());
                Debug.Log("사망 확인");
                return;
            }
        }

        protected IEnumerator Logic_Action_Attack()
        {
            transform.DOKill();
            Logic_ChangeState(eUnitState.Attack);

            while (true)
            {
                _target.Logic_Act_Damaged(this, Global_DamageEngine.Logic_Calculator(_target.ability, ability.damage));

                Sound_Hit();
                yield return _dd.attackDelay;
            }

        }

        protected virtual void Logic_Action_Die()
        {
            transform.DOKill();
            Logic_ChangeState(eUnitState.Die);

            _onBroadcastDie?.Invoke();
            Sound_Die();

            // TODO :: 사망처리를 별도로... 하던가... 딱히 필요 없어보임..

            Pool_Return_Base();
        }

        protected virtual void Logic_Action_Move(Vector3 m_target)
        {
            _dd.target_movePoint = m_target;
            Logic_Action_Move();
        }

        /// <summary>
        /// [기능] 유닛이 이동하는 행동을 정의합니다.
        /// </summary>
        protected virtual void Logic_Action_Move()
        {
            transform.DOKill();
            Logic_ChangeState(eUnitState.Move);

            float moveTime = Vector3.Distance(transform.position, _dd.target_movePoint) * ability.moveSpeed;

            transform.DOMove(_dd.target_movePoint, moveTime)
                .SetEase(Ease.Linear)
                .OnComplete(
                () =>
                {
                    StartCoroutine(Logic_OperatorAct());
                });

            Sound_Move();
        }

        #endregion

        #region 보조 기능

        /// <summary>
        /// [기능] 현재 타겟을 찾지 못한 경우 대상을 물색합니다. 
        /// </summary>
        protected virtual bool Logic_SearchTarget_Base()
        {
            _target._onBroadcastDie += Logic_ReTryTargetClear_Base;

            return _target != null;
        }

        /// <summary>
        /// [초기화] 타겟에 대한 정보들을 초기화시킵니다. 
        /// </summary>
        protected virtual void Logic_TargetClear_Base()
        {
            if (_target != null)
            {
                _target._onBroadcastDie -= Logic_ReTryTargetClear_Base;
                _target = null;
            }

            Logic_StopAction();
        }

        /// <summary>
        /// [초기화] 기존 타겟을 정리한 이후에, 새로운 행동을 찾습니다. 
        /// </summary>
        protected virtual void Logic_ReTryTargetClear_Base()
        {
            Logic_TargetClear_Base();

            if (_state.cur == eUnitState.Die)
                return;


            Logic_ChangeState(eUnitState.Clear);
            StartCoroutine(Logic_OperatorAct());
        }

        /// <summary>
        /// [기능] 진행중인 행동을 중지시킵니다.
        /// </summary>
        private void Logic_StopAction()
        {
            if (_stateAction == null)
                return;

            StopCoroutine(_stateAction);
            _stateAction = null;
        }

        /// <summary>
        /// [설정] 유닛의 현재 상태값을 변경합니다.
        /// </summary>
        protected void Logic_ChangeState(eUnitState m_state, eUnitState m_next = eUnitState.None)
        {
            if (m_state == eUnitState.Clear)
            {
                _state.Clear();
                return;
            }

            // 현재 상태가 다음상태와 동일하면 None상태로 변경
            if (m_state == m_next)
                _state.next = eUnitState.None;
            // 현재와 다음의 행동이 다르면 다음 행동을 설정
            else if (_state.next != m_next && m_next != eUnitState.None)
                _state.next = m_next;

            // 현재 상태를 지속적으로 받고있으면 종료
            if (_state.cur == m_state)
                return;

            _state.prev = _state.cur;   // 현재 상태를 이전상태로 만들고
            _state.cur = m_state;       // 다음 행동을 현재상태로 만든다.
        }

        #endregion


        #region 사운드 설정
        /// <summary>
        /// [사운드] 등장시 들리는 사운드 설정입니다.
        /// </summary>
        protected virtual void Sound_Appear() { }

        /// <summary>
        /// [사운드] 이동시 들리는 사운드입니다.
        /// </summary>
        protected virtual void Sound_Move() { }

        /// <summary>
        /// [사운드] 이동 사운드를 정지시킵니다.
        /// </summary>
        protected virtual void Sound_StopMove() { }

        /// <summary>
        /// [사운드] 공격시 들리는 사운드입니다.
        /// </summary>
        protected virtual void Sound_Hit() { }

        /// <summary>
        /// [사운드] 피격시 들리는 사운드입니다. 
        /// </summary>
        protected virtual void Sound_Damaged() { }

        /// <summary>
        /// [사운드] 사망시 들리는 사운드입니다. 
        /// </summary>
        protected virtual void Sound_Die() { }
        #endregion

        #endregion
    }
}