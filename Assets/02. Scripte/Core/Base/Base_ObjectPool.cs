using UnityEngine;
using System.Collections.Generic;

public class Base_ObjectPool : MonoBehaviour
{
    public GameObject prefab;   // 오브젝트 풀링용 프리팹
    public int initialPoolSize; // 초기 풀 사이즈

    private Queue<GameObject> pool = new Queue<GameObject>();   // 오브젝트 풀

    private void Start()
    {
        InitialPool();
    }

    /// <summary>
    /// 초기 풀 생성
    /// </summary>
    private void InitialPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    /// <summary>
    /// 풀의 오브젝트 반환
    /// </summary>
    /// <returns></returns>
    public GameObject GetObject()
    {
        // 풀 오브젝트 가져오기
        if(pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);

            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab);

            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);       // 비활성화
        pool.Enqueue(obj);          // 풀에 반환    
    }
}
