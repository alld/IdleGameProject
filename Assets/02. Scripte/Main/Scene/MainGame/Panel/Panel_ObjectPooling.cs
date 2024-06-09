using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;       // List사용을 위해 추가.
using IdleGame.Core.Popup;
using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Log;
using UnityEngine.WSA;

public class Panel_ObjectPooling : MonoBehaviour
{
    // 1. 오브젝트 풀링 만들기

    /// <summary>
    /// 하이어라키에 생성된 오브젝트가 없어서 현재 임시 명칭입니다.
    /// </summary>
    [SerializeField] private List<GameObject> enemyPool = new List<GameObject>();             // 적 오브젝트 풀링 (적에 대한 오브젝트가 현재 없어서 임시명칭임)
    [SerializeField] private List<GameObject> alliesPool = new List<GameObject>();            // 아군 오브젝트 풀링 (아군에 대한 오브젝트가 현재 없어서 임시명칭임)
    [SerializeField] private List<GameObject> dungeonPool = new List<GameObject>();           // 던전 오브젝트 풀링 (던전에 대한 오브젝트가 현재 없어서 임시명칭임)  

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

    [SerializeField] private Dictionary<EnemyType, List<GameObject>> poolDic = new Dictionary<EnemyType, List<GameObject>>();

    [SerializeField] private Transform enemyrespawnLocation;         // 적 생성 위치

    private void Awake()
    {
        enemyrespawnLocation = GameObject.Find("EnemyRespawnLocation").transform;     // 적 생성 위치
    }

    private void Start()
    {
        //딕셔너리 테스트용
        DictionaryCreateObject();                               // 딕셔너리 생성 (임시)

    }

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

        foreach(GameObject obj in pool)
        {
            if(!obj.activeSelf)
            {
                obj.SetActive(true);
                active++;

                // 최대갯수 넘길시 종료 (임시 : 20개)
                if(active > poolSize)
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
        for(int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPool[(int)type]);
            obj.SetActive(false);
            poolDic[type].Add(obj);
        }
    }

    // 오브젝트 반환하기
    private void ReturnObject(GameObject obj, EnemyType type)
    {
        if(poolDic.ContainsKey(type))
        {
            obj.SetActive(false);        // 비활성화
            poolDic[type].Add(obj);
        }
    }
}
