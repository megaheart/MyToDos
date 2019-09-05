using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public delegate void SQLCollectionChangedEventHandler(IIdentifiedObject item);
    public delegate void SQLCollectionUpdatesItemEventHandler(string id, string property, string value);

}
