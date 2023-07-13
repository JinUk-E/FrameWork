#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Mine.Framework.Editor.AutoScriptTemplate
{
    public sealed class AutoCreateScript : AssetModificationProcessor
    {
        //todo : 1. UINavigator 생성시 자동으로 생성되는 스크립트에 대한 템플릿을 만들어야함.
        //todo : 2. 해당 코드 작성 후 형식에 맞춰서 코드 완성
        public static void OnCreateScripts(string path)
        {
            var suffixIndex = path.LastIndexOf(".meta", StringComparison.Ordinal);
            if (suffixIndex.Equals(-1)) return;
            
            SetPaths(path, suffixIndex, out var scriptPath, out var directoryPath, out var className, out var extensionName);
            
            if (!extensionName.Equals(".cs")) return;
            
            var templatePath = GetTemplatePath(className);
            if (templatePath == default) return;
            
            var content = File.ReadAllText(templatePath);
            

        }

        #region private methods
        
        private static void SetPaths(string path, int suffixIndex, out string scriptPath, out string directoryPath, out string className, out string extensionName)
        {
            scriptPath = path[..suffixIndex];
            directoryPath = Path.GetDirectoryName(scriptPath)?.Split("Assets\\").Last();
            className = Path.GetFileNameWithoutExtension(scriptPath);
            extensionName = Path.GetExtension(scriptPath);
        }
        
        
        private static string GetTemplatePath(string className)
        {
            if (className.Contains("Context"))
            {
                return className.Contains("Sheet") ? AssetDatabase.GUIDToAssetPath("sheetUID") :
                    className.Contains("Page") ? AssetDatabase.GUIDToAssetPath("pageUID") :
                    AssetDatabase.GUIDToAssetPath(className.Contains("Modal")
                        ? "modalUID"
                        : "ada43bacc1787304fa569ec4a4e44fd8");
            }
            return className.Contains("Presenter") ? AssetDatabase.GUIDToAssetPath("PresenterUID") : default;
        }

        #endregion
    
    }
}
#endif