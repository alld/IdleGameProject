using IdleGame.Core.Procedure;
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

        /// <summary>
        /// [데이터] 해당 풀에 할당된 타입을 반환합니다. 
        /// </summary>
        public ePoolType GetPoolType => _poolType;

        /// <summary>
        /// [데이터] 해당 풀에 할당된 고유 인덱스를 반환합니다. 
        /// </summary>
        public int GetPoolIndex => _poolIndex;

        /// <summary>
        /// [설정] 풀에 필요한 기본데이터를 지정합니다.
        /// <br> 초기화 역할도 겸하고 있습니다. </br>
        /// </summary>
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

        /// <summary>
        /// [설정] 풀에서 사용되어지는 데이터입니다. 풀 리스트에 할당된 인덱스 기준이기때문에 해당 값은 가변적으로 변합니다. 
        /// </summary>
        public void Pool_SetIndex(int m_index) => _poolIndex = m_index;

        /// <summary>
        /// [초기화] 오브젝트풀이 회수되기전에 호출되는 함수입니다. 
        /// <br> 초기화에 필요한 부분들을 정의합니다. 초기화는 풀에서 이루어집니다. </br>
        /// </summary>
        public abstract void Pool_Clear();

        /// <summary>
        /// [기능] 오브젝트를 풀에 반환합니다.
        /// </summary>
        public virtual void Pool_Return_Base()
        {
            Base_Engine.Pool.Logic_PutObject(this);
        }
    }
}