using System;
using System.Collections;
using System.Collections.Generic;

namespace Toolkit.Collections
{
    [Serializable]
    public class DictMap<T> : IDynamicMap<T>
    {
        public T this[int x, int y]
        {
            get => Dict[y * Height + x];
            set => Dict[y * Height + x] = value;
        }

        public int Width { get; }
        public int Height { get; }
        
        protected SerializableDictionary<int, T> Dict;

        public DictMap(DictMap<T> other) : this(other.Width, other.Height)
        {
            CopyFrom(other);
        }
        
        public DictMap(T[,] other) : this(other.GetLength(0), other.GetLength(1))
        {
            CopyFrom(other);
        }
        
        public DictMap(int width, int height)
        {
            Dict = new SerializableDictionary<int, T>();
            Width = width;
            Height = height;
        }

        public void CopyFrom(DictMap<T> other)
        {
            Dict = new SerializableDictionary<int, T>(other.Dict);
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

        public void CopyTo(ref T[,] other)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    other[x, y] = this[x, y];
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
        
        public static explicit operator T[,](DictMap<T> map)
        {
            var array2d = new T[map.Width, map.Height];
            map.CopyTo(ref array2d);
            return array2d;
        }

        public static explicit operator DictMap<T>(T[,] array2d) => new DictMap<T>(array2d);
    }
}