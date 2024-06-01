using IdleGame.Core;
using IdleGame.Core.Popup;
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
        /// [상태] 현재 활성화중인 패널의 정보를 나타냅니다. 
        /// </summary>
        private eBottomUIPage _currentActivePage = eBottomUIPage.AbilityUpgrade;


        /// <summary>
        /// [기능] 하단 UI 버튼에 반응하여 활성화시킬 패널을 변경합니다. 
        /// </summary>
        public void Logic_ChangeBottomPnael(eBottomUIPage m_changePage)
        {
            int index = (int)m_changePage;

            switch (m_changePage)
            {
                case eBottomUIPage.AbilityUpgrade:
                    if (index != -1)
                        _pages[index].OnClickClose_Base();
                    break;
                case eBottomUIPage.Character:
                case eBottomUIPage.Party:
                case eBottomUIPage.Dungeon:
                case eBottomUIPage.Mine:
                case eBottomUIPage.Shop:
                    if (_currentActivePage == m_changePage && _pages[index].Logic_GetIsShowPopup())
                        goto case eBottomUIPage.AbilityUpgrade;

                    if (_currentActivePage != eBottomUIPage.AbilityUpgrade && _pages[(int)_currentActivePage].Logic_GetIsShowPopup())
                        _pages[(int)_currentActivePage].OnClickClose_Base();

                    _pages[index].OnClickOpen_Base();
                    break;
                default:
                    break;
            }

            _currentActivePage = m_changePage;
        }




        #region 콜백함수
        /// <summary>
        /// [버튼콜백] 캐릭터 패널로 전환합니다.
        /// </summary>
        public void OnClickCharacter()
        {
            Logic_ChangeBottomPnael(eBottomUIPage.Character);
        }

        /// <summary>
        /// [버튼콜백] 파티 패널로 전환합니다.
        /// </summary>
        public void OnClickParty()
        {
            Logic_ChangeBottomPnael(eBottomUIPage.Party);
        }

        /// <summary>
        /// [버튼콜백] 던정 패널로 전환합니다.
        /// </summary>
        public void OnClickDungeon()
        {
            Logic_ChangeBottomPnael(eBottomUIPage.Dungeon);
        }

        /// <summary>
        /// [버튼콜백] 광산 패널로 전환합니다.
        /// </summary>
        public void OnClickMine()
        {
            Logic_ChangeBottomPnael(eBottomUIPage.Mine);
        }

        /// <summary>
        /// [버튼콜백] 상점 패널로 전환합니다.
        /// </summary>
        public void OnClickShop()
        {
            Logic_ChangeBottomPnael(eBottomUIPage.Shop);
        }
        #endregion
    }
}