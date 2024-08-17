using IdleGame.Core;
using IdleGame.Core.Unit;
using IdleGame.Data;
using IdleGame.Data.Common;
using IdleGame.Data.Common.Event;
using IdleGame.Data.Numeric;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.Main.Scene.Main.UI
{
    public class Item_AbilitySlot : MonoBehaviour
    {
        /// <summary>
        /// [캐시] 슬롯에서 취급되는 타입입니다.
        /// </summary>
        public eAbilityType type = eAbilityType.None;

        /// <summary>
        /// [캐시] 슬롯의 아이콘을 표시합니다. 
        /// </summary>
        public Image i_icon;
        /// <summary>
        /// [캐시] 해당 슬롯의 현재 레벨을 나타냅니다. 
        /// </summary>
        public Graphic_Text t_level;

        /// <summary>
        /// [캐시] 해당 슬롯의 이름에 해당하는 타이틀 텍스트입니다.
        /// </summary>
        public Graphic_Text t_valueTitle;
        /// <summary>
        /// [캐시] 해당 슬롯의 현재 능력치 상승량을 표시하는 텍스트입니다. 
        /// </summary>
        public Graphic_Text t_value;

        /// <summary>
        /// [캐시] 해당 슬롯을 묶음 단위로 상승 시킬 수 있는 구매 버튼입니다.
        /// </summary>
        public Button b_upMega;
        /// <summary>
        /// [캐시] 버튼의 내용이 표시되는 텍스트입니다.
        /// </summary>
        public Graphic_Text t_titleMega;
        /// <summary>
        /// [캐시] 버튼의 가격이 표시되는 텍스트입니다.
        /// </summary>
        public Graphic_Text t_priceMega;

        /// <summary>
        /// [캐시] 해당 슬롯을 1개 단위로 상승 시킬 수 있는 구매 버튼입니다. 
        /// </summary>
        public Button b_upNormal;
        /// <summary>
        /// [캐시] 버튼의 내용이 표시되는 텍스트입니다.
        /// </summary>
        public Graphic_Text t_titleNormal;
        /// <summary>
        /// [캐시] 버튼의 가격이 표시되는 텍스트입니다.
        /// </summary>
        public Graphic_Text t_priceNormal;

        /// <summary>
        /// [데이터] 1개의 구매 가격입니다.
        /// </summary>
        [HideInInspector]
        public ExactInt price
        {
            get { return _price; }
            set
            {
                _price = value;
                _megaPrice = value * 10;
            }
        }

        /// <summary>
        /// [데이터] 1회 구매 비용입니다. 
        /// </summary>
        private ExactInt _price;

        /// <summary>
        /// [데이터] 10회 구매 비용입니다.
        /// </summary>
        private ExactInt _megaPrice;

        public void Start()
        {
            GameManager.Event.RegisterEvent(eGlobalEventType.Currency_UpdateGold, Logic_UpdateUI);
        }

        private void OnDestroy()
        {
            GameManager.Event.RemoveEvent(eGlobalEventType.Currency_UpdateGold, Logic_UpdateUI);
        }

        /// <summary>
        /// [기능] UI상태를 업데이트합니다. 
        /// </summary>
        public void Logic_UpdateUI()
        {
            // 조건 :: 1회 사용 돈도 없음
            if (Global_Data.Player.cc_Gold < _price)
            {
                b_upNormal.interactable = false;
                b_upMega.interactable = false;
            }
            else
            {
                b_upNormal.interactable = true;

                // 조건 :: 10회 살돈은 없음.
                if (Global_Data.Player.cc_Gold < _megaPrice)
                {
                    b_upMega.interactable = false;
                }
                else
                {
                    b_upMega.interactable = true;
                }
            }
        }

        #region 콜백 함수
        /// <summary>
        /// [기능] 1회 구매를 한 경우
        /// </summary>
        public void OnClickUpgrade()
        {
            GameManager.Currency.Logic_SetAddCurrency(eCurrencyType.Gold, -_price);
            Global_Data.Player.slot_Ability[type].LevelUp();
        }

        /// <summary>
        /// [기능] 10회 구매를 한 경우 
        /// </summary>
        public void OnClickUpgrade_Mega()
        {
            GameManager.Currency.Logic_SetAddCurrency(eCurrencyType.Gold, -_megaPrice);
            Global_Data.Player.slot_Ability[type].LevelUp(10);
        }
        #endregion
    }
}