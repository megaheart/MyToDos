using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    class ToDoComment : NotifiableObject
    {
        public ToDoComment(/*int orderNumber,*/DateTime time, string content)
        {
            _time = time;
            _content = content;
            //OrderNumber = orderNumber;
        }
        //public int OrderNumber { get; internal set; }
        private DateTime _time;
        public DateTime Time
        {
            get => _time;
            internal set
            {
                if(_time != value)
                {
                    _time = value;
                    OnPropertyChanged("Time");
                }
            }
        }
        private string _content;
        public string Content
        {
            get => _content;
            internal set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged("Content");
                }
            }
        }
    }
}
