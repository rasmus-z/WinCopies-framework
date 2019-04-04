using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WinCopies.Util.DataConverters
{

    /// <summary>
    /// Provides a base-class for any data <see cref="Binding"/> converter and can be directly use in an XAML view. This class can be used as a singleton class.
    /// </summary>
    public abstract class ConverterBase : MarkupExtension, IValueConverter
    {

        private BooleanToVisibilityConverter _instance = null;

        public BooleanToVisibilityConverter Instance { get { if (_instance == null) _instance = new BooleanToVisibilityConverter(); return _instance; } }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null"/>, the valid null value is used.</returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);


        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null"/>, the valid null value is used.</returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        public override object ProvideValue(IServiceProvider serviceProvider) => this;

    }

}
