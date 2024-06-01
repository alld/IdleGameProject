using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.Data.Popup
{
    /// <summary>
    /// [데이터] 팝업의 구성 요소 및 설정 데이터를 구성하고 있습니다. 
    /// </summary>
    [System.Serializable]
    public struct Data_PopupComponent
    {
        /// <summary>
        /// [캐시] 팝업창의 내용에 해당하는 오브젝트입니다. 
        /// </summary>
        [SerializeField]
        public GameObject obj_graphic;

        /// <summary>
        /// [캐시] 팝업창이 활성화 되었을때 배경을 어둡게해서 팝업창을 강조하는 암막효과입니다.
        /// </summary>
        [SerializeField]
        public Image i_dim;

        /// <summary>
        /// [캐시] 팝업창이 활성화 되었을때 팝업창 너머로 클릭을 하지못하도록 막는 역할을 합니다. 
        /// </summary>
        [SerializeField]
        public Button b_inputBlock;

        /// <summary>
        /// [설정] 입력 방지를 하는 오브젝트를 클릭했을때 팝업창이 닫히도록 하는 역할을 합니다. 
        /// </summary>
        [SerializeField]
        public bool usedInputClose;
    }
}