using IdleGame.Data.Pool;
using UnityEngine;


namespace IdleGame.Core.Pool
{
    /// <summary>
    /// [기능] 오브젝트 풀에 적용되는 대상들을 정의합니다.
    /// </summary>
    public abstract class Base_PoolObject : MonoBehaviour
    {
        private int _poolIndex = -1;
        private ePoolType _poolType = ePoolType.None;

        public ePoolType GetPoolType => _poolType;

        public int GetPoolIndex => _poolIndex;

        public void Pool_SetPoolData(ePoolType m_type, int m_index = -1)
        {
            _poolType = m_type;
            if (m_type == ePoolType.None)
            {
                m_index = -1;
                return;
            }
            _poolIndex = m_index;
        }

        public void Pool_SetIndex(int m_index) => _poolIndex = m_index;

        /// <summary>
        /// [초기화] 오브젝트풀이 회수되기전에 호출되는 함수입니다. 
        /// <br> 초기화에 필요한 부분들을 정의합니다. </br>
        /// </summary>
        public abstract void Pool_Clear();
    }
}