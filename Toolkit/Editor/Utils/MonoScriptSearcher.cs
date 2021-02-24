using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace ToolkitEditor.Utils
{
    public static class MonoScriptSearcher
    {
        public static void SearchForScript(string absolutePath, Action<MonoScript> onResult)
        {
            MonoScript result = SearchForAssetsScript(absolutePath);
            if (result != null)
            {
                onResult.Invoke(result);
                return;
            }
            SearchForPackageScript(absolutePath, onResult);
        }
        
        public static MonoScript SearchForAssetsScript(string absolutePath)
        {
            // Check for project assets folder
            Match assetsMatch = Regex.Match(absolutePath, @"(..*)(?=Assets)");
            if (assetsMatch.Success)
            {
                string assetsRelativeScriptPath = absolutePath.Replace(assetsMatch.Value, "");
                return AssetDatabase.LoadAssetAtPath<MonoScript>(assetsRelativeScriptPath);
            }

            return null;
        }

        public static void SearchForPackageScript(string absolutePath, Action<MonoScript> onResult)
        {
            // Check for package script folders and grab package folder name
            Match packageMatch = Regex.Match(absolutePath, @"(?<=\/)([^\/]*)(?=\/Runtime|\/Editor)");
            if (!packageMatch.Success)
            {
                return;
            }

            RequestProjectPackages(packages => onResult(SearchForPackageScript(absolutePath, packageMatch, packages)));
        }
        
        private static MonoScript SearchForPackageScript(string absolutePath, Match packageMatch, 
            PackageCollection packages)
        {
            string packageMatchValue = packageMatch.Value;
            
            foreach (PackageInfo package in packages)
            {
                if (package.displayName != packageMatchValue 
                    || $"{package.name}@{package.version}" != packageMatchValue)
                {
                    continue;
                }

                string scriptFolderPath = Regex.Split(absolutePath, @"(..*)(?=\/Runtime|\/Editor)").Last();
                string projectRelativeScriptPath = "Packages/" + package.name + scriptFolderPath;
                return AssetDatabase.LoadAssetAtPath<MonoScript>(projectRelativeScriptPath);
            }

            return null;
        }
        
        private static void RequestProjectPackages(Action<PackageCollection> onComplete)
        {
            ListRequest requestedList = Client.List();

            void Process()
            {
                if (requestedList.IsCompleted)
                {
                    EditorApplication.update -= Process;
                    onComplete?.Invoke(requestedList.Result);
                }
            }
            EditorApplication.update += Process;
        }
    }
}
