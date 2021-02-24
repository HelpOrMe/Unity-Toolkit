using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ToolkitEditor.Utils
{
    public class RandomGameObject
    {
        public const string PrefabPath = "Packages/Toolkit/Resources/NoneGameObject.prefab";

        public static GameObject Instance
        {
            get
            {
                if (SceneGameObjects.Count == 0)
                    SceneManager.GetActiveScene().GetRootGameObjects(SceneGameObjects);
                return SceneGameObjects.Count > 0 ? SceneGameObjects[0] : new GameObject();
            }
        }

        private static readonly List<GameObject> SceneGameObjects = new List<GameObject>();
    }
}