using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public interface INoteTaking:IIdentifiedObject
    {
        bool HasNote { get; }
    }
}
