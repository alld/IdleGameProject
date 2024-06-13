using System;
using UnityEngine;

public class Data_Pool
{
    public Enum Type { get; private set; }
    public string Name { get; private set; }
    private GameObject gameObject;
    
    public Data_Pool(Enum type, GameObject prefab)
    {
        Name = prefab.name + "_" + type;
        Type = type;
        gameObject = prefab;
        gameObject.SetActive(false);
    }

    // 활성화 되어있으면 true, 비활성화 시 false 반환
    public bool IsActive => gameObject.activeSelf;

    public void SetActive(bool isBool) => gameObject.SetActive(isBool);
    public Data_Pool Clone() => new Data_Pool(Type, gameObject);
}

public enum EnemyType
{
    None = 0,
    Orc,
    Zombie,
    Skeleton,
    Slime,
    Goblin          // 기타 등등... (임시 명칭)
}

public enum UIType
{
    None = 0,
    Button,
    Text,
    Image,
    Slider,
    Toggle          // 기타 등등... (임시 명칭)
}

public enum DungeonType
{
    Cave = 0,
    Sea,
    Hell,
    Forest,
    Void,
    Desert          // 기타 등등... (임시 명칭)
}