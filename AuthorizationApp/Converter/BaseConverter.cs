using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace AuthorizationApp.Converter
{
    public abstract class BaseConverter<T> : MarkupExtension, IValueConverter where T : class, new()
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new T();
            return _converter;
        }

        private static T _converter = null;
    }
}