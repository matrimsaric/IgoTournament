using System.Globalization;

namespace StoneLedger.Resources.Converters;

public class NullToDefaultImageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string url && !string.IsNullOrWhiteSpace(url))
            return url;

        return "fallback.jpg"; // your fallback image
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
