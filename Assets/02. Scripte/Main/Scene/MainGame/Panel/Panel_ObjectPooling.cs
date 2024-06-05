using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;       // List사용을 위해 추가.

public class Panel_ObjectPooling : MonoBehaviour
{
    // 1. 오브젝트 풀링 만들기

    /// <summary>
    /// 하이어라키에 생성된 오브젝트가 없어서 현재 임시 명칭입니다.
    /// </summary>
    [SerializeField] private GameObject enemyPool;                   // 적 오브젝트 풀링 (적에 대한 오브젝트가 현재 없어서 임시명칭임)
    [SerializeField] private GameObject alliesPool;                  // 아군 오브젝트 풀링 (아군에 대한 오브젝트가 현재 없어서 임시명칭임)
    [SerializeField] private GameObject dungeonPool;                 // 던전 오브젝트 풀링 (던전에 대한 오브젝트가 현재 없어서 임시명칭임)  

    /// <summary>
    /// 오브젝트 풀링의 크기(갯수)를 설정합니다.
    /// </summary>
    [SerializeField] private int poolSize = 20;     // 오브젝트 생성 갯수

    private List<GameObject> poolList;           // 오브젝트 풀링 리스트

    public enum EnemyType
    {
        None = 0,
        Orc,
        Zombie,
        Skeleton,
        Slime,
        Goblin          // 기타 등등... (임시 명칭)
    }

    private Dictionary<EnemyType, List<GameObject>> poolDic;

    private void Start()
    {
        poolList = new List<GameObject>();       // 초기화

        for(int i = 0; i < poolSize; i++)
        {
            //실전용
            //GameObject targetObject = Instantiate(enemyPool);     // 우선 한개만 넣고 3가지 다 넣을떄는 함수로 넣자
            //targetObject.SetActive(false);                        // 비활성화
            //poolList.Add(targetObject);                           // 리스트에 추가

            //딕셔너리 테스트용
            DictionaryCreateObject();                               // 딕셔너리 생성 (임시)

            //테스트용
            //CreateObject();                                       // 오브젝트 생성 (임시)
        }
    }

    /// <summary>
    /// 오브젝트 생성 및 초기 자리를 세팅합니다.
    /// </summary>
    private void CreateObject()
    {
        GameObject createObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // 생성 설정
        createObj.transform.SetParent(gameObject.transform);
        createObj.name = "Enemy";
        createObj.transform.position = new Vector3(Screen.width + Random.Range(Screen.width * 0.05f, Screen.width * 0.1f), 
                                                    Screen.height / 2f + Random.Range((Screen.height / 2f) * -0.2f, (Screen.height / 2f) * 0.2f));   // 화면 범위 밖에서 생성 (매직 넘버는 개선 필요)
        createObj.transform.rotation = Quaternion.identity;
        createObj.transform.localScale = new Vector3(20f, 20f, 1f);
    }

    /// <summary>
    /// 딕셔너리용 오브젝트 생성 및 초기자리 세팅
    /// </summary>
    private void DictionaryCreateObject()
    {
        ObjectPoolingSetting();    

        GameObject createDicObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        createDicObject.name = "Enemy";
        createDicObject.tag = "Enemy";
        createDicObject.transform.SetParent(gameObject.transform);
        createDicObject.transform.position = new Vector3(Screen.width + Random.Range(Screen.width * 0.05f, Screen.width * 0.1f),
                                                    Screen.height / 2f + Random.Range((Screen.height / 2f) * -0.2f, (Screen.height / 2f) * 0.2f));   // 화면 범위 밖에서 생성 (매직 넘버는 개선 필요)
        createDicObject.transform.rotation = Quaternion.identity;
        createDicObject.transform.localScale = new Vector3(20f, 20f, 1f);


        poolDic.Add(EnemyType.Orc, new List<GameObject>());
    }

    /// <summary>
    /// 생성할 오브젝트 세팅
    /// </summary>
    private void ObjectPoolingSetting()
    {
        poolDic.Add(EnemyType.Orc, new List<GameObject>());
        poolDic.Add(EnemyType.Zombie, new List<GameObject>());
        poolDic.Add(EnemyType.Skeleton, new List<GameObject>());
        poolDic.Add(EnemyType.Slime, new List<GameObject>());
        poolDic.Add(EnemyType.Goblin, new List<GameObject>());
    }

    /// <summary>
    /// 오브젝트 가지고 오기 
    /// </summary>
    private GameObject GetObject()
    {
        foreach(GameObject obj in poolList)
        {
            // 오브젝트가 활성화되어 있지 않다면
            if(!obj.activeSelf)
            {
                obj.SetActive(true);        // 활성화

                return obj;
            }
        }

        GameObject newObj = Instantiate(enemyPool);     // 새로운 오브젝트 생성
        newObj.SetActive(true);

        return newObj;
    }

    // 오브젝트 반환하기
    private void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);        // 비활성화
    }
}
