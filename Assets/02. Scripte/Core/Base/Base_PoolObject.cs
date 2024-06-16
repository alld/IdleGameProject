using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;       // List사용을 위해 추가.
using IdleGame.Core.Popup;
using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Log;
using UnityEngine.WSA;
using Unity.VisualScripting;
using System;

public class Base_PoolObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();      // 적 오브젝트 풀링 (적에 대한 오브젝트가 현재 없어서 임시명칭임)
    [SerializeField] private List<GameObject> uiPrefabs = new List<GameObject>();             // 적 오브젝트 풀링 (적에 대한 오브젝트가 현재 없어서 임시명칭임)
    [SerializeField] private List<GameObject> dungeonPrefabs = new List<GameObject>();        // 던전 오브젝트 풀링 (던전에 대한 오브젝트가 현재 없어서 임시명칭임)  
    [SerializeField] private Dictionary<Enum, List<Data_Pool>> poolDictionary = new Dictionary<Enum, List<Data_Pool>>();

    /// <summary>
    /// 오브젝트 풀링의 크기(갯수)를 설정합니다.
    /// </summary>
    [SerializeField] private int poolSize = 40;
    [SerializeField] private int betweenPoolSize = 20;              // 변동적인 풀 사이즈즈

    [SerializeField] private List<GameObject> poolList;           // 오브젝트 풀링 리스트

    [SerializeField] private Transform enemyrespawnLocation;         // 적 생성 위치

    private void Awake()
    {
        //enemyrespawnLocation = GameObject.Find("EnemyRespawnLocation").transform;     // 적 생성 위치
    
        foreach (GameObject prefab in prefabs)
        {
            Data_Pool data = CreateObjectData(prefab);
            RegisterPool(data.Type, data, poolSize);        // 오브젝트 풀링 등록
        }
    }

    // 추가중
    public Data_Pool CreateObjectData(GameObject prefab)
    {
        if (prefab.CompareTag("Enemy"))
        {
            return new Data_Pool(EnemyType.None, prefab);
        }
        else if (prefab.CompareTag("UI"))
        {
            return new Data_Pool(UIType.None, prefab);
        }

        return null;
    }

    /// <summary>
    /// 오브젝트 등록 기능
    /// </summary>
    public void RegisterPool<T>(T type, Data_Pool prefab, int poolSize2 = -1) where T : Enum
    {
        // 받은 타입이 없는 타입이면 등록
        if (!poolDictionary.ContainsKey(type))
        {
            poolDictionary.Add(type, new List<Data_Pool>());  // 키-값 존재시 오류 발생
            //poolDictionary[type] = new List<Data_Pool>();       // 키 존재시 새로운 값 할당 | 키 없으면 키-값 추가
        }

        // 비활성화 상태 풀 생성 오브젝트 추가.
        List<Data_Pool> pool = new List<Data_Pool>();

        for (int i = 0; i < poolSize2; i += betweenPoolSize)
        {
            int count = poolSize;
            pool.AddRange(CreateObjectDataBatch(prefab, count));
        }

        // 딕셔너리에 추가
        poolDictionary[type].AddRange(pool);
    }

    private List<Data_Pool> CreateObjectDataBatch(Data_Pool prefab, int count)
    {
        List<Data_Pool> batch = new List<Data_Pool>(count);

        for(int i = 0; i < count; i++)
        {
            batch.Add(prefab.Clone());
        }

        return batch;
    }

    public Data_Pool GetObject<T>(T type) where T : Enum
    {
        // 해당 값 체크
        if (poolDictionary.TryGetValue(type, out List<Data_Pool> pool))
        {
            foreach (Data_Pool obj in pool)
            {
                if (!obj.IsActive)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }
        }

        Data_Pool prefab = CreateObjectData(prefabs.Find(go => go.CompareTag(type.ToString())));
        RegisterPool(type, prefab, poolSize);

        return GetObject(type);
    }

    public void ReturnObject<T>(T type, Data_Pool obj) where T : Enum
    {
        if (poolDictionary.ContainsKey(type))
        {
            obj.SetActive(false);
            poolDictionary[type].Add(obj);
        }
    }
    
    // Legacy
    /*
    /// <summary>
    /// 딕셔너리용 오브젝트 생성 및 초기자리 세팅
    /// </summary>
    private void DictionaryCreateObject()
    {
        ObjectPoolingEnemySetting();

        // New
        // EnemyActive();

        // Old
        //GameObject createDicObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //createDicObject.name = "Enemy";
        //createDicObject.tag = "Enemy";
        //createDicObject.transform.SetParent(gameObject.transform);
        //createDicObject.transform.position = new Vector3(Screen.width + Random.Range(Screen.width * 0.05f, Screen.width * 0.1f),
        //                                            Screen.height / 2f + Random.Range((Screen.height / 2f) * -0.2f, (Screen.height / 2f) * 0.2f));   // 화면 범위 밖에서 생성 (매직 넘버는 개선 필요)
        //createDicObject.transform.rotation = Quaternion.identity;
        //createDicObject.transform.localScale = new Vector3(20f, 20f, 1f);
    }

    /// <summary>
    /// 적 리스폰 기능
    /// </summary>
    /// <param name="type"></param>
    public void EnemyActive(EnemyType type)
    {
        List<GameObject> pool = poolDic[type];
        int active = 0;

        foreach (GameObject obj in pool)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                active++;

                // 최대갯수 넘길시 종료 (임시 : 20개)
                if (active > poolSize)
                {
                    break;
                }
                // PS : 최대갯수 형식말고 라운드별로 몬스터갯수 1++하는 형식이 더 좋을Sudo?
            }
        }
    }

    /// <summary>
    /// 생성할 오브젝트 세팅
    /// </summary>
    private void ObjectPoolingEnemySetting()
    {
        // 인스펙터창에서 설정하지 않을 경우 사용할 코드.
        //enemyPool.Add(GameObject.CreatePrimitive(PrimitiveType.Cube));
        //enemyPool.Add(GameObject.CreatePrimitive(PrimitiveType.Sphere));
        //enemyPool.Add(GameObject.CreatePrimitive(PrimitiveType.Capsule));
        //enemyPool.Add(GameObject.CreatePrimitive(PrimitiveType.Cylinder));
        //enemyPool.Add(GameObject.CreatePrimitive(PrimitiveType.Plane));
        //enemyPool.Add(GameObject.CreatePrimitive(PrimitiveType.Quad));

        // 몬스터 프리팹 종류만큼 딕셔너리에 추가
        for (int i = 0; i < enemyPool.Count; i++)
        {
            EnemyType type = (EnemyType)i;
            poolDic.Add(type, new List<GameObject>());
            InitializePool(type);
        }
    }

    /// <summary>
    /// 실제 게임오브젝트 생성 및 비활성화
    /// 갯수는 최대 생성 갯수로.
    /// </summary>
    /// <param name="type"></param>
    private void InitializePool(EnemyType type)
    {
        // 
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPool[(int)type]);
            obj.SetActive(false);
            poolDic[type].Add(obj);
        }
    }

    /// <summary>
    /// 오브젝트 반환하기
    /// </summary>
    private void ReturnObject(GameObject obj, EnemyType type)
    {
        if (poolDic.ContainsKey(type))
        {
            obj.SetActive(false);        // 비활성화
            poolDic[type].Add(obj);
        }
    }
    */
}
