using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace WinCopies.GUI.Controls
{
    public partial class ProgressRectangle : Control
    {
        private FrameworkElement InnerRectangle = null;

        private Timer Timer { get; set; } = new Timer() { Interval = 10 };



        static ProgressRectangle() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressRectangle), new FrameworkPropertyMetadata(typeof(ProgressRectangle)));



        public ProgressRectangle() =>

            // InitializeComponent();

            Timer.Elapsed += Timer_Elapsed;



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InnerRectangle = GetTemplateChild("PART_InnerRectangle") as FrameworkElement;
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            if (InnerRectangle == null) return;

            InnerRectangle.Margin = new Thickness(InnerRectangle.Margin.Left + 1, 0, 0, 0);

            if (InnerRectangle.Margin.Left >= ActualWidth + 50) InnerRectangle.Margin = new Thickness(-50, 0, 0, 0);
        }

        private void this_SizeChanged(object sender, SizeChangedEventArgs e)

        {

            if (ActualWidth > 0 && !Timer.Enabled)

                Timer.Start();

            else if (ActualWidth == 0 && Timer.Enabled)

                Timer.Stop();



        }
    }
}
