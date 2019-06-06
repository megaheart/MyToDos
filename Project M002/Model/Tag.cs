using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MyToDos.Model
{
    public class Tag : NotifiableObject
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
        protected Brush _color;
        public Brush Color
        {
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged("Color");
                }
            }
            get
            {
                return _color;
            }
        }
    }
}
