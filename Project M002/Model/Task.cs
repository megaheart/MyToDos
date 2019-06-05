using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    class Task : NotifiableObject
    {
        public Task()
        {
        }
        //public int Index
        //{
        //    get
        //    {
        //        return (int)_repeat.Type;
        //    }
        //}
        public long LastChange { private set; get; }
        protected IRepeat _repeat;
        public IRepeat Repeat
        {
            set
            {
                if (_repeat != value)
                {
                    _repeat = value;
                    OnPropertyChanged("Repeat");
                    //OnPropertyChanged("Index");
                    LastChange = DateTime.Now.Ticks;
                }
            }
            get
            {
                return _repeat;
            }
        }
        protected string _title;
        public string Title
        {
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                    LastChange = DateTime.Now.Ticks;
                }
            }
            get
            {
                return _title;
            }
        }
        protected string _description;
        public string Description
        {
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                    LastChange = DateTime.Now.Ticks;
                }
            }
            get
            {
                return _description;
            }
        }
        public NotifiableCollection<uint> Tags { set; get; }
        private uint _id;
        public uint ID
        {
            set
            {
                if (_id != 0) throw new InvalidOperationException("Don't allow to set this ID value");
                _id = value;
            }
            get
            {
                return _id;
            }
        }
        //public Note Note;
        //------------- Will add file-working function
        private string _webAddress;
        public string WebAddress
        {
            set
            {
                if (_webAddress != value)
                {
                    _webAddress = value;
                    OnPropertyChanged("HasWebAddress");
                    LastChange = DateTime.Now.Ticks;
                }
            }
            get => _webAddress;
        }
        public bool HasWebAddress
        {
            get => !String.IsNullOrEmpty(_webAddress);
        }
        //---------------
        public bool Equals(Task task)
        {
            return ID == task.ID;
        }
        public override string ToString()
        {
            return this.GetType() + " id:" + ID.ToString();
        }
    }
}
}
