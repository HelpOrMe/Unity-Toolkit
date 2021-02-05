namespace Toolkit.Collections
{
    public interface IDynamicMap<T> : IMap<T>
    {
        public bool Contains(int x, int y);
        public void Remove(int x, int y);
    }
}