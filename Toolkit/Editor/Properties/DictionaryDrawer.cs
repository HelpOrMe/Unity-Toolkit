using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Toolkit.Collections;
using Toolkit.Editor.Extensions;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace Toolkit.Editor.Properties
{
    [CustomPropertyDrawer(typeof(IEditorDictionary), true)]
    public class DictionaryDrawer : PropertyDrawer
    {
        private static readonly Dictionary<int, bool> Foldouts = new Dictionary<int, bool>();
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = 18;

            if (!TryValidate(property, out string message))
            {
                if (message != null && GetViewAttr().ShowUnsupportedBox)
                {
                    EditorGUI.HelpBox(position, message, MessageType.Error);
                }
                return;
            }

            int fieldToken = fieldInfo.MetadataToken;
            if (!Foldouts.ContainsKey(fieldToken))
            {
                Foldouts[fieldToken] = false;
            }

            var viewAttr = fieldInfo.GetCustomAttribute<DictionaryViewAttribute>();
            IEditorDictionary dict = GetDict(property);
            
            if (DrawFoldout(position, label, dict, viewAttr.DefaultKey, viewAttr.DefaultValue))
            {
                DrawBody(position, dict);
            }
        }

        private bool DrawFoldout(Rect position, GUIContent label, IEditorDictionary dict, 
            object defaultKey, object defaultValue)
        {
            int fieldToken = fieldInfo.MetadataToken;
            if (!Foldouts.ContainsKey(fieldToken))
            {
                Foldouts[fieldToken] = false;
            }

            float buttonWidth = ButtonDrawers.ButtonSize.x;
            Rect[] rects = position.SplitRow(position.width - buttonWidth, buttonWidth);
            
            bool foldout = EditorGUI.Foldout(rects[0], Foldouts[fieldToken], label);
            if (ButtonDrawers.Plus(rects[1]))
            {
                defaultKey ??= CreateInstance(dict.KeyType);
                defaultValue ??= CreateInstance(dict.ValueType);
                dict.AddEntry(defaultKey, defaultValue);
            }

            return Foldouts[fieldToken] = foldout;
        }

        private void DrawBody(Rect position, IEditorDictionary dict)
        {
            foreach (KeyValuePair<object, object> entry in dict.GetEntries().ToList())
            {
                position = position.ColumnNext();
                
                float buttonWidth = ButtonDrawers.ButtonSize.x;
                float columnSize = (position.width - buttonWidth) / 2; 
                
                Rect[] row = position.SplitRow(columnSize, columnSize, buttonWidth);
                
                object newKey = Drawers.Draw(row[0], dict.KeyType, entry.Key);
                object newValue = Drawers.Draw(row[1], dict.ValueType, entry.Value);

                if (newKey != entry.Key || newValue != entry.Value)
                {
                    dict.RemoveEntry(entry.Key);
                    dict.AddEntry(newKey, newValue);
                }
                
                if (ButtonDrawers.Minus(row[2]))
                {
                    dict.RemoveEntry(newKey);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!TryValidate(property, out string message))
                return GetViewAttr().ShowUnsupportedBox && message != null ? 18 : 0;

            int fieldToken = fieldInfo.MetadataToken;
            if (!Foldouts.ContainsKey(fieldToken) || !Foldouts[fieldToken])
                return 18;
            
            const float space = 2;
            return 18 + GetDict(property).GetEntries().Count() * (18 + space) + space;
        }

        private bool TryValidate(SerializedProperty property, out string message) 
            => ValidateUnsupported(property, out message) && ValidateDefaultConstructors(property, out message);

        private bool ValidateUnsupported(SerializedProperty property, out string message)
        {
            message = null;
            
            if (GetViewAttr() == null)
                return false;

            var dict = GetDict(property);

            if (dict == null)
            {
                message = "Unsupported dictionary nesting level";
                return false;
            }

            if (!CanBeDrawn(dict))
            {
                message = "Unsupported dictionary generic types";
                return false;
            }

            return true;
        }

        private bool ValidateDefaultConstructors(SerializedProperty property, out string message)
        {
            DictionaryViewAttribute viewAttr = GetViewAttr();
            IEditorDictionary dict = GetDict(property);
            
            if (viewAttr.DefaultKey == null)
            {
                try
                {
                    CreateInstance(dict!.KeyType);
                }
                catch (MissingMethodException)
                {
                    message = "Invalid key constructor. Use DictionaryView(defaultKey: ...)";
                    return false;
                }
            }
            
            if (viewAttr.DefaultValue == null)
            {
                try
                {
                    CreateInstance(dict!.ValueType);
                }
                catch (MissingMethodException)
                {
                    message = "Invalid value constructor. Use DictionaryView(defaultValue: ...)";
                    return false;
                }
            }

            message = null;
            return true;
        }

        private object CreateInstance(Type type)
        {
            if (!typeof(UnityEngine.Object).IsAssignableFrom(type))
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
        
        private bool CanBeDrawn(IEditorDictionary dict)
            => Drawers.CanDraw(dict.KeyType) && Drawers.CanDraw(dict.ValueType);

        private DictionaryViewAttribute GetViewAttr() => fieldInfo.GetCustomAttribute<DictionaryViewAttribute>();
        
        private IEditorDictionary GetDict(SerializedProperty property)
        {
            try
            {
                return fieldInfo.GetValue(property.serializedObject.targetObject) as IEditorDictionary;
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}