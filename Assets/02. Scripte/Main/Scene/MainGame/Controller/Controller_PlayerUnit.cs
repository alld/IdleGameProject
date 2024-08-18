using IdleGame.Core.Unit;
using IdleGame.Data;
using IdleGame.Data.Common.Event;
using IdleGame.Main.GameLogic;
using IdleGame.Main.Scene.Main;

namespace IdleGame.Main.Unit
{
    /// <summary>
    /// [기능] 플레이어 유닛은 일반적으로 적들의 타겟이되며 사망시 게임 로직적인 처리가 이루어집니다.
    /// <br> 아군유닛과 플레이어유닛은 적용되는 전용 능력치가 다릅니다. </br>
    /// </summary>
    public class Controller_PlayerUnit : Controller_AllyUnit
    {
        //public override Data_UnitAbility ability { get => Global_Data.Player.unit_Ability; set => Global_Data.Player.unit_Ability = value; }

        protected override void Logic_StopMove_Base()
        {
            base.Logic_StopMove_Base();

            Panel_MainGameScene.Event.CallEvent(eSceneEventType_MainGame.Act_BGMoveStop);
        }

        protected override void Logic_Action_Move()
        {
            base.Logic_Action_Move();

            Panel_MainGameScene.Event.CallEvent(eSceneEventType_MainGame.Act_BGMoveStart, ability.moveSpeed);
        }

        #region 생명주기
        protected override void Logic_SetModule(eUnitTpye m_type, int m_index)
        {
            ability = Global_Data.Player.unit_Ability;

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