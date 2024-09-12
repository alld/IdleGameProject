using IdleGame.Core.Unit;
using IdleGame.Core.Utility;
using IdleGame.Data.Adrees;
using IdleGame.Data.DataTable;
using IdleGame.Main.GameLogic;
using UnityEngine.Rendering.UI;

namespace IdleGame.Main.Unit
{
    /// <summary>
    /// [기능] 아군 유닛을 지정합니다. 아군유닛에는 플레이어 유닛도 포함됩니다.
    /// <br> 추후 기획에 따라서 각종 버프, 업그레이드 효과등 공유 데이터들이 적용될 수 있습니다.</br>
    /// </summary>
    public abstract class Controller_AllyUnit : Base_Unit
    {
        protected override void Logic_SetModule(eUnitTpye m_type, int m_index)
        {
            base.Logic_SetModule(m_type, m_index);

            _dd.attackDelay = Utility_Common.WaitForSeconds(1f);
            _dd.isPlayerUnit = true;

            _ani.runtimeAnimatorController = Library_Animator.dic_ani[Library_DataTable.character[m_index].shape_id];
        }

        #region 보조 기능

        protected override bool Logic_SearchTarget_Base()
        {
            Controller_EnemyUnit compareUnit = null;
            _dd.target_movePoint = transform.position;

            if (Panel_StageManager.Unit_Monsters.Count == 0) return false;
            else if (Panel_StageManager.Unit_Monsters.Count == 1)
            {
                if (Panel_StageManager.Unit_Monsters[0].isDie)
                    return false;

                _target = Panel_StageManager.Unit_Monsters[0];
            }
            else
            {
                // 조건 :: 현재 기준, 가장 가까운 유닛을 탐색함.

                for (int i = 0; i < Panel_StageManager.Unit_Monsters.Count; i++)
                {
                    if (Panel_StageManager.Unit_Monsters[i].isDie)
                        continue;

                    if (_target == null)
                    {
                        compareUnit = Panel_StageManager.Unit_Monsters[i];
                        continue;
                    }

                    if (Panel_StageManager.Unit_Monsters[i].transform.position.x >= compareUnit.transform.position.x)
                        continue;

                    compareUnit = Panel_StageManager.Unit_Monsters[i];
                }

                _target = compareUnit;

            }

            if (_target != null && _target.transform.position.x > transform.position.x + ((ability.attackRange / 200)))
                _target = null;

            if (_target == null)
                return false;

            return base.Logic_SearchTarget_Base();
        }
        #endregion
    }
}