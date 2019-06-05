using System;
using System.ComponentModel;

namespace MyToDos.Model
{
    public abstract class NotifiableObject : INotifyPropertyChanged
    {
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;
    }
}
