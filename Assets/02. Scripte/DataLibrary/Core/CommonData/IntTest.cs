using UnityEngine;
using System.Collections;
using IdleGame.Data.Numeric;

public class IntTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    [Header("ExactInt")] public SimpleInt int1;
    [Header("ExactInt")] public SimpleInt int2;

    private void Start()
    {
        int1 = new SimpleInt(1000, true, 1);
        int2 = new SimpleInt(1000, true, 1);
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
        while (true)
        {   
            yield return new WaitForSeconds(DelayTime);
        }
    }

    [ContextMenu(("Add Function Test"))]
    public void AddFunctionTest()
    {
        int1 += int2;
        Debug.Log("AddFunctionTest : " + int1.ToString());
    }
    
    [ContextMenu(("Subtract Function Test"))]
    public void SubtractFunctionTest()
    {
        int1 -= int2;
        Debug.Log("SubtractFunctionTest : " + int1.ToString());
    }
    
    [ContextMenu(("Multiply Function Test"))]
    public void MultiplyFunctionTest()
    {
        int1 *= int2;
        Debug.Log("MultiplyFunctionTest : " + int1.ToString());
    }
    
    [ContextMenu(("Devide Function Test"))]
    public void DevideFunctionTest()
    {
        int1 /= int2;
        Debug.Log("MultiplyFunctionTest : " + int1.ToString());
    }
}