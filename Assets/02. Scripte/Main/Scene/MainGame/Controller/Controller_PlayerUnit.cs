using IdleGame.Core.Unit;
using IdleGame.Main.GameLogic;

namespace IdleGame.Main.Unit
{
    /// <summary>
    /// [기능] 플레이어 유닛은 일반적으로 적들의 타겟이되며 사망시 게임 로직적인 처리가 이루어집니다.
    /// <br> 아군유닛과 플레이어유닛은 적용되는 전용 능력치가 다릅니다. </br>
    /// </summary>
    public class Controller_PlayerUnit : Controller_AllyUnit
    {



        #region 생명주기
        protected override void Logic_SetModule(eUnitTpye m_type, int m_index)
        {
            base.Logic_SetModule(m_type, m_index);

            Panel_StageManager.Unit_Player = this;
        }

        protected override void Logic_RemoveModule()
        {
            base.Logic_RemoveModule();

            Panel_StageManager.Unit_Player = null;
        }

        #endregion
    }
}