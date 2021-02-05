using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Toolkit.Collections;
using UnityEditor;
using UnityEngine;

namespace Toolkit.Editor.Properties
{
    [CustomPropertyDrawer(typeof(IEditorDictionary), true)]
    public class DictionaryDrawer : PropertyDrawer
    {
        private static readonly Dictionary<int, bool> Foldouts = new Dictionary<int, bool>();
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var viewAttr = fieldInfo.GetCustomAttribute<DictionaryViewAttribute>();
            if (viewAttr == null)
                return;

            var dict = fieldInfo.GetValue(property.serializedObject.targetObject) as IEditorDictionary;
            if (dict == null)
            {
                EditorGUILayout.HelpBox("Unsupported nesting level", MessageType.Warning);
                return;
            }
            
            int fieldToken = fieldInfo.MetadataToken;
            if (!Foldouts.ContainsKey(fieldToken))
            {
                Foldouts[fieldToken] = false;
            }

            if (DrawFoldout(label, dict, viewAttr.DefaultKey, viewAttr.DefaultValue))
            {
                DrawBody(dict);
            }
        }

        private bool DrawFoldout(GUIContent label, IEditorDictionary dict, object defaultKey, object defaultValue)
        {
            int fieldToken = fieldInfo.MetadataToken;
            if (!Foldouts.ContainsKey(fieldToken))
            {
                Foldouts[fieldToken] = false;
            }

            EditorGUILayout.BeginHorizontal();
            bool foldout = EditorGUILayout.Foldout(Foldouts[fieldToken], label);

            if (ButtonDrawers.Plus())
            {
                dict.AddEntry(defaultKey, defaultValue);
            }
            EditorGUILayout.EndHorizontal();

            return Foldouts[fieldToken] = foldout;
        }

        private void DrawBody(IEditorDictionary dict)
        {
            foreach (KeyValuePair<object, object> entry in dict.GetEntries().ToList())
            {
                EditorGUILayout.BeginHorizontal();
                
                dict.RemoveEntry(entry.Key);
                
                object newKey = Drawers.Draw(entry.Key);
                object newValue = Drawers.Draw(entry.Value);

                dict.AddEntry(newKey, newValue);
                
                if (ButtonDrawers.Minus())
                {
                    dict.RemoveEntry(newKey);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => 0;
    }
}