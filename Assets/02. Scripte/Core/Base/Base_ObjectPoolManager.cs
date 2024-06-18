using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Log;
using System.Collections.Generic;
using UnityEngine;

public class Base_ObjectPoolManager : MonoBehaviour
{
    private Dictionary<GameObject, Base_ObjectPool> pools = new Dictionary<GameObject, Base_ObjectPool>();

    public static Base_ObjectPoolManager Instance;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void CreatePool(GameObject prefab, int initialPoolSize, int betweenPoolSize)
    {
        Base_ObjectPool pool = new Base_ObjectPool(prefab, initialPoolSize, betweenPoolSize);
        pools.Add(prefab, pool);

        Base_Engine.Log.Logic_PutLog(new Data_Log("딕셔너리에 저장 되었습니다. : " + pools.Keys + " | " + pools.Values));
        Debug.Log("딕셔너리에 저장 되었습니다. : " + pools.Keys + " | " + pools.Values);
    }

    public GameObject GetPool(GameObject prefab)
    { 
        if(pools.ContainsKey(prefab))
        {
            return pools[prefab].GetObject();
        }
        else
        {
            Base_Engine.Log.Logic_PutLog(new Data_Log("키 값이 존재하지 않습니다. : " + prefab.name, Data_ErrorType.Error_DataLoadFailed));

            return null;
        }
    }

    /// <summary>
    /// 오브젝트 풀링 해제
    /// </summary>
    /// <param name="obj"></param>
    public void Release(GameObject obj)
    {
        foreach(var pool in pools.Values)
        {
            // 해제 가능한 오브젝트인지 확인 후 해제
            if(pool.CanRelease(obj))
            {
                pool.ReturnObject(obj);

                return;
            }
        }

        Base_Engine.Log.Logic_PutLog(new Data_Log("해제할 수 없는 오브젝트입니다. : " + obj.name, Data_ErrorType.Error_DataLoadFailed));
    }

}
