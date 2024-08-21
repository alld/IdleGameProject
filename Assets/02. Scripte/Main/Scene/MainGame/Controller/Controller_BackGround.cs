using IdleGame.Data.Common.Event;
using IdleGame.Main.Scene.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            /// [캐시] 배경들을 담고 있는 부모객체입니다.
            /// </summary>
            [SerializeField]
            private RectTransform parent;

            /// <summary>
            /// [캐시] 실제 들어가 있는 배경 파츠들입니다.
            /// </summary>
            [HideInInspector]
            public List<RectTransform> list;

            /// <summary>
            /// [데이터] 배경이 움직이는 기본 속도입니다. 깊이에 따라 차등 적용해야됩니다.
            /// </summary>
            [Range(0, 100)]
            public float speed;

            /// <summary>
            /// [초기화] 자식들을 검색해서 bgList를 채웁니다.
            /// </summary>
            public void Init()
            {
                list = new List<RectTransform>();

                for (int i = 0; i < parent.childCount; i++)
                {
                    list.Add(parent.GetChild(i) as RectTransform);
                }
            }
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

        /// <summary>
        /// [상태] 현재 스크롤링 상태인지를 나타냅니다.
        /// </summary>
        private bool _isScrolling = false; // 스크롤링 상태

        private void Start()
        {
            Panel_MainGameScene.Event.RegisterEvent<float>(eSceneEventType_MainGame.Act_BGMoveStart, Logic_BackGroundMoveStart);
            Panel_MainGameScene.Event.RegisterEvent(eSceneEventType_MainGame.Act_BGMoveStop, Logic_BackGroundMoveStop);

            Logic_Init();
            Logic_BackGroundMoveStart(5);
        }


        /// <summary>
        /// [기능] 컨트롤러를 초기화시킵니다.
        /// </summary>
        public void Logic_Init()
        {
            for (int i = 0; i < _bg.Length; i++)
            {
                _bg[i].Init();
            }
        }

        /// <summary>
        /// [기능] 배경을 움직이게 합니다.
        /// </summary>
        private void Logic_BackGroundMoveStart(float m_speed)
        {
            _currentTargetSpeed = m_speed;

            if (!_isScrolling)
            {
                _isScrolling = true;
                _co_MoveRoutine = StartCoroutine(Logic_BgMoveRoutine());
            }
        }

        /// <summary>
        /// [기능] 배경을 실제 움직이게 하는 루틴입니다.
        /// </summary>
        private IEnumerator Logic_BgMoveRoutine()
        {
            while (_isScrolling)
            {
                for (int i = 0; i < _bg.Length; i++)
                {
                    for (int j = 0; j < _bg[i].list.Count; j++)
                    {
                        _bg[i].list[j].anchoredPosition += Vector2.left * _currentTargetSpeed * _bg[i].speed * Time.deltaTime;

                        if (_bg[i].list[j].anchoredPosition.x < -_bg[i].list[j].rect.width)
                        {
                            Logic_MoveBackgroundToEnd(i, j);
                        }
                    }
                    yield return null;
                }
            }

            _co_MoveRoutine = null;
        }

        /// <summary>
        /// [기능] 첫번째 배경을 가장 뒤로 넘깁니다. 
        /// </summary>
        private void Logic_MoveBackgroundToEnd(int m_layer, int m_index)
        {
            var bgList = _bg[m_layer].list;

            float lastBackgroundX = bgList[(bgList.Count - 1) % bgList.Count].anchoredPosition.x;
            bgList[m_index].anchoredPosition = new Vector2(lastBackgroundX + bgList[m_index].rect.width, bgList[m_index].anchoredPosition.y);

            Logic_RearrangeArray(m_layer);
        }

        /// <summary>
        /// [기능] 리스트의 첫번째 값을 맨 뒤로 옮깁니다. 
        /// </summary>
        private void Logic_RearrangeArray(int m_layer)
        {
            var moveBg = _bg[m_layer].list[0];

            for (int i = 1; i < _bg[m_layer].list.Count; i++)
            {
                _bg[m_layer].list[i - 1] = _bg[m_layer].list[i];
            }

            _bg[m_layer].list[_bg[m_layer].list.Count - 1] = moveBg;
        }

        /// <summary>
        /// [기능] 배경의 움직임을 멈춥니다.
        /// </summary>
        private void Logic_BackGroundMoveStop()
        {
            if (_isScrolling)
            {
                _isScrolling = false;
                StopCoroutine(_co_MoveRoutine);
                _co_MoveRoutine = null;
            }
        }
    }
}