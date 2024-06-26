using IdleGame.Core.Unit;
using UnityEngine;

public class Party : Character
{
    // 동료 전용 기능
    // 1. 동료 얻는 방법 : 특정 스테이지 최초 클리어 시 획득.
    // 2. 동료 등록 : 동료 창에서 등록 가능
    // 3. 고유 스킬 : 동료 전용 스킬 (쿨타임마다 자동으로 사용)

    public Party(int index) : base(eUnitTpye.Party, index)
    {
        // Party 특정 초기화
    }

    public void Start()
    {
        //Logic_Init();       // 유닛 초기화 
    }
}