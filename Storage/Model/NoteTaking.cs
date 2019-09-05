using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class NoteTaking : IdentifiedObject, INoteTaking
    {
        public bool HasNote { get; internal set; }
    }
}
