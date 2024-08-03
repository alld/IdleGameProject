using IdleGame.Core.Unit;
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

        #region 생명주기
        protected override void Logic_SetModule(eUnitTpye m_type, int m_index)
        {
            base.Logic_SetModule(m_type, m_index);

            Panel_StageManager.Unit_Monsters.Add(this);
        }

        protected override void Logic_RemoveModule()
        {
            base.Logic_RemoveModule();

            Panel_StageManager.Unit_Monsters.Remove(this);
        }

        #endregion

        #region 보조 기능

        protected override void Logic_SearchTarget_Base()
        {
            _target = Panel_StageManager.Unit_Player;

            _dd.target_movePoint = new Vector3(_target.transform.position.x + ability.attackRange, _target.transform.position.y);

            base.Logic_SearchTarget_Base();
        }
        #endregion
    }
}