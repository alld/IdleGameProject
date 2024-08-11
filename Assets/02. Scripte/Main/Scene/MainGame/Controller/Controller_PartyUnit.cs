using IdleGame.Core.Unit;
using IdleGame.Data.DataTable;

namespace IdleGame.Main.Unit
{
    /// <summary>
    /// [기능] 동료 유닛에대한 설정이 담깁니다.
    /// </summary>
    public class Controller_PartyUnit : Controller_AllyUnit
    {
        protected override void Logic_SetModule(eUnitTpye m_type, int m_index)
        {
            base.Logic_SetModule(m_type, m_index);

            ability = (Data_UnitAbility)Library_DataTable.character[m_index];
        }
    }
}