using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CPE.Sales.Converters
{
    [ValueConversion(typeof (DateTime), typeof (SolidColorBrush))]
    public class DeliveryUrgencyBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targerType, object parameter, CultureInfo culture)
        {
            var date = (DateTime) value;

            if (date == DateTime.MinValue)
            {
                return new SolidColorBrush(Colors.DodgerBlue);
            }
            if (date <= DateTime.Today.AddDays(7))
            {
                return new SolidColorBrush(Color.FromRgb(238, 17, 17));
            }

            if (date <= DateTime.Today.AddDays(31))
            {
                return new SolidColorBrush(Color.FromRgb(255, 196, 13));
            }

            return new SolidColorBrush(Color.FromRgb(0, 163, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}