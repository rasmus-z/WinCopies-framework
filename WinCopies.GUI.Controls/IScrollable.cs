using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WinCopies.GUI.Controls
{
    // todo: better name?
    public interface IScrollable
    {
        ScrollViewer ScrollHost { get; }
    }
}
