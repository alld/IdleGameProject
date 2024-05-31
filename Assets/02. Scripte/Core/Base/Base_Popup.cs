using DG.Tweening;
using IdleGame.Core.Procedure;
using IdleGame.Data.Base;
using IdleGame.Data.Common.Log;
using IdleGame.Data.Popup;
using UnityEngine;

namespace IdleGame.Core.Popup
{
    /// <summary>
    /// [기능] 팝업의 일반적인 기능을 가지고 있는 팝업입니다. 
    /// </summary>
    public abstract class Base_Popup : Base_Panel
    {
        /// <summary>
        /// [캐시&설정] 팝업을 구성하는 기본 요소들을 담고 있습니다.
        /// </summary>
        [Header("BasePopup")]
        [SerializeField] private Data_PopupComponent _component;

        /// <summary>
        /// [캐시] 그래픽에 할당된 캔버스 그룹입니다. 입력 제한 및 일부 페이드 연출등에 사용됩니다.
        /// </summary>
        private CanvasGroup _cg_graphicCG = null;

        /// <summary>
        /// [캐시] 확인 버튼이 눌려서 팝업창이 닫힌 경우 호출되는 콜백함수입니다. 
        /// </summary>
        protected Dele_Action _callback_OK;

        protected const float _Duration_fadeDim = 0.5f;

        /// <summary>
        /// [상태] 현재 팝업창이 활성화 되어있는지 유무를 나타냅니다. 
        /// </summary>
        [Tooltip("\nTrue : 팝업창이 활성화됨 \nFalse : 팝업창이 닫혀있음")]
        protected bool _isShowPopup = false;

        /// <summary>
        /// [상태] 팝업 매니저에서 관리되는 인덱스 번호입니다. 
        /// </summary>
        protected int _activeIndex = -1;

        protected sealed override void Logic_Init_Custom()
        {
            Logic_BasePopupSetting();

            Logic_Init_Base();
        }

        /// <summary>
        /// [초기화] 팝업의 구성을 초기 설정이 필요할때 해당 함수를 통해 구현할 수 있습니다.
        /// <br> Base가 할당된 함수는 상위의 상속된 함수의 실행이 가능한 보장되어야합니다. </br>
        /// </summary>
        protected virtual void Logic_Init_Base()
        {

        }

        /// <summary>
        /// [설정] 팝업 베이스에 필요한 기본 설정들을 지정합니다. 안정성 검증들이 들어갑니다. 
        /// </summary>
        private void Logic_BasePopupSetting()
        {
            // 역할 :: 팝업에는 무조건 내용에 해당하는 그래픽이 존재해야합니다.
            if (_component.obj_graphic == null)
            {
                Base_Engine.Log.Logic_PutLog(new Data_Log(Data_ErrorType.Warning_InsufficientSetting, _tag.tag));
                return;
            }
            else
            {
                _component.obj_graphic.SetActive(false);
            }

            TryGetComponent<CanvasGroup>(out _cg_graphicCG);

            // 역할 :: 암막 이미지가 존재 할 경우에 대한 처리
            if (_component.i_dim != null)
            {
                _component.i_dim.gameObject.SetActive(false);
                _component.i_dim.color = new Color(1, 1, 1, 0.5f);
            }


            // 역할 :: 입력 방지가 존재 할 경우에대한 처리
            if (_component.b_inputBlock != null)
            {
                _component.b_inputBlock.gameObject.SetActive(false);

                if (_component.usedInputClose)
                    _component.b_inputBlock.onClick.AddListener(OnClickClose_Base);
            }
        }

        /// <summary>
        /// [기능] 팝업창을 엽니다.
        /// </summary>
        protected virtual void Logic_Open_Base()
        {
            Logic_FadeInDim();

            _isShowPopup = true;

            Logic_OpenComplate();
        }

        /// <summary>
        /// [기능] 팝업창이 완전히 열린 후 호출됩니다.
        /// </summary>
        private void Logic_OpenComplate()
        {
            Logic_OpenComplate_Custom();
        }

        /// <summary>
        /// [기능] 팝업창이 완전히 열린 후 호출됩니다.
        /// </summary>
        protected virtual void Logic_OpenComplate_Custom()
        {

        }

        /// <summary>
        /// [기능] 팝업창을 닫습니다.
        /// </summary>
        protected virtual void Logic_Close_Base()
        {
            Logic_FadeOutDim();

            _isShowPopup = false;

            Logic_CloseComplate();
        }

        /// <summary>
        /// [기능] 팝업창을 닫은 후 콜백함수를 실행시킵니다. 
        /// </summary>
        protected virtual void Logic_Close_Callback()
        {
            Logic_Close_Base();

            _callback_OK?.Invoke();
        }

        /// <summary>
        /// [기능] 팝업창이 완전히 닫힌 후 호출됩니다.
        /// </summary>
        private void Logic_CloseComplate()
        {
            Logic_CloseComplate_Custom();

        }

        /// <summary>
        /// [기능] 팝업창이 완전히 닫힌 후 호출됩니다.
        /// </summary>
        protected virtual void Logic_CloseComplate_Custom()
        {

        }


        /// <summary>
        /// [기능] 암막 오브젝트를 페이드인으로 활성화 시킵니다.
        /// </summary>
        private void Logic_FadeInDim()
        {
            Logic_ActiveBlocker(true);
            if (_component.i_dim == null) return;

            _component.i_dim.gameObject.SetActive(true);
            _component.i_dim.DOKill();
            _component.i_dim.DOFade(0.5f, _Duration_fadeDim);
        }

        /// <summary>
        /// [기능] 암막 오브젝트를 페이드아웃으로 비활성화 시킵니다.
        /// </summary>
        private void Logic_FadeOutDim()
        {
            if (_component.i_dim == null) return;

            _component.i_dim.DOKill();
            _component.i_dim.DOFade(0, _Duration_fadeDim).OnComplete(() =>
            {
                _component.i_dim.gameObject.SetActive(false);
                Logic_ActiveBlocker(false);
            });
        }

        /// <summary>
        /// [기능] 입력 방지 기능의 활성화 유무를 지정합니다. 
        /// </summary>
        private void Logic_ActiveBlocker(bool m_active)
        {
            if (_component.b_inputBlock == null) return;

            _component.b_inputBlock.gameObject.SetActive(m_active);
        }

        #region 버튼콜백
        /// <summary>
        /// [버튼콜백] 팝업창을 닫습니다.
        /// </summary>
        public virtual void OnClickClose_Base()
        {
            Logic_Close_Base();
        }

        /// <summary>
        /// [버튼콜백] 팝업창이 닫히면서 콜백함수를 실행시킵니다. 
        /// </summary>
        public virtual void OnClickOk_Base()
        {
            Logic_Close_Callback();
        }
        #endregion
    }
}