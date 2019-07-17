using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    public class Config
    {
        
        public class PersonalInfo
        {
            internal string Password { get; set; }
            public string DecodePassword()
            {
                return Password;
            }
        }
    }
}
