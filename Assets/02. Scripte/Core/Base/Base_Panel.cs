using IdleGame.Data.Base;
using UnityEngine;


namespace IdleGame.Core
{
    /// <summary>
    /// [기능] 패널의 역할은 외부 입력을 받아서 내부적 로직에 전달하는 입력과 기능을 통제하는 역할을 담당합니다. 
    /// </summary>
    public abstract class Base_Panel : MonoBehaviour
    {
        /// <summary>
        /// [정보] 로직에대한 기초 정보를 담고 있습니다.
        /// </summary>
        [Header("BasicInfo")]
        [SerializeField] protected TagInfo _tag;

        protected virtual void Awake()
        {
            Logic_Init();
        }

        /// <summary>
        /// [초기화] 로직이 실행되면서 초기 필요한 설정들을 지정해주는 초기화 로직입니다.
        /// </summary>
        private void Logic_Init()
        {
            Logic_Init_Custom();

            Logic_RegisterEvent_Custom();
        }

        /// <summary>
        /// [초기화] 로직이 진행에 필요한 초기 설정들을 지정합니다. 
        /// </summary>
        protected virtual void Logic_Init_Custom() { }

        /// <summary>
        /// [설정] 이벤트 등록이 필요한 로직들을 등록시킵니다. 
        /// </summary>
        protected virtual void Logic_RegisterEvent_Custom() { }
    }
}