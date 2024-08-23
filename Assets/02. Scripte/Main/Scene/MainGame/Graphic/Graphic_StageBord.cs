using UnityEngine;
using UnityEngine.UI;
namespace IdleGame.Main.Scene.Main.UI
{
    /// <summary>
    /// [기능] 현재 스테이지 정보를 가시적으로 표현해주는 스테이지보드입니다.
    /// </summary>
    public class Graphic_StageBord : MonoBehaviour
    {
        [SerializeField]
        private Image _i_progress;


        private void Awake()
        {
            GameManager.Main.Logic_RegisterStageBord(this);
        }

    }
}