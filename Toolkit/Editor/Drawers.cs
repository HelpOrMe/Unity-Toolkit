using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Toolkit.Editor
{
    public static class Drawers
    {
        public static readonly Dictionary<Type, Func<Rect, object, object>> TypeDrawers =
            new Dictionary<Type, Func<Rect,object, object>>
            {
                [typeof(string)] = (p, o) => EditorGUI.TextField(p, (string)o),
                [typeof(float)] = (p, o) => EditorGUI.FloatField(p, (float)o),
                [typeof(bool)] = (p, o) => EditorGUI.Toggle(p, (bool)o),
                [typeof(Vector2)] = (pos, o) => EditorGUI.Vector2Field(pos, GUIContent.none, (Vector2)o),
                [typeof(Vector3)] = (p, o) => EditorGUI.Vector3Field(p, GUIContent.none, (Vector3)o),
                [typeof(Vector2Int)] = (pos, o) => EditorGUI.Vector2IntField(pos, GUIContent.none, (Vector2Int)o),
                [typeof(Vector3Int)] = (p, o) => EditorGUI.Vector3IntField(p, GUIContent.none, (Vector3Int)o),
                [typeof(Rect)] = (p, o) => EditorGUI.RectField(p, (Rect)o),
                [typeof(AnimationCurve)] = (p, o) => EditorGUI.CurveField(p, (AnimationCurve)o),
                [typeof(Color)] = (p, o) => EditorGUI.ColorField(p, (Color)o),
                [typeof(Object)] = (p, o) => EditorGUI.ObjectField(p, GUIContent.none, (Object)o, o.GetType(), true),
                [typeof(Enum)] = (p, o) => EditorGUI.EnumPopup(p, (Enum)o),
            };
        
        public static readonly Dictionary<Type, Func<object, object>> TypeLayoutDrawers =
            new Dictionary<Type, Func<object, object>>
            {
                [typeof(string)] = o => EditorGUILayout.TextField((string)o),
                [typeof(int)] = o => EditorGUILayout.IntField((int)o),
                [typeof(float)] = o => EditorGUILayout.FloatField((float)o),
                [typeof(bool)] = o => EditorGUILayout.Toggle((bool)o),
                [typeof(Vector2)] = o => EditorGUILayout.Vector2Field(GUIContent.none, (Vector2)o),
                [typeof(Vector3)] = o => EditorGUILayout.Vector3Field(GUIContent.none, (Vector2)o),
                [typeof(Vector2Int)] = o => EditorGUILayout.Vector2IntField(GUIContent.none, (Vector2Int)o),
                [typeof(Vector3Int)] = o => EditorGUILayout.Vector3IntField(GUIContent.none, (Vector3Int)o),
                [typeof(Rect)] = o => EditorGUILayout.RectField((Rect)o),
                [typeof(AnimationCurve)] = o => EditorGUILayout.CurveField((AnimationCurve)o),
                [typeof(Color)] = o => EditorGUILayout.ColorField((Color)o),
                [typeof(Object)] = o => EditorGUILayout.ObjectField((Object)o, o.GetType(), true),
                [typeof(Enum)] = o => EditorGUILayout.EnumPopup((Enum)o)
            };

        public static T Draw<T>(Rect position, T obj)
        {
            for (Type t = typeof(T); t != null; t = t.BaseType)
            {
                if (TypeDrawers.ContainsKey(t))
                {
                    return (T)TypeDrawers[t](position, obj);
                }
            }
            return obj;
        }
        
        public static object Draw(Rect position,object obj)
        {
            for (Type t = obj.GetType(); t != null; t = t.BaseType)
            {
                if (TypeDrawers.ContainsKey(t))
                {
                    return TypeDrawers[t](position, obj);
                }
            }
            return obj;
        }
        
        public static T Draw<T>(T obj)
        {
            for (Type t = typeof(T); t != null; t = t.BaseType)
            {
                if (TypeLayoutDrawers.ContainsKey(t))
                {
                    return (T)TypeLayoutDrawers[t](obj);
                }
            }
            return obj;
        }
        
        public static object Draw(object obj)
        {
            for (Type t = obj.GetType(); t != null; t = t.BaseType)
            {
                if (TypeLayoutDrawers.ContainsKey(t))
                {
                    return TypeLayoutDrawers[t](obj);
                }
            }
            return obj;
        }
    }
}