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
    public class SQLCollection<T> : CollectionForIdentificationObject<T>//, ISQLUpdateCollectionChanged
        where T:IIdentifiedObject,ISQLUpdatePropertyChanged
    {
        
    }
}
