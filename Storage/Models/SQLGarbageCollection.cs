using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class SQLGarbageCollection<T> : CollectionForIdentificationObject<T>, INotifyCollectionChanged, INotifyPropertyChanged
        where T : IIdentifiedObject, INotifySQLUpdatePropertyChanged
    {
        public SQLGarbageCollection() : base() { }
        public SQLGarbageCollection(IEnumerable<T> list) : base(list) { }
        public event SQLCollectionChangedEventHandler SQLRestoresItem;
        public event SQLCollectionChangedEventHandler SQLRemovesItem;
        public event Action SQLClearsAllItems;
        public void RestoreFromGarbage(T item)
        {
            SQLRestoresItem(new SQLCollectionChangedArgs(SQLCollectionChangedAction.Remove, item));
            int index = IndexOfID(item.ID);
            RemoveItem(index);
        }
        public void RemoveFromGarbage(T item)
        {
            SQLRemovesItem(new SQLCollectionChangedArgs(SQLCollectionChangedAction.Remove, item));
            int index = IndexOfID(item.ID);
            RemoveItem(index);
        }
        public void RemoveAllFromGarbage()
        {
            SQLClearsAllItems();
            ClearItems();
        }
    }
}
