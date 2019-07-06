using System;
using System.ComponentModel;

namespace MyToDos.Model
{
    public abstract class NotifiableObject : INotifyPropertyChanged
    {
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        [field: NonSerialized] public event PropertyChangedEventHandler PropertyChanged;
    }
}
