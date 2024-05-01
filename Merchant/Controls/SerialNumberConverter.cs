using System;
using System.Globalization;
using System.Windows.Data;

namespace Merchant.Controls
{
    public class SerialNumberConverter : IValueConverter
    {
        private int _counter = 1;
        public void ResetCounter()
        {
            _counter = 1;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _counter++;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
