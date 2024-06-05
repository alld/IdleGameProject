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

    private void Start()
    {
        poolList = new List<GameObject>();       // 초기화

        for(int i = 0; i < poolSize; i++)
        {
            //GameObject targetObject = Instantiate(enemyPool);   // 우선 한개만 넣고 3가지 다 넣을떄는 함수로 넣자
            //targetObject.SetActive(false);                        // 비활성화
            //poolList.Add(targetObject);                           // 리스트에 추가
            CreateObject();                                     // 오브젝트 생성 (임시)
        }
    }

    private void CreateObject()
    {
        GameObject createObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        createObj.transform.SetParent(gameObject.transform);
        // 생성 위치 설정 (범위)
        createObj.name = "Enemy";
        createObj.transform.position = new Vector3(Screen.width + Random.Range(Screen.width * 0.05f, Screen.width * 0.1f), 
                                                    Screen.height / 2f + Random.Range((Screen.height / 2f) * -0.2f, (Screen.height / 2f) * 0.2f));   // 화면 범위 밖에서 생성 (매직 넘버는 개선 필요)
        createObj.transform.rotation = Quaternion.identity;
        createObj.transform.localScale = new Vector3(20f, 20f, 1f);
    }

    // 오브젝트 가지고 오기
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
