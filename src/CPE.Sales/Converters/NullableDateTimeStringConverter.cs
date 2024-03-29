﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace CPE.Sales.Converters
{
    [ValueConversion(typeof (DateTime?), typeof (string))]
    public class NullableDateTimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targerType, object parameter, CultureInfo culture)
        {
            var nullableDateTime = (DateTime?) value;

            return nullableDateTime?.ToShortDateString() ?? "N/A";
        }

        public object ConvertBack(object value, Type targerType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}