using IdleGame.Core;
using IdleGame.Core.Unit;
using IdleGame.Data;
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
        public ExactInt price;

        /// <summary>
        /// [기능] UI상태를 업데이트합니다. 
        /// </summary>
        public void Logic_UpdateUI()
        {
            if (Global_Data.Player.cc_Gold == price)
            {

            }
        }

        /// <summary>
        /// [기능] 
        /// </summary>
        public void OnClickUpgrade()
        {

        }

        /// <summary>
        /// [기능] 
        /// </summary>
        public void OnClickUpgrade_Mega()
        {

        }
    }
}