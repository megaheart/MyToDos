using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Storage.QueryMethods;

namespace Storage.Model
{
    public class CollectionForIdentificationObject<T> : IEnumerable<T>, INotifyCollectionChanged, INotifyPropertyChanged,IList<T>
            where T : IIdentifiedObject
    {
        protected List<T> _items;
        public CollectionForIdentificationObject()
        {
            _items = new List<T>();
        }
        public CollectionForIdentificationObject(IEnumerable<T> list)
        {
            _items = new List<T>(list);
        }
        public T this[int index]
        {
            set => throw new NotImplementedException();
            get => _items[index];
        }
        public T this[string id]
        {
            get
            {
                int index = IndexOfID(id);
                if (index == -1) throw new Exception("This ID doesn't exist in collection");
                return _items[index];
            }
        }
        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        const string s = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public static string GetID()
        {
            string o = "";
            long i = DateTime.Now.Ticks / 10000 - 63702773000369;
            while (i != 0)
            {
                o = s[checked((int)(i % 62))] + o;
                i /= 62;
            }
            return o;
        }
        public static int CompareID(string Id1, string Id2)
        {
            if (Id1.Length == Id2.Length)
                return Id1.CompareTo(Id2);
            else return Id1.Length - Id2.Length;
        }
        public bool ContainsID(string id)
        {
            return IndexOfID(id) != -1;
        }
        public int IndexOfID(string id)
        {
            return Extensions.BinarySearch(_items, x => x.ID, id, CompareID);
        }
        protected void ClearItems()
        {
            _items.Clear();
            OnPropertyChanged("Item[]");
            OnPropertyChanged("Count");
            CollectionChanged?.Invoke(this,
                 new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        protected void InsertItem(T item)
        {
            //if (Contains(item)) throw new Exception("CollectionForIdentificationObject.cs InsertItem failed: item existed");
            int index = _items.Count;
            while (index > 0 && CompareID(_items[index - 1].ID, item.ID) > 0) --index;
            _items.Insert(index, item);
            OnPropertyChanged("Item[]");
            OnPropertyChanged("Count");
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }
        protected void RemoveItem(int index)
        {
            _items.RemoveAt(index);
            OnPropertyChanged("Item[]");
            OnPropertyChanged("Count");
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, _items[index], index));
        }
        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public int IndexOf(T item)
        {
            return Extensions.BinarySearch(_items, x => x.ID, item.ID, CompareID);
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public virtual void Add(T item)
        {
            throw new NotImplementedException();
        }

        public virtual void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            return IndexOfID(item.ID) != -1;
        }

        public virtual bool Remove(T item)
        {
            throw new NotImplementedException();
        }
    }
}
