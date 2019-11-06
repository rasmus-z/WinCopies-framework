using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.GUI.Windows.Dialogs;
using System.Windows.Controls;
using WinCopies.GUI.Controls.Models;

namespace WinCopies.GUI.Windows.Dialogs.Models
{

    public interface IDialogModelBase
    {

        string Title { get; set; }

        DialogButton DialogButton { get; set; }

        DefaultButton DefaultButton { get; set; }

    }

    public class DialogModelBase : IDialogModelBase
    {

        public string Title { get; set; }

        public DialogButton DialogButton { get; set; }

        public DefaultButton DefaultButton { get; set; }

    }

    public interface IPropertyDialogModelBase : IDialogModelBase
    {

        ICollection<IPropertyTabItemModelBase> Items { get; set; }

    }

    public class PropertyDialogModelBase : DialogModelBase, IPropertyDialogModelBase
    {

        public ICollection<IPropertyTabItemModelBase> Items { get; set; }

    }

}

namespace WinCopies.GUI.Controls.Models
{

    /// <summary>
    /// Represents a base model for <see cref="GroupBox"/> controls.
    /// </summary>
    public interface IGroupBoxModelBase
    {

        /// <summary>
        /// Gets or sets the header of the current <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        object Header { get; set; }

        /// <summary>
        /// Gets or sets the content of the current <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        object Content { get; set; }

    }

    /// <summary>
    /// Represents a base model for <see cref="GroupBox"/> controls.
    /// </summary>
    public interface IGroupBoxModelBase<THeader, TContent>
    {

        /// <summary>
        /// Gets or sets the header of the current <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        THeader Header { get; set; }

        /// <summary>
        /// Gets or sets the content of the current <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        TContent Content { get; set; }

    }

    public class GroupBoxModelBase
    {

        /// <summary>
        /// Gets or sets the header of the current <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        public object Header { get; set; }

        /// <summary>
        /// Gets or sets the content of the current <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        public object Content { get; set; }

    }

    public class GroupBoxModelBase<THeader, TContent>
    {

        /// <summary>
        /// Gets or sets the header of the current <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        public THeader Header { get; set; }

        /// <summary>
        /// Gets or sets the content of the current <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        public TContent Content { get; set; }

    }

    /// <summary>
    /// Represents a base model for <see cref="TabItem"/> controls.
    /// </summary>
    public interface ITabItemModelBase
    {

        /// <summary>
        /// Gets or sets the header of the current <see cref="ITabItemModelBase"/>.
        /// </summary>
        object Header { get; set; }

        object Content { get; set; }

    }

    /// <summary>
    /// Represents a base model for <see cref="TabItem"/> controls.
    /// </summary>
    public interface ITabItemModelBase<THeader, TContent>
    {

        /// <summary>
        /// Gets or sets the header of the current <see cref="ITabItemModelBase"/>.
        /// </summary>
        THeader Header { get; set; }

        TContent Content { get; set; }

    }

    /// <summary>
    /// Represents a base model for <see cref="TabItem"/> controls.
    /// </summary>
    public class TabItemModelBase
    {

        /// <summary>
        /// Gets or sets the header of the current <see cref="ITabItemModelBase"/>.
        /// </summary>
        public object Header { get; set; }

        public object Content { get; set; }

    }

    /// <summary>
    /// Represents a base model for <see cref="TabItem"/> controls.
    /// </summary>
    public class TabItemModelBase<THeader, TContent>
    {

        /// <summary>
        /// Gets or sets the header of the current <see cref="ITabItemModelBase"/>.
        /// </summary>
        public THeader Header { get; set; }

        public TContent Content { get; set; }

    }

    public interface IPropertyTabItemModelBase
    {

        object Header { get; set; }

        /// <summary>
        /// Gets or sets the items of the current <see cref="ITabItemModelBase"/>.
        /// </summary>
        ICollection<IGroupBoxModelBase> Items { get; set; }

    }

    public interface IPropertyTabItemModelBase<T>
    {

        T Header { get; set; }

        /// <summary>
        /// Gets or sets the items of the current <see cref="ITabItemModelBase"/>.
        /// </summary>
        ICollection<IGroupBoxModelBase> Items { get; set; }

    }

    public class PropertyTabItemModelBase
    {

        public object Header { get; set; }

        /// <summary>
        /// Gets or sets the items of the current <see cref="ITabItemModelBase"/>.
        /// </summary>
        public ICollection<IGroupBoxModelBase> Items { get; set; }

    }

    public class PropertyTabItemModelBase<T>
    {

        public T Header { get; set; }

        /// <summary>
        /// Gets or sets the items of the current <see cref="ITabItemModelBase"/>.
        /// </summary>
        public ICollection<IGroupBoxModelBase> Items { get; set; }

    }

}
