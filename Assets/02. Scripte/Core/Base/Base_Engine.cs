using IdleGame.Data.Base;
using UnityEngine;


namespace IdleGame.Core.Procedure
{
    /// <summary>
    /// [기능] 게임의 전반적인 진행을 통제하는 로직을 담고 있습니다. 
    /// </summary>
    public class Base_Engine : MonoBehaviour
    {
        /// <summary>
        /// [정보] 로직에대한 기초 정보를 담고 있습니다.
        /// </summary>
        [Header("BaseEngine")]
        [SerializeField] private TagInfo _tag;
    }
}