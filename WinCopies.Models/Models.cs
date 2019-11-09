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
using System.Collections;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;

namespace WinCopies.GUI.Windows.Dialogs.Models
{

    /// <summary>
    /// Represents a base model that corresponds to a default view for dialog windows.
    /// </summary>
    public interface IDialogModelBase
    {

        /// <summary>
        /// Gets or sets the title of this <see cref="IDialogModelBase"/>.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DialogButton"/> value of this <see cref="IDialogModelBase"/>.
        /// </summary>
        DialogButton DialogButton { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DefaultButton"/> value of this <see cref="IDialogModelBase"/>.
        /// </summary>
        DefaultButton DefaultButton { get; set; }

    }

    public class DialogModelBase : IDialogModelBase
    {

        /// <summary>
        /// Gets or sets the title of this <see cref="DialogModelBase"/>.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DialogButton"/> value of this <see cref="DialogModelBase"/>.
        /// </summary>
        public DialogButton DialogButton { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DefaultButton"/> value of this <see cref="DialogModelBase"/>.
        /// </summary>
        public DefaultButton DefaultButton { get; set; }

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

    public interface IContentControlModelBase
    {

        /// <summary>
        /// Gets or sets the content of this <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        object Content { get; set; }

    }

    public class ContentControlModelBase : IContentControlModelBase
    {

        public object Content { get; set; }

    }

    public interface IContentControlModelBase<T> : IContentControlModelBase
    {

        /// <summary>
        /// Gets or sets the content of this <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        new T Content { get; set; }

    }

    public class ContentControlModelBase<T> : IContentControlModelBase<T>

    {

        public T Content { get; set; }

        object IContentControlModelBase.Content { get => Content; set => Content = GetOrThrowIfNotType<T>(value, nameof(value)); }

    }

    public interface IHeaderedControlModelBase

    {

        /// <summary>
        /// Gets or sets the header of this <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        object Header { get; set; }

    }

    public interface IHeaderedContentControlModelBase : IContentControlModelBase, IHeaderedControlModelBase
    {



    }

    public class HeaderedContentControlModelBase : ContentControlModelBase, IHeaderedContentControlModelBase

    {

        public object Header { get; set; }

    }

    public interface IHeaderedControlModelBase<T> : IHeaderedControlModelBase

    {

        /// <summary>
        /// Gets or sets the header of this <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        new T Header { get; set; }

    }

    public interface IHeaderedContentControlModelBase<THeader, TContent> : IContentControlModelBase<TContent>, IHeaderedControlModelBase<THeader>
    {



    }

    public class HeaderedContentControlModelBase<THeader, TContent> : ContentControlModelBase<TContent>, IHeaderedControlModelBase<THeader>

    {

        public THeader Header { get; set; }

        object IHeaderedControlModelBase.Header { get => Header; set => Header = GetOrThrowIfNotType<THeader>(value, nameof(value)); }

    }

    public interface IItemsControlModelBase
    {

        IEnumerable Items { get; set; }

    }

    public class ItemsControlModelBase : IItemsControlModelBase
    {

        public IEnumerable Items { get; set; }

    }

    public interface IItemsControlModelBase<T> : IItemsControlModelBase
    {

        new IEnumerable<T> Items { get; set; }

    }

    public class ItemsControlModelBase<T> : IItemsControlModelBase<T>

    {

        public IEnumerable<T> Items { get; set; }

        IEnumerable IItemsControlModelBase.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<T>>(value, nameof(value)); }

    }

    public interface IHeaderedItemsControlModelBase : IItemsControlModelBase, IHeaderedControlModelBase
    {



    }

    public class HeaderedItemsControlModelBase : ItemsControlModelBase, IHeaderedItemsControlModelBase

    {

        public object Header { get; set; }

    }

    public interface IHeaderedItemsControlModelBase<THeader, TItems> : IItemsControlModelBase<TItems>, IHeaderedItemsControlModelBase
    {

    }

    public class HeaderedItemsControlModelBase<THeader, TItems> : ItemsControlModelBase<TItems>, IHeaderedItemsControlModelBase<THeader, TItems>

    {

        public THeader Header { get; set; }

        object IHeaderedControlModelBase.Header { get => Header; set => Header = GetOrThrowIfNotType<THeader>(value, nameof(value)); }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="GroupBox"/> controls.
    /// </summary>
    public interface IGroupBoxModelBase : IHeaderedContentControlModelBase
    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="GroupBox"/> controls.
    /// </summary>
    public interface IGroupBoxModelBase<THeader, TContent> : IGroupBoxModelBase, IHeaderedContentControlModelBase<THeader, TContent>
    {

    }

    public class GroupBoxModelBase : HeaderedContentControlModelBase, IGroupBoxModelBase
    {



    }

    public class GroupBoxModelBase<THeader, TContent> : HeaderedContentControlModelBase<THeader, TContent>, IGroupBoxModelBase<THeader, TContent>
    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="TabItem"/> controls.
    /// </summary>
    public interface ITabItemModelBase : IHeaderedContentControlModelBase
    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="TabItem"/> controls.
    /// </summary>
    public interface ITabItemModelBase<THeader, TContent> : ITabItemModelBase, IHeaderedContentControlModelBase<THeader, TContent>
    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="TabItem"/> controls.
    /// </summary>
    public class TabItemModelBase : HeaderedContentControlModelBase, ITabItemModelBase
    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="TabItem"/> controls.
    /// </summary>
    public class TabItemModelBase<THeader, TContent> : HeaderedContentControlModelBase<THeader, TContent>, ITabItemModelBase<THeader, TContent>
    {



    }

