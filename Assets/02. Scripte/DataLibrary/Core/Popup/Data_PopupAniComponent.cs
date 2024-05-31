using UnityEngine;

namespace IdleGame.Data.Popup
{
    /// <summary>
    /// [데이터] 애니메이션이 적용된 팝업에 필요한 별도 구성요소들을 정의하고 있습니다.
    /// </summary>
    [System.Serializable]
    public struct Data_PopupAniComponent
    {
        /// <summary>
        /// [종류] 팝업의 애니메이션 종류중, 등장하는 연출입니다.
        /// </summary>
        public enum eAnimationType_In
        {
            None = 0,
            /// <summary> [종류] 투명한 상태에서 서서히 등장하는 연출입니다. </summary>
            FadeIn,
            /// <summary> [종류] 왼쪽에서 등장해오는 연출입니다. </summary>
            LeftIn,
            /// <summary> [종류] 오른쪽에서 등장해오는 연출입니다. </summary>
            RightIn,
            /// <summary> [종류] 위에서 내려오는 연출입니다. </summary>
            UpIn,
            /// <summary> [종류] 아래에서 올라오는 연출입니다. </summary>
            DownIn,
            /// <summary> [종류] 중앙에서 빠르게 확대되면서 등장하는 연출입니다.</summary>
            BounceIn,
            /// <summary> [종류] 위에서 떨어트린것과 같은 연출입니다. </summary>
            DropIn,
        }

        /// <summary>
        /// [종류] 팝업의 애니메이션 종류중, 퇴장하는 연출입니다.
        /// </summary>
        public enum eAnimationType_Out
        {
            None = 0,
            /// <summary> [종류] 서서히 투명해져서 사라지는 연출입니다. </summary>
            FadeOut,
            /// <summary> [종류] 왼쪽으로 나가는 연출입니다. </summary>
            Leftout,
            /// <summary> [종류] 오른쪽으로 나가는 연출입니다. </summary>
            RightOut,
            /// <summary> [종류] 위로 올라가는 연출입니다. </summary>
            UpOut,
            /// <summary> [종류] 아래로 내려가는 연출입니다.. </summary>
            DownOut,
            /// <summary> [종류] 빠르게 작아지면서 사라지는 연출입니다.</summary>
            BounceOut,
        }


        /// <summary>
        /// [기능] 팝업이 열릴때 작동되는 애니메이션 연출 타입입니다.
        /// </summary>
        [SerializeField] public eAnimationType_In aniType_in;

        /// <summary>
        /// [기능] 팝업이 닫힐때 작동되는 애니메이션 연출 타입입니다.
        /// </summary>
        [SerializeField] public eAnimationType_Out aniType_out;

        /// <summary>
        /// [기능] 팝업창이 열릴때까지 애니메이션이 소요되는 시간입니다.
        /// </summary>
        [SerializeField][Range(0.01f, 1.0f)] public float duration_appearance;

        /// <summary>
        /// [기능] 팝업창이 닫힐때까지 애니메이션이 소요되는 시간입니다.
        /// </summary>
        [SerializeField][Range(0.01f, 1.0f)] public float duration_extinction;

    }
}