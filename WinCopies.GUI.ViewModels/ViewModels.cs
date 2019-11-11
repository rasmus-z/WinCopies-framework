using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WinCopies.GUI.Controls.Models;
using WinCopies.GUI.Windows.Dialogs;
using WinCopies.GUI.Windows.Dialogs.Models;
using WinCopies.GUI.Windows.Dialogs.ViewModels;
using WinCopies.Util.Data;
using static WinCopies.Util.Util;

namespace WinCopies.GUI.Windows.Dialogs.ViewModels
{

    public class DialogViewModelBase<T> : ViewModel<T>, IDialogModelBase where T : IDialogModelBase
    {

        /// <summary>
        /// Gets or sets the title of this <see cref="DialogViewModelBase{T}"/>.
        /// </summary>
        public string Title { get => ModelGeneric.Title; set => OnPropertyChanged(nameof(Title), value, GetType()); }

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DialogButton"/> value of this <see cref="DialogViewModelBase{T}"/>.
        /// </summary>
        public DialogButton DialogButton { get => ModelGeneric.DialogButton; set => OnPropertyChanged(nameof(DialogButton), value, GetType()); }

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DefaultButton"/> value of this <see cref="DialogViewModelBase{T}"/>.
        /// </summary>
        public DefaultButton DefaultButton { get => ModelGeneric.DefaultButton; set => OnPropertyChanged(nameof(DefaultButton), value, GetType()); }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="model">The <typeparamref name="T"/> model to wrap in this <see cref="ViewModel{T}"/>.</param>
        public DialogViewModelBase(T model) : base(model) { }

    }

    public class PropertyDialogViewModelBase<T> : DialogViewModelBase<T>, IPropertyDialogModelBase where T : IPropertyDialogModelBase

    {

        /// <summary>
        /// Gets or sets the items of this <see cref="PropertyDialogViewModelBase{T}"/>.
        /// </summary>
        public IEnumerable<IPropertyTabItemModelBase> Items { get => ModelGeneric.Items; set => OnPropertyChanged(nameof(Items), value, GetType()); }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDialogViewModelBase{T}"/> class.
        /// </summary>
        /// <param name="model">The <typeparamref name="T"/> model to wrap in this <see cref="ViewModel{T}"/>.</param>
        public PropertyDialogViewModelBase(T model) : base(model) { }

    }

}

namespace WinCopies.GUI.Controls.ViewModels

{

    public class ContentControlViewModelBase<T> : ViewModel<T>, IContentControlModelBase where T : IContentControlModelBase
    {

        public object Content { get => ModelGeneric.Content; set => OnPropertyChanged(nameof(Content), value, GetType()); }

        public ContentControlViewModelBase(T model) : base(model) { }

    }

    public class ContentControlViewModelBase<TModel, TContent> : ViewModel<TModel>, IContentControlModelBase<TContent> where TModel : IContentControlModelBase<TContent>
    {

        public TContent Content { get => ModelGeneric.Content; set => OnPropertyChanged(nameof(Content), value, GetType()); }

        object IContentControlModelBase.Content { get => Content; set => Content = GetOrThrowIfNotType<TContent>(value, nameof(value)); }

        public ContentControlViewModelBase(TModel model) : base(model) { }

    }

    public class HeaderedContentControlViewModelBase<T> : ContentControlViewModelBase<T>, IHeaderedContentControlModelBase where T : IHeaderedContentControlModelBase

    {

        public object Header { get => ModelGeneric.Header; set => OnPropertyChanged(nameof(Header), value, GetType()); }

        public HeaderedContentControlViewModelBase(T model) : base(model) { }

    }

    public class HeaderedContentControlViewModelBase<TModel, THeader, TContent> : ContentControlViewModelBase<TModel, TContent>, IHeaderedContentControlModelBase<THeader, TContent> where TModel : IHeaderedContentControlModelBase<THeader, TContent>

    {

        public THeader Header { get => ModelGeneric.Header; set => OnPropertyChanged(nameof(Header), value, GetType()); }

        object IHeaderedControlModelBase.Header { get => Header; set => Header = GetOrThrowIfNotType<THeader>(value, nameof(value)); }

        public HeaderedContentControlViewModelBase(TModel model) : base(model) { }

    }

    public class ItemsControlViewModelBase<T> : ViewModel<T>, IItemsControlModelBase where T : IItemsControlModelBase
    {

