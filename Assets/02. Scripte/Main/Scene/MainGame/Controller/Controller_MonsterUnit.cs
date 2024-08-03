using IdleGame.Core.Unit;
using IdleGame.Data.DataTable;

namespace IdleGame.Main.Unit
{
    /// <summary>
    /// [기능] 몬스터 유닛에대한 행동을 정의하고 있습니다.
    /// </summary>
    public class Controller_MonsterUnit : Controller_EnemyUnit
    {
        protected override void Logic_SetModule(eUnitTpye m_type, int m_index)
        {
            base.Logic_SetModule(m_type, m_index);

            ability = (Data_UnitAbility)Library_DataTable.monster[m_index];
        }
    }
}