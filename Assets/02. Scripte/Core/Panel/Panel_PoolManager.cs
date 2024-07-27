using IdleGame.Core.Pool;
using IdleGame.Core.Procedure;
using IdleGame.Data.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace IdleGame.Core.Panel.Pool
{
    /// <summary>
    /// [기능] 오브젝트들을 풀단위로 관리하는 오브젝트풀 매니저입니다.
    /// </summary>
    public class Panel_PoolManager : Base_ManagerPanel
    {
        public class Data_Pool
        {
            /// <summary>
            /// [데이터] 오브젝트 풀에 취급되는 타입을 나타냅니다. 
            /// </summary>
            private ePoolType _type;

            /// <summary>
            /// [상태] 자동으로 사이즈업을 진행할지를 지정합니다. 
            /// </summary>
            public bool autoLock = false;

            /// <summary>
            /// [상태] 기본적으로 
            /// </summary>
            public int sizeUpRange = 20;

            /// <summary>
            /// [캐시] 오브젝트풀에서 관리되는 활성화 오브젝트 리스트입니다.
            /// </summary>
            private List<Base_PoolObject> _activeList = new List<Base_PoolObject>();

            /// <summary>
            /// [캐시] 오브젝트풀에서 관리되는 비활성화 오브젝트 리스트입니다. 
            /// </summary>
            private Stack<Base_PoolObject> _deActiveList = new Stack<Base_PoolObject>();

            /// <summary>
            /// [캐시] 오브젝트가 새로 만들어지거나 반환될때 되돌아가는 대상입니다.
            /// </summary>
            private Transform _parent;

            /// <summary>
            /// [캐시] 오브젝트가 새로 만들어질때 사용되는 프리팹입니다.
            /// </summary>
            private Base_PoolObject _prefab;

            /// <summary>
            /// [생성자] 기본형을 구성합니다.
            /// </summary>
            public Data_Pool(ePoolType m_type, Base_PoolObject m_prefab)
            {
                _type = m_type;
                _prefab = m_prefab;
                _parent = Base_Engine.Pool.transform;
            }

            /// <summary>
            /// [생성자] 기본형을 구성합니다.
            /// </summary>
            public Data_Pool(ePoolType m_type, Base_PoolObject m_prefab, int m_size)
            {
                _type = m_type;
                _prefab = m_prefab;
                _parent = Base_Engine.Pool.transform;
                sizeUpRange = m_size;
            }

            /// <summary>
            /// [초기화] 풀을 최초 상태로 되돌립니다.
            /// </summary>
            public void Init()
            {
                int initSize = sizeUpRange * 10;

                Recall();

                if (_deActiveList.Count >= initSize)
                    return;

                for (int i = _deActiveList.Count; i < initSize; i++)
                {
                    CreateObject();
                }
            }

            /// <summary>
            /// [기능] 오브젝트를 집어넣습니다.
            /// </summary>
            public void Push(Base_PoolObject m_object)
            {
                m_object.Pool_Clear();
                _deActiveList.Push(m_object);

                if (_activeList[m_object.GetPoolIndex] != m_object)
                    ReSetIndex();

                _activeList.RemoveAt(m_object.GetPoolIndex);
                m_object.Pool_SetPoolData(ePoolType.None);
                m_object.gameObject.SetActive(false);
            }

            /// <summary>
            /// [기능] 현재 할당된 인덱스를 기준으로 오브젝트들을 재설정합니다.
            /// </summary>
            public void ReSetIndex()
            {
                for (int i = 0; i < _activeList.Count; i++)
                {
                    _activeList[i].Pool_SetIndex(i);
                }
            }

            /// <summary>
            /// [기능] 자동으로 풀의 최대갯수를 늘립니다.
            /// </summary>
            private bool TryAutoSizeUp()
            {
                if (autoLock) return false;
                if (_deActiveList.Count > 3) return true;

                for (int i = 0; i < sizeUpRange; i++)
                {
                    CreateObject();
                }

                return true;
            }

            /// <summary>
            /// [기능] 오브젝트를 생성합니다. 
            /// </summary>
            private void CreateObject()
            {
                Base_PoolObject obj = Instantiate(_prefab, _parent);

                _deActiveList.Push(obj);

                obj.Pool_SetPoolData(ePoolType.None);
                obj.gameObject.SetActive(false);
            }

            /// <summary>
            /// [기능] 오브젝트를 반환받습니다. 
            /// </summary>
            public Base_PoolObject Pop(Transform m_parent)
            {
                if (!TryAutoSizeUp()) return null;

                Base_PoolObject obj = _deActiveList.Pop();
                _activeList.Add(obj);
                obj.transform.SetParent(m_parent);
                obj.Pool_SetPoolData(_type, _activeList.Count - 1);
                return obj;
            }

            /// <summary>
            /// [기능] 모든 오브젝트를 회수합니다.
            /// </summary>
            public void Recall()
            {
                foreach (var obj in _activeList)
                {
                    obj.Pool_Clear();
                    obj.Pool_SetPoolData(ePoolType.None);
                    obj.transform.SetParent(_parent);

                    obj.gameObject.SetActive(false);
                    _deActiveList.Push(obj);
                }

                _activeList.Clear();
            }
        }

        private Dictionary<ePoolType, Data_Pool> poolList = new Dictionary<ePoolType, Data_Pool>();

        /// <summary>
        /// [기능] 새로운 풀을 등록시킵니다.
        /// </summary>
        private void Logic_RegisterPool(ePoolType m_type, Base_PoolObject m_prefab)
        {
            if (poolList.ContainsKey(m_type))
                return;

            poolList.Add(m_type, new Data_Pool(m_type, m_prefab));

            poolList[m_type].Init();
        }

        /// <summary>
        /// [기능] 오브젝트를 꺼냅니다.
        /// </summary>
        public Base_PoolObject Logic_GetObject(ePoolType m_type, Transform m_parent)
        {
            return poolList[m_type].Pop(m_parent);
        }

        /// <summary>
        /// [기능] 오브젝트를 반환합니다.
        /// </summary>
        public void Logic_PutObject(Base_PoolObject m_object)
        {
            poolList[m_object.GetPoolType].Push(m_object);
        }

    }
}