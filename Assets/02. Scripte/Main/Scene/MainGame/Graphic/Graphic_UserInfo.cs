using DG.Tweening;
using IdleGame.Core;
using IdleGame.Core.Procedure;
using IdleGame.Data;
using IdleGame.Data.Common.Event;
using UnityEngine;
using UnityEngine.UI;
namespace IdleGame.Main.Scene.Main.UI
{
    public class Graphic_UserInfo : MonoBehaviour
    {
        [SerializeField] private Image i_expImage;

        [SerializeField] private Graphic_Text t_level;

        private void Start()
        {
            Base_Engine.Event.RegisterEvent(eGlobalEventType.On_UpdateExp, SetLevel);

            SetLevel();
        }



        public void SetLevel()
        {
            float fill = 1.0f;
            fill = Global_Data.Player.cur_Exp / 100f;

            i_expImage.DOKill();
            i_expImage.DOFillAmount(fill, 0.5f);

            t_level.SetText(Global_Data.Player.level.ToString());
        }

    }
}