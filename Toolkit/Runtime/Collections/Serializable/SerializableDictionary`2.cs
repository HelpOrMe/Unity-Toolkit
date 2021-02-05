using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Toolkit.Collections
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IEditorDictionary, 
        ISerializationCallbackReceiver
    {
        [SerializeField] private SerializeContainer<TKey>[] keys;
        [SerializeField] private SerializeContainer<TValue>[] values;

        public SerializableDictionary() { }
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
        
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddEntry(object key, object value)
        {
            this[(TKey)key] = (TValue)value;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void RemoveEntry(object key)
        {
            Remove((TKey)key);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<KeyValuePair<object, object>> GetEntries() 
            => this.Select(pair => new KeyValuePair<object, object>(pair.Key, pair.Value));

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