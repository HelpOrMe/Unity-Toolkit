using System;

namespace Toolkit.Serializable
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DictionaryViewAttribute : Attribute
    {
        public readonly object DefaultKey;
        public readonly object DefaultValue;
        public readonly bool ShowUnsupportedBox; 
        
        public DictionaryViewAttribute(object defaultKey = default, object defaultValue = default, 
            bool showUnsupportedBox = true)
        {
            DefaultKey = defaultKey;
            DefaultValue = defaultValue;
            ShowUnsupportedBox = showUnsupportedBox;
        }
    }
}