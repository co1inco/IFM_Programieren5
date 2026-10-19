using System.Globalization;
using System.Windows.Data;

namespace Cagd;

public class SubtractConverter : IValueConverter, IMultiValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is float f1 && parameter is float f2) return f1 - f2;
        if (value is double d1 && parameter is double d2) return d1 - d2;
        if (value is float f3 && float.TryParse(parameter?.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var f4)) return f3 - f4;
        if (value is double d3 && double.TryParse(parameter?.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var d4)) return d3 - d4;
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is [float f1, float f2]) return f1 - f2;
        if (values is [double d1, double d2]) return d1 - d2;
        if (values.Length > 0) return values[0];
        return 0;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
