using IdleGame.Data.Base;
using UnityEngine;


namespace IdleGame.Core
{
    /// <summary>
    /// [기능] 패널의 역할은 외부 입력을 받아서 내부적 로직에 전달하는 입력과 기능을 통제하는 역할을 담당합니다. 
    /// </summary>
    public class Base_Panel : MonoBehaviour
    {
        /// <summary>
        /// [정보] 로직에대한 기초 정보를 담고 있습니다.
        /// </summary>
        [Header("BasicInfo")]
        [SerializeField] private TagInfo _tag;
    }
}