using DG.Tweening;
using IdleGame.Core.Procedure;
using IdleGame.Data.Common.Log;
using IdleGame.Data.Popup;
using UnityEngine;

namespace IdleGame.Core.Popup
{
    /// <summary>
    /// [기능] 기본 팝업 기능을 베이스로 삼으면서 애니메이션 동작이 가미된 팝업입니다. 
    /// </summary>
    public abstract class Base_AnimationPopup : Base_Popup
    {
        /// <summary>
        /// [캐시&설정] 애니메이션 팝업에 적용되는 구성요소들을 담고 있습니다.
        /// </summary>
        [Header("BaseAnimation")]
        [SerializeField]
        private Data_PopupAniComponent _aniComponent;

        private RectTransform pv_target = null;
        /// <summary>
        /// [캐시] 팝업 애니메이션의 대상입니다.
        /// </summary>
        private RectTransform obj_target
        {
            get
            {
                if (pv_target == null)
                {
                    Base_Engine.Log.Logic_PutLog(new Data_Log(Data_ErrorType.Warning_InsufficientSetting, tag));
                    if (!transform.GetChild(0).TryGetComponent(out pv_target)) return null;
                }
                return pv_target;
            }
        }

        /// <summary>
        /// [상태] 현재 애니메이션중인지를 체크합니다. 중복 실행을 방지합니다. 
        /// </summary>
        private bool isAnimating = false;

        /// <summary>
        /// [상태] 현재 팝업 애니메이션이 작동중인지를 확인합니다.. 
        /// </summary>
        /// <returns>true : 작동중 <br>false : 대기상태</br></returns>
        protected bool CheckIsAnimating() => isAnimating;


        #region 기본 팝업 로직
        protected sealed override void Logic_Open_Base()
        {
            if (isAnimating == false && _isShowPopup == false)
            {
                isAnimating = true;
                base.Logic_Open_Base();

                Logic_OpenAnimation();
            }
        }

        protected sealed override void Logic_Close_Base()
        {
            if (isAnimating == false && _isShowPopup == true)
            {
                isAnimating = true;
                Logic_CloseAnimation();
            }
        }

        protected sealed override void Logic_Close_Callback()
        {
            if (isAnimating == false && _isShowPopup == true)
            {
                isAnimating = true;
                Logic_CloseAnimation();

                _callback_OK?.Invoke();
            }
        }

        /// <summary>
        /// [기능] 팝입이 열린 이후 애니메이션 연출까지 끝나고나서 호출되는 함수입니다. 
        /// <br>애니메이션이 끝난직후 특정 로직을 실행시키고자 한다면 해당 함수를 상속하십시오.</br>
        /// </summary>
        protected virtual void Logic_Open_CompleteCallback_Custom() { }

        /// <summary>
        /// [제한] 시작 타이밍을 보장할 수 없기때문에 Logic_Open_CompleteCallback_Custom을 사용해야합니다. 
        /// </summary>
        protected sealed override void Logic_OpenComplate_Custom() { }

        /// <summary>
        /// [기능] 현재 타입에 알맞는 분기의 애니메이션 함수를 실행시킵니다. 
        /// </summary>
        private void Logic_CloseAnimation()
        {
            switch (_aniComponent.aniType_out)
            {
                case Data_PopupAniComponent.eAnimationType_Out.FadeOut:
                    FadeOut();
                    break;
                case Data_PopupAniComponent.eAnimationType_Out.RightOut:
                    RightOutMove();
                    break;
                case Data_PopupAniComponent.eAnimationType_Out.Leftout:
                    LeftOutMove();
                    break;
                case Data_PopupAniComponent.eAnimationType_Out.UpOut:
                    UpOutMove();
                    break;
                case Data_PopupAniComponent.eAnimationType_Out.DownOut:
                    DownOutMove();
                    break;
                case Data_PopupAniComponent.eAnimationType_Out.BounceOut:
                    LeavingScale();
                    break;
                case Data_PopupAniComponent.eAnimationType_Out.None:
                default:
                    isAnimating = false;
                    base.Logic_Close_Base();
                    break;
            }
        }

        /// <summary>
        /// [기능] 현재 타입에 알맞는 분기의 애니메이션 함수를 실행시킵니다. 
        /// </summary>
        private void Logic_OpenAnimation()
        {
            switch (_aniComponent.aniType_in)
            {
                case Data_PopupAniComponent.eAnimationType_In.FadeIn:
                    FadeIn();
                    break;
                case Data_PopupAniComponent.eAnimationType_In.LeftIn:
                    LeftInMove();
                    break;
                case Data_PopupAniComponent.eAnimationType_In.RightIn:
                    RightInMove();
                    break;
                case Data_PopupAniComponent.eAnimationType_In.UpIn:
                    UpInMove();
                    break;
                case Data_PopupAniComponent.eAnimationType_In.DownIn:
                    DownInMove();
                    break;
                case Data_PopupAniComponent.eAnimationType_In.DropIn:
                    DropInDown();
                    break;
                case Data_PopupAniComponent.eAnimationType_In.BounceIn:
                    AppearScale();
                    break;
                case Data_PopupAniComponent.eAnimationType_In.None:
                default:
                    isAnimating = false;
                    Logic_Open_CompleteCallback_Custom();
                    break;
            }
        }
        #endregion


