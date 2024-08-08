using DG.Tweening;
using IdleGame.Core.Pool;
using IdleGame.Core.Procedure;
using IdleGame.Data.Common;
using IdleGame.Data.Numeric;
using IdleGame.Data.Pool;
using UnityEngine;

namespace IdleGame.Core.Panel
{
    /// <summary>
    /// [기능] 보상 절차를 진행하는 매니저입니다. 
    /// </summary>
    public class Panel_RewardManager : Base_ManagerPanel
    {
        /// <summary>
        /// [캐시] 보상을 지급할때 사용되는 재화별 오브젝트입니다.
        /// </summary>
        public Sprite[] sprite_reward;

        /// <summary>
        /// [기능] 보상을 발생시켜서 재화매니저로 보냅니다.
        /// </summary>
        public void Logic_SendCurrency(eCurrencyType m_type, long m_data, Vector3 m_startPosition)
        {
            Logic_SendCurrency(m_type, (ExactInt)m_data, m_startPosition);
        }

        /// <summary>
        /// [기능] 보상을 발생시켜서 재화매니저로 보냅니다.
        /// </summary>
        public void Logic_SendCurrency(eCurrencyType m_type, ExactInt m_data, Vector3 m_startPosition)
        {
            if (!Base_Engine.Currency.Logic_IsCurrentType(m_type))
            {
                Base_Engine.Currency.Logic_SetAddCurrency(m_type, m_data);
                return;
            }

            var obj = (Graphic_CurrencyObj)Base_Engine.Pool.Logic_GetObject(ePoolType.Currency, transform);
            obj.Logic_SetData((int)m_type, m_startPosition);
            obj.gameObject.SetActive(true);

            //TODO :: 해당 오브젝트가 비활성화 된 경우에, 정산에대한 보장을 해줄수 없음. 완료와, 시작타이밍에서 재화 반영 리스트든 대기열이 있어야함.
            obj.transform.DOMove(Base_Engine.Currency.Logic_GetEndPoint(m_type), 1)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Base_Engine.Currency.Logic_SetAddCurrency(m_type, m_data);
                    Base_Engine.Pool.Logic_PutObject(obj);
                });
        }


    }
}