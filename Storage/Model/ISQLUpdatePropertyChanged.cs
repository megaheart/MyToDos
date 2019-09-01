using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public delegate void SQLUpdatePropertyChangedEventHandler(string id, string property, string value);
    public interface ISQLUpdatePropertyChanged : IIdentifiedObject
    {
        event SQLUpdatePropertyChangedEventHandler SQLUpdateProperty;
    }
}
