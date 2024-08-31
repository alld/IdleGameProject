using DG.Tweening;
using IdleGame.Core;
using IdleGame.Core.Unit;
using IdleGame.Data.DataTable;
using IdleGame.Main.GameLogic;
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
        [Header("스테이지 정보")]
        [SerializeField]
        private Graphic_Text _t_stageInfo;

        /// <summary>
        /// [캐시] 일반 스테이지 표시 오브젝트 박스입니다.
        /// </summary>
        [Header("일반 스테이지")]
        [SerializeField]
        private GameObject _obj_normal;

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
        /// [캐시] 일반 스테이지 표시 오브젝트 박스입니다.
        /// </summary>
        [Header("보스 스테이지")]
        [SerializeField]
        private GameObject _obj_boss;

        /// <summary>
        /// [캐시] 보스 유닛을 캐시처리합니다.
        /// </summary>
        private Base_Unit _unit_boss;

        /// <summary>
        /// [캐시] 보스 타이머입니다.
        /// </summary>
        [SerializeField]
        private Image _i_timer;

        /// <summary>
        /// [캐시] 보스의 체력 바입니다.
        /// </summary>
        [SerializeField]
        private Image _i_bossHpBar;

        [Header("other")]
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
            Logic_Clear();
            if (m_level != _maxWave)
            {
                Logic_ChangeType(false);

                Logic_SetNormalStage(m_level);
            }
            else
            {
                Logic_ChangeType(true);

                Logic_SetBossStage(m_level);
            }
        }

        /// <summary>
        /// [기능] 현재 진행중인 웨이브 타입에 따라서 스테이지 정보를 변경합니다.
        /// </summary>
        public void Logic_ChangeType(bool m_isBoss)
        {
            _obj_boss.SetActive(m_isBoss);
            _obj_normal.SetActive(!m_isBoss);

            if (m_isBoss)
            {
                if (_unit_boss != null)
                    _unit_boss._onBroadcastDamaged -= Logic_UpdateBossHpbar;

                _unit_boss = Panel_StageManager.Unit_Monsters[0];
                _unit_boss._onBroadcastDamaged += Logic_UpdateBossHpbar;
            }
        }

        /// <summary>
        /// [기능] 일반 스테이지 정보를 갱신합니다.
        /// </summary>
        public void Logic_SetNormalStage(int m_level)
        {
            _i_progressBar.DOKill();

            float fillPoint = (float)m_level / (float)_maxWave;
            _i_progressBar.DOFillAmount(fillPoint, 0.5f);
            _i_point.rectTransform.anchoredPosition = new Vector2(_rect_pointparent.sizeDelta.x * fillPoint - (_rect_pointparent.sizeDelta.x / 2), _i_point.rectTransform.anchoredPosition.y);
        }

        /// <summary>
        /// [기능] 보스 스테이지 정보를 갱신합니다.
        /// </summary>
        public void Logic_SetBossStage(int m_level)
        {
            _i_timer.DOKill();

            _i_timer.fillAmount = 1;
            _i_timer.DOFillAmount(0, Library_DataTable.stage[GameManager.Main.stage.currnetStageID].boss_battletime)
                .OnComplete(() =>
                {

                });


            Logic_UpdateBossHpbar();
        }

        /// <summary>
        /// [기능] 보스 체력 게이지를 갱신합니다. 
        /// </summary>
        public void Logic_UpdateBossHpbar()
        {
            _i_bossHpBar.DOKill();
            _i_bossHpBar.DOFillAmount((_unit_boss.ability.hp / _unit_boss.ability.maxHp).quotient, 0.3f);
        }

        /// <summary>
        /// [기능] 사용한 데이터를 초기화합니다.
        /// </summary>
        public void Logic_Clear()
        {
            _i_bossHpBar.DOKill();
            _i_timer.DOKill();
            _i_progressBar.DOKill();

        }
    }
}