        public IEnumerable Items { get => ModelGeneric.Items; set => OnPropertyChanged(nameof(Items), value, GetType()); }

        public ItemsControlViewModelBase(T model) : base(model) { }

    }

    public class ItemsControlViewModelBase<TModel, TItems> : ViewModel<TModel>, IItemsControlModelBase<TItems> where TModel : IItemsControlModelBase<TItems>

    {

        public IEnumerable<TItems> Items { get => ModelGeneric.Items; set => OnPropertyChanged(nameof(Items), value, GetType()); }

        IEnumerable IItemsControlModelBase.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<TItems>>(value, nameof(value)); }

        public ItemsControlViewModelBase(TModel model) : base(model) { }

    }

    public class HeaderedItemsControlViewModelBase<T> : ItemsControlViewModelBase<T>, IHeaderedItemsControlModelBase where T : IHeaderedItemsControlModelBase

    {

        public object Header { get => ModelGeneric.Header; set => OnPropertyChanged(nameof(Header), value, GetType()); }

        public HeaderedItemsControlViewModelBase(T model) : base(model) { }

    }

    public class HeaderedItemsControlViewModelBase<TModel, THeader, TItems> : ItemsControlViewModelBase<TModel, TItems>, IHeaderedItemsControlModelBase<THeader, TItems> where TModel : IHeaderedItemsControlModelBase<THeader, TItems>

    {

        public THeader Header { get => ModelGeneric.Header; set => OnPropertyChanged(nameof(Header), value, GetType()); }

        object IHeaderedControlModelBase.Header { get => Header; set => Header = GetOrThrowIfNotType<THeader>(value, nameof(value)); }

        public HeaderedItemsControlViewModelBase(TModel model) : base(model) { }

    }

    public class GroupBoxViewModelBase<T> : HeaderedContentControlViewModelBase<T>, IGroupBoxModelBase where T : IGroupBoxModelBase
    {

        public GroupBoxViewModelBase(T model) : base(model) { }

    }

    public class GroupBoxViewModelBase<TModel, THeader, TContent> : HeaderedContentControlViewModelBase<TModel, THeader, TContent>, IGroupBoxModelBase<THeader, TContent> where TModel : IGroupBoxModelBase<THeader, TContent>
    {

        public GroupBoxViewModelBase(TModel model) : base(model) { }

    }

    public class TabItemViewModelBase<T> : HeaderedContentControlViewModelBase<T>, ITabItemModelBase where T : ITabItemModelBase

    {

        public TabItemViewModelBase(T model) : base(model) { }

    }

    public class TabItemViewModelBase<TModel, THeader, TContent> : HeaderedContentControlViewModelBase<TModel, THeader, TContent>, ITabItemModelBase<THeader, TContent> where TModel : ITabItemModelBase<THeader, TContent>

    {

        public TabItemViewModelBase(TModel model) : base(model) { }

    }

    public class PropertyTabItemViewModelBase<T> : ViewModel<T>, IPropertyTabItemModelBase where T : IPropertyTabItemModelBase

    {

        public object Header { get => ModelGeneric.Header; set => OnPropertyChanged(nameof(Header), value, GetType()); }

        public IEnumerable<IGroupBoxModelBase> Items { get => ModelGeneric.Items; set => OnPropertyChanged(nameof(Items), value, GetType()); }

        IEnumerable IItemsControlModelBase.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<IGroupBoxModelBase>>(value, nameof(value)); }

        public PropertyTabItemViewModelBase(T model) : base(model) { }

    }

    public class PropertyTabItemViewModelBase<TModel, TItemHeader, TGroupBoxHeader, TGroupBoxContent> : HeaderedItemsControlViewModelBase<TModel, TItemHeader, IGroupBoxModelBase<TGroupBoxHeader, TGroupBoxContent>>, IPropertyTabItemModelBase<TItemHeader, TGroupBoxHeader, TGroupBoxContent> where TModel : IPropertyTabItemModelBase<TItemHeader, TGroupBoxHeader, TGroupBoxContent>

    {

        IEnumerable<IGroupBoxModelBase> IPropertyTabItemModelBase.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<IGroupBoxModelBase<TGroupBoxHeader, TGroupBoxContent>>>(value, nameof(value)); }

        public PropertyTabItemViewModelBase(TModel model) : base(model) { }

    }


    public class ButtonViewModelBase<T> : ContentControlViewModelBase<T>, IButtonModelBase where T : IButtonModelBase

    {

        public ICommand Command { get => ModelGeneric.Command; set => OnPropertyChanged(nameof(Command), value, GetType()); }

        public object CommandParameter { get => ModelGeneric.CommandParameter; set => OnPropertyChanged(nameof(CommandParameter), value, GetType()); }

        public IInputElement CommandTarget { get => ModelGeneric.CommandTarget; set => OnPropertyChanged(nameof(CommandTarget), value, GetType()); }

        public ButtonViewModelBase(T model) : base(model) { }

    }

    public class ButtonViewModelBase<TModel, TContent> : ContentControlViewModelBase<TModel, TContent>, IButtonModelBase<TContent> where TModel : IButtonModelBase<TContent>

    {

        public ICommand Command { get => ModelGeneric.Command; set => OnPropertyChanged(nameof(Command), value, GetType()); }

        public object CommandParameter { get => ModelGeneric.CommandParameter; set => OnPropertyChanged(nameof(CommandParameter), value, GetType()); }

        public IInputElement CommandTarget { get => ModelGeneric.CommandTarget; set => OnPropertyChanged(nameof(CommandTarget), value, GetType()); }

        public ButtonViewModelBase(TModel model) : base(model) { }

    }

    public class ToggleButtonViewModelBase<T> : ButtonViewModelBase<T>, IToggleButtonModelBase where T : IToggleButtonModelBase

    {

        public bool? IsChecked { get => ModelGeneric.IsChecked; set => OnPropertyChanged(nameof(IsChecked), value, GetType()); }

        public bool IsThreeState { get => ModelGeneric.IsThreeState; set => OnPropertyChanged(nameof(IsThreeState), value, GetType()); }

        public ToggleButtonViewModelBase(T model) : base(model) { }

    }

    public class ToggleButtonViewModelBase<TModel, TContent> : ButtonViewModelBase<TModel, TContent>, IToggleButtonModelBase<TContent> where TModel : IToggleButtonModelBase<TContent>

    {

        public bool? IsChecked { get => ModelGeneric.IsChecked; set => OnPropertyChanged(nameof(IsChecked), value, GetType()); }

        public bool IsThreeState { get => ModelGeneric.IsThreeState; set => OnPropertyChanged(nameof(IsThreeState), value, GetType()); }

        public ToggleButtonViewModelBase(TModel model) : base(model) { }

    }

    public class CheckBoxViewModelBase<T> : ToggleButtonViewModelBase<T>, ICheckBoxModelBase where T : ICheckBoxModelBase

    {

        public CheckBoxViewModelBase(T model) : base(model) { }

    }

    public class CheckBoxViewModelBase<TModel, TContent> : ToggleButtonViewModelBase<TModel, TContent>, ICheckBoxModelBase<TContent> where TModel : ICheckBoxModelBase<TContent>

    {

        public CheckBoxViewModelBase( TModel model) : base(model) { }

    }

    public class TextBoxViewModelTextOriented<T> : ViewModel<T>, ITextBoxModelTextOriented where T : ITextBoxModelTextOriented

    {

        public string Text { get=>ModelGeneric.Text; set=> OnPropertyChanged(nameof(Text), value, GetType()); }

        public bool IsReadOnly { get=>ModelGeneric.IsReadOnly; set=>OnPropertyChanged(nameof(IsReadOnly),value,GetType()); }

        public TextBoxViewModelTextOriented(T model) : base(model) { }

    }

    public class TextBoxViewModelSelectionOriented<T> : TextBoxViewModelTextOriented<T>, ITextBoxModelSelectionOriented where T : ITextBoxModelSelectionOriented

    {

        public int CaretIndex { get=>ModelGeneric.CaretIndex; set=>OnPropertyChanged(nameof(CaretIndex),value,GetType()); }

        public int SelectionLength { get=>ModelGeneric.SelectionLength; set=> OnPropertyChanged(nameof(SelectionLength), value, GetType()); }

        public int SelectionStart { get=>ModelGeneric.SelectionStart; set=> OnPropertyChanged(nameof(SelectionStart), value, GetType()); }

        public string SelectedText { get=>ModelGeneric.SelectedText; set=> OnPropertyChanged(nameof(SelectedText), value, GetType()); }

        public bool IsReadOnlyCaretVisible { get=>ModelGeneric.IsReadOnlyCaretVisible; set=> OnPropertyChanged(nameof(IsReadOnlyCaretVisible), value, GetType()); }

        public bool AutoWordSelection { get=>ModelGeneric.AutoWordSelection; set=> OnPropertyChanged(nameof(AutoWordSelection), value, GetType()); }

        public Brush SelectionBrush { get=>ModelGeneric.SelectionBrush; set=> OnPropertyChanged(nameof(SelectionBrush), value, GetType()); }

        public Brush SelectionTextBrush { get=>ModelGeneric.SelectionTextBrush; set=> OnPropertyChanged(nameof(SelectionTextBrush), value, GetType()); }

        public Brush CaretBrush { get=>ModelGeneric.CaretBrush; set=> OnPropertyChanged(nameof(CaretBrush), value, GetType()); }

        public bool IsInactiveSelectionHighlightEnabled { get=>ModelGeneric.IsInactiveSelectionHighlightEnabled; set=> OnPropertyChanged(nameof(IsInactiveSelectionHighlightEnabled), value, GetType()); }

        public TextBoxViewModelSelectionOriented(T model) : base(model) { }

    }

    public class TextBoxViewModelTextEditingOriented<T> : TextBoxViewModelTextOriented<T>, ITextBoxModelTextEditingOriented where T : ITextBoxModelTextEditingOriented

    {

        public int MinLines { get=>ModelGeneric.MinLines; set=>OnPropertyChanged(nameof(MinLines),value,GetType()); }

        public int MaxLines { get=>ModelGeneric.MaxLines; set=> OnPropertyChanged(nameof(MaxLines), value, GetType()); }

        public CharacterCasing CharacterCasing { get=>ModelGeneric.CharacterCasing; set=> OnPropertyChanged(nameof(CharacterCasing), value, GetType()); }

        public int MaxLength { get=>ModelGeneric.MaxLength; set=> OnPropertyChanged(nameof(MaxLength), value, GetType()); }

        public TextAlignment TextAlignment { get=>ModelGeneric.TextAlignment; set=> OnPropertyChanged(nameof(TextAlignment), value, GetType()); }

        public TextDecorationCollection TextDecorations { get=>ModelGeneric.TextDecorations; set=> OnPropertyChanged(nameof(TextDecorations), value, GetType()); }

        public TextWrapping TextWrapping { get=>ModelGeneric.TextWrapping; set=> OnPropertyChanged(nameof(TextWrapping), value, GetType()); }

        public bool AcceptsReturn { get=>ModelGeneric.AcceptsReturn; set=> OnPropertyChanged(nameof(AcceptsReturn), value, GetType()); }

        public bool AcceptsTab { get=>ModelGeneric.AcceptsTab; set=> OnPropertyChanged(nameof(AcceptsTab), value, GetType()); }

        public double SelectionOpacity { get=>ModelGeneric.SelectionOpacity; set=> OnPropertyChanged(nameof(SelectionOpacity), value, GetType()); }

        public bool IsUndoEnabled { get=>ModelGeneric.IsUndoEnabled; set=> OnPropertyChanged(nameof(IsUndoEnabled), value, GetType()); }

        public int UndoLimit { get=>ModelGeneric.UndoLimit; set=> OnPropertyChanged(nameof(UndoLimit), value, GetType()); }

        public TextBoxViewModelTextEditingOriented(T model) : base(model) { }
    
    }

    public class TextBoxViewModelBase<T> : TextBoxViewModelTextOriented<T>, ITextBoxModelBase where T : ITextBoxModelBase

    {

        public int MinLines { get => ModelGeneric.MinLines; set => OnPropertyChanged(nameof(MinLines), value, GetType()); }

        public int MaxLines { get => ModelGeneric.MaxLines; set => OnPropertyChanged(nameof(MaxLines), value, GetType()); }

        public CharacterCasing CharacterCasing { get => ModelGeneric.CharacterCasing; set => OnPropertyChanged(nameof(CharacterCasing), value, GetType()); }

        public int MaxLength { get => ModelGeneric.MaxLength; set => OnPropertyChanged(nameof(MaxLength), value, GetType()); }

        public TextAlignment TextAlignment { get => ModelGeneric.TextAlignment; set => OnPropertyChanged(nameof(TextAlignment), value, GetType()); }

        public TextDecorationCollection TextDecorations { get => ModelGeneric.TextDecorations; set => OnPropertyChanged(nameof(TextDecorations), value, GetType()); }

        public TextWrapping TextWrapping { get => ModelGeneric.TextWrapping; set => OnPropertyChanged(nameof(TextWrapping), value, GetType()); }

        public bool AcceptsReturn { get => ModelGeneric.AcceptsReturn; set => OnPropertyChanged(nameof(AcceptsReturn), value, GetType()); }

        public bool AcceptsTab { get => ModelGeneric.AcceptsTab; set => OnPropertyChanged(nameof(AcceptsTab), value, GetType()); }

        public double SelectionOpacity { get => ModelGeneric.SelectionOpacity; set => OnPropertyChanged(nameof(SelectionOpacity), value, GetType()); }

        public bool IsUndoEnabled { get => ModelGeneric.IsUndoEnabled; set => OnPropertyChanged(nameof(IsUndoEnabled), value, GetType()); }

        public int UndoLimit { get => ModelGeneric.UndoLimit; set => OnPropertyChanged(nameof(UndoLimit), value, GetType()); }

        public int CaretIndex { get => ModelGeneric.CaretIndex; set => OnPropertyChanged(nameof(CaretIndex), value, GetType()); }

        public int SelectionLength { get => ModelGeneric.SelectionLength; set => OnPropertyChanged(nameof(SelectionLength), value, GetType()); }

        public int SelectionStart { get => ModelGeneric.SelectionStart; set => OnPropertyChanged(nameof(SelectionStart), value, GetType()); }

        public string SelectedText { get => ModelGeneric.SelectedText; set => OnPropertyChanged(nameof(SelectedText), value, GetType()); }

        public bool IsReadOnlyCaretVisible { get => ModelGeneric.IsReadOnlyCaretVisible; set => OnPropertyChanged(nameof(IsReadOnlyCaretVisible), value, GetType()); }

        public bool AutoWordSelection { get => ModelGeneric.AutoWordSelection; set => OnPropertyChanged(nameof(AutoWordSelection), value, GetType()); }

        public Brush SelectionBrush { get => ModelGeneric.SelectionBrush; set => OnPropertyChanged(nameof(SelectionBrush), value, GetType()); }

        public Brush SelectionTextBrush { get => ModelGeneric.SelectionTextBrush; set => OnPropertyChanged(nameof(SelectionTextBrush), value, GetType()); }

        public Brush CaretBrush { get => ModelGeneric.CaretBrush; set => OnPropertyChanged(nameof(CaretBrush), value, GetType()); }

        public bool IsInactiveSelectionHighlightEnabled { get => ModelGeneric.IsInactiveSelectionHighlightEnabled; set => OnPropertyChanged(nameof(IsInactiveSelectionHighlightEnabled), value, GetType()); }

        public TextBoxViewModelBase(T model) : base(model) { }

    }

    public class ObservableRadioButtonCollection : ObservableCollection<IRadioButtonModelBase>, IRadioButtonCollection

    {

        public string GroupName
        {

            get => ((RadioButtonCollection)Items).GroupName; set

            {

                ((RadioButtonCollection)Items).GroupName = value;

                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(GroupName)));

            }

        }

        public ObservableRadioButtonCollection( RadioButtonCollection items) : base( items) { }

    }

    public class ObservableRadioButtonCollection<T> : ObservableCollection<IRadioButtonModelBase<T>>, IRadioButtonCollection<T>

    {

        public string GroupName
        {

            get => ((RadioButtonCollection)Items).GroupName; set

            {

                ((RadioButtonCollection)Items).GroupName = value;

                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(GroupName)));

            }

        }

        IEnumerator<IRadioButtonModelBase> IEnumerable<IRadioButtonModelBase>.GetEnumerator() => GetEnumerator();

        public ObservableRadioButtonCollection(RadioButtonCollection<T> items) : base(items) { }

    }

    public class GroupingRadioButtonViewModelBase<T> : ToggleButtonViewModelBase<T>, IGroupingRadioButtonModelBase where T : IGroupingRadioButtonModelBase

    {

        public string GroupName { get => ModelGeneric.GroupName; set => OnPropertyChanged(nameof(GroupName), value, GetType()); }

        public GroupingRadioButtonViewModelBase(T model) : base(model) { }

    }

    public class GroupingRadioButtonViewModelBase<TModel, TContent> : ToggleButtonViewModelBase<TModel, TContent>, IGroupingRadioButtonModelBase where TModel : IGroupingRadioButtonModelBase<TContent>

    {

        public string GroupName { get => ModelGeneric.GroupName; set => OnPropertyChanged(nameof(GroupName), value, GetType()); }

        public GroupingRadioButtonViewModelBase( TModel model ) : base(model) { }

    }

}
