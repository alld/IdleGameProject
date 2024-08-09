using IdleGame.Data.Common.Event;
using IdleGame.Main.Scene.Main;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace IdleGame.Main.UI
{
    /// <summary>
    /// [기능] 게임 배경을 변경하거나 움직이도록 조작합니다.
    /// </summary>
    public class Controller_BackGround : MonoBehaviour
    {
        [System.Serializable]
        public struct Data_BackGroundComponenet
        {
            /// <summary>
            /// [캐시] 배경을 담고있는 스크롤뷰입니다.
            /// </summary>
            public ScrollRect scroll;

            /// <summary>
            /// [데이터] 배경이 움직이는 기본 속도입니다. 깊이에 따라 차등 적용해야됩니다.
            /// </summary>
            [Range(1, 5)]
            public float speed;
        }

        /// <summary>
        /// [데이터] 배경 그룹에 대한 정보를 담고 있습니다.
        /// </summary>
        [SerializeField]
        private Data_BackGroundComponenet[] _bg;

        /// <summary>
        /// [캐시] 배경을 움직이는 로직을 담습니다.
        /// </summary>
        private Coroutine _co_MoveRoutine;

        /// <summary>
        /// [데이터] 현재 배경이 움직이는 속도입니다.
        /// </summary>
        private float _currentTargetSpeed = 0;

        private void Start()
        {
            Panel_MainGameScene.Event.RegisterEvent<float>(eSceneEventType_MainGame.Act_BGMoveStart, Logic_BackGroundMoveStart);
            Panel_MainGameScene.Event.RegisterEvent(eSceneEventType_MainGame.Act_BGMoveStop, Logic_BackGroundMoveStop);
        }

        /// <summary>
        /// [기능] 배경을 움직이게 합니다.
        /// </summary>
        private void Logic_BackGroundMoveStart(float m_speed)
        {
            _currentTargetSpeed = m_speed;

            if (_co_MoveRoutine != null)
                return;

            _co_MoveRoutine = StartCoroutine(Logic_BgMoveRoutine());
        }

        /// <summary>
        /// [기능] 배경을 실제 움직이게 하는 루틴입니다.
        /// </summary>
        private IEnumerator Logic_BgMoveRoutine()
        {
            while (true)
            {
                yield return null;

                if (true)
                    break;
            }

            _co_MoveRoutine = null;
        }

        /// <summary>
        /// [기능] 배경의 움직임을 멈춥니다.
        /// </summary>
        private void Logic_BackGroundMoveStop()
        {
            if (_co_MoveRoutine == null)
                return;

            StopCoroutine(_co_MoveRoutine);
            _co_MoveRoutine = null;
        }
    }
}