using UnityEditor;
using UnityEngine;

namespace IdleGame.IdleEditor
{
    public class Editor_TextCtrl : EditorWindow
    {
        private Editor_TextObject _data;

        [MenuItem("IdleGames/Tools/TextCtrl")]
        private static void Menu_TextController()
        {
            GetWindow<Editor_TextCtrl>("텍스트 컨트롤러");
        }

        private void OnEnable()
        {
            string[] guids = AssetDatabase.FindAssets("t:Editor_TextObject");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                _data = AssetDatabase.LoadAssetAtPath<Editor_TextObject>(path);
            }
        }


        private void OnGUI()
        {
            if (_data == null)
            {
                if (GUILayout.Button("기본 데이터가 존재하지 않습니다. 새로 생성하기"))
                {
                    Logic_CreatePrefabData();
                }
                return;
            }

            GUILayout.Label("텍스트 업데이트 기능", EditorStyles.boldLabel);

            if (GUILayout.Button("모든 텍스트 갱신하기"))
            {

            }

            GUILayout.Space(15f);

            if (GUILayout.Button("새로운 Graphic_Text 만들기"))
            {
                Logic_NewCreateGraphicText();
            }
        }


        private void Logic_NewCreateGraphicText()
        {
            if (_data == null || _data.prefab == null)
            {
                Debug.LogError("프리팹 할당이 이루어지지 않았습니다.");
                return;
            }

            if (Selection.activeTransform != null)
            {
                GameObject text = (GameObject)PrefabUtility.InstantiatePrefab(_data.prefab, Selection.activeTransform);
                Undo.RegisterCreatedObjectUndo(text, text.name);
                Selection.activeObject = text;
            }
        }

        private void Logic_CreatePrefabData()
        {
            Editor_TextObject newPrefabData = ScriptableObject.CreateInstance<Editor_TextObject>();
            string path = "Assets/11. ScriptableObject/Text/TextData.asset";
            AssetDatabase.CreateAsset(newPrefabData, path);
            AssetDatabase.SaveAssets();

            _data = newPrefabData;
        }
    }
}