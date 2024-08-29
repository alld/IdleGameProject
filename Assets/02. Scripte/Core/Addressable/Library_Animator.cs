using System.Collections.Generic;
using UnityEngine;

namespace IdleGame.Data.Adrees
{
    /// <summary>
    /// [기능] 어드레서블로 관리되는 애니메이터 그룹입니다.
    /// </summary>
    public class Library_Animator : MonoBehaviour
    {
        [System.Serializable]
        public class Data_AnimatorGroup
        {
            /// <summary>
            /// [데이터] 애니메이터를 구분하기 위한 이름입니다.
            /// </summary>
            public string name;

            /// <summary>
            /// [캐시] 애니메이터를 캐싱처리하는 공간입니다.
            /// </summary>
            public AnimatorOverrideController ani;

            /// <summary>
            /// [데이터] 애니메이터를 찾기위한 고유 인덱스입니다.
            /// </summary>
            public int index;
        }

        /// <summary>
        /// [캐시] 애니메이터 리스트입니다.
        /// </summary>
        public static Dictionary<int, AnimatorOverrideController> dic_ani = new Dictionary<int, AnimatorOverrideController>();

        /// <summary>
        /// [캐시] 애니메이터 할당리스트입니다.
        /// </summary>
        [SerializeField]
        private Data_AnimatorGroup[] characters;

        /// <summary>
        /// [캐시] 애니메이터 할당리스트입니다.
        /// </summary>
        [SerializeField]
        private Data_AnimatorGroup[] monsters;

        private void Start()
        {
            dic_ani.Clear();

            for (int i = 0; i < characters.Length; i++)
            {
                dic_ani.Add(characters[i].index, characters[i].ani);
            }

            for (int i = 0; i < monsters.Length; i++)
            {
                dic_ani.Add(monsters[i].index, monsters[i].ani);
            }
        }
    }
}