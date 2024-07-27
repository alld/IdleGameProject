using IdleGame.Core;
using IdleGame.Core.Popup;
using IdleGame.Data;
using IdleGame.Data.Base.BottomPage;
using UnityEngine;

namespace IdleGame.Main.Scene.Main.UI
{
    /// <summary>
    /// [기능] 하단 버튼들의 이벤트 콜백과 페이지 전환을 관리합니다.
    /// </summary>
    public class Panel_BottomUI : Base_Panel
    {
        /// <summary>
        /// [캐시] 능력치 강화 패널을 활성화 시키기위한 캐시입니다.
        /// </summary>
        [Header("BottomUI Component")]
        [SerializeField]
        private Panel_AbilityUpgrade _panel_abilityUpgrade;

        /// <summary>
        /// [캐시] 페이지에 해당하는 패널들을 담습니다. 
        /// <br> eBottomUIPage 에 할당된 순서대로 담아야합니다. </br>
        /// </summary>
        [SerializeField]
        private Base_Popup[] _pages;

        /// <summary>
        /// [기능] 하단 UI 버튼에 반응하여 활성화시킬 패널을 변경합니다. 
        /// </summary>
        public void Logic_ChangeBottomPnael(eUIPage m_changePage)
        {
            int index = (int)m_changePage;

            switch (m_changePage)
            {
                case eUIPage.AbilityUpgrade:
                    if (index != -1)
                        _pages[index].OnClickClose_Base();
                    break;
                case eUIPage.Character:
                case eUIPage.Party:
                case eUIPage.Dungeon:
                case eUIPage.Mine:
                case eUIPage.Shop:
                    if (Global_Data.Player.currentPage == m_changePage && _pages[index].Logic_GetIsShowPopup())
                        goto case eUIPage.AbilityUpgrade;

                    if (Global_Data.Player.currentPage != eUIPage.AbilityUpgrade && _pages[(int)Global_Data.Player.currentPage].Logic_GetIsShowPopup())
                        _pages[(int)Global_Data.Player.currentPage].OnClickClose_Base();

                    _pages[index].OnClickOpen_Base();
                    break;
                default:
                    break;
            }

            Global_Data.Player.currentPage = m_changePage;
        }




        #region 콜백함수
        /// <summary>
        /// [버튼콜백] 캐릭터 패널로 전환합니다.
        /// </summary>
        public void OnClickCharacter()
        {
            Logic_ChangeBottomPnael(eUIPage.Character);
        }

        /// <summary>
        /// [버튼콜백] 파티 패널로 전환합니다.
        /// </summary>
        public void OnClickParty()
        {
            Logic_ChangeBottomPnael(eUIPage.Party);
        }

        /// <summary>
        /// [버튼콜백] 던정 패널로 전환합니다.
        /// </summary>
        public void OnClickDungeon()
        {
            Logic_ChangeBottomPnael(eUIPage.Dungeon);
        }

        /// <summary>
        /// [버튼콜백] 광산 패널로 전환합니다.
        /// </summary>
        public void OnClickMine()
        {
            Logic_ChangeBottomPnael(eUIPage.Mine);
        }

        /// <summary>
        /// [버튼콜백] 상점 패널로 전환합니다.
        /// </summary>
        public void OnClickShop()
        {
            Logic_ChangeBottomPnael(eUIPage.Shop);
        }
        #endregion
    }
}