using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Log;
using System.Collections.Generic;
using UnityEngine;


namespace IdleGame.Core.Panel.DataTable
{
    public class Base_ObjectPool : MonoBehaviour
    {
        private GameObject prefab;   // 오브젝트 풀링용 프리팹
        
        private int initialPoolSize; // 초기 풀 사이즈
        private int betweenPoolSize;     // 추가 풀 사이즈
        
        public Dictionary<string, GameObject> ParentObjects = new Dictionary<string, GameObject>();
        private Queue<PooledObject> pool = new Queue<PooledObject>();   // 오브젝트 풀
        private List<PooledObject> activeObjects = new List<PooledObject>();

        public const string ParentName = "Parent";

        [SerializeField] private Vector3 respawnZone;

        private class PooledObject
        {
            public GameObject gameObject;

            public PooledObject(GameObject gameObj)
            { gameObject = gameObj; }
        }

        public Base_ObjectPool(GameObject prefab, int initialPoolSize, int betweenPoolSize) : base()
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
                ParentObjects[name] = parent;
                ParentObjects[name].transform.SetParent(Base_ObjectPoolManager.Instance.transform);

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
            go.transform.SetParent(ParentObjects[prefab.name].transform);
            go.SetActive(false);                    // 비활성화 상태로
            pool.Enqueue(new PooledObject(go));     // 풀에 추가

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

            PooledObject po = pool.Dequeue();    // 풀에서 꺼내옴
            po.gameObject.SetActive(true);       // 활성화
            activeObjects.Add(po);               // 활성화 리스트에 추가

            return po.gameObject;                // 반환
        }

        /// <summary>
        /// 오브젝트 반환 함수
        /// </summary>
        /// <param name="obj"></param>
        public void ReturnObject(GameObject obj)
        {
            PooledObject po = activeObjects.Find(p => p.gameObject == obj); // 활성화 리스트에서 찾음

            // 찾지 못하면 종료
            if (po == null)
            {
                Base_Engine.Log.Logic_PutLog(new Data_Log("오브젝트를 찾지 못했습니다.", Data_ErrorType.Error_DataLoadFailed));

                return;
            }

            activeObjects.Remove(po);       // 활성화 리스트에서 제거 (문제가 될 수도 있을 수 있는 부분)
            po.gameObject.SetActive(false); // 비활성화
            ResetTransform(po.gameObject);  // 위치 초기화
            pool.Enqueue(po);               // 풀에 추가
        }

        /// <summary>
        /// 오브젝트 해제 가능 여부 확인
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CanRelease(GameObject obj)
        {
            return activeObjects.Exists(p => p.gameObject == obj);      // obj가 활성화 리스트에 존재하는지 확인
        }

        /// <summary>
        /// 오브젝트 기존 위치로 이동
        /// </summary>
        public void ResetTransform(GameObject obj)
        {
            // 어디다 하든 상관없지만 더 나은 관리를 위한 의문점.
            // 위치 초기화는 게임매니저 함수에 있는게 나은가?
            // 아니면 오브젝트 풀링에서 함수로 있는게 나은가?
            // 만들고 나서 물어보기

            obj.transform.position = respawnZone;
        }

        /// <summary>
        /// 오브젝트 풀에 등록
        /// </summary>
        /// <param name="obj"></param>
        public void RegisterPooledObject(GameObject obj)
        {
            pool.Enqueue(new PooledObject(obj));
        }


        public void LogPool()
        {
            Debug.Log("풀 로그 : " + Base_ObjectPoolManager.Instance.pools.Count);

            foreach (var log in Base_ObjectPoolManager.Instance.pools)
            {
                Base_ObjectPool pool = log.Value;
                Debug.Log("풀 프리팹 : " + log.Key.name + "\n" +
                            "활성화 오브젝트 : " + pool.activeObjects.Count + "\n" +
                            "초기 풀 사이즈 : + " + pool.initialPoolSize + "\n" +
                            "확장 풀 사이즈 : + " + pool.betweenPoolSize + "\n" +
                            "총 갯수 : " + pool.pool.Count);
            }
        }
    }
}