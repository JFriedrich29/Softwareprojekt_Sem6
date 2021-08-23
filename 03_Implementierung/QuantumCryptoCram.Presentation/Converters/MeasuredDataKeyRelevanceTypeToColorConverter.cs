using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using QuantumCryptoCram.Domain.Data;

namespace QuantumCryptoCram.Presentation.Converters
{
    public class MeasuredDataKeyRelevanceTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MeasuredDataKeyRelevanceType measuredDataKeyRelevanceType)
            {
                switch (measuredDataKeyRelevanceType)
                {
                    case MeasuredDataKeyRelevanceType.AliceBobEveMatch:
                        return Brushes.LightGreen;

                    case MeasuredDataKeyRelevanceType.AliceBobMatch:
                        return Brushes.Orange;

                    case MeasuredDataKeyRelevanceType.AliceBobMatchButDiscarded:
                        return new SolidColorBrush(Color.FromRgb(255, 153, 153));

                    case MeasuredDataKeyRelevanceType.NoMatch:
                        return DependencyProperty.UnsetValue;
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}