using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.GUI.Windows.Dialogs;
using System.Windows.Controls;
using WinCopies.GUI.Controls.Models;
using static WinCopies.Util.Util;
using WinCopies.Collections;
using WinCopies.Models;
using System.Collections;

namespace WinCopies.Models
{

    public interface IIsReadOnly
    {

        bool IsReadOnly { get; }

    }

}

namespace WinCopies.GUI.Windows.Dialogs.Models
{

    /// <summary>
    /// Represents a base model for dialog windows.
    /// </summary>
    public interface IDialogModelBase : IIsReadOnly
    {

        /// <summary>
        /// Gets or sets the title of this <see cref="IDialogModelBase"/>.
        /// </summary>
        /// <seealso cref="IIsReadOnly.IsReadOnly"/>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DialogButton"/> value of this <see cref="IDialogModelBase"/>.
        /// </summary>
        /// <seealso cref="IIsReadOnly.IsReadOnly"/>
        DialogButton DialogButton { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DefaultButton"/> value of this <see cref="IDialogModelBase"/>.
        /// </summary>
        /// <seealso cref="IIsReadOnly.IsReadOnly"/>
        DefaultButton DefaultButton { get; set; }

    }

    public class DialogModelBase : IDialogModelBase
    {

        /// <summary>
        /// Gets or sets the title of this <see cref="DialogModelBase"/>.
        /// </summary>
        /// <seealso cref="IsReadOnly"/>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DialogButton"/> value of this <see cref="DialogModelBase"/>.
        /// </summary>
        /// <seealso cref="IsReadOnly"/>
        public DialogButton DialogButton { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DefaultButton"/> value of this <see cref="DialogModelBase"/>.
        /// </summary>
        /// <seealso cref="IsReadOnly"/>
        public DefaultButton DefaultButton { get; set; }

        public virtual bool IsReadOnly { get; }

    }

    public interface IPropertyDialogModelBase : IDialogModelBase
    {

        IEnumerable<IPropertyTabItemModelBase> Items { get; set; }

    }

    public class PropertyDialogModelBase : DialogModelBase, IPropertyDialogModelBase
    {

        public IEnumerable<IPropertyTabItemModelBase> Items { get; set; }

    }

}

namespace WinCopies.GUI.Controls.Models
{

    public interface IContentControlModelBase : IIsReadOnly
    {

        /// <summary>
        /// Gets or sets the content of this <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        /// <seealso cref="IIsReadOnly.IsReadOnly"/>
        object Content { get; set; }

    }

    public interface IContentControlModelBase<T> : IIsReadOnly, IContentControlModelBase
    {

        /// <summary>
        /// Gets or sets the content of this <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        /// <seealso cref="IIsReadOnly.IsReadOnly"/>
        new T Content { get; set; }

    }

    public interface IHeaderedContentControlModelBase : IIsReadOnly, IContentControlModelBase
    {

        /// <summary>
        /// Gets or sets the header of this <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        /// <seealso cref="IIsReadOnly.IsReadOnly"/>
        object Header { get; set; }

    }

    public interface IHeaderedContentControlModelBase<THeader, TContent> : IIsReadOnly, IContentControlModelBase<TContent>, IHeaderedContentControlModelBase
    {

        /// <summary>
        /// Gets or sets the header of this <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        /// <seealso cref="IIsReadOnly.IsReadOnly"/>
        new THeader Header { get; set; }

    }

    public interface IItemsControlModelBase : IIsReadOnly
    {

        IEnumerable Items { get; set; }

    }

    public interface IItemsControlModelBase<T> : IIsReadOnly, IItemsControlModelBase
    {

        new IEnumerable<T> Items { get; set; }

    }

    public interface IHeaderedItemsControlModelBase : IIsReadOnly, IItemsControlModelBase
    {

        object Header { get; set; }

    }

    public interface IHeaderedItemsControlModelBase<THeader, TItems> : IIsReadOnly, IItemsControlModelBase<TItems>, IHeaderedItemsControlModelBase
    {

        new THeader Header { get; set; }

    }

    /// <summary>
    /// Represents a base model for <see cref="GroupBox"/> controls.
    /// </summary>
    public interface IGroupBoxModelBase : IIsReadOnly, IHeaderedContentControlModelBase
    {

    }

    /// <summary>
    /// Represents a base model for <see cref="GroupBox"/> controls.
    /// </summary>
    public interface IGroupBoxModelBase<THeader, TContent> : IGroupBoxModelBase, IHeaderedContentControlModelBase<THeader, TContent>
    {

    }

    public class GroupBoxModelBase : IGroupBoxModelBase
    {

        /// <summary>
        /// Gets or sets the header of this <see cref="GroupBoxModelBase"/>.
        /// </summary>
        /// <seealso cref="IsReadOnly"/>
        public object Header { get; set; }

        /// <summary>
        /// Gets or sets the content of this <see cref="GroupBoxModelBase"/>.
        /// </summary>
        /// <seealso cref="IsReadOnly"/>
        public object Content { get; set; }

        public virtual bool IsReadOnly { get; }

    }

