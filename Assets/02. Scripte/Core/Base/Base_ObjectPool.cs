using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Log;
using System.Collections.Generic;
using UnityEngine;


namespace IdleGame.Core.Panel.DataTable
{
    public class Base_ObjectPool : MonoBehaviour
    {
        private GameObject prefab;   // 오브젝트 풀링용 프리팹
        
        public int initialPoolSize; // 초기 풀 사이즈
        public int betweenPoolSize;     // 추가 풀 사이즈
        
        public Dictionary<string, Stack<Transform>> ParentObjects = new Dictionary<string, Stack<Transform>>();
        private Stack<PooledObject> pool = new Stack<PooledObject>();   // 오브젝트 풀
        private Dictionary<int, Stack<PooledObject>> activeObjects = new Dictionary<int, Stack<PooledObject>>();

        public const string ParentName = "Parent";

        [SerializeField] private Vector3 respawnZone;

        private class PooledObject
        {
            public GameObject gameObject;

            public PooledObject(GameObject gameObj)
            { gameObject = gameObj; }
        }

        public void Initialize(GameObject prefab, int initialPoolSize, int betweenPoolSize)
        {
            this.prefab = prefab;
            this.initialPoolSize = initialPoolSize;
            this.betweenPoolSize = betweenPoolSize;
            CreateParentObject(prefab.name);
            InitialPool();

            //Base_Engine.Log.Logic_PutLog(new Data_Log(prefab + "를 " + initialPoolSize + "개 만들었습니다!"));
            Debug.Log(prefab + "를 " + initialPoolSize + "개 만들었습니다!");
        }

        /// <summary>
        /// 초기화 함수 (풀 초기세팅)
        /// </summary>
        public void InitialPool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateObject();
            }
        }

        /// <summary>
        /// 부모 오브젝트 생성 함수
        /// </summary>
        /// <param name="name"></param>
        private void CreateParentObject(string name)
        {
            if (!ParentObjects.ContainsKey(name))
            {
                GameObject parent = new GameObject(name + ParentName);
                parent.transform.SetParent(Base_ObjectPoolManager.Instance.transform);
                Stack<Transform> stackTransform = new Stack<Transform>();
                stackTransform.Push(parent.transform);
                ParentObjects[name] = stackTransform;       // 부모 오브젝트 위치값 저장
            }
        }

        /// <summary>
        /// 비활성화 객체 생성 함수
        /// InitialPool에서만 사용될 경우 가독성을 위해 삭제하고,
        /// InitialPool로 코드뭉치 이동 시킬 예정.
        /// </summary>
        /// <returns></returns>
        private GameObject CreateObject()
        {
            GameObject go = Instantiate(prefab);    // 프리팹 생성
            
            if(ParentObjects.TryGetValue(prefab.name, out Stack<Transform> objectParent) && 
                objectParent.Count > 0)
            {
                go.transform.SetParent(objectParent.Peek());
            }

            go.SetActive(false);                    // 비활성화 상태로
            // 여기도 Prefab이 들어가야하지 않나? new키워드로 생성하는게 맞나?
            pool.Push(new PooledObject(go));     // 풀에 추가

            return go;                              // 반환 (확장용으로 return하고 있지만 Initial에서만 쓸경우 굳이 필요없음)
        }

        /// <summary>
        /// 풀 사이즈 확장
        /// </summary>
        public void ExpandPool()
        {
            // 우선 추가.
            initialPoolSize += betweenPoolSize;     // 1. N개씩 자동 추가용
            //initialPoolSize = maxPoolSize;      // 2. 최대값까지 추가용
            //initialPoolSize++;                  // 3. 1개씩 추가용

            InitialPool();                      // 오브젝트 추가.
        }

        /// <summary>
        /// 풀의 오브젝트 활성화
        /// </summary>
        /// <returns></returns>
        public GameObject GetObject()
        {
            // 만약 풀이 비어있다면
            if (pool.Count == 0)
            {
                ExpandPool();
            }

            PooledObject po = pool.Pop();    // 풀에서 꺼내옴
            po.gameObject.SetActive(true);       // 활성화
            int key = po.gameObject.GetInstanceID();

            if(!activeObjects.ContainsKey(key))
            {
                activeObjects[key] = new Stack<PooledObject>();
            }

            activeObjects[key].Push(po);
            
            // 여기에 activateObjects로 넣어줘야 하지 않나? 나중에 까먹을까봐 미리 적어둠
            return po.gameObject;                // 반환
        }

        /// <summary>
        /// 오브젝트 반환 함수
        /// </summary>
        /// <param name="obj"></param>
        public void ReturnObject(GameObject obj)
        {
            int key = obj.GetInstanceID();    

            if(activeObjects.TryGetValue(key, out Stack<PooledObject> stackPool) &&
                stackPool.Count > 0)
            {
                PooledObject po = stackPool.Pop(); // 스택에서 제거
                po.gameObject.SetActive(false); // 비활성화
                ResetTransform(po.gameObject.transform);  // 위치 초기화
                pool.Push(po);                  // 풀에 추가
                
                // 뭔가 Pop으로 바꿔서 pools에 넣으면 더 깔쌈할거 같은데 뭔가 로직이 계속 꼬이는 느낌
                activeObjects.Remove(key);      // 활성화된 오브젝트 풀에서 제거
            }
            // 찾지 못하면 종료
            else
            {
                Base_Engine.Log.Logic_PutLog(new Data_Log("오브젝트를 찾지 못했습니다.", Data_ErrorType.Error_DataLoadFailed));
                Debug.Log("오브젝트를 찾지 못했습니다.");

                return;
            }
        }

        /// <summary>
        /// 오브젝트 해제 가능 여부 확인
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CanRelease(GameObject obj)
        {
            int key = obj.GetInstanceID();
            return activeObjects.ContainsKey(key);      // obj가 활성화 리스트에 존재하는지 확인
        }

        /// <summary>
        /// 오브젝트 기존 위치로 이동
        /// </summary>
        public void ResetTransform(Transform objTransform)
        {
            objTransform.position = respawnZone;
        }

        /// <summary>
        /// 오브젝트 풀에 등록
        /// </summary>
        /// <param name="obj"></param>
        public void RegisterPooledObject(GameObject obj)
        {
            pool.Push(new PooledObject(obj));
        }

        /// <summary>
        /// 오브젝트 풀 로그 테스트용
        /// </summary>
        public void LogPool()
        {
            Debug.Log("풀 로그 : " + Base_ObjectPoolManager.Instance.pools.Count);

            foreach (var log in Base_ObjectPoolManager.Instance.pools)
            {
                Stack<Base_ObjectPool> poolStack = log.Value;
                GameObject logPrefab = log.Key;

                if (poolStack.Count > 0)
                {
                    int totalObjects = 0;
                    int activeObjects = 0;

                    foreach (var pool in poolStack)
                    {
                        totalObjects += pool.pool.Count;
                        activeObjects += pool.activeObjects.Count;
                    }
                    Debug.Log("풀 프리팹 : " + logPrefab.name + "\n" +
                                "활성화 오브젝트 : " + activeObjects + "\n" +
                                //"초기 풀 사이즈 : + " + pool.initialPoolSize + "\n" +
                                //"확장 풀 사이즈 : + " + pool.betweenPoolSize + "\n" +
                                "총 갯수 : " + pool.Count);
                }
            }
        }
    }
}