using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class IdentifiedObject : NotifiableObject,IIdentifiedObject
    {
        protected string _id = "";
        public string ID
        {
            set
            {
                if (_id != "") throw new InvalidOperationException("ID is only set once.");
                _id = value;
            }
            get
            {
                return _id;
            }
        }
    }
}
