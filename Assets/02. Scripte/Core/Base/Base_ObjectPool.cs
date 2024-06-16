using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

// 이거 New타입 
public class Base_ObjectPool : MonoBehaviour
{
    /// <summary>
    /// 오브젝트 풀링을 담는 데이터
    /// </summary>
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();     
    
   
}
