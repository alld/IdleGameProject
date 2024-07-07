using IdleGame.Data;
using UnityEngine;

namespace IdleGame.Core.EditorSetting
{
    public class Panel_EditorSetting : MonoBehaviour
    {
#if DevelopeMode

        public bool editorLog = false;

        private void Update()
        {
            if (Global_Data.Editor.unityLog != editorLog)
                Global_Data.Editor.unityLog = editorLog;
        }
    }

#endif
}