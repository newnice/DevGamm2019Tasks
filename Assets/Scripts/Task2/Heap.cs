using System;

public class Heap<T> where T : IComparable<T>
{
    private T[] _heap;
    private int _size;

    public Heap(int capacity)
    {
        _heap = new T[capacity];
    }


    public void Add(T item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        if (_heap.Length < (_size + 1))
        {
            Array.Resize(ref _heap, (int) (_size * 1.5));
        }

        _heap[_size] = item;
        _size++;

        SiftUp(_size - 1);
    }

    public void AddAll(T[] items, int startIndex, int length)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));
        if (items.Length == 0) throw new ArgumentException("Collection must be not empty");
        if (_heap.Length < (_size + length))
        {
            Array.Resize(ref _heap, (int) (_size + items.Length));
        }

        var endIndex = startIndex + length;
        for (var i = startIndex; i < endIndex; i++)
        {
            _heap[_size + i] = items[i];
        }

        _size += length;
        Reorder();
    }

    private void Reorder()
    {
        for (var i = _size / 2; i >= 0; i--)
            SiftDown(i);
    }

    private void SiftUp(int startIndex)
    {
        if (startIndex == 0) return;
        var current = startIndex;
        var parentIndex = (current - 1) / 2;
        while (current > 0)
        {
            if (_heap[current].CompareTo(_heap[parentIndex]) >= 0) break;

            Swap(current, parentIndex);
            current = parentIndex;
        }
    }

    private void SiftDown(int startIndex)
    {
        var parentIndex = startIndex;

        while (parentIndex * 2 + 1 < _size)
        {
            var childInd1 = parentIndex * 2 + 1;
            var childInd2 = childInd1 + 1;
            if (_heap[parentIndex].CompareTo(_heap[childInd1]) > 0)
            {
                var newParent = (childInd2 < _size && _heap[childInd2].CompareTo(_heap[childInd1]) < 0)
                    ? childInd2
                    : childInd1;

                Swap(newParent, parentIndex);
                parentIndex = newParent;
            }
            else if (childInd2 < _size && _heap[childInd2].CompareTo(_heap[parentIndex]) < 0)
            {
                Swap(childInd2, parentIndex);
                parentIndex = childInd2;
            }
            else
                break;
        }
    }

    private void Swap(int current, int parentIndex)
    {
        var temp = _heap[current];
        _heap[current] = _heap[parentIndex];
        _heap[parentIndex] = temp;
    }


    public void ReplaceMin(T itemToPlace)
    {
        _heap[0] = itemToPlace;
        SiftDown(0);
    }

    public void Clear()
    {
        for (var i = 0; i < _size; i++)
            _heap[i] = default(T);
        _size = 0;
    }

    public T Min
    {
        get
        {
            if (_size == 0) return default(T);
            return _heap[0];
        }
    }

    public T[] Elements => _heap;
}