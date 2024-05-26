using IdleGame.Data.Base.Scene;
using System;
using UnityEngine.SceneManagement;

namespace IdleGame.Core.Module.Scene
{
    /// <summary>
    /// [모듈] 씬을 관리하는 매니저입니다. 씬이 변경되는 과정을 통제합니다.
    /// </summary>
    public class Module_SceneManager : Base_Module
    {
        /* TODO :: 
         * 씬의 전환, 로딩씬 호출 유무 등이 반영되어야합니다.
         * 해당 기능들은 임시로 구현되어있습니다.
         * 
         */

        /// <summary>
        /// [상태] 현재 적용중인 메인 씬을 나타냅니다. 
        /// </summary>
        private eSceneKind _currentScene = eSceneKind.None;

        /// <summary>
        /// [기능] 특정 씬으로 전환을 시도합니다.
        /// </summary>
        public void Logic_TryChangeScene(eSceneKind m_sceneKind)
        {
            Logic_TryChangeScene(m_sceneKind.ToString());
        }

        /// <summary>
        /// [기능] 특정 씬으로 전환을 시도합니다.
        /// </summary>
        public void Logic_TryChangeScene(string m_sceneName)
        {
            eSceneKind nextSceneKind = eSceneKind.None;

            if (Enum.TryParse(typeof(eSceneKind), m_sceneName, out object eScene))
                nextSceneKind = (eSceneKind)eScene;
            else
                return;

            SceneManager.LoadSceneAsync(m_sceneName);

            _currentScene = nextSceneKind;
        }

        /// <summary>
        /// [기능] 특정 씬을 제거하는 절차를 진행합니다.
        /// </summary>
        public void Logic_TryUnLoadScene(string m_sceneName)
        {
            SceneManager.LoadSceneAsync(m_sceneName);
        }

        /// <summary>
        /// [기능] 특정 씬을 추가하는 절차를 진행합니다.
        /// </summary>
        public void Logic_TryLoadScene(string m_sceneName, bool m_addScene = false)
        {
            SceneManager.LoadSceneAsync(m_sceneName);

        }
    }
}