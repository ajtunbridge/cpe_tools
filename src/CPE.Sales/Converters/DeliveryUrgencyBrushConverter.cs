using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CPE.Sales.Converters
{
    [ValueConversion(typeof(DateTime), typeof(SolidColorBrush))]
    public class DeliveryUrgencyBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targerType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;

            if (date <= DateTime.Today.AddDays(7))
            {
                return new SolidColorBrush(Colors.Firebrick);
            }

            if (date <= DateTime.Today.AddDays(31))
            {
                return new SolidColorBrush(Colors.DarkGoldenrod);
            }

            return new SolidColorBrush(Colors.DarkGreen);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
