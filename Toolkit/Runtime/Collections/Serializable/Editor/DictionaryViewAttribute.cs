using System;

namespace Toolkit.Collections
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DictionaryViewAttribute : Attribute
    {
        public object DefaultKey;
        public object DefaultValue;

        public DictionaryViewAttribute(object defaultKey, object defaultValue)
        {
            DefaultKey = defaultKey;
            DefaultValue = defaultValue;
        }
    }
}