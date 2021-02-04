using System.Collections.Generic;

namespace Toolkit.Collections
{
    public interface IMap<T> : IEnumerable<T>
    {
        public T this[int x, int y] { get; set; }
        
        public int Width { get; }
        public int Height { get; }
    }
}