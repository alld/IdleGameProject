using IdleGame.Core;
using UnityEngine;

namespace IdleGame.Main.Scene.Main.InGame
{
    public class Panel_MainGame : Base_Panel
    {
        /// <summary>
        /// [캐시] 플레이어 유닛이 배치될 오브젝트위치입니다.
        /// </summary>
        public Transform playerGroup;

        /// <summary>
        /// [캐시] 적 유닛이 배치될 오브젝트 위치입니다.
        /// </summary>
        public Transform enemyGroup;

        /// <summary>
        /// [캐시] 바닥을 표시하는 기준입니다. 
        /// </summary>
        public BoxCollider2D plane;
    }
}