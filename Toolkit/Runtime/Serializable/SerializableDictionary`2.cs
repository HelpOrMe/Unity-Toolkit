using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Toolkit.Serializable
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
#if UNITY_EDITOR
        ,IEditorDictionary
#endif
    {
        [SerializeField] private SerializeContainer<TKey>[] keys;
        [SerializeField] private SerializeContainer<TValue>[] values;

        public SerializableDictionary() { }
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }

#if UNITY_EDITOR
        public Type KeyType => typeof(TKey);
        public Type ValueType => typeof(TValue);
        
        public void AddEntry(object key, object value)
        {
            if (!ContainsKey((TKey)key))
            {
                this[(TKey)key] = (TValue)value;
            }
        }

        public void RemoveEntry(object key)
        {
            if (key is TKey tKey)
            {
                Remove(tKey);
            }
        }

        public IEnumerable<KeyValuePair<object, object>> GetEntries() 
            => this.Select(pair => new KeyValuePair<object, object>(pair.Key, pair.Value));
#endif

        public void OnBeforeSerialize()
        {
            keys = Keys.Select(key => new SerializeContainer<TKey>(key)).ToArray();
            values = Values.Select(value => new SerializeContainer<TValue>(value)).ToArray();
        }

        public void OnAfterDeserialize()
        {
            if (keys == null || values == null || keys.Length != values.Length) return;

            Clear();
            for (int i = 0; i < keys.Length; i++)
            {
                this[keys[i].value] = values[i].value;
            }

            keys = null;
            values = null;
        }

        [Serializable]
        private class SerializeContainer<T>
        {
            public T value;

            public SerializeContainer(T value)
            {
                this.value = value;
            }
        }
    }
}