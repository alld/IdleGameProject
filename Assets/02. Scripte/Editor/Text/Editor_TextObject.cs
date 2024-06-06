using UnityEngine;

namespace IdleGame.IdleEditor
{

    [CreateAssetMenu(fileName = "GraphicText", menuName = "IdleGames/Object/Text")]
    public class Editor_TextObject : ScriptableObject
    {
        public GameObject prefab;
    }
}