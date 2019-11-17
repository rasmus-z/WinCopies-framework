/* Copyright © Pierre Sprimont, 2019
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

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
