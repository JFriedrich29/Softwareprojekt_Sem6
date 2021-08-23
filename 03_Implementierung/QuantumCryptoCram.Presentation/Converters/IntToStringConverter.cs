using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QuantumCryptoCram.Presentation.Converters
{
    public class NumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;
            int output;
            if (int.TryParse(input, out output))
                return output;
            else
                return DependencyProperty.UnsetValue;
        }
    }
}