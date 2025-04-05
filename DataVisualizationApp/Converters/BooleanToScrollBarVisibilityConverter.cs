using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace DataVisualizationApp.Converters
{
    public class BooleanToScrollBarVisibilityConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool isVisible)
            {
                return isVisible ? "Auto" : "Disabled"; // Return string values for Avalonia
            }
            return "Disabled";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}