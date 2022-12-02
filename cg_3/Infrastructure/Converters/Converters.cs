namespace cg_3.Infrastructure.Converters;

[ValueConversion(typeof(Vector2D), typeof(string))]
public class Vector2DToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var vector = value as Vector2D?;
        vector ??= Vector2D.Zero;
        return vector.ToString()!;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var str = value.ToString() ?? Vector2D.Zero.ToString();
        Vector2D.TryParse(str, out var vector);
        return vector;
    }
}