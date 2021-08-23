using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QuantumCryptoCram.Presentation.Converters
{
    internal class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool successValue)
            {
                return successValue ? "1" : "0";
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;
            switch (input)
            {
                case "1":
                    return true;

                case "0":
                    return false;

                case "":
                    return null;

                default:
                    return DependencyProperty.UnsetValue;
            }
        }
    }
}