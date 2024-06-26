using IdleGame.Core.Unit;
using UnityEngine;

public class Enemy : Character
{
    public Enemy(int index) : base(eUnitTpye.Enemy, index)
    {
        // Enemy 특정 초기화
    }

    public void Start()
    {
       // Logic_Init();       // 유닛 초기화 
    }
}
