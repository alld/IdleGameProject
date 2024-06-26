using IdleGame.Core.Unit;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

public class Player : Character
{
    // 플레이어 전용 기능
    // 1. 스킬 
    // 2. 스킬 트리
    // 3. 스킬 등록
    // 4. 스킬 사용
    // 5. 스킬 습득 확률 (등급에 따라 확률이 다름)
    // 6. 특성
    // 7. 동료 (단 동료는 Party로 분류)
    // 8. 카메라 (플레이어에게 초점을 맞춤)

    // 방치형 게임인데 왜 100레벨로 한정했을까? 나중에 물어보자.
    
    public int Experience { get; set; }              // 경험치 (필요할지 몰라서 우선 작성)
    public int NextLevelExperience { get; set; }     // 다음 레벨까지 필요한 경험치
    public int CurrentPlayableCharacter { get; set; }  // 선택한 내 캐릭터

    public Player(int index) : base(eUnitTpye.Player, index)
    {
    }

    protected override void Logic_Init_Custom()
    {
        base.Logic_Init_Custom();

        // 플레이어 초기화
        Level = 1;
        CharacterName = "Player1";
        Experience = 0;
        NextLevelExperience = 100;
        CurrentPlayableCharacter = 0;

    }

    public void Start()
    {
        //Logic_Init();       // 유닛 초기화 
    }

    private void Update()
    {
        transform.Translate(Vector2.right * 5f * Time.deltaTime);
    }
}