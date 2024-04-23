using System;
using System.Globalization;

namespace Merchant.Helper
{
    public class CellTextFormater : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double weight)
            {
                string formattedWeight = $"{weight} Kgs";

                System.Windows.Controls.TextBlock textBlock = new System.Windows.Controls.TextBlock();
                textBlock.Text = formattedWeight;
                textBlock.Foreground = System.Windows.Media.Brushes.Red;

                return textBlock;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}