using IdleGame.Core.Unit;
using IdleGame.Data.Numeric;
using IdleGame.Main.GameLogic;
using UnityEngine;

namespace IdleGame.Main.Unit
{
    /// <summary>
    /// [기능] 적들은 진행되는 방향이 다르며, 공격대상이 단일 타겟으로 이루어져 있습니다.
    /// <br> 그 차이를 놓고보면 AllyUnit과 큰차이가 존재하지않습니다. </br>
    /// </summary>
    public abstract class Controller_EnemyUnit : Base_Unit
    {

        public override void Logic_Act_Damaged(Base_Unit m_attacker, ExactInt m_damage)
        {
            base.Logic_Act_Damaged(m_attacker, m_damage);
            Debug.Log(ability.hp.ToString() + m_attacker.name);
        }
        #region 생명주기
        protected override void Logic_SetModule(eUnitTpye m_type, int m_index)
        {
            base.Logic_SetModule(m_type, m_index);
            _dd.attackDelay = new WaitForSeconds(1f);

            Panel_StageManager.Unit_Monsters.Add(this);
        }

        protected override void Logic_RemoveModule()
        {
            base.Logic_RemoveModule();

            Panel_StageManager.Unit_Monsters.Remove(this);
        }

        #endregion

        #region 보조 기능

        protected override bool Logic_SearchTarget_Base()
        {
            if (Panel_StageManager.Unit_Player.isDie)
            {
                Logic_SetAction(eUnitState.Idle);
                return false;
            }

            _target = Panel_StageManager.Unit_Player;

            _dd.target_movePoint = new Vector3(_target.transform.position.x + ability.attackRange, _target.transform.position.y);

            return base.Logic_SearchTarget_Base();
        }
        #endregion
    }
}