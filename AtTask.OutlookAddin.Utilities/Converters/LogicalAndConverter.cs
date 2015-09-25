using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace AtTask.OutlookAddin.Utilities.Converters
{
    public class LogicalAndConverter : MarkupExtension, IMultiValueConverter
    {
        public LogicalAndConverter()
        {
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Cast<bool>().All(v => v);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}