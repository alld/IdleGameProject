using IdleGame.Core.Panel;
using IdleGame.Core.Procedure;
using UnityEngine;

namespace IdleGame.Main.UI
{
    /// <summary>
    /// [기능] 특정 재화를 보여주는 디스플레이 패널입니다.
    /// </summary>
    public class Graphic_CurrencyDisplay : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _rect;

        [SerializeField]
        private Panel_CurrencyManager.Data_DisplayComponent _dc;


        private void Start()
        {
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, _rect.position);

            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y));
            _dc.endPoint = worldPoint;

            Base_Engine.Currency.Logic_SetCurrencyType(_dc.type);
            Base_Engine.Currency.Logic_RegisterPanel(_dc);
        }

        private void OnDisable()
        {
            Base_Engine.Currency.Logic_RemoveCurrencyType(_dc.type);
        }

        private void OnEnable()
        {
            Base_Engine.Currency.Logic_SetCurrencyType(_dc.type);
            Base_Engine.Currency.Logic_RegisterPanel(_dc);
        }
    }
}