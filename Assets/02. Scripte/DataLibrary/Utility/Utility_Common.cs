using System.Buffers;
using System.Collections.Generic;
using UnityEngine;

namespace IdleGame.Core.Utility
{
    /// <summary>
    /// [유틸리티] 기본적인 공용 기능을 제공합니다. 
    /// </summary>
    public static class Utility_Common
    {
        /// <summary>
        /// [캐시] 딜레이 함수를 미리 선언하여 캐시처리해둔 콜렉션입니다.
        /// </summary>
        private static Dictionary<float, WaitForSeconds> _waitList = new Dictionary<float, WaitForSeconds>();

        /// <summary>
        /// [기능] 메모리 배열 풀입니다. 커스텀 숫자에 사용되는 배열값을 메모리풀링으로 효과적으로 관리하기위해서 사용되었습니다. 
        /// </summary>
        public static ArrayPool<int> mPool_int = ArrayPool<int>.Shared;


        /// <summary>
        /// [기능] 캐시처리 되어있는 딜레이 함수를 반환받습니다.
        /// </summary>
        public static WaitForSeconds WaitForSeconds(float m_delay)
        {
            if (!_waitList.ContainsKey(m_delay))
                _waitList.Add(m_delay, new WaitForSeconds(m_delay));

            return _waitList[m_delay];
        }
    }
}