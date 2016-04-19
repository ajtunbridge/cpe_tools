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
    public sealed class RescheduledDateUrgencyForegroundBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targerType, object parameter, CultureInfo culture)
        {
            var nullableDateTime = (DateTime?)value;

            return !nullableDateTime.HasValue ? Brushes.Black : Brushes.WhiteSmoke;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
