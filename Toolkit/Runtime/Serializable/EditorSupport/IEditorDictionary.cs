using System;
using System.Collections.Generic;

namespace Toolkit.Serializable
{
    public interface IEditorDictionary
    {
        Type KeyType { get; } 
        Type ValueType { get; } 
        
        void AddEntry(object key, object value);
        void RemoveEntry(object key);
        IEnumerable<KeyValuePair<object, object>> GetEntries();
    }
}