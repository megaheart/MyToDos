using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDos.Model
{
    enum TaskStatus
    {
        Available,
        Unavailable,
        Expired
    }
    class Task : NotifiableObject, IRecyclable
    {
        public Task()
        {}
        public Task(string title, string id, Repeater repeater, DateTime? activatedTime, DateTime? expiryTime,TimeInfo time)
        {
            _id = ID;
            _title = title;
            _repeater = repeater;
            _activatedTime = activatedTime.HasValue ? activatedTime.Value : DateTime.MinValue;
            _expiryTime = expiryTime.HasValue ? expiryTime.Value : DateTime.MaxValue;
            _time = time;
        }
        //public long LastChange { private set; get; }
        private TimeInfo _time;
        public TimeInfo Time
        {
            internal set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
            get =>_time;
        }
        protected Repeater _repeater;
        public Repeater Repeater
        {
            internal set
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
            internal set
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
            internal set
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
            internal set
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
            internal set
            {
                if (_id != "") throw new InvalidOperationException("ID is only set once.");
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
            internal set
            {
                if (_webAddress != value)
                {
                    _webAddress = value;
                }
            }
            get => _webAddress;
        }
        private DateTime _activatedTime;
        public DateTime ActivatedTime
        {
            internal set
            {
                if (_activatedTime != value)
                {
                    _activatedTime = value;
                }
            }
            get => _activatedTime;
        }
        private DateTime _expiryTime;
        public DateTime ExpiryTime
        {
            internal set
            {
                if (_expiryTime != value)
                {
                    _expiryTime = value;
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

