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
        public Dictionary<GameObject, Base_ObjectPool> pools = new Dictionary<GameObject, Base_ObjectPool>();
        
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
            GameObject poolObject = new GameObject(prefab.name);
            Base_ObjectPool pool = poolObject.AddComponent<Base_ObjectPool>();
            pool.Initialize(prefab, initialPoolSize, betweenPoolSize);
            pools.Add(prefab, pool);

            //objectPool.LogPool();
            //Base_Engine.Log.Logic_PutLog(new Data_Log("딕셔너리에 저장 되었습니다. : " + pools.Keys + " | " + pools.Values));
            Debug.Log("딕셔너리에 저장 되었습니다. : " + pools.Keys + " | " + pools.Values);
        }

        public GameObject GetPool(GameObject prefab)
        {
            GameObject obj = pools.Keys.FirstOrDefault(key => key.name == prefab.name);

            Debug.Log("받은 오브젝트는 " + obj.name + "입니다");

            if (pools.TryGetValue(prefab, out Base_ObjectPool pool))
            {
                obj = pool.GetObject();
                pools[obj] = pool;

                return obj;
            }
            else
            {
                //Base_Engine.Log.Logic_PutLog(new Data_Log("키 값이 존재하지 않습니다. : " + prefab.name, Data_ErrorType.Error_DataLoadFailed));
                Base_Engine.Log.Logic_PutLog(new Data_Log("키 값이 존재하지 않아 오브젝트를 활성화 할수 없습니다. 오브젝트를 생성합니다. " + prefab.name, Data_ErrorType.Error_DataLoadFailed));
                CreatePool(prefab, objectPool.initialPoolSize, objectPool.betweenPoolSize);
                
                return null;
            }
        }

        /// <summary>
        /// 오브젝트 풀링 해제
        /// </summary>
        /// <param name="obj"></param>
        public void ReleasePool(GameObject obj)
         {
            string parent = obj.name + Base_ObjectPool.ParentName;    // 받은 오브젝트의 1단계 상위 오브젝트 찾기

            if (pools.TryGetValue(obj, out Base_ObjectPool pool))
            {
                pool.ReturnObject(obj);
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
                if(pool.CanRelease(obj))
                {
                    pool.ReturnObject(obj);
                }
            }
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