    public class GroupBoxModelBase<THeader, TContent> : IGroupBoxModelBase<THeader, TContent>
    {

        /// <summary>
        /// Gets or sets the header of this <see cref="GroupBoxModelBase{THeader, TContent}"/>.
        /// </summary>
        /// <seealso cref="IsReadOnly"/>
        public THeader Header { get; set; }

        object IHeaderedContentControlModelBase.Header { get => Header; set => Header = GetOrThrowIfNotType<THeader>(value, nameof(value)); }

        /// <summary>
        /// Gets or sets the content of this <see cref="GroupBoxModelBase{THeader, TContent}"/>.
        /// </summary>
        /// <seealso cref="IsReadOnly"/>
        public TContent Content { get; set; }

        object IContentControlModelBase.Content { get => Content; set => Content = GetOrThrowIfNotType<TContent>(value, nameof(value)); }

        public virtual bool IsReadOnly { get; }

    }

    /// <summary>
    /// Represents a base model for <see cref="TabItem"/> controls.
    /// </summary>
    public interface ITabItemModelBase : IIsReadOnly, IHeaderedContentControlModelBase
    {

    }

    /// <summary>
    /// Represents a base model for <see cref="TabItem"/> controls.
    /// </summary>
    public interface ITabItemModelBase<THeader, TContent> : ITabItemModelBase, IHeaderedContentControlModelBase<THeader, TContent>
    {

    }

    /// <summary>
    /// Represents a base model for <see cref="TabItem"/> controls.
    /// </summary>
    public class TabItemModelBase : ITabItemModelBase
    {

        /// <summary>
        /// Gets or sets the header of this <see cref="TabItemModelBase"/>.
        /// </summary>
        /// <seealso cref="IsReadOnly"/>
        public object Header { get; set; }

        /// <summary>
        /// Gets or sets the content of this <see cref="TabItemModelBase"/>.
        /// </summary>
        /// <seealso cref="IsReadOnly"/>
        public object Content { get; set; }

        public virtual bool IsReadOnly { get; }

    }

    /// <summary>
    /// Represents a base model for <see cref="TabItem"/> controls.
    /// </summary>
    public class TabItemModelBase<THeader, TContent> : ITabItemModelBase<THeader, TContent>
    {

        /// <summary>
        /// Gets or sets the header of this <see cref="TabItemModelBase{THeader, TContent}"/>.
        /// </summary>
        /// <seealso cref="IsReadOnly"/>
        public THeader Header { get; set; }

        object IHeaderedContentControlModelBase.Header { get => Header; set => Header = GetOrThrowIfNotType<THeader>(value, nameof(value)); }

        /// <summary>
        /// Gets or sets the content of this <see cref="TabItemModelBase{THeader, TContent}"/>.
        /// </summary>
        /// <seealso cref="IIsReadOnly.IsReadOnly"/>
        public TContent Content { get; set; }

        object IContentControlModelBase.Content { get => Content; set => Content = GetOrThrowIfNotType<TContent>(value, nameof(value)); }

        public virtual bool IsReadOnly { get; }

    }

    public interface IPropertyTabItemModelBase : IIsReadOnly, IHeaderedItemsControlModelBase
    {

        new IEnumerable<IGroupBoxModelBase> Items { get; set; }

    }

    public interface IPropertyTabItemModelBase<TItemHeader, TGroupBoxHeader, TGroupBoxContent> : IPropertyTabItemModelBase

    {

        new IEnumerable<IGroupBoxModelBase<TGroupBoxHeader, TGroupBoxContent>> Items { get; set; }
    
    }

    public class PropertyTabItemModelBase : IPropertyTabItemModelBase
    {

        public object Header { get; set; }

        public IEnumerable<IGroupBoxModelBase> Items { get; set; }

        IEnumerable IItemsControlModelBase.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<IGroupBoxModelBase>>(value, nameof(value)); }

        public virtual bool IsReadOnly { get; }

    }

    public class PropertyTabItemModelBase<TItemHeader, TGroupBoxHeader, TGroupBoxContent> : IPropertyTabItemModelBase<TItemHeader, TGroupBoxHeader, TGroupBoxContent>
    {

        public TItemHeader Header { get; set; }

        object IHeaderedItemsControlModelBase.Header { get => Header; set => Header = GetOrThrowIfNotType<TItemHeader>(value, nameof(value)); }

        /// <summary>
        /// Gets or sets the items of this <see cref="ITabItemModelBase"/>.
        /// </summary>
        public IEnumerable<IGroupBoxModelBase<TGroupBoxHeader, TGroupBoxContent>> Items { get; set; }

        IEnumerable<IGroupBoxModelBase> IPropertyTabItemModelBase.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<IGroupBoxModelBase<TGroupBoxHeader, TGroupBoxContent>>>(value, nameof(value)); }

        IEnumerable IItemsControlModelBase.Items { get => Items; set => GetOrThrowIfNotType<IEnumerable<IGroupBoxModelBase<TGroupBoxHeader, TGroupBoxContent>>>(value, nameof(value)); }

        public virtual bool IsReadOnly { get; }

    }

}
