using IdleGame.Data.Popup;

namespace IdleGame.Core.Popup
{
    /// <summary>
    /// [기능] 공통 팝업으로 동일한 양식으로 내용을 변경해서 사용되는 코드기반 팝업입니다. 
    /// </summary>
    public class Base_CommonPopup : Base_AnimationPopup
    {
        /// <summary>
        /// [기능] 팝업에 대한 데이터를 설정합니다. 
        /// </summary>
        protected virtual void Logic_SetPopupData(Data_Popup _data)
        {

        }

        /// <summary>
        /// [초기화] 팝업 데이터를 초기화시킵니다. 
        /// </summary>
        protected virtual void Logic_PopupInit()
        {

        }

    }
}