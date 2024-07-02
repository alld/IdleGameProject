using DG.Tweening;
using IdleGame.Core.Panel.DataTable;
using IdleGame.Core.Procedure;
using IdleGame.Data.Base;
using System.Collections;
using UnityEngine;

namespace IdleGame.Core.Unit
{
    /// <summary>
    /// [기능] 유닛의 가장 기본적인 구성들을 담고 있습니다. 
    /// <br> 유니티내에서 관리되는 참조리스트에서 유닛들의 할당을 최소화하기 위해서 유니티 콜백함수를 사용하지않습니다. </br>
    /// </summary>
    public class Base_Unit : MonoBehaviour
    {
        /// <summary>
        /// [캐시] 유닛이 공통적으로 사용되어지는 여러 구성요소들을 포함하고 있습니다.
        /// </summary>
        [SerializeField]
        protected Data_UnitComponent _componenet;

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

        protected float temp_speed = 1f;

        /// <summary>
        /// [초기화] 유닛을 특정 설정합니다. 
        /// </summary>
        public void Logic_Init(Data_UnitType m_type)
        {
            Logic_SetModule(m_type.type, m_type.index);

            Logic_Init();
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

        /// <summary>
        /// TODO :: 추후 오브젝트 풀이 적용 되었을때 해당 함수 내용을 반영해야함.
        /// </summary>
        public virtual void Pool_Return()
        {
            Debug.Log("내 유닛 : " + this.gameObject);
            Logic_RemoveModule();

            //Base_Engine.Pool.ReturnObject(this.gameObject);
            Base_ObjectPoolManager.Instance.ReleaseObjectParent(this.gameObject);
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

            // TODO :: 여러 데이터 기반으로 판단을함.
        }

        /// <summary>
        /// [기능] 유닛이 대기 상태일때의 행동을 정의합니다.
        /// </summary>
        public virtual void Logic_Act_Stay()
        {
            Coroutine prevAction = null;
            if (_stateAction != null)
                prevAction = _stateAction;

            _stateAction = StartCoroutine(Logic_Action_Idle());

            StopCoroutine(prevAction);
        }

        /// <summary>
        /// [기능] 유닛이 이동하다가 공격상태로 전환하는 행동을 정의합니다.
        /// </summary>
        public virtual void Logic_Act_AttackMove()
        {
            if (_target == null) Logic_SearchTarget_Base();

            Logic_ChangeState(eUnitState.Move, eUnitState.Attack);
        }

        /// <summary>
        /// [기능] 유닛이 죽었을떄의 행동을 정의합니다.
        /// </summary>
        public virtual void Logic_Act_Die()
        {
            Logic_Action_Die();
        }

        /// <summary>
        /// [기능] 공격받는 행위가 들어오면 피격에대한 동작을 취합니다.
        /// <br> TODO :: 매개변수로 피해량을 넘겨받아서 처리합니다. </br>
        /// </summary>
        public virtual void Logic_Act_Damaged()
        {
            // TODO 데미지 엔진에서 계산을 한번 때림 결과값을 보고 체력이 남으면 피격 연출, 있으면 다음 행동은 사망처리
            if (0 > 0)
                Logic_ChangeState(eUnitState.None, eUnitState.Die);

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
            yield return new WaitForSeconds(2f);

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
            Logic_ChangeState(eUnitState.Idle, m_delayTime == 0 ? eUnitState.None : _state.cur);

            yield return new WaitForSeconds(m_delayTime);

            StartCoroutine(Logic_OperatorAct());
        }

        protected virtual void Logic_Action_Attacked()
        {
            Sound_Damaged();
            // TODO 피격 연출 

            if (_state.next == eUnitState.Die)
                StartCoroutine(Logic_OperatorAct());
        }

        protected IEnumerator Logic_Action_Attack()
        {
            transform.DOKill();
            Logic_ChangeState(eUnitState.Attack);

            while (true)
            {
                // TODO 피해량을 한번 계산해서 매개변수로 넘깁니다.
                _target.Logic_Act_Damaged();

                Sound_Hit();
                yield return _dd.attackDelay;
            }

        }

        protected virtual void Logic_Action_Die()
        {
            transform.DOKill();
            Logic_ChangeState(eUnitState.Die);

            // TODO :: 보상 정보를 보상 매니저? 재화 매니저? 아무튼 그쪽으로 전달

            _onBroadcastDie?.Invoke();
            Sound_Die();

            // TODO :: 사망처리를 별도로... 하던가... 딱히 필요 없어보임..

            Pool_Return();
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

            float moveTime = Vector3.Distance(transform.position, _dd.target_movePoint) * temp_speed;

            transform.DOMove(_dd.target_movePoint, moveTime)
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
        protected virtual void Logic_SearchTarget_Base()
        {
            // TODO :: 상속된 곳에서 알맞게 대상을 써치해야함. (적은 플레이어를, 플레이어와 동료는 (기획미정) 가깝던,, 우선도가 있던.. 적을 타겟팅함)
            _target._onBroadcastDie += Logic_TargetClear_Base;
        }

        /// <summary>
        /// [초기화] 타겟에 대한 정보들을 초기화시킵니다. 
        /// </summary>
        protected virtual void Logic_TargetClear_Base()
        {
            _target._onBroadcastDie -= Logic_TargetClear_Base; 
            _target = null;

            Logic_StopAction();
            StartCoroutine(Logic_OperatorAct());
        }

        /// <summary>
        /// [기능] 진행중인 행동을 중지시킵니다.
        /// </summary>
        private void Logic_StopAction()
        {
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