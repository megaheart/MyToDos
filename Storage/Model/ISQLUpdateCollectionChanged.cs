using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public delegate void SQLCollectionInsertsItemEventHandler(object item);
    public delegate void SQLCollectionRemovesItemEventHandler(string id);
    public interface ISQLUpdateCollectionChanged
    {
        event SQLCollectionInsertsItemEventHandler SQLInsertsItem;
        event SQLCollectionRemovesItemEventHandler SQLRemovesItem;
    }
}
