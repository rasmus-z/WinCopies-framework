using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinCopies.GUI.Controls.Models;
using WinCopies.GUI.Windows.Dialogs;
using WinCopies.GUI.Windows.Dialogs.Models;
using WinCopies.GUI.Windows.Dialogs.ViewModels;
using WinCopies.Util.Data;
using static WinCopies.Util.Util;

namespace WinCopies.GUI.Windows.Dialogs.ViewModels
{

    public class IsReadOnlyViewModel<T> : WinCopies.Util.Data.ViewModel<T>

    {

        public virtual bool IsReadOnly { get; }

        public IsReadOnlyViewModel(T model) : base(model) { }

    }

    public class DialogViewModelBase<T> : IsReadOnlyViewModel<T>, IDialogModelBase where T : IDialogModelBase
    {

        public string Title { get => ModelGeneric.Title; set => OnPropertyChanged(nameof(Title), value, GetType()); }

        public DialogButton DialogButton { get => ModelGeneric.DialogButton; set => OnPropertyChanged(nameof(DialogButton), value, GetType()); }

        public DefaultButton DefaultButton { get => ModelGeneric.DefaultButton; set => OnPropertyChanged(nameof(DefaultButton), value, GetType()); }

        public DialogViewModelBase(T model) : base(model) { }

    }

    public class PropertyDialogViewModelBase<T> : DialogViewModelBase<T>, IPropertyDialogModelBase where T : IPropertyDialogModelBase

    {

        public ICollection<IPropertyTabItemModelBase> Items { get => ModelGeneric.Items; set => OnPropertyChanged(nameof(Items), value, GetType()); }

        public PropertyDialogViewModelBase(T model) : base(model) { }

    }

}

namespace WinCopies.GUI.Controls.ViewModels

{

    public class GroupBoxViewModelBase<T> : IsReadOnlyViewModel <T>, IGroupBoxModelBase where T : IGroupBoxModelBase
    {

        public object Header { get => ModelGeneric.Header; set => OnPropertyChanged(nameof(Header), value, GetType()); }

        public object Content { get => ModelGeneric.Content; set => OnPropertyChanged(nameof(Content), value, GetType()); }

        public GroupBoxViewModelBase( T model) : base(model) { }

    }

    public class GroupBoxViewModelBase< T, THeader, TContent> : IsReadOnlyViewModel<T>, IGroupBoxModelBase<THeader, TContent> where T : IGroupBoxModelBase<THeader, TContent>
    {

        public THeader Header { get => ModelGeneric.Header; set => OnPropertyChanged(nameof(Header), value, GetType()); }

        object IGroupBoxModelBase. Header { get => Header; set => Header = GetOrThrowIfNotType<THeader>(value, nameof(value)); }

        public TContent Content { get => ModelGeneric.Content; set => OnPropertyChanged(nameof(Content), value, GetType()); }

        object IGroupBoxModelBase.Content { get => Content; set => Content = GetOrThrowIfNotType<TContent>(value, nameof(value)); }

        public GroupBoxViewModelBase( T model) : base(model) { }

    }

    public class TabItemViewModelBase : ViewModel<ITabItemModelBase>, ITabItemModelBase

    {

        public object Header { get; set; }

        public object Content { get; set; }

        public TabItemViewModelBase(ITabItemModelBase model) : base(model) { }

    }

    public class TabItemViewModelBase<THeader, TContent> : ViewModel<ITabItemModelBase<THeader, TContent>>, ITabItemModelBase<THeader, TContent>

    {

        public THeader Header { get; set; }

        public TContent Content { get; set; }

        public TabItemViewModelBase(ITabItemModelBase<THeader, TContent> model) : base(model) { }

    }

    public class PropertyTabItemViewModelBase : ViewModel<IPropertyTabItemModelBase>, IPropertyTabItemModelBase

    {

        public object Header { get; set; }

        public ICollection<IGroupBoxModelBase> Items { get; set; }

        public PropertyTabItemViewModelBase(IPropertyTabItemModelBase model) : base(model) { }

    }

    public class PropertyTabItemViewModelBase<T> : ViewModel<IPropertyTabItemModelBase<T>>, IPropertyTabItemModelBase<T>

    {

        public T Header { get; set; }

        public ICollection<IGroupBoxModelBase> Items { get; set; }

        public PropertyTabItemViewModelBase(IPropertyTabItemModelBase<T> model) : base(model) { }

    }

}