    public interface IPropertyTabItemModelBase : IHeaderedItemsControlModelBase
    {

        new IEnumerable<IGroupBoxModelBase> Items { get; set; }

    }

    public interface IPropertyTabItemModelBase<TItemHeader, TGroupBoxHeader, TGroupBoxContent> : IPropertyTabItemModelBase, IHeaderedItemsControlModelBase<TItemHeader, IGroupBoxModelBase<TGroupBoxHeader, TGroupBoxContent>>

    {



    }

    public class PropertyTabItemModelBase : IPropertyTabItemModelBase
    {

        public object Header { get; set; }

        public IEnumerable<IGroupBoxModelBase> Items { get; set; }

        IEnumerable IItemsControlModelBase.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<IGroupBoxModelBase>>(value, nameof(value)); }

    }

    public class PropertyTabItemModelBase<TItemHeader, TGroupBoxHeader, TGroupBoxContent> : HeaderedItemsControlModelBase<TItemHeader, IGroupBoxModelBase<TGroupBoxHeader, TGroupBoxContent>>, IPropertyTabItemModelBase<TItemHeader, TGroupBoxHeader, TGroupBoxContent>
    {

        IEnumerable<IGroupBoxModelBase> IPropertyTabItemModelBase.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<IGroupBoxModelBase<TGroupBoxHeader, TGroupBoxContent>>>(value, nameof(value)); }

    }

    public interface IButtonModelBase : IContentControlModelBase, ICommandSource

    {



    }

    public class ButtonModelBase : ContentControlModelBase, IButtonModelBase

    {

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        public IInputElement CommandTarget { get; set; }

    }

    public interface IButtonModelBase<T> : IButtonModelBase, IContentControlModelBase<T>

    {



    }

    public class ButtonModelBase<T> : ContentControlModelBase<T>, IButtonModelBase<T>

    {

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        public IInputElement CommandTarget { get; set; }

    }

    public interface IToggleButtonModelBase : IButtonModelBase

    {

        bool? IsChecked { get; set; }

        bool IsThreeState { get; set; }

    }

    public class ToggleButtonModelBase : ButtonModelBase

    {

        public bool? IsChecked { get; set; }

        public bool IsThreeState { get; set; }

    }

    public interface IToggleButtonModelBase<T> : IToggleButtonModelBase, IButtonModelBase<T>

    {



    }

    public class ToggleButtonModelBase<T> : ButtonModelBase<T>, IToggleButtonModelBase<T>

    {

        public bool? IsChecked { get; set; }

        public bool IsThreeState { get; set; }
    
    }

    public interface ICheckBoxModelBase : IToggleButtonModelBase
    {

    }

    public interface ICheckBoxModelBase<T> : IToggleButtonModelBase<T>, ICheckBoxModelBase
    {



    }

    public interface ITextBoxModelTextOrientedBase

    {

        public string Text { get; set; }
        public bool IsReadOnly { get; set; }

    }

    public interface ITextBoxModelSelectionOriented : ITextBoxModelTextOrientedBase

    {
        public int CaretIndex { get; set; }
        public int SelectionLength { get; set; }
        public int SelectionStart { get; set; }
        public string SelectedText { get; set; }
        public bool IsReadOnlyCaretVisible { get; set; }
        public bool AutoWordSelection { get; set; }
        public Brush SelectionBrush { get; set; }
        public Brush SelectionTextBrush { get; set; }
        public Brush CaretBrush { get; set; }
        public bool IsSelectionActive { get; }
        public bool IsInactiveSelectionHighlightEnabled { get; set; }



    }

    public interface ITextBoxModelTextEditingOriented : ITextBoxModelTextOrientedBase

    {

        public int MinLines { get; set; }
        public int MaxLines { get; set; }
        public CharacterCasing CharacterCasing { get; set; }
        public int MaxLength { get; set; }
        public TextAlignment TextAlignment { get; set; }
        public int LineCount { get; }
        public TextDecorationCollection TextDecorations { get; set; }
        public TextWrapping TextWrapping { get; set; }
        public bool AcceptsReturn { get; set; }
        public bool AcceptsTab { get; set; }
        public double SelectionOpacity { get; set; }
        public bool CanUndo { get; }
        public bool CanRedo { get; }
        public bool IsUndoEnabled { get; set; }
        public int UndoLimit { get; set; }

    }

    public interface ITextBoxModelBase : ITextBoxModelTextOrientedBase, ITextBoxModelTextEditingOriented, ITextBoxModelSelectionOriented

    {



    }

    public interface IRadioButtonCollection : IItemsControlModelBase<IRadioButton>

    {

        string GroupName { get; set; }

    }

    public interface IRadioButtonCollection<T> : IRadioButtonCollection, IItemsControlModelBase<IRadioButton<T>>

    {



    }

    public interface IRadioButton : IToggleButtonModelBase

    {



    }

    public interface IRadioButton<T> : IRadioButton, IToggleButtonModelBase<T>

    {



    }

    public interface IGroupingRadioButtonModelBase : IContentControlModelBase

    {

        string GroupName { get; set; }

    }

}
