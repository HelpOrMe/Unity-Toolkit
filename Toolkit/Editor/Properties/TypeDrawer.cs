using System;
using Toolkit.Serializable;
using ToolkitEditor.Extensions;
using ToolkitEditor.Utils;
using UnityEditor;
using UnityEngine;

namespace ToolkitEditor.Properties
{
    [CustomPropertyDrawer(typeof(SerializableType), true)]
    public class TypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);
            SerializedProperty typeNameProperty = property.FindPropertyRelative("fullTypeName");
            Rect[] slices = position.SplitRow(position.width - 18, 18);

            typeNameProperty.stringValue = EditorGUI.TextField(slices[0], typeNameProperty.stringValue);
            
            if (!GUI.Button(slices[1], EditorGUIUtility.IconContent("FolderEmpty Icon"), EditorStyles.label))
                return;
            
            string path = EditorUtility.OpenFilePanel("Select script", "", "cs");
            MonoScriptSearcher.SearchForScript(path, script =>
            {
                if (script == null)
                {
                    Debug.LogError("Unsupported script path");
                    return;
                }

                Type type = script.GetClass();
                if (type == null)
                {
                    Debug.LogError("Unity MonoScript unsupported type");
                    return;
                }

                typeNameProperty.stringValue = type.AssemblyQualifiedName;
                typeNameProperty.serializedObject.ApplyModifiedProperties();
            });
        }
    }
}