        #region 애니메이션 연출 부분
        // 가시적인 부분이고 수정할것같지않아 해당내용에대한 주석을 생략합니다. 

        #region FadeInFadeOut
        private void FadeIn()
        {
            obj_target.DOKill();

            if (obj_target.TryGetComponent<CanvasGroup>(out CanvasGroup canvas) == false)
            {
                Base_Engine.Log.Logic_PutLog(new Data_Log(Data_ErrorType.Warning_InsufficientSetting, tag));
                canvas = obj_target.gameObject.AddComponent<CanvasGroup>();
            }

            canvas.alpha = 0.0f;
            canvas.DOFade(1, _aniComponent.duration_appearance).OnComplete(() =>
            {
                isAnimating = false;
                Logic_Open_CompleteCallback_Custom();
            });
        }

        private void FadeOut()
        {
            obj_target.DOKill();

            if (obj_target.TryGetComponent<CanvasGroup>(out CanvasGroup canvas) == false)
            {
                Base_Engine.Log.Logic_PutLog(new Data_Log(Data_ErrorType.Warning_InsufficientSetting, tag));
                canvas = obj_target.gameObject.AddComponent<CanvasGroup>();
            }

            canvas.alpha = 1.0f;
            canvas.DOFade(0, _aniComponent.duration_extinction).OnComplete(() =>
            {
                isAnimating = false;
                base.Logic_Close_Base();
            });
        }
        #endregion

        #region CenterScale
        private void AppearScale()
        {
            obj_target.DOKill();

            obj_target.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            obj_target.DOScale(1, _aniComponent.duration_appearance).OnComplete(() =>
            {
                isAnimating = false;
                Logic_Open_CompleteCallback_Custom();
            });
        }

        private void LeavingScale()
        {
            obj_target.localScale = Vector3.one;
            obj_target.DOScale(0.1f, _aniComponent.duration_extinction).OnComplete(() =>
            {
                isAnimating = false;
                base.Logic_Close_Base();
            });
        }
        #endregion

        #region LeftInRightOut
        private void LeftInMove()
        {
            obj_target.DOKill();

            obj_target.anchoredPosition = new Vector3(-obj_target.rect.width, 0);
            obj_target.DOAnchorPosX(0, _aniComponent.duration_appearance).OnComplete(() =>
            {
                isAnimating = false;
                Logic_Open_CompleteCallback_Custom();
            });
        }

        private void RightOutMove()
        {
            obj_target.DOKill();

            obj_target.anchoredPosition = Vector3.zero;
            obj_target.DOAnchorPosX(-obj_target.rect.width, _aniComponent.duration_extinction).OnComplete(() =>
            {
                isAnimating = false;
                base.Logic_Close_Base();
                Logic_Open_CompleteCallback_Custom();
            });
        }
        #endregion

        #region RightInLeftOut
        private void RightInMove()
        {
            obj_target.DOKill();

            obj_target.anchoredPosition = new Vector3(obj_target.rect.width, 0);
            obj_target.DOAnchorPosX(0, _aniComponent.duration_appearance).OnComplete(() =>
            {
                isAnimating = false;
                Logic_Open_CompleteCallback_Custom();
            });
        }

        private void LeftOutMove()
        {
            obj_target.DOKill();

            obj_target.anchoredPosition = Vector3.zero;
            obj_target.DOAnchorPosX(obj_target.rect.width, _aniComponent.duration_extinction).OnComplete(() =>
            {
                isAnimating = false;
                base.Logic_Close_Base();
            });
        }
        #endregion

        #region UpInUpOut
        private void UpInMove()
        {
            obj_target.DOKill();

            obj_target.anchoredPosition = new Vector3(0, obj_target.rect.height);
            obj_target.DOAnchorPosY(0, _aniComponent.duration_appearance).OnComplete(() =>
            {
                isAnimating = false;
                Logic_Open_CompleteCallback_Custom();
            });
        }

        private void UpOutMove()
        {
            obj_target.DOKill();

            obj_target.anchoredPosition = Vector3.zero;
            obj_target.DOAnchorPosY(obj_target.rect.height, _aniComponent.duration_extinction).OnComplete(() =>
            {
                isAnimating = false;
                base.Logic_Close_Base();
            });
        }
        #endregion

        #region DownInDownOut
        private void DownInMove()
        {
            obj_target.DOKill();

            obj_target.anchoredPosition = new Vector3(0, -obj_target.rect.height);
            obj_target.DOAnchorPosY(0, _aniComponent.duration_appearance).OnComplete(() =>
            {
                isAnimating = false;
                Logic_Open_CompleteCallback_Custom();
            });
        }

        private void DownOutMove()
        {
            obj_target.DOKill();

            obj_target.anchoredPosition = Vector3.zero;
            obj_target.DOAnchorPosY(-obj_target.rect.height, _aniComponent.duration_extinction).OnComplete(() =>
            {
                isAnimating = false;
                base.Logic_Close_Base();
            });
        }
        #endregion

        #region
        private void DropInDown()
        {
            obj_target.DOKill();

            obj_target.anchoredPosition = new Vector3(0, obj_target.rect.height);
            obj_target.DOAnchorPosY(0, _aniComponent.duration_appearance * 2f).OnComplete(() =>
            {
                isAnimating = false;
                Logic_Open_CompleteCallback_Custom();
            }).SetEase(Ease.OutBounce);
        }
        #endregion

        #endregion

    }
}