﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MyToDos.View.Converters
{
    
    /// <summary>
    /// Calculate CustomizedTabControl>UniformGrid(Name="HeaderPanel") number of Column from TabStripPlacement
    /// </summary>
    public class DockToColumns : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Dock dock = (Dock)value;
            if (dock == Dock.Left || dock == Dock.Right) return 1;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
