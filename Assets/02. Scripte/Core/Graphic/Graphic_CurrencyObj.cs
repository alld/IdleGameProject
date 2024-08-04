using DG.Tweening;
using IdleGame.Core.Procedure;
using UnityEngine;

namespace IdleGame.Core.Pool
{


    public class Graphic_CurrencyObj : Base_PoolObject
    {
        public SpriteRenderer s_body;

        /// <summary>
        /// [초기화] 보상 오브젝트의 이미지를 변경합니다.
        /// </summary>
        /// <param name="m_index"></param>
        public void Logic_SetData(int m_index, Vector3 m_startPosition)
        {
            Logic_Clear();
            transform.position = m_startPosition;
            s_body.sprite = Base_Engine.Reward.sprite_reward[m_index];
        }

        /// <summary>
        /// [초기화] 오브젝트 내용을 모두 비웁니다. 
        /// </summary>
        private void Logic_Clear()
        {
            transform.DOKill();
            transform.position = Vector3.zero;
            s_body.sprite = null;
        }


        #region 생명주기
        public override void Pool_Clear()
        {
            Logic_Clear();
        }

        #endregion
    }
}