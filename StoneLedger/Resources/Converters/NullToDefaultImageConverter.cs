using System.Globalization;

namespace StoneLedger.Resources.Converters;

public class NullToDefaultImageConverter : IValueConverter
{
    // TODO: replace with a shared configuration value once the API base address is centralized.
    private const string ApiBaseAddress = "http://localhost:5000";

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string url && !string.IsNullOrWhiteSpace(url))
        {
            // Local device file path (e.g. picked via FilePicker and stored as-is) - pass through unchanged.
            if (File.Exists(url))
                return url;

            // Already an absolute http(s) URI - pass through unchanged.
            if (Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                return url;

            return $"{ApiBaseAddress}/{url.TrimStart('/')}";
        }

        return "fallback.jpg"; // your fallback image
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
