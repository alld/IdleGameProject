using UnityEngine;
using IdleGame.Data.Numeric;


public class IntTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private int data1;
    [SerializeField]
    private int data2;
    
    [SerializeField]
    private string dataSize1;
    [SerializeField]
    private string dataSize2;
    
    void Start()
    {
        IntData intData1 = new IntData(data1, dataSize1);
        IntData intData2 = new IntData(data2, dataSize2);

        intData1 += intData2;
        
        Debug.Log(intData1.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}