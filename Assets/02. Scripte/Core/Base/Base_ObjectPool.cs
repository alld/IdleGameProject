using UnityEngine;
using System.Collections.Generic;

public class Base_ObjectPool : MonoBehaviour
{
    public GameObject prefab;   // 오브젝트 풀링용 프리팹
    public int initialPoolSize; // 초기 풀 사이즈

    private Queue<PooledObject> pool = new Queue<PooledObject>();   // 오브젝트 풀
    private List<PooledObject> activeObjects = new List<PooledObject>();

    [SerializeField] Vector3 respawnZone;


    private class PooledObject
    {
        public GameObject gameObject;
        public PooledObject(GameObject gameObj) { gameObject = gameObj; }
    }

    private void Start()
    {
        InitialPool();
    }

    
    /// <summary>
    /// 비활성화 객체 생성 함수
    /// </summary>
    /// <returns></returns>
    private GameObject CreateObject()
    {
        GameObject go = Instantiate(prefab);    // 프리팹 생성
        go.SetActive(false);                    // 비활성화 상태로
        pool.Enqueue(new PooledObject(go));     // 풀에 추가

        return go;                              // 반환
    }

    /// <summary>
    /// 초기화 함수 (풀 초기세팅)
    /// </summary>
    private void InitialPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateObject();
        }
    }

    /// <summary>
    /// 풀의 오브젝트 반환
    /// </summary>
    /// <returns></returns>
    public GameObject GetObject()
    {
        // 만약 풀이 비어있다면
        if(pool.Count == 0)
        {
            InitialPool();
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
            Debug.Log("오브젝트를 찾지 못했습니다. ");
            return;
        }

        activeObjects.Remove(po);       // 활성화 리스트에서 제거 (문제가 될 수도 있을 수 있는 부분)
        po.gameObject.SetActive(false); // 비활성화
        ResetTransform(po.gameObject);  // 위치 초기화
        pool.Enqueue(po);               // 풀에 추가
    }

    /// <summary>
    /// 오브젝트 기존 위치로 이동
    /// </summary>
    public void ResetTransform(GameObject obj)
    {
        // 위치 초기화는 게임매니저 함수에 있는게 나은가?
        // 아니면 오브젝트 풀링에서 함수로 있는게 나은가?
        // 만들고 나서 물어보기

        obj.transform.position = respawnZone;
    }

    public void RegisterPooledObject(GameObject obj)
    {
        pool.Enqueue(new PooledObject(obj));
    }
}
