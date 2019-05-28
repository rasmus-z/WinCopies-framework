using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WinCopies.Util
{
    public delegate void RoutedEventHandler<T>(object sender, RoutedEventArgs<T> e) where T : EventArgs;

    public class RoutedEventArgs<T> : System.Windows.RoutedEventArgs where T : EventArgs
    {

        public T OriginalEventArgs { get; } = null;

        public RoutedEventArgs(T originalEventArgs) => OriginalEventArgs = originalEventArgs;

        public RoutedEventArgs(RoutedEvent routedEvent, T originalEventArgs) : base(routedEvent) => OriginalEventArgs = originalEventArgs;

        public RoutedEventArgs(RoutedEvent routedEvent, object source, T originalEventArgs) : base(routedEvent, source) => OriginalEventArgs = originalEventArgs;

    }
}
