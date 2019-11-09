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
using System.Windows.Controls.Primitives;

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

    /// <summary>
    /// Represents a base model that corresponds to a default view for dialog windows.
    /// </summary>
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

    /// <summary>
    /// Represents a base model that corresponds to a default view for property dialog windows.
    /// </summary>
    public interface IPropertyDialogModelBase : IDialogModelBase
    {

        IEnumerable<IPropertyTabItemModelBase> Items { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for property dialog windows.
    /// </summary>
    public class PropertyDialogModelBase : DialogModelBase, IPropertyDialogModelBase
    {

        public IEnumerable<IPropertyTabItemModelBase> Items { get; set; }

    }

}

namespace WinCopies.GUI.Controls.Models
{

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="ContentControl"/>s.
    /// </summary>
    public interface IContentControlModelBase
    {

        /// <summary>
        /// Gets or sets the content of this <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        object Content { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="ContentControl"/>s.
    /// </summary>
    public class ContentControlModelBase : IContentControlModelBase
    {

        public object Content { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="ContentControl"/>s.
    /// </summary>
    public interface IContentControlModelBase<T> : IContentControlModelBase
    {

        /// <summary>
        /// Gets or sets the content of this <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        new T Content { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="ContentControl"/>s.
    /// </summary>
    public class ContentControlModelBase<T> : IContentControlModelBase<T>

    {

        public T Content { get; set; }

        object IContentControlModelBase.Content { get => Content; set => Content = GetOrThrowIfNotType<T>(value, nameof(value)); }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for headered controls.
    /// </summary>
    public interface IHeaderedControlModelBase

    {

        /// <summary>
        /// Gets or sets the header of this <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        object Header { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="HeaderedContentControl"/>s.
    /// </summary>
    public interface IHeaderedContentControlModelBase : IContentControlModelBase, IHeaderedControlModelBase
    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="HeaderedContentControl"/>s.
    /// </summary>
    public class HeaderedContentControlModelBase : ContentControlModelBase, IHeaderedContentControlModelBase

    {

        public object Header { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for headered controls.
    /// </summary>
    public interface IHeaderedControlModelBase<T> : IHeaderedControlModelBase

    {

        /// <summary>
        /// Gets or sets the header of this <see cref="IGroupBoxModelBase"/>.
        /// </summary>
        new T Header { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="HeaderedContentControl"/>s.
    /// </summary>
    public interface IHeaderedContentControlModelBase<THeader, TContent> : IContentControlModelBase<TContent>, IHeaderedControlModelBase<THeader>
    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="HeaderedContentControl"/>s.
    /// </summary>
    public class HeaderedContentControlModelBase<THeader, TContent> : ContentControlModelBase<TContent>, IHeaderedControlModelBase<THeader>

    {

        public THeader Header { get; set; }

        object IHeaderedControlModelBase.Header { get => Header; set => Header = GetOrThrowIfNotType<THeader>(value, nameof(value)); }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="ItemsControl"/>s.
    /// </summary>
    public interface IItemsControlModelBase
    {

        IEnumerable Items { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="ItemsControl"/>s.
    /// </summary>
    public class ItemsControlModelBase : IItemsControlModelBase
    {

        public IEnumerable Items { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="ItemsControl"/>s.
    /// </summary>
    public interface IItemsControlModelBase<T> : IItemsControlModelBase
    {

        new IEnumerable<T> Items { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="ItemsControl"/>s.
    /// </summary>
    public class ItemsControlModelBase<T> : IItemsControlModelBase<T>

    {

        public IEnumerable<T> Items { get; set; }

        IEnumerable IItemsControlModelBase.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<T>>(value, nameof(value)); }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="HeaderedItemsControl"/>s.
    /// </summary>
    public interface IHeaderedItemsControlModelBase : IItemsControlModelBase, IHeaderedControlModelBase
    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="HeaderedItemsControl"/>s.
    /// </summary>
    public class HeaderedItemsControlModelBase : ItemsControlModelBase, IHeaderedItemsControlModelBase

    {

        public object Header { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="HeaderedItemsControl"/>s.
    /// </summary>
    public interface IHeaderedItemsControlModelBase<THeader, TItems> : IItemsControlModelBase<TItems>, IHeaderedItemsControlModelBase
    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="HeaderedItemsControl"/>s.
    /// </summary>
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

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="GroupBox"/> controls.
    /// </summary>
    public class GroupBoxModelBase : HeaderedContentControlModelBase, IGroupBoxModelBase
    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="GroupBox"/> controls.
    /// </summary>
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

    /// <summary>
    /// Represents a base model that corresponds to a default view for property tab items.
    /// </summary>
    public interface IPropertyTabItemModelBase : IHeaderedItemsControlModelBase
    {

        new IEnumerable<IGroupBoxModelBase> Items { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for property tab items.
    /// </summary>
    public interface IPropertyTabItemModelBase<TItemHeader, TGroupBoxHeader, TGroupBoxContent> : IPropertyTabItemModelBase, IHeaderedItemsControlModelBase<TItemHeader, IGroupBoxModelBase<TGroupBoxHeader, TGroupBoxContent>>

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for property tab items.
    /// </summary>
    public class PropertyTabItemModelBase : IPropertyTabItemModelBase
    {

        public object Header { get; set; }

        public IEnumerable<IGroupBoxModelBase> Items { get; set; }

        IEnumerable IItemsControlModelBase.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<IGroupBoxModelBase>>(value, nameof(value)); }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for property tab items.
    /// </summary>
    public class PropertyTabItemModelBase<TItemHeader, TGroupBoxHeader, TGroupBoxContent> : HeaderedItemsControlModelBase<TItemHeader, IGroupBoxModelBase<TGroupBoxHeader, TGroupBoxContent>>, IPropertyTabItemModelBase<TItemHeader, TGroupBoxHeader, TGroupBoxContent>
    {

        IEnumerable<IGroupBoxModelBase> IPropertyTabItemModelBase.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<IGroupBoxModelBase<TGroupBoxHeader, TGroupBoxContent>>>(value, nameof(value)); }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="Button"/>s.
    /// </summary>
    public interface IButtonModelBase : IContentControlModelBase, ICommandSource

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="Button"/>s.
    /// </summary>
    public class ButtonModelBase : ContentControlModelBase, IButtonModelBase

    {

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        public IInputElement CommandTarget { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="Button"/>s.
    /// </summary>
    public interface IButtonModelBase<T> : IButtonModelBase, IContentControlModelBase<T>

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="Button"/>s.
    /// </summary>
    public class ButtonModelBase<T> : ContentControlModelBase<T>, IButtonModelBase<T>

    {

        public ICommand Command { get; set; }

        public object CommandParameter { get; set; }

        public IInputElement CommandTarget { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="ToggleButton"/>s.
    /// </summary>
    public interface IToggleButtonModelBase : IButtonModelBase

    {

        bool? IsChecked { get; set; }

        bool IsThreeState { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="ToggleButton"/>s.
    /// </summary>
    public class ToggleButtonModelBase : ButtonModelBase

    {

        public bool? IsChecked { get; set; }

        public bool IsThreeState { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="ToggleButton"/>s.
    /// </summary>
    public interface IToggleButtonModelBase<T> : IToggleButtonModelBase, IButtonModelBase<T>

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="ToggleButton"/>s.
    /// </summary>
    public class ToggleButtonModelBase<T> : ButtonModelBase<T>, IToggleButtonModelBase<T>

    {

        public bool? IsChecked { get; set; }

        public bool IsThreeState { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="CheckBox"/>'s.
    /// </summary>
    public interface ICheckBoxModelBase : IToggleButtonModelBase
    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="CheckBox"/>'.
    /// </summary>
    public class CheckBoxModelBase : ToggleButtonModelBase, ICheckBoxModelBase

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="CheckBox"/>'.
    /// </summary>
    public interface ICheckBoxModelBase<T> : IToggleButtonModelBase<T>, ICheckBoxModelBase
    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="CheckBox"/>'.
    /// </summary>
    public class CheckBoxModelBase<T> : ToggleButtonModelBase<T>, ICheckBoxModelBase<T>

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="TextBox"/>'s.
    /// </summary>
    public interface ITextBoxModelTextOriented

    {

        string Text { get; set; }

        bool IsReadOnly { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="TextBox"/>'s.
    /// </summary>
    public class TextBoxModelTextOriented : ITextBoxModelTextOriented

    {

        public string Text { get; set; }

        public bool IsReadOnly { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="TextBox"/>'s.
    /// </summary>
    public interface ITextBoxModelSelectionOriented : ITextBoxModelTextOriented

    {

         int CaretIndex { get; set; }

         int SelectionLength { get; set; }

         int SelectionStart { get; set; }

         string SelectedText { get; set; }

         bool IsReadOnlyCaretVisible { get; set; }

         bool AutoWordSelection { get; set; }

         Brush SelectionBrush { get; set; }

         Brush SelectionTextBrush { get; set; }

         Brush CaretBrush { get; set; }

         bool IsSelectionActive { get; }

         bool IsInactiveSelectionHighlightEnabled { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="TextBox"/>'s.
    /// </summary>
    public class TextBoxModelSelectionOriented : TextBoxModelTextOriented, ITextBoxModelSelectionOriented

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

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="TextBox"/>'s.
    /// </summary>
    public interface ITextBoxModelTextEditingOriented : ITextBoxModelTextOriented

    {

         int MinLines { get; set; }

         int MaxLines { get; set; }

         CharacterCasing CharacterCasing { get; set; }

         int MaxLength { get; set; }

         TextAlignment TextAlignment { get; set; }

         int LineCount { get; }

         TextDecorationCollection TextDecorations { get; set; }

         TextWrapping TextWrapping { get; set; }

         bool AcceptsReturn { get; set; }

         bool AcceptsTab { get; set; }

         double SelectionOpacity { get; set; }

         bool CanUndo { get; }

         bool CanRedo { get; }

         bool IsUndoEnabled { get; set; }

         int UndoLimit { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="TextBox"/>'s.
    /// </summary>
    public class TextBoxModelTextEditingOriented : TextBoxModelTextOriented, ITextBoxModelTextEditingOriented

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

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="TextBox"/>'s.
    /// </summary>
    public interface ITextBoxModelBase : ITextBoxModelTextOriented, ITextBoxModelTextEditingOriented, ITextBoxModelSelectionOriented

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="TextBox"/>'s.
    /// </summary>
    public class TextBoxModelBase : TextBoxModelTextOriented, ITextBoxModelBase

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

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="RadioButton"/> collection.
    /// </summary>
    public interface IRadioButtonCollection : IItemsControlModelBase<IRadioButtonModelBase>

    {

        string GroupName { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="RadioButton"/> collection.
    /// </summary>
    public class RadioButtonCollection : ItemsControlModelBase<IRadioButtonModelBase>, IRadioButtonCollection

    {

        public string GroupName { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="RadioButton"/> collection.
    /// </summary>
    public interface IRadioButtonCollection<T> : IRadioButtonCollection, IItemsControlModelBase<IRadioButtonModelBase<T>>

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="RadioButton"/> collection.
    /// </summary>
    public class RadioButtonCollection<T> : ItemsControlModelBase<IRadioButtonModelBase<T>>, IRadioButtonCollection<T>

    {

        public string GroupName { get; set; }

        IEnumerable<IRadioButtonModelBase> IItemsControlModelBase<IRadioButtonModelBase>.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<IRadioButtonModelBase<T>>>(value, nameof(value)); }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="RadioButton"/>s.
    /// </summary>
    public interface IRadioButtonModelBase : IToggleButtonModelBase

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="RadioButton"/>s.
    /// </summary>
    public class RadioButtonModelBase : ToggleButtonModelBase, IRadioButtonModelBase

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="RadioButton"/>s.
    /// </summary>
    public interface IRadioButtonModelBase<T> : IRadioButtonModelBase, IToggleButtonModelBase<T>

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="RadioButton"/>s.
    /// </summary>
    public class RadioButtonModelBase<T> : ToggleButtonModelBase<T>, IRadioButtonModelBase<T>

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="RadioButton"/>s.
    /// </summary>
    public interface IGroupingRadioButtonModelBase : IRadioButtonModelBase

    {

        string GroupName { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="RadioButton"/>s.
    /// </summary>
    public interface IGroupingRadioButtonModelBase<T> : IGroupingRadioButtonModelBase, IRadioButtonModelBase<T>

    {



    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="RadioButton"/>s.
    /// </summary>
    public class GroupingRadioButtonModelBase : RadioButtonModelBase, IGroupingRadioButtonModelBase

    {

        public string GroupName { get; set; }

    }

    /// <summary>
    /// Represents a base model that corresponds to a default view for <see cref="RadioButton"/>s.
    /// </summary>
    public class GroupingRadioButtonModelBase<T> : RadioButtonModelBase<T>, IGroupingRadioButtonModelBase<T>

    {

        public string GroupName { get; set; }

    }

}
