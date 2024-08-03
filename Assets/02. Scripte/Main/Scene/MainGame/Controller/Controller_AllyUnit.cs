using IdleGame.Core.Unit;
using IdleGame.Main.GameLogic;

namespace IdleGame.Main.Unit
{
    /// <summary>
    /// [기능] 아군 유닛을 지정합니다. 아군유닛에는 플레이어 유닛도 포함됩니다.
    /// <br> 추후 기획에 따라서 각종 버프, 업그레이드 효과등 공유 데이터들이 적용될 수 있습니다.</br>
    /// </summary>
    public abstract class Controller_AllyUnit : Base_Unit
    {
        #region 보조 기능

        protected override void Logic_SearchTarget_Base()
        {
            Controller_EnemyUnit compareUnit = null;
            _dd.target_movePoint = transform.position;

            if (Panel_StageManager.Unit_Monsters.Count == 0) return;
            else if (Panel_StageManager.Unit_Monsters.Count == 1)
                _target = Panel_StageManager.Unit_Monsters[0];
            else
            {
                // 조건 :: 현재 기준, 가장 가까운 유닛을 탐색함.
                compareUnit = Panel_StageManager.Unit_Monsters[0];

                for (int i = 1; i < Panel_StageManager.Unit_Monsters.Count; i++)
                {
                    if (Panel_StageManager.Unit_Monsters[i].transform.position.x >= compareUnit.transform.position.x)
                        continue;

                    compareUnit = Panel_StageManager.Unit_Monsters[i];
                }

                _target = compareUnit;
            }

            base.Logic_SearchTarget_Base();
        }
        #endregion
    }
}