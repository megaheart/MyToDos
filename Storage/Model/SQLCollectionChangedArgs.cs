using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class SQLCollectionChangedArgs
    {
        public SQLCollectionChangedArgs(SQLCollectionChangedAction action, IIdentifiedObject item)
        {
            Action = action;
            Item = item;
            //Index = index;
        }
        public SQLCollectionChangedArgs(IIdentifiedObject item/*, int index*/, string property, string newValue)
        {
            Action = SQLCollectionChangedAction.Edit;
            Item = item;
            //Index = index;
            Property = property;
            NewValue = newValue;
        }
        public IIdentifiedObject Item { private set; get; }
        //public int Index { private set; get; }
        public SQLCollectionChangedAction Action { private set; get; }
        public string Property { private set; get; }
        internal string NewValue { private set; get; }
    }
    public enum SQLCollectionChangedAction
    {
        Add, Remove, Edit
    }
}
