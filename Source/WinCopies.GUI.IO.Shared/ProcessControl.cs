using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WinCopies.GUI.Controls;
using Size = WinCopies.IO.Size;

namespace WinCopies.GUI.IO
{
    public enum ProcessStatus : sbyte
    {
        None,

        InProgress,

        Succeeded,

        CancelledByUser,

        Error
    }

    /// <summary>
    /// Represents an I/O process control.
    /// </summary>
    public class ProcessControl : HeaderedControl
    {
        /// <summary>
        /// Identifies the <see cref="SourcePath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SourcePathProperty = DependencyProperty.Register(nameof(SourcePath), typeof(string), typeof(ProcessControl));

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        public string SourcePath { get => (string)GetValue(SourcePathProperty); set => SetValue(SourcePathProperty, value); }

        /// <summary>
        /// Identifies the <see cref="ArePathsLoaded"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ArePathsLoadedProperty = DependencyProperty.Register(nameof(ArePathsLoaded), typeof(bool), typeof(ProcessControl));

        /// <summary>
        /// Gets or sets a value that indicates whether the paths of the associated process are loaded.
        /// </summary>
        public bool ArePathsLoaded { get => (bool)GetValue(ArePathsLoadedProperty); set => SetValue(ArePathsLoadedProperty, value); }

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof(Status), typeof(ProcessStatus), typeof(ProcessControl));

        public ProcessStatus Status { get => (ProcessStatus)GetValue(StatusProperty); set => SetValue(StatusProperty, value); } 

        /// <summary>
        /// Identifies the <see cref="InitialItemSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InitialItemSizeProperty = DependencyProperty.Register(nameof(InitialItemSize), typeof(Size), typeof(ProcessControl));

        /// <summary>
        /// Gets or sets the initial item size.
        /// </summary>
        public Size InitialItemSize { get => (Size)GetValue(InitialItemSizeProperty); set => SetValue(InitialItemSizeProperty, value); }

        /// <summary>
        /// Identifies the <see cref="InitialItemCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty InitialItemCountProperty = DependencyProperty.Register(nameof(InitialItemCount), typeof(int), typeof(ProcessControl));

        /// <summary>
        /// Gets or sets the initial item count.
        /// </summary>
        public int InitialItemCount { get => (int)GetValue(InitialItemCountProperty); set => SetValue(InitialItemCountProperty, value); }

        /// <summary>
        /// Identifies the <see cref="RemainingItemSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RemainingItemSizeProperty = DependencyProperty.Register(nameof(RemainingItemSize), typeof(Size), typeof(ProcessControl));

        /// <summary>
        /// Gets or sets the Remaining item size.
        /// </summary>
        public Size RemainingItemSize { get => (Size)GetValue(RemainingItemSizeProperty); set => SetValue(RemainingItemSizeProperty, value); }

        /// <summary>
        /// Identifies the <see cref="RemainingItemCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RemainingItemCountProperty = DependencyProperty.Register(nameof(RemainingItemCount), typeof(int), typeof(ProcessControl));

        /// <summary>
        /// Gets or sets the Remaining item count.
        /// </summary>
        public int RemainingItemCount { get => (int)GetValue(RemainingItemCountProperty); set => SetValue(RemainingItemCountProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CurrentPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentPathProperty = DependencyProperty.Register(nameof(CurrentPath), typeof(IPathInfo), typeof(ProcessControl));

        public IPathInfo CurrentPath { get => (IPathInfo)GetValue(CurrentPathProperty); set => SetValue(CurrentPathProperty, value); }

        /// <summary>
        /// Identifies the <see cref="ProgressPercentage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ProgressPercentageProperty = DependencyProperty.Register(nameof(ProgressPercentage), typeof(sbyte), typeof(ProcessControl));

        public sbyte ProgressPercentage { get => (sbyte)GetValue(ProgressPercentageProperty); set => SetValue(ProgressPercentageProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CurrentPathProgressPercentage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentPathProgressPercentageProperty = DependencyProperty.Register(nameof(CurrentPathProgressPercentage), typeof(sbyte), typeof(ProcessControl));

        public sbyte CurrentPathProgressPercentage { get => (sbyte)GetValue(CurrentPathProgressPercentageProperty); set => SetValue(CurrentPathProgressPercentageProperty, value); }

        static ProcessControl() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ProcessControl), new FrameworkPropertyMetadata(typeof(ProcessControl)));
    }

    public class CopyProcessControl : ProcessControl
    {
        public static readonly DependencyProperty DestPathProperty = DependencyProperty.Register(nameof(DestPath), typeof(string), typeof(CopyProcessControl));

        public string DestPath { get => (string)GetValue(DestPathProperty); set => SetValue(DestPathProperty, value); }
    }
}
