using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class SQLGarbageCollection<T>:CollectionForIdentificationObject<T>, INotifyCollectionChanged, INotifyPropertyChanged 
        where T:IIdentifiedObject,ISQLUpdatePropertyChanged
    {
        //throw Exception when change any ISQLUpdatePropertyChanged properties
    }
}
