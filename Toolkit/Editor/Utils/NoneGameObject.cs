using System.IO;
using UnityEditor;
using UnityEngine;

namespace ToolkitEditor.Utils
{
    public class NoneGameObject
    {
        public const string PrefabPath = "Packages/Toolkit/Resources/NoneGameObject.prefab";

        public static GameObject Instance => _instance ??= 
            PrefabUtility.LoadPrefabContents(Path.GetFullPath(PrefabPath));
        private static GameObject _instance;
    }
}