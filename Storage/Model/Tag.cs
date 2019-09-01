using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class Tag : NotifiableObject,IIdentifiedObject, ISQLUpdatePropertyChanged
    {
        public bool IsDefault { get; private set; }
        public Tag()
        {
            IsDefault = false;
        }
        public Tag(bool isDefault)
        {
            IsDefault = isDefault;
        }
        public Tag(bool isDefault, string title, string id, string color)
        {
            IsDefault = isDefault;
            _title = title;
            _id = id;
            _color = color;
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
        protected string _id = "";
        public string ID
        {
            set
            {
                if (_id != "") throw new Exception("Can't set ID property of Tag object twice.");
                _id = value;
            }
            get
            {
                return _id;
            }
        }
        protected string _color;

        public event SQLUpdatePropertyChangedEventHandler SQLUpdateProperty;

        public string Color
        {
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged("Color");
                    SQLUpdateProperty?.Invoke(ID, "Color", _title);
                }
            }
            get
            {
                return _color;
            }
        }
    }
}
