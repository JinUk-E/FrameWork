#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Mine.Framework.Editor.GetAssetsUID
{
    public class GetAssetGUID : EditorWindow
    {
        [MenuItem("Assets/Get Asset UID")]
        private static void GetUID()
        {
            if (Selection.assetGUIDs.Length < 0)
            {
                Debug.Log("No asset selected");
                return;
            }
            var selectedGUID = Selection.assetGUIDs[0];
            Debug.Log($"Selected asset GUID : {selectedGUID}");
            var textEditor = new TextEditor {text = selectedGUID};
            textEditor.SelectAll();
            textEditor.Copy();
        }
    }
}
#endif