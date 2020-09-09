using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyToDos.View.CustomizedControls
{
    public class RoundedButton : Button
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(RoundedButton), new UIPropertyMetadata(2));
        public CornerRadius CornerRadius
        {
            set => SetValue(CornerRadiusProperty, value);
            get => (CornerRadius)GetValue(CornerRadiusProperty);
        }
    }
}
