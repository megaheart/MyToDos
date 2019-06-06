using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MyToDos.Model
{
    class Task : NotifiableObject
    {
        public Task()
        {
        }
        //public long LastChange { private set; get; }
        protected Repeater _repeater;
        public Repeater Repeater
        {
            set
            {
                if (_repeater != value)
                {
                    _repeater = value;
                    _repeater.RepeaterInfoChanged += OnRepeaterInfoChanged;
                    OnPropertyChanged("Repeater");
                }
            }
            get
            {
                return _repeater;
            }
        }
        private void OnRepeaterInfoChanged() => OnPropertyChanged("Repeater");
        protected string _title;
        public string Title
        {
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
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
                }
            }
            get
            {
                return _description;
            }
        }
        private ObservableCollection<Tag> _tags;
        public ObservableCollection<Tag> Tags
        {
            get => _tags;
            set
            {
                if (_tags != value)
                {
                    _tags = value;
                    OnPropertyChanged("Tags");
                }
            }
        }
        private string _id = "";
        public string ID
        {
            set
            {
                if (_id != "") throw new InvalidOperationException("Don't allow to set this ID value");
                _id = value;
            }
            get
            {
                return _id;
            }
        }
        private string _webAddress;
        public string WebAddress
        {
            set
            {
                if (_webAddress != value)
                {
                    _webAddress = value;
                }
            }
            get => _webAddress;
        }
        public FlowDocument Note;
        public override string ToString()
        {
            return this.GetType() + " id:" + ID.ToString();
        }
    }
}
}
