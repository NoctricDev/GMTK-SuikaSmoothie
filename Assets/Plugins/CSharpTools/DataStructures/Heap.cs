using System;

namespace CSharpTools.DataStructures
{
    public interface IHeapItem<T> : IComparable<T>
    {
        public int HeapIndex { get; set; }
    }
    
    public class Heap<T> where T : IHeapItem<T>
    {
        private readonly T[] _items;

        public int Count => _itemCount;
        
        private int _itemCount;

        public Heap(int heapSize)
        {
            _items = new T[heapSize];
        }

        public void AddItem(T item)
        {
            item.HeapIndex = _itemCount;
            _items[_itemCount] = item;
            SortHeapUp(item);
            _itemCount++;
        }

        public T RemoveFirstItem()
        {
            T firstItem = _items[0];
            _itemCount--;
            _items[0] = _items[_itemCount];
            _items[0].HeapIndex = 0;
            SortHeapDown(_items[0]);
            return firstItem;
        }

        public void UpdateItem(T item)
        {
            SortHeapUp(item);
        }

        public bool Contains(T item)
        {
            return Equals(_items[item.HeapIndex], item);
        }

        private void SortHeapUp(T item)
        {
            int parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                T parentItem = _items[parentIndex];
                
                if (item.CompareTo(parentItem) > 0)
                    SwapItems(item, parentItem);
                else
                    break;

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        private void SortHeapDown(T item)
        {
            while (true)
            {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;

                if (childIndexLeft < _itemCount)
                {
                    int swapIndex = childIndexLeft;

                    if (childIndexRight < _itemCount)
                    {
                        if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                            swapIndex = childIndexRight;
                    }

                    if (item.CompareTo(_items[swapIndex]) < 0)
                        SwapItems(item, _items[swapIndex]);
                    else
                        return;
                }
                else
                    return;
            }
        }

        private void SwapItems(T itemA, T itemB)
        {
            _items[itemA.HeapIndex] = itemB;
            _items[itemB.HeapIndex] = itemA;
            (itemA.HeapIndex, itemB.HeapIndex) = (itemB.HeapIndex, itemA.HeapIndex);
        }
    }
}
