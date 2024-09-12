using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IdleGame.Main.Scene.AutoBuild
{
    public class Panel_UpdatePopup : MonoBehaviour
    {
        [SerializeField]
        private GameObject _graphic;

        [SerializeField]
        private TMP_Text _t_context;

        /// <summary>
        /// [기능] 팝업창을 엽니다.
        /// </summary>
        public void Logic_OpenPopup(Dictionary<int, string> data, int curVersion)
        {
            _graphic.SetActive(true);

            string result = string.Empty;

            foreach (var item in data)
            {
                if (item.Key < curVersion)
                    return;

                result += "\n\n";
                result += "---------\n";
                result += $"Version :: {item.Key}\n";
                result += $"info :: \n";
                result += item.Value;
                result += "\n---------\n";
            }

            _t_context.text = result;
        }

        /// <summary>
        /// [기능] 팝업창을 닫습니다.
        /// </summary>
        public void Logic_ClosePopup()
        {
            _graphic.SetActive(false);
        }



        #region 콜백함수

        /// <summary>
        /// [버튼콜백] 팝업창에 닫기버튼이 클릭된 경우 호출됩니다. 
        /// </summary>
        public void OnClickClosePopup()
        {
            Logic_ClosePopup();
        }
        #endregion

    }
}