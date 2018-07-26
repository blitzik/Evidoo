using intf.Messages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace intf.Converters
{
    public class OverlayState2VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            OverlayState os = (OverlayState)value;
            switch (os) {
                case OverlayState.VISIBLE:
                    return Visibility.Visible;

                default:
                    return Visibility.Collapsed;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = (Visibility)value;
            switch (v) {
                case Visibility.Visible:
                    return OverlayState.VISIBLE;

                default:
                    return OverlayState.HIDDEN;
            }
        }
    }
}
