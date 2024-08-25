using DG.Tweening;
using IdleGame.Core;
using IdleGame.Main.UI;
using UnityEngine;
using UnityEngine.UI;
namespace IdleGame.Main.Scene.Main.UI
{
    /// <summary>
    /// [기능] 현재 스테이지 정보를 가시적으로 표현해주는 스테이지보드입니다.
    /// </summary>
    public class Graphic_StageBord : MonoBehaviour
    {
        /// <summary>
        /// [캐시] 스테이지에 대한 정보를 표시하는 텍스트입니다.
        /// </summary>
        [SerializeField]
        private Graphic_Text _t_stageInfo;

        /// <summary>
        /// [캐시] 스테이지 진행 표시 바입니다.
        /// </summary>
        [SerializeField]
        private Image _i_progressBar;

        /// <summary>
        /// [캐시] 스테이지가 몇개로 구분되는지 표시되는 그리드 프레임입니다. 
        /// </summary>
        [SerializeField]
        private Image _i_gridFrame;

        /// <summary>
        /// [캐시] 스테이지가 현재 진행중인곳을 표시해주는 포인트 이미지입니다.
        /// </summary>
        [SerializeField]
        private Image _i_point;

        /// <summary>
        /// [캐시] 포인트의 위치를 잡기위한 부모 오브젝트를 캐시합니다. 
        /// </summary>
        [SerializeField]
        private RectTransform _rect_pointparent;

        /// <summary>
        /// [캐시] 현재 스테이지에 적용되고 있는 속성정보를 나타내는 아이콘 리스트입니다. 
        /// </summary>
        [SerializeField]
        private Item_StageBuff[] _buffs;

        /// <summary>
        /// [데이터] 최대 레벨을 나타냅니다. 
        /// </summary>
        private int _maxWave = 0;

        private void Awake()
        {
            GameManager.Main.Logic_RegisterStageBord(this);
        }

        public void Logic_NewStageSetting(int m_maxWave)
        {
            _maxWave = m_maxWave - 1;
        }

        public void Logic_SetLevel(int m_level)
        {
            _t_stageInfo.SetText($"{m_level} - 웨이브 (스테이지 정보없음)");

            _i_progressBar.DOKill();

            float fillPoint = (float)m_level / (float)_maxWave;
            _i_progressBar.DOFillAmount(fillPoint, 0.5f);
            _i_point.rectTransform.anchoredPosition = new Vector2(_rect_pointparent.sizeDelta.x * fillPoint - (_rect_pointparent.sizeDelta.x / 2), _i_point.rectTransform.anchoredPosition.y);
        }

    }
}