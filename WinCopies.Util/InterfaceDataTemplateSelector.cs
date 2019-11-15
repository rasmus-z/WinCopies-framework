using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WinCopies.Util
{
    public class InterfaceDataTemplateSelector : DataTemplateSelector
    {

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            if (item == null || !(container is FrameworkElement containerElement))

                return base.SelectTemplate(item, container);

            Type itemType = item.GetType();

            return Enumerable.Repeat(itemType, 1).Concat(itemType.GetInterfaces()).Select(t => containerElement.TryFindResource(new DataTemplateKey(t)))
                    .FirstOrDefault<DataTemplate>() ?? base.SelectTemplate(item, container);

        }

    }
}
