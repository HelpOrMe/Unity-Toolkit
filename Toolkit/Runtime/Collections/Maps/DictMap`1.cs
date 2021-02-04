using System.Collections;
using System.Collections.Generic;

namespace Toolkit.Collections
{
    public class DictMap<T> : IMapEditable<T>
    {
        public T this[int x, int y]
        {
            get => Dict[y * Height + x];
            set => Dict[y * Height + x] = value;
        }

        public int Width { get; }
        public int Height { get; }
        
        protected Dictionary<int, T> Dict;

        public DictMap(int width, int height)
        {
            Dict = new Dictionary<int, T>();
            Width = width;
            Height = height;
        }
        
        public DictMap(DictMap<T> other)
        {
            CopyFrom(other);
        }

        public void CopyFrom(DictMap<T> other)
        {
            Dict = other.Dict;
        }

        public void CopyFrom(T[,] other)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    this[x, y] = other[x, y];
                }
            }
        }

        public bool Contains(int x, int y) => Dict.ContainsKey(y * Height + x);
        
        public void Remove(int x, int y)
        {
            Dict.Remove(y * Height + x);
        }
        
        public IEnumerator<T> GetEnumerator() => Dict.Values.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}