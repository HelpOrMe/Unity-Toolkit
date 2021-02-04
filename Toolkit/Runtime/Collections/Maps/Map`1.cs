using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolkit.Collections
{
    [Serializable]
    public class Map<T> : IMap<T>
    {
        public T this[int x, int y]
        {
            get => array[y * height + x];
            set => array[y * height + x] = value;
        }

        public int Width => width;
        public int Height => height;
        
        [SerializeField] protected int width; 
        [SerializeField] protected int height; 
        [SerializeField] protected T[] array;

        public Map(T[,] array) : this(array.GetLength(0), array.GetLength(1))
        {
            CopyFrom(array);
        }
        
        public Map(Map<T> other) : this(other.width, other.height)
        {
            CopyFrom(other);
        }
        
        public Map(int width, int height)
        {
            array = new T[width * height];
        }

        public void CopyFrom(Map<T> other)
        {
            array = other.array;
        }

        public void CopyFrom(T[,] other)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    this[x, y] = other[x, y];
                }
            }
        }
        
        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)array).GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static explicit operator T[,](Map<T> map)
        {
            var array2d = new T[map.width, map.height];
            
            for (int x = 0; x < map.width; x++)
            {
                for (int y = 0; y < map.height; y++)
                {
                    array2d[x, y] = map[x, y];
                }
            }

            return array2d;
        }

        public static explicit operator Map<T>(T[,] array2d) => new Map<T>(array2d);

        public static implicit operator T[](Map<T> map) => map.array;
    }
}