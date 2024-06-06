using UnityEditor;
using UnityEngine;

namespace IdleGame.IdleEditor
{
    [CustomEditor(typeof(Graphic_Text))]
    public class Editor_Text : Editor
    {
        private Graphic_Text text;


        public override void OnInspectorGUI()
        {
            text = (Graphic_Text)target;

            base.OnInspectorGUI();


            GUILayout.Space(45f);
            if (GUILayout.Button("텍스트 갱신하기"))
            {
                text.UpdateText();
            }
        }
    }
}