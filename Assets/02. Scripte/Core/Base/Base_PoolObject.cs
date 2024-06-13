using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;       // List사용을 위해 추가.
using IdleGame.Core.Popup;
using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Log;
using UnityEngine.WSA;
using Unity.VisualScripting;
using System;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

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

    [SerializeField] private Transform enemyrespawnLocation;         // 적 생성 위치

    private void Awake()
    {
        //enemyrespawnLocation = GameObject.Find("EnemyRespawnLocation").transform;     // 적 생성 위치

        InitializeAllPools<EnemyType>(prefabs);
        InitializeAllPools<UIType>(prefabs);
        //InitializeAllPools<DungeonType>(prefabs);
    }

    /// <summary>
    /// 오브젝트 풀 전체 초기화
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prefabs"></param>
    private void InitializeAllPools<T>(List<GameObject> prefabList) where T : Enum
    {
        foreach(GameObject prefab in prefabList)
        {
            CreateObjectDataForAllEnumTypes<T>(prefab);
        }
    }

    public void CreateObjectDataForAllEnumTypes<T>(GameObject prefab) where T : Enum
    {
        Array enumValues = Enum.GetValues(typeof(T));

        foreach (Enum enumValue in enumValues)
        {
            Data_Pool data = CreateObjectData(enumValue, prefab);
            RegisterPool(enumValue, data, poolSize);
        }
    }

    /// <summary>
    /// 오브젝트 데이터 생성
    /// </summary>
    public Data_Pool CreateObjectData<T>(T type, GameObject prefab) where T : Enum
    {
        return new Data_Pool(type, prefab);
    }

    /// <summary>
    /// 오브젝트 풀 등록
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

        for (int i = 0; i < count; i++)
        {
            batch.Add(prefab.Clone());
        }

        return batch;
    }

    /// <summary>
    /// 오브젝트 풀에서 오브젝트 가져오기
    /// 게임매니저 같은 대서 사용 할 것.
    /// </summary>
    public Data_Pool GetObject<T>(T type) where T : Enum
    {
        // 해당 타입의 오브젝트가 존재하면 반환
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

            Data_Pool prefabData = pool[0];
            RegisterPool(type, prefabData, poolSize);

            return GetObject(type);
        }
        // 없으면 새로 생성
        else
        {
            GameObject prefab = NewDynamicPrefabCreate(type);

            if(prefab != null)
            {
                Data_Pool data = CreateObjectData(type, prefab);
                RegisterPool(type, data, poolSize);
                return GetObject(type);
            }

            Base_Engine.Log.Logic_PutLog(new Data_Log("동적 생성을 하려 했으나 null값이 받아져 실패했습니다."));
            
            throw new Exception("GetObject함수에서 동적 생성을 하려 했으나 null값이 받아져 실패했습니다.");
        }
    }

    private GameObject NewDynamicPrefabCreate<T>(T type) where T : Enum
    {
        string typeName = typeof(T).Name.Replace("Type", "");
        List<GameObject> allPrefabs = new List<GameObject>();
        allPrefabs.AddRange(prefabs);

        return allPrefabs.Find(go => go.CompareTag(typeName));
    }

    public void ReturnObject<T>(T type, Data_Pool obj) where T : Enum
    {
        if (poolDictionary.ContainsKey(type))
        {
            obj.SetActive(false);
            poolDictionary[type].Add(obj);
        }
    }
}
