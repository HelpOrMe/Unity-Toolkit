using System;
using UnityEngine;

namespace Toolkit.Serializable
{
    [Serializable]
    public class SerializableType : ISerializationCallbackReceiver
    {
        public Type Value { get; set; }

        [SerializeField] private string fullTypeName;

        public SerializableType(Type value)
        {
            Value = value;
        }

        public void OnBeforeSerialize()
        {
            fullTypeName = Value?.AssemblyQualifiedName;
        }

        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(fullTypeName))
            {
                Value = Type.GetType(fullTypeName);
            }

            fullTypeName = null;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is SerializableType other)
            {
                return Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Value?.GetHashCode() ?? 0;
        }

        public override string ToString() => Value?.ToString() ?? base.ToString();

        public static implicit operator Type(SerializableType serializableType) => serializableType.Value;
        
        public static implicit operator SerializableType(Type type) => new SerializableType(type);
    }
}