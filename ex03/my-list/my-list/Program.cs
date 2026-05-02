using System.Runtime.InteropServices;

namespace my_list;

class MyList
{
    private int[] _items;
    private int   _count;

    public int this[int index]
    {
        get { return _items[index]; }
        set { _items[index] = value; }
    }

    public void Print()
    {
        for (int i = 0; i < _count; i++)
            Console.Write($"'{_items[i]}'");
        Console.WriteLine();
    }
    public MyList()
    {
        _items = new int[4];
        _count = 0;
    }
    private void Resize()
    {
        int[] newItems = new int[_items.Length * 2];
        for (int i = 0; i < _count; i++)
            newItems[i] = _items[i];
        _items = newItems;
    }

    public void Add(int item)
    {
        if (_count == _items.Length)
            Resize();
        _items[_count] = item;
        _count++;
    }

    public void AddRange(int[] items)
    {
        if (items == null)
            return;
        for (int i = 0; i < items.Length; i++)
            Add(items[i]);
    }

    public bool Remove(int item)
    {
        for (int i = 0; i < _count; i++)
            if (item == _items[i])
            {
                for (int j = i; j < _count - 1; j++)
                    _items[j] = _items[j + 1];
                _count--;
                return true;
            }
        return false;
    }
    
    public bool TryGet(int index, out int value)
    {
        if (index < 0 || index >= _count)
        {
            value = 0;
            return false;
        }
        value = _items[index];
        return true;
    }

    public int IndexOf(int item)
    {
        for (int i = 0; i < _count; i++)
        {
            if (item == _items[i])
                return i;
        }
        return -1;
    }

    public bool Contains(int item)
    {
        for (int i = 0; i < _count; i++)
        {
            if (item == _items[i])
                return true;
        }

        return false;
    }

    public void Clear()
    {
        _count = 0;
    }
}

class Program
{
    static void Main(string[] args)
    {
        MyList myList = new MyList();
        
        myList.Add(1);
        myList.Add(2);
        myList.Add(3);
        myList.Add(4);
        myList.Add(5);
        
        myList.Print();
        myList.AddRange(new int[] { 6, 7, 8, 9});
        myList.AddRange(null);
        myList.Print();

        myList.Remove(3);
        myList.Print();

        int i = 0;
        Console.WriteLine($"{myList.TryGet(100, out i)} | {i}");
        
        Console.WriteLine($"{myList.TryGet(3, out i)} | {i}");
        
    }
}