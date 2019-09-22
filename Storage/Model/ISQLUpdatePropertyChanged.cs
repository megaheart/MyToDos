using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public delegate void SQLUpdatePropertyChangedEventHandler(IdentifiedObject item, string property, string value);
    public interface INotifySQLUpdatePropertyChanged : IIdentifiedObject
    {
        event SQLUpdatePropertyChangedEventHandler SQLUpdateProperty;
    }
}
