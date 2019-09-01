using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public enum TaskStatus
    {
        Available,
        Unavailable,
        Expired
    }
    public class Task : NotifiableObject, IRecyclable, INoteTaking, ISQLUpdatePropertyChanged
    {
        public Task()
        {
            _tags = new ObservableCollection<Tag>();
            _time = new ObservableCollection<TimeInfo>();
        }
        public Task(string title, string id, Repeater repeater, DateTime? activatedTime, DateTime? expiryTime, ObservableCollection<TimeInfo> time, ObservableCollection<Tag> tags, string webAddress)
        {
            _id = id;
            _title = title;
            _repeater = repeater;
            _activatedTime = activatedTime.HasValue ? activatedTime.Value : DateTime.MinValue;
            _expiryTime = expiryTime.HasValue ? expiryTime.Value : DateTime.MaxValue;
            if (time == null) _time = new ObservableCollection<TimeInfo>();
            else _time = time;
            if (tags == null) _tags = new ObservableCollection<Tag>();
            else _tags = tags;
            _webAddress = webAddress;
        }
        //public long LastChange { private set; get; }
        private ObservableCollection<TimeInfo> _time;
        public ObservableCollection<TimeInfo> Time
        {
            set
            {
                if(value != _time)
                {
                    _time = value;
                    _time.CollectionChanged += TimeChanged;
                    OnPropertyChanged("Time");
                    SQLUpdateProperty?.Invoke(ID, "Time", TimeInfosStorageConverter.ToString(_time));
                }
            }
            get =>_time;
        }

        private void TimeChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SQLUpdateProperty?.Invoke(ID, "Time", TimeInfosStorageConverter.ToString(_time));
        }

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
                    SQLUpdateProperty?.Invoke(ID, "Repeater", RepeaterStorageConverter.ToString(_repeater));
                }
            }
            get
            {
                return _repeater;
            }
        }
        private void OnRepeaterInfoChanged()
        {
            OnPropertyChanged("Repeater");
            SQLUpdateProperty?.Invoke(ID, "Repeater", RepeaterStorageConverter.ToString(_repeater));
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
                    SQLUpdateProperty?.Invoke(ID, "Title", _title);
                }
            }
            get
            {
                return _title;
            }
        }
        //protected string _description;
        //public string Description
        //{
        //    set
        //    {
        //        if (_description != value)
        //        {
        //            _description = value;
        //            OnPropertyChanged("Description");
        //        }
        //    }
        //    get
        //    {
        //        return _description;
        //    }
        //}
        private ObservableCollection<Tag> _tags;
        public ObservableCollection<Tag> Tags
        {
            get => _tags;
            set
            {
                if (_tags != value)
                {
                    _tags = value;
                    _tags.CollectionChanged += TagsChanged;
                    OnPropertyChanged("Tags");
                    string tags;
                    if (_tags.Count == 0) tags = "";
                    else
                    {
                        tags = _tags[0].ID;
                        for (int i = 1; i < _tags.Count; ++i)
                        {
                            tags += "," + _tags[i].ID;
                        }
                    }
                    SQLUpdateProperty?.Invoke(ID, "Tags", tags);
                }
            }
        }

        private void TagsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            string tags;
            if (_tags.Count == 0) tags = "";
            else
            {
                tags = _tags[0].ID;
                for (int i = 1; i < _tags.Count; ++i)
                {
                    tags += "," + _tags[i].ID;
                }
            }
            SQLUpdateProperty?.Invoke(ID, "Tags", tags);
        }

        private string _id = "";
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
        private string _webAddress;
        public string WebAddress
        {
            set
            {
                if (_webAddress != value)
                {
                    _webAddress = value;
                    SQLUpdateProperty?.Invoke(ID, "WebAddress", _webAddress);
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
                    SQLUpdateProperty?.Invoke(ID, "ActivatedTime", value.ToString("yyyy-MM-dd HH:mm"));
                }
            }
            get => _activatedTime;
        }
        private DateTime _expiryTime;

        public event SQLUpdatePropertyChangedEventHandler SQLUpdateProperty;

        public DateTime ExpiryTime
        {
            set
            {
                if (_expiryTime != value)
                {
                    _expiryTime = value;
                    SQLUpdateProperty?.Invoke(ID, "ExpiryTime", value.ToString("yyyy-MM-dd HH:mm"));
                }
            }
            get => _expiryTime;
        }

        public bool HasNote { internal set; get; }

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
            //task._description = this._description;
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

