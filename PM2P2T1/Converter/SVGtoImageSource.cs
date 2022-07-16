using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Svg;

namespace PM2P2T1.Converter
{
    public class SVGtoImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var uriString = (string)value;
            Uri uri = new Uri(uriString);
            if (uri.AbsolutePath.ToLowerInvariant().EndsWith(".svg"))
            {
                return SvgImageSource.FromSvgUri(uriString, 50, 30, default(Color));
            }
            else
            {
                return ImageSource.FromUri(uri);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
