/* Copyright © Pierre Sprimont, 2019
 *
 * This file is part of the WinCopies Framework.
 *
 * The WinCopies Framework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * The WinCopies Framework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

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

    public class DialogViewModel<T> : ViewModel<T>, IDialogModel where T : IDialogModel
    {

        /// <summary>
        /// Gets or sets the title of this <see cref="DialogViewModel{T}"/>.
        /// </summary>
        public string Title { get => ModelGeneric.Title; set => OnPropertyChanged(nameof(Title), value, GetType()); }

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DialogButton"/> value of this <see cref="DialogViewModel{T}"/>.
        /// </summary>
        public DialogButton DialogButton { get => ModelGeneric.DialogButton; set => OnPropertyChanged(nameof(DialogButton), value, GetType()); }

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DefaultButton"/> value of this <see cref="DialogViewModel{T}"/>.
        /// </summary>
        public DefaultButton DefaultButton { get => ModelGeneric.DefaultButton; set => OnPropertyChanged(nameof(DefaultButton), value, GetType()); }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogViewModel{T}"/> class.
        /// </summary>
        /// <param name="model">The <typeparamref name="T"/> model to wrap in this <see cref="ViewModel{T}"/>.</param>
        public DialogViewModel(T model) : base(model) { }

    }

    public class PropertyDialogViewModel<T> : DialogViewModel<T>, IPropertyDialogModel where T : IPropertyDialogModel

    {

        /// <summary>
        /// Gets or sets the items of this <see cref="PropertyDialogViewModel{T}"/>.
        /// </summary>
        public IEnumerable<IPropertyTabItemModel> Items { get => ModelGeneric.Items; set => OnPropertyChanged(nameof(Items), value, GetType()); }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDialogViewModel{T}"/> class.
        /// </summary>
        /// <param name="model">The <typeparamref name="T"/> model to wrap in this <see cref="ViewModel{T}"/>.</param>
        public PropertyDialogViewModel(T model) : base(model) { }

    }

}

namespace WinCopies.GUI.Controls.ViewModels

{

    public class ContentControlViewModel<T> : ViewModel<T>, IContentControlModel where T : IContentControlModel
    {

        public object Content { get => ModelGeneric.Content; set => OnPropertyChanged(nameof(Content), value, GetType()); }

        public ContentControlViewModel(T model) : base(model) { }

    }

    public class ContentControlViewModel<TModel, TContent> : ViewModel<TModel>, IContentControlModel<TContent> where TModel : IContentControlModel<TContent>
    {

        public TContent Content { get => ModelGeneric.Content; set => OnPropertyChanged(nameof(Content), value, GetType()); }

        object IContentControlModel.Content { get => Content; set => Content = GetOrThrowIfNotType<TContent>(value, nameof(value)); }

        public ContentControlViewModel(TModel model) : base(model) { }

    }

    public class HeaderedContentControlViewModel<T> : ContentControlViewModel<T>, IHeaderedContentControlModel where T : IHeaderedContentControlModel

    {

        public object Header { get => ModelGeneric.Header; set => OnPropertyChanged(nameof(Header), value, GetType()); }

        public HeaderedContentControlViewModel(T model) : base(model) { }

    }

    public class HeaderedContentControlViewModel<TModel, THeader, TContent> : ContentControlViewModel<TModel, TContent>, IHeaderedContentControlModel<THeader, TContent> where TModel : IHeaderedContentControlModel<THeader, TContent>

    {

        public THeader Header { get => ModelGeneric.Header; set => OnPropertyChanged(nameof(Header), value, GetType()); }

        object IHeaderedControlModel.Header { get => Header; set => Header = GetOrThrowIfNotType<THeader>(value, nameof(value)); }

        public HeaderedContentControlViewModel(TModel model) : base(model) { }

    }

    public class ItemsControlViewModel<T> : ViewModel<T>, IItemsControlModel where T : IItemsControlModel
    {

        public IEnumerable Items { get => ModelGeneric.Items; set => OnPropertyChanged(nameof(Items), value, GetType()); }

        public ItemsControlViewModel(T model) : base(model) { }

    }

    public class ItemsControlViewModel<TModel, TItems> : ViewModel<TModel>, IItemsControlModel<TItems> where TModel : IItemsControlModel<TItems>

    {

        public IEnumerable<TItems> Items { get => ModelGeneric.Items; set => OnPropertyChanged(nameof(Items), value, GetType()); }

        IEnumerable IItemsControlModel.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<TItems>>(value, nameof(value)); }

        public ItemsControlViewModel(TModel model) : base(model) { }

    }

    public class HeaderedItemsControlViewModel<T> : ItemsControlViewModel<T>, IHeaderedItemsControlModel where T : IHeaderedItemsControlModel

    {

        public object Header { get => ModelGeneric.Header; set => OnPropertyChanged(nameof(Header), value, GetType()); }

        public HeaderedItemsControlViewModel(T model) : base(model) { }

    }

    public class HeaderedItemsControlViewModel<TModel, THeader, TItems> : ItemsControlViewModel<TModel, TItems>, IHeaderedItemsControlModel<THeader, TItems> where TModel : IHeaderedItemsControlModel<THeader, TItems>

    {

        public THeader Header { get => ModelGeneric.Header; set => OnPropertyChanged(nameof(Header), value, GetType()); }

        object IHeaderedControlModel.Header { get => Header; set => Header = GetOrThrowIfNotType<THeader>(value, nameof(value)); }

        public HeaderedItemsControlViewModel(TModel model) : base(model) { }

    }

    public class GroupBoxViewModel<T> : HeaderedContentControlViewModel<T>, IGroupBoxModel where T : IGroupBoxModel
    {

        public GroupBoxViewModel(T model) : base(model) { }

    }

    public class GroupBoxViewModel<TModel, THeader, TContent> : HeaderedContentControlViewModel<TModel, THeader, TContent>, IGroupBoxModel<THeader, TContent> where TModel : IGroupBoxModel<THeader, TContent>
    {

        public GroupBoxViewModel(TModel model) : base(model) { }

    }

    public class TabItemViewModel<T> : HeaderedContentControlViewModel<T>, ITabItemModel where T : ITabItemModel

    {

        public TabItemViewModel(T model) : base(model) { }

    }

    public class TabItemViewModel<TModel, THeader, TContent> : HeaderedContentControlViewModel<TModel, THeader, TContent>, ITabItemModel<THeader, TContent> where TModel : ITabItemModel<THeader, TContent>

    {

        public TabItemViewModel(TModel model) : base(model) { }

    }

    public class PropertyTabItemViewModel<T> : ViewModel<T>, IPropertyTabItemModel where T : IPropertyTabItemModel

    {

        public object Header { get => ModelGeneric.Header; set => OnPropertyChanged(nameof(Header), value, GetType()); }

        public IEnumerable<IGroupBoxModel> Items { get => ModelGeneric.Items; set => OnPropertyChanged(nameof(Items), value, GetType()); }

        IEnumerable IItemsControlModel.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<IGroupBoxModel>>(value, nameof(value)); }

        public PropertyTabItemViewModel(T model) : base(model) { }

    }

    public class PropertyTabItemViewModel<TModel, TItemHeader, TGroupBoxHeader, TGroupBoxContent> : HeaderedItemsControlViewModel<TModel, TItemHeader, IGroupBoxModel<TGroupBoxHeader, TGroupBoxContent>>, IPropertyTabItemModel<TItemHeader, TGroupBoxHeader, TGroupBoxContent> where TModel : IPropertyTabItemModel<TItemHeader, TGroupBoxHeader, TGroupBoxContent>

    {

        IEnumerable<IGroupBoxModel> IPropertyTabItemModel.Items { get => Items; set => Items = GetOrThrowIfNotType<IEnumerable<IGroupBoxModel<TGroupBoxHeader, TGroupBoxContent>>>(value, nameof(value)); }

        public PropertyTabItemViewModel(TModel model) : base(model) { }

    }


    public class ButtonViewModel<T> : ContentControlViewModel<T>, IButtonModel where T : IButtonModel

    {

        public ICommand Command { get => ModelGeneric.Command; set => OnPropertyChanged(nameof(Command), value, GetType()); }

        public object CommandParameter { get => ModelGeneric.CommandParameter; set => OnPropertyChanged(nameof(CommandParameter), value, GetType()); }

        public IInputElement CommandTarget { get => ModelGeneric.CommandTarget; set => OnPropertyChanged(nameof(CommandTarget), value, GetType()); }

        public ButtonViewModel(T model) : base(model) { }

    }

    public class ButtonViewModel<TModel, TContent> : ContentControlViewModel<TModel, TContent>, IButtonModel<TContent> where TModel : IButtonModel<TContent>

    {

        public ICommand Command { get => ModelGeneric.Command; set => OnPropertyChanged(nameof(Command), value, GetType()); }

        public object CommandParameter { get => ModelGeneric.CommandParameter; set => OnPropertyChanged(nameof(CommandParameter), value, GetType()); }

        public IInputElement CommandTarget { get => ModelGeneric.CommandTarget; set => OnPropertyChanged(nameof(CommandTarget), value, GetType()); }

        public ButtonViewModel(TModel model) : base(model) { }

    }

    public class ToggleButtonViewModel<T> : ButtonViewModel<T>, IToggleButtonModel where T : IToggleButtonModel

    {

        public bool? IsChecked { get => ModelGeneric.IsChecked; set => OnPropertyChanged(nameof(IsChecked), value, GetType()); }

        public bool IsThreeState { get => ModelGeneric.IsThreeState; set => OnPropertyChanged(nameof(IsThreeState), value, GetType()); }

        public ToggleButtonViewModel(T model) : base(model) { }

    }

    public class ToggleButtonViewModel<TModel, TContent> : ButtonViewModel<TModel, TContent>, IToggleButtonModel<TContent> where TModel : IToggleButtonModel<TContent>

    {

        public bool? IsChecked { get => ModelGeneric.IsChecked; set => OnPropertyChanged(nameof(IsChecked), value, GetType()); }

        public bool IsThreeState { get => ModelGeneric.IsThreeState; set => OnPropertyChanged(nameof(IsThreeState), value, GetType()); }

        public ToggleButtonViewModel(TModel model) : base(model) { }

    }

    public class CheckBoxViewModel<T> : ToggleButtonViewModel<T>, ICheckBoxModel where T : ICheckBoxModel

    {

        public CheckBoxViewModel(T model) : base(model) { }

    }

    public class CheckBoxViewModel<TModel, TContent> : ToggleButtonViewModel<TModel, TContent>, ICheckBoxModel<TContent> where TModel : ICheckBoxModel<TContent>

    {

        public CheckBoxViewModel(TModel model) : base(model) { }

    }

    public class TextBoxViewModelTextOriented<T> : ViewModel<T>, ITextBoxModelTextOriented where T : ITextBoxModelTextOriented

    {

        public string Text { get => ModelGeneric.Text; set => OnPropertyChanged(nameof(Text), value, GetType()); }

        public bool IsReadOnly { get => ModelGeneric.IsReadOnly; set => OnPropertyChanged(nameof(IsReadOnly), value, GetType()); }

        public TextBoxViewModelTextOriented(T model) : base(model) { }

    }

    public class TextBoxViewModelSelectionOriented<T> : TextBoxViewModelTextOriented<T>, ITextBoxModelSelectionOriented where T : ITextBoxModelSelectionOriented

    {

        public bool IsReadOnlyCaretVisible { get => ModelGeneric.IsReadOnlyCaretVisible; set => OnPropertyChanged(nameof(IsReadOnlyCaretVisible), value, GetType()); }

        public bool AutoWordSelection { get => ModelGeneric.AutoWordSelection; set => OnPropertyChanged(nameof(AutoWordSelection), value, GetType()); }

        public Brush SelectionBrush { get => ModelGeneric.SelectionBrush; set => OnPropertyChanged(nameof(SelectionBrush), value, GetType()); }

        public double SelectionOpacity { get => ModelGeneric.SelectionOpacity; set => OnPropertyChanged(nameof(SelectionOpacity), value, GetType()); }

        public Brush SelectionTextBrush { get => ModelGeneric.SelectionTextBrush; set => OnPropertyChanged(nameof(SelectionTextBrush), value, GetType()); }

        public Brush CaretBrush { get => ModelGeneric.CaretBrush; set => OnPropertyChanged(nameof(CaretBrush), value, GetType()); }

        public bool IsInactiveSelectionHighlightEnabled { get => ModelGeneric.IsInactiveSelectionHighlightEnabled; set => OnPropertyChanged(nameof(IsInactiveSelectionHighlightEnabled), value, GetType()); }

        public TextBoxViewModelSelectionOriented(T model) : base(model) { }

    }

    public class TextBoxViewModelTextEditingOriented<T> : TextBoxViewModelTextOriented<T>, ITextBoxModelTextEditingOriented where T : ITextBoxModelTextEditingOriented

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

        public bool IsUndoEnabled { get => ModelGeneric.IsUndoEnabled; set => OnPropertyChanged(nameof(IsUndoEnabled), value, GetType()); }

        public int UndoLimit { get => ModelGeneric.UndoLimit; set => OnPropertyChanged(nameof(UndoLimit), value, GetType()); }

        public TextBoxViewModelTextEditingOriented(T model) : base(model) { }

    }

    public class TextBoxViewModel<T> : TextBoxViewModelTextOriented<T>, ITextBoxModel where T : ITextBoxModel

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

        public bool IsReadOnlyCaretVisible { get => ModelGeneric.IsReadOnlyCaretVisible; set => OnPropertyChanged(nameof(IsReadOnlyCaretVisible), value, GetType()); }

        public bool AutoWordSelection { get => ModelGeneric.AutoWordSelection; set => OnPropertyChanged(nameof(AutoWordSelection), value, GetType()); }

        public Brush SelectionBrush { get => ModelGeneric.SelectionBrush; set => OnPropertyChanged(nameof(SelectionBrush), value, GetType()); }

        public Brush SelectionTextBrush { get => ModelGeneric.SelectionTextBrush; set => OnPropertyChanged(nameof(SelectionTextBrush), value, GetType()); }

        public Brush CaretBrush { get => ModelGeneric.CaretBrush; set => OnPropertyChanged(nameof(CaretBrush), value, GetType()); }

        public bool IsInactiveSelectionHighlightEnabled { get => ModelGeneric.IsInactiveSelectionHighlightEnabled; set => OnPropertyChanged(nameof(IsInactiveSelectionHighlightEnabled), value, GetType()); }

        public TextBoxViewModel(T model) : base(model) { }

    }

    public class ObservableRadioButtonCollection : ObservableCollection<IRadioButtonModel>, IRadioButtonCollection

    {

        public string GroupName
        {

            get => ((RadioButtonCollection)Items).GroupName; set

            {

                ((RadioButtonCollection)Items).GroupName = value;

                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(GroupName)));

            }

        }

        public ObservableRadioButtonCollection(RadioButtonCollection items) : base(items) { }

    }

    public class ObservableRadioButtonCollection<T> : ObservableCollection<IRadioButtonModel<T>>, IRadioButtonCollection<T>

    {

        public string GroupName
        {

            get => ((RadioButtonCollection)Items).GroupName; set

            {

                ((RadioButtonCollection)Items).GroupName = value;

                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(GroupName)));

            }

        }

        IEnumerator<IRadioButtonModel> IEnumerable<IRadioButtonModel>.GetEnumerator() => GetEnumerator();

        public ObservableRadioButtonCollection(RadioButtonCollection<T> items) : base(items) { }

    }

    public class GroupingRadioButtonViewModel<T> : ToggleButtonViewModel<T>, IGroupingRadioButtonModel where T : IGroupingRadioButtonModel

    {

        public string GroupName { get => ModelGeneric.GroupName; set => OnPropertyChanged(nameof(GroupName), value, GetType()); }

        public GroupingRadioButtonViewModel(T model) : base(model) { }

    }

    public class GroupingRadioButtonViewModel<TModel, TContent> : ToggleButtonViewModel<TModel, TContent>, IGroupingRadioButtonModel where TModel : IGroupingRadioButtonModel<TContent>

    {

        public string GroupName { get => ModelGeneric.GroupName; set => OnPropertyChanged(nameof(GroupName), value, GetType()); }

        public GroupingRadioButtonViewModel(TModel model) : base(model) { }

    }

}
