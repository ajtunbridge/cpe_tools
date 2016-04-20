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
    public sealed class RescheduledDateUrgencyBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targerType, object parameter, CultureInfo culture)
        {
            var nullableDateTime = (DateTime?)value;

            if (!nullableDateTime.HasValue)
            {
                return Brushes.Black;
            }
            
            if (nullableDateTime.Value <= DateTime.Today.AddDays(7))
            {
                return new SolidColorBrush(Color.FromRgb(238, 17, 17));
            }

            if (nullableDateTime.Value <= DateTime.Today.AddDays(31))
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
