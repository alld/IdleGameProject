using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Log;
using System.Collections.Generic;
using UnityEngine;


namespace IdleGame.Core.Panel.DataTable
{
    public class Base_ObjectPoolManager : Base_ManagerPanel
    {
        protected Dictionary<GameObject, Base_ObjectPool> pools = new Dictionary<GameObject, Base_ObjectPool>();
        
        [SerializeField] private Base_ObjectPool objectPool;

        public static Base_ObjectPoolManager Instance { get; private set; }
        public GameObject testCube;

        private void Awake()
        {
            #region 싱글턴
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            #endregion

            /// 여기서 CreatePool 함수로 오브젝트 풀 생성.
            CreatePool(testCube, 20, 20);
            //GetPool(new GameObject());

            StartCoroutine(TestRelease());
        }

        public void CreatePool(GameObject prefab, int initialPoolSize, int betweenPoolSize)
        {
            Base_ObjectPool pool = new Base_ObjectPool(prefab, initialPoolSize, betweenPoolSize);
            pools.Add(prefab, pool);

            //Base_Engine.Log.Logic_PutLog(new Data_Log("딕셔너리에 저장 되었습니다. : " + pools.Keys + " | " + pools.Values));
            Debug.Log("딕셔너리에 저장 되었습니다. : " + pools.Keys + " | " + pools.Values);
        }

        public GameObject GetPool(GameObject prefab)
        {
            if (pools.ContainsKey(prefab))
            {
                return pools[prefab].GetObject();
            }
            else
            {
                //Base_Engine.Log.Logic_PutLog(new Data_Log("키 값이 존재하지 않습니다. : " + prefab.name, Data_ErrorType.Error_DataLoadFailed));
                Base_Engine.Log.Logic_PutLog(new Data_Log("키 값이 존재하지 않아 오브젝트를 활성화 할수 없습니다. 오브젝트를 생성합니다. " + prefab.name, Data_ErrorType.Error_DataLoadFailed));
                CreatePool(prefab, 20, 20);

                return null;
            }
        }

        /// <summary>
        /// 오브젝트 풀링 해제
        /// </summary>
        /// <param name="obj"></param>
        public void Release(GameObject obj)
        {
            string parent = obj.transform.parent.name + Base_ObjectPool.ParentName;    // 받은 오브젝트의 1단계 상위 오브젝트 찾기
            
            if(parent == null)
            {
                // 널이면 전체를 찾을 수 밖에 없음.
                foreach (var pool in pools.Values)
                {
                    // 해제 가능한 오브젝트인지 확인 후 해제
                    if (pool.CanRelease(obj))
                    {
                        pool.ReturnObject(obj);
                        
                        return;
                    }
                }
            }
            else
            {
                if (objectPool.ParentObjects.TryGetValue(parent, out GameObject prefab));
                {
                    if(pools.TryGetValue(prefab, out Base_ObjectPool pool))
                    {
                        pool.ReturnObject(obj);

                        return;
                    }
                }
                
            }

            Base_Engine.Log.Logic_PutLog(new Data_Log("해제할 수 없는 오브젝트입니다. : " + obj.name, Data_ErrorType.Error_DataLoadFailed));
        }

        public System.Collections.IEnumerator TestRelease()
        {
            yield return new WaitForSeconds(5f);

            Debug.Log("해제 요청");

            Release(testCube);
            
        }
    }
}
