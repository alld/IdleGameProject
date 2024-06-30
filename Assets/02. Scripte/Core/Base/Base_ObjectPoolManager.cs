using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Log;
using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IdleGame.Core.Panel.DataTable
{
    public class Base_ObjectPoolManager : Base_ManagerPanel
    {
        public Dictionary<GameObject, Stack<Base_ObjectPool>> pools = new Dictionary<GameObject, Stack<Base_ObjectPool>>();
        
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

            StartCoroutine(ActivateAndDeactivateObjects(testCube, 10));
        }

        public void CreatePool(GameObject prefab, int initialPoolSize, int betweenPoolSize)
        {
            Base_ObjectPool pool = prefab.gameObject.AddComponent<Base_ObjectPool>();
            pool.Initialize(prefab, initialPoolSize, betweenPoolSize);
            
            // 오류 생길 수 있는 부분?
            pools[prefab].Push(pool);

            //objectPool.LogPool();
            //Base_Engine.Log.Logic_PutLog(new Data_Log("딕셔너리에 저장 되었습니다. : " + pools.Keys + " | " + pools.Values));
            Debug.Log("딕셔너리에 저장 되었습니다. : " + pools.Keys + " | " + pools.Values);
        }

        public GameObject GetPool(GameObject prefab)
        {
            GameObject obj = pools.Keys.FirstOrDefault(key => key.name == prefab.name);

            Debug.Log("받은 오브젝트는 " + obj.name + "입니다");

            if (pools.TryGetValue(prefab, out Stack<Base_ObjectPool> poolStack) &&
                poolStack.Count > 0)
            {
                Base_ObjectPool pool = poolStack.Peek();
                GameObject poolObj = pool.GetObject();

                if (poolObj != null)
                {
                    return obj;
                }
            }

            Base_Engine.Log.Logic_PutLog(new Data_Log("키 값이 존재하지 않아 오브젝트를 활성화 할수 없습니다. 오브젝트를 생성합니다. " + prefab.name, Data_ErrorType.Error_DataLoadFailed));
            CreatePool(prefab, objectPool.initialPoolSize, objectPool.betweenPoolSize);

            if(pools.TryGetValue(prefab, out Stack<Base_ObjectPool> poolStack2) &&
               poolStack2.Count > 0 )
            {
                Base_ObjectPool pool = poolStack2.Peek();
                
                return pool.GetObject();
            }

            Base_Engine.Log.Logic_PutLog(new Data_Log("마지막까지 생성 실패!" + prefab.name, Data_ErrorType.Error_DataLoadFailed));
            
            return null;
        }

        /// <summary>
        /// 오브젝트 풀링 해제
        /// </summary>
        /// <param name="obj"></param>
        public void ReleasePool(GameObject obj)
        {
            string parent = obj.name + Base_ObjectPool.ParentName;    // 받은 오브젝트의 1단계 상위 오브젝트 찾기

            if (pools.TryGetValue(obj, out Stack<Base_ObjectPool> poolStack) &&
                poolStack.Count > 0)
            {
                Base_ObjectPool baseObj = obj.GetComponent<Base_ObjectPool>();    

                poolStack.Push(baseObj);
                pools.Remove(obj);
            }
            else
            {
                Base_Engine.Log.Logic_PutLog(new Data_Log("해제할 수 없는 오브젝트입니다. : " + obj.name, Data_ErrorType.Error_DataLoadFailed));
                ReleaseObjectParent(obj);
            }

            //objectPool.LogPool();
        }

        /// <summary>
        /// 오브젝트의 부모 오브젝트를 찾아서 해제
        /// </summary>
        /// <param name="obj"></param>
        private void ReleaseObjectParent(GameObject obj)
        {
            foreach (var pool in pools.Values)
            {
                Base_ObjectPool baseObj = pool.Peek();

                if (baseObj != null)
                {
                    if (baseObj.CanRelease(obj))
                    {
                        baseObj.ReturnObject(obj);
                        return;
                    }
                }            }
        }

        public IEnumerator ActivateAndDeactivateObjects(GameObject pool, int count)
        {
            for(int i= 0; i< count; i++)
            {
                GetPool(pool);    
            }

            yield return new WaitForSeconds(5f);

            ReleasePool(pool);

        }

        
    }
}
