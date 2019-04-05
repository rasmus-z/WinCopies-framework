using System.Windows;
using System.Windows.Controls;

namespace WinCopies.GUI.Controls
{
    public class ListView : System.Windows.Controls.ListView, IScrollable, ISettableSelector
    {
        public ScrollViewer ScrollHost { get; private set; } = null;

        static ListView() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(typeof(ListView)));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ScrollHost = Template.FindName("PART_ScrollViewer", this) as ScrollViewer;
        }
    }
}
