using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;       // List사용을 위해 추가.
using IdleGame.Core.Popup;
using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Log;
using UnityEngine.WSA;
using Unity.VisualScripting;

public class Panel_ObjectPooling : MonoBehaviour
{
    /// <summary>
    /// 하이어라키에 생성된 오브젝트가 없어서 현재 임시 명칭입니다.
    /// </summary>
    //[SerializeField] private List<GameObject> enemyPool = new List<GameObject>();             // 적 오브젝트 풀링 (적에 대한 오브젝트가 현재 없어서 임시명칭임)
    //[SerializeField] private List<GameObject> alliesPool = new List<GameObject>();            // 아군 오브젝트 풀링 (아군에 대한 오브젝트가 현재 없어서 임시명칭임)
    //[SerializeField] private List<GameObject> dungeonPool = new List<GameObject>();           // 던전 오브젝트 풀링 (던전에 대한 오브젝트가 현재 없어서 임시명칭임)  

    // New
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();      // 적 오브젝트 풀링 (적에 대한 오브젝트가 현재 없어서 임시명칭임)
    //[SerializeField] private Dictionary<System.Enum, List<GameObject>> prefabs = new Dictionary<System.Enum, List<GameObject>>();      // 적 오브젝트 풀링 (적에 대한 오브젝트가 현재 없어서 임시명칭임)
    [SerializeField] private List<GameObject> uiPrefabs = new List<GameObject>();             // 적 오브젝트 풀링 (적에 대한 오브젝트가 현재 없어서 임시명칭임)
    [SerializeField] private List<GameObject> dungeonPrefabs = new List<GameObject>();        // 던전 오브젝트 풀링 (던전에 대한 오브젝트가 현재 없어서 임시명칭임)  
    [SerializeField] private Dictionary<System.Enum, List<GameObject>> prefabsDictionary = new Dictionary<System.Enum, List<GameObject>>();


    /// <summary>
    /// 오브젝트 풀링의 크기(갯수)를 설정합니다.
    /// </summary>
    [SerializeField] private int poolSize = 20;     // 오브젝트 생성 갯수

    [SerializeField] private List<GameObject> poolList;           // 오브젝트 풀링 리스트

    public enum EnemyType
    {
        None = 0,
        Orc,
        Zombie,
        Skeleton,
        Slime,
        Goblin          // 기타 등등... (임시 명칭)
    }
    public enum UIType
    {
        None = 0,
        Button,
        Text,
        Image,
        Slider,
        Toggle          // 기타 등등... (임시 명칭)
    }


    [SerializeField] private Dictionary<EnemyType, List<GameObject>> poolDic = new Dictionary<EnemyType, List<GameObject>>();

    [SerializeField] private Transform enemyrespawnLocation;         // 적 생성 위치

    private void Awake()
    {
        //enemyrespawnLocation = GameObject.Find("EnemyRespawnLocation").transform;     // 적 생성 위치
    }

    private void Start()
    {
        // 딕셔너리 테스트용 Legacy
        //DictionaryCreateObject();                               // 딕셔너리 생성 (임시)

        // 오브젝트 첫 등록
        RegisterPool(EnemyType.Orc, prefabs[0]);
        RegisterPool(EnemyType.Zombie, prefabs[1]);
        RegisterPool(EnemyType.Skeleton, prefabs[2]);
        RegisterPool(EnemyType.Slime, prefabs[3]);
        RegisterPool(EnemyType.Goblin, prefabs[4]);
        
        /// UI 오브젝트 등록
        //RegisterPool(UIType.Button, uiPrefabs[0]);
        //RegisterPool(UIType.Text, uiPrefabs[1]);
        //RegisterPool(UIType.Image, uiPrefabs[2]);
        //RegisterPool(UIType.Slider, uiPrefabs[3]);
        //RegisterPool(UIType.Toggle, uiPrefabs[4]);
    }

    /// <summary>
    /// 오브젝트 등록 기능
    /// </summary>
    public void RegisterPool<T>(T type, GameObject prefab, int poolSize2 = -1) where T : System.Enum
    {
        // 받은 타입이 없는 타입이면 등록
        if (!prefabsDictionary.ContainsKey(type))
        {
            prefabsDictionary.Add(type, new List<GameObject>());
        }

        // 첫할당시 사이즈 설정
        if (poolSize2 == -1)
        {
            poolSize2 = poolSize;
        }

        // 비활성화 상태 풀 생성 오브젝트 추가.
        List<GameObject> pool = new List<GameObject>();
        for (int i = 0; i < poolSize2; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }

        // 딕셔너리에 추가
        prefabsDictionary[type].AddRange(pool);
    }

    public GameObject GetObject<T>(T type) where T : System.Enum
    {
        // 해당 타입이 존재하면 오브젝트 활성화 및 반환
        if (prefabsDictionary.ContainsKey(type))
        {
            List<GameObject> pool = prefabsDictionary[type];

            foreach (GameObject obj in pool)
            {
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }
        }

        // 해당 타입이 존재하지 않으면 타입 추가 및 오브젝트 반환
        GameObject prefab = prefabs.Find(go => go.CompareTag(type.ToString()));
        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(true);

        return newObj;
    }

    public void ReturnObject<T>(T type, GameObject obj) where T : System.Enum
    {
        if (prefabsDictionary.ContainsKey(type))
        {
            obj.SetActive(false);
            prefabsDictionary[type].Add(obj);
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
