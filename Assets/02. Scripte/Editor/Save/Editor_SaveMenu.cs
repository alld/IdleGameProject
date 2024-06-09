using IdleGame.Core.Procedure;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace IdleGame.IdleEditor
{
    public class Editor_SaveMenu : EditorWindow
    {

        [MenuItem("IdleGames/Tools/Save")]
        private static void Menu_TextController()
        {
            GetWindow<Editor_SaveMenu>("세이브 컨트롤러");
        }



        private void OnGUI()
        {
            if (!EditorApplication.isPlaying)
            {
                GUILayout.Label("해당 기능은 게임 진행중에만 사용이 가능합니다.", EditorStyles.boldLabel);

                return;
            }

            if (GUILayout.Button("세이브 하기"))
            {
                Base_Engine.Save.Editor_Save();
            }

            GUILayout.Space(15f);

            if (!File.Exists(Application.persistentDataPath + "/TempSave.save"))
                return;

            if (GUILayout.Button("데이터 불러오기"))
            {
                Base_Engine.Save.Editor_Load();
            }

            GUILayout.Space(15f);


            if (GUILayout.Button("데이터 삭제하기"))
            {
                Base_Engine.Save.Editor_DeleteSave();
            }



            if (GUILayout.Button("세이브 파일이 저장된 폴더 열기"))
            {
                Application.OpenURL("file://" + Application.persistentDataPath);
            }
        }

    }
}