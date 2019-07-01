using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MyToDos.Model
{
    enum TaskStatus
    {
        Available,
        Unavailable,
        Expired
    }
    class Task : NotifiableObject
    {
        public Task()
        {}
        private void CheckPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            throw new Exception("Can't change Task's properties because IsReadOnly is true");
        }
        //This property can be copied or cloned
        private bool _isReadOnly;
        public bool IsReadOnly
        {
            set
            {
                if(value != _isReadOnly)
                {
                    if (value)
                    {
                        PropertyChanged += CheckPropertyChanged;
                    }
                    else PropertyChanged -= CheckPropertyChanged;
                }
            }
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
                    if (_isReadOnly) CheckPropertyChanged(null, null);
                }
            }
            get => _webAddress;
        }
        private DateTime _activatedTime;
        public DateTime ActivatedTime
        {
            set
            {
                if (_activatedTime != value)
                {
                    _activatedTime = value;
                    if (_isReadOnly) CheckPropertyChanged(null, null);
                }
            }
            get => _activatedTime;
        }
        private DateTime _expiryTime;
        public DateTime ExpiryTime
        {
            set
            {
                if (_expiryTime != value)
                {
                    _expiryTime = value;
                    if (_isReadOnly) CheckPropertyChanged(null, null);
                }
            }
            get => _expiryTime;
        }
        public TaskStatus GetStatusOn(DateTime date)
        {
            if (date >= _expiryTime) return TaskStatus.Expired;
            if (date < _activatedTime && !_repeater.IsUsableOn(date)) return TaskStatus.Unavailable;
            return TaskStatus.Available;
        }
        /// <returns>
        /// a copy of this Task, but new Task doesn't have ID or IsReadObly properties
        /// </returns>
        public Task Clone()
        {
            Task task = new Task();
            task._description = this._description;
            task._id = this._id;
            task._repeater = this._repeater.Clone();
            task._tags = new ObservableCollection<Tag>(this._tags);
            task._title = this._title;
            task._webAddress = this._webAddress;
            return task;
        }
        
        public override string ToString()
        {
            return this.GetType() + " id:" + ID.ToString();
        }
    }
}

