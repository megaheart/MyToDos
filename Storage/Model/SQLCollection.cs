using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class SQLCollection<T> : CollectionForIdentificationObject<T>
        where T : IIdentifiedObject, ISQLUpdatePropertyChanged
    {
        public SQLCollection() : base() { }
        public SQLCollection(IEnumerable<T> list) : base(list) { }
        public event SQLCollectionUpdatesItemEventHandler SQLUpdatesItem;
        public event SQLCollectionChangedEventHandler SQLInsertsItem;
        public event SQLCollectionChangedEventHandler SQLRemovesItem;
        public void Add(T item)
        {
            item.ID = GetID();
            item.SQLUpdateProperty += OnSQLUpdatesItem;
            SQLInsertsItem(item);
            InsertItem(item);
        }
        public void RestoreItem(T item)
        {
            item.SQLUpdateProperty += OnSQLUpdatesItem;
            InsertItem(item);
        }
        public void Remove(T item)
        {
            SQLRemovesItem(item);
            int index = IndexOfID(item.ID);
            RemoveItem(index);
        }
        private void OnSQLUpdatesItem(string id, string property, string value)
        {
            SQLUpdatesItem(id, property, value);
        }
    }
}
