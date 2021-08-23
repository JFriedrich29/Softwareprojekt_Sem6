using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using QuantumCryptoCram.Domain.Quantum;

namespace QuantumCryptoCram.Presentation.Converters
{
    public class PolarisationToImagesourceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Polarisation? pol = null;
            DataBit? bit = null;

            foreach (object item in values)
            {
                if (item is Polarisation polarisation)
                {
                    pol = polarisation;
                }

                if (item is DataBit dataBit)
                {
                    bit = dataBit;
                }
            }

            Uri uri = GetImage(pol, bit);

            if (uri == null)
            {
                return DependencyProperty.UnsetValue;
            }

            BitmapImage img = new BitmapImage(uri);
            return img;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Uri GetImage(Polarisation? pol, DataBit? bit)
        {
            if (!pol.HasValue || !bit.HasValue)
            {
                return null;
            }

            if (pol == Polarisation.Diagonal && bit == DataBit.Zero)
            {
                return new Uri("/Ressources/diagonal_left.png", UriKind.Relative);
            }
            else if (pol == Polarisation.Diagonal && bit == DataBit.One)
            {
                return new Uri("/Ressources/diagonal_right.png", UriKind.Relative);
            }
            else if (pol == Polarisation.Rectilinear && bit == DataBit.One)
            {
                return new Uri("/Ressources/horizontal_up.png", UriKind.Relative);
            }
            else if (pol == Polarisation.Rectilinear && bit == DataBit.Zero)
            {
                return new Uri("/Ressources/horizontal_right.png", UriKind.Relative);
            }

            return null;
        }
    }
}