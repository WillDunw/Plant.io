using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.Converters
{
    public class BoolToColor : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((bool)value)
                {
                    return Color.FromHex("#FFA500");
                }
                else
                {
                    return Color.FromHex("#808080");
                }
            }
            else
            {
                return Color.FromHex("#800080");
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
