using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Model
{
    public class Tag : IdentifiedObject, ISQLUpdatePropertyChanged
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
        //internal void ResetID()
        //{
            
        //}
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
