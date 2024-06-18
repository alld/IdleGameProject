using UnityEngine;
using System.Collections;
using IdleGame.Data.Numeric;
using System;


public class IntTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    [Header("ExactInt")] public ExactInt int1;

    private void Start()
    {
        int1 = new ExactInt(1000, true, 1);
    }

    [ContextMenu("ScaleToAlphabet Function Test")]
    public void ScaleToAlphabetFunctionTest()
    {
        for (int i = 0; i <= 1000; ++i)
        {
            int1.Scale = i;
            
            Debug.Log(i + " : " + int1.ToString());
        }
    }
    
    [ContextMenu("AutoFunctionTest")]
    public void AutoFunctionTest()
    {
        StartCoroutine(FunctionTestCorutine());
    }

    [SerializeField] private int AddCount = 1;
    [SerializeField] private int Amount = 1;
    [SerializeField] private float DelayTime = 1.0f;
    public IEnumerator FunctionTestCorutine()
    {
        ExactInt int2 = new ExactInt(Amount);
        while (true)
        {
            MultiplyFunctionTest(ref int2);
            
            yield return new WaitForSeconds(DelayTime);
        }
    }

    [ContextMenu(("Subtract Function Test"))]
    public void SubtractFunctionTest()
    {
        int1 -= new ExactInt(Amount);
        Debug.Log("SubtractFunctionTest : " + int1.ToString());
    }
    
    [ContextMenu(("Multiply Function Test"))]
    public void MultiplyFunctionTest(ref ExactInt int2)
    {
        int1 *= int2;
        Debug.Log("MultiplyFunctionTest : " + int1.ToString());
    }
    
    [ContextMenu(("Devide Function Test"))]
    public void DevideFunctionTest()
    {
        int1 /= new ExactInt(Amount);
        Debug.Log("MultiplyFunctionTest : " + int1.ToString());
    }
}