using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QuantumCryptoCram.Presentation.Converters
{
    public class BoolToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool successValue)
            {
                return successValue ? FontWeights.Bold : FontWeights.Normal;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}