using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WinCopies.Util.DataConverters
{

    /// <summary>
    /// Provides a base-class for any data-<see cref="MultiBinding"/> converter and can be directly use in an XAML view. This class can be used as a singleton class.
    /// </summary>
    public abstract class MultiConverterBase : MarkupExtension, IMultiValueConverter
    {

        private BooleanToVisibilityConverter _instance = null;

        public BooleanToVisibilityConverter Instance { get { if (_instance == null) _instance = new BooleanToVisibilityConverter(); return _instance; } }    

        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the <see cref="MultiBinding"/> produces. The value <see cref="System.Windows.DependencyProperty.UnsetValue"/> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value.</returns>
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);



        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);

        public override object ProvideValue(IServiceProvider serviceProvider) => this;

    }

}
