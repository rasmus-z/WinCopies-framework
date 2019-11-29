﻿/* Copyright © Pierre Sprimont, 2019
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

using AttachedCommandBehavior;
using System;
using System.Windows;
using System.Windows.Input;
using WinCopies.Util;
using WinCopies.Util.Commands;
using static WinCopies.Util.Util;
using IfCT = WinCopies.Util.Util.ComparisonType;
using IfCM = WinCopies.Util.Util.ComparisonMode;
using IfComp = WinCopies.Util.Util.Comparison;
using System.Windows.Controls;
using System.Collections.Generic;

namespace WinCopies.GUI.Windows.Dialogs
{
    /// <summary>
    /// Represents a common dialog window for WPF.
    /// </summary>
    [TemplatePart(Name = PART_OkButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_ApplyButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_YesButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_NoButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_CancelButton, Type = typeof(Button))]
    public partial class DialogWindow : Window, ICommandSource
    {

        private const string PART_OkButton = "PART_OkButton";
        private const string PART_ApplyButton = "PART_ApplyButton";
        private const string PART_YesButton = "PART_YesButton";
        private const string PART_NoButton = "PART_NoButton";
        private const string PART_CancelButton = "PART_CancelButton";

        /// <summary>
        /// Identifies the <see cref="DialogButton"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DialogButtonProperty = DependencyProperty.Register(nameof(DialogButton), typeof(DialogButton), typeof(DialogWindow), new PropertyMetadata(DialogButton.OK, (DependencyObject sender, DependencyPropertyChangedEventArgs e) => ((DialogWindow)sender).OnDialogButtonChanged((DialogButton)e.OldValue, (DialogButton)e.NewValue)
        ));

        /// <summary>
        /// Gets or sets the <see cref="Dialogs.DialogButton"/>. <see cref="Command"/> will not be executed if this property is set to <see cref="DialogButton.OK"/>, or if the user clicks on one of the following buttons: Cancel, Ignore, Abort. This is a dependency property.
        /// </summary>
        public DialogButton DialogButton { get => (DialogButton)GetValue(DialogButtonProperty); set => SetValue(DialogButtonProperty, value); }

        /// <summary>
        /// Identifies the <see cref="ButtonAlignment"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonAlignmentProperty = DependencyProperty.Register(nameof(ButtonAlignment), typeof(HorizontalAlignment), typeof(DialogWindow), new PropertyMetadata(Dialogs.HorizontalAlignment.Left));

        /// <summary>
        /// Gets or sets the <see cref="HorizontalAlignment"/> for the button alignment. This is a dependency property.
        /// </summary>
        public HorizontalAlignment ButtonAlignment { get => (HorizontalAlignment)GetValue(ButtonAlignmentProperty); set => SetValue(ButtonAlignmentProperty, value); }

        public static readonly DependencyProperty DefaultButtonProperty = DependencyProperty.Register(nameof(DefaultButton), typeof(DefaultButton), typeof(DialogWindow), new PropertyMetadata(DefaultButton.None, (DependencyObject d, DependencyPropertyChangedEventArgs e) => ((DialogWindow)d).OnDefaultButtonChanged((DefaultButton)e.OldValue, (DefaultButton)e.NewValue)));

        public DefaultButton DefaultButton { get => (DefaultButton)GetValue(DefaultButtonProperty); set => SetValue(DefaultButtonProperty, value); }

        public static DependencyProperty ShowHelpButtonAsCommandButtonProperty = DependencyProperty.Register(nameof(ShowHelpButtonAsCommandButton), typeof(bool), typeof(DialogWindow));

        public bool ShowHelpButtonAsCommandButton { get => (bool)GetValue(ShowHelpButtonAsCommandButtonProperty); set => SetValue(ShowHelpButtonAsCommandButtonProperty, value); }

        /// <summary>
        /// Gets the <see cref="WinCopies.GUI.Windows.Dialogs.MessageBoxResult"/>.
        /// </summary>
        public MessageBoxResult MessageBoxResult { get; private set; } = MessageBoxResult.None;

        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(DialogWindow), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the command that will be executed when the command source is invoked.
        /// </summary>
        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(DialogWindow), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value that represents a user defined data value that can be passed to the command when it is executed.
        /// </summary>
        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        /// <summary>
        /// Identifies the <see cref="CommandTarget"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(nameof(CommandTarget), typeof(IInputElement), typeof(DialogWindow), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value that represents the object that the command is being executed on.
        /// </summary>
        public IInputElement CommandTarget { get => (IInputElement)GetValue(CommandTargetProperty); set => SetValue(CommandTargetProperty, value); }



        static DialogWindow() => DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogWindow), new FrameworkPropertyMetadata(typeof(DialogWindow)));



        public DialogWindow() : this(System.Reflection.Assembly.GetEntryAssembly().GetName().Name) { }

        // InitializeComponent();

        // ActionCommand = new WinCopies.Util.DelegateCommand(ActionCommandMethod);

        public DialogWindow(string title) => Title = title;

        //public override void OnApplyTemplate()
        //{

        //    base.OnApplyTemplate();

        //    //switch (DialogButton)

        //    //{

        //    //    case DialogButton.OK:
        //    //        var b = (Template.FindName(PART_OkButton, this) as Button);
        //    //        _ = b.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));
        //    //        MessageBox.Show(b.Content.ToString());
        //    //        break;

        //    //    case DialogButton.OKCancel:

        //    //        _ = (GetTemplateChild(PART_OkButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

        //    //        _ = (GetTemplateChild(PART_CancelButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

        //    //        break;

        //    //    case DialogButton.OKApplyCancel:

        //    //        _ = (GetTemplateChild(PART_OkButton) as Button).CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

        //    //        _ = (GetTemplateChild(PART_ApplyButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

        //    //        _ = (GetTemplateChild(PART_CancelButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

        //    //        break;

        //    //    case DialogButton.YesNo:

        //    //        _ = (GetTemplateChild(PART_YesButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

        //    //        _ = (GetTemplateChild(PART_NoButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

        //    //        break;

        //    //    case DialogButton.YesNoCancel:

        //    //        _ = (GetTemplateChild(PART_YesButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

        //    //        _ = (GetTemplateChild(PART_NoButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

        //    //        _ = (GetTemplateChild(PART_CancelButton) as Button)?.CommandBindings.Add(new CommandBinding(Commands.CommonCommand, Command_Executed, Command_CanExecute));

        //    //        break;

        //    //}

        //}

        protected virtual void OnDialogButtonChanged(DialogButton oldValue, DialogButton newValue)

        {

            // MessageBox.Show(e.NewValue.ToString());

            if (DefaultButton != DefaultButton.None) throw new InvalidOperationException($"{nameof(DefaultButton)} must be set to {nameof(DefaultButton.None)} in order to perform this action.");

            if (CommandBindings.Count > 0) throw new InvalidOperationException("This dialog already has command bindings.");

            switch (newValue)

            {

                case DialogButton.OK:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Ok, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.OKCancel:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Ok, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Cancel, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.AbortRetryIgnore:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Abort, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Retry, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Ignore, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.YesNoCancel:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Yes, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.No, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Cancel, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.YesNo:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Yes, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.No, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.RetryCancel:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Retry, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Cancel, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.CancelTryContinue:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Cancel, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Retry, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Continue, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.ContinueIgnoreCancel:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Continue, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Ignore, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Cancel, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.OKApplyCancel:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Ok, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Apply, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Cancel, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.RetryIgnoreCancel:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Retry, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Ignore, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Cancel, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.IgnoreCancel:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Ignore, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Cancel, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.YesToAllNoToAllCancel:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.YesToAll, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.NoToAll, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.Cancel, Command_Executed, Command_CanExecute));

                    break;

                case DialogButton.YesToAllNoToAll:

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.YesToAll, Command_Executed, Command_CanExecute));

                    _ = CommandBindings.Add(new CommandBinding(DialogCommands.NoToAll, Command_Executed, Command_CanExecute));

                    break;

            }

            // ((DialogWindow)d).DefaultButton = DefaultButton.None;

        }

        protected virtual void OnDefaultButtonChanged(DefaultButton oldValue, DefaultButton newValue)

        {

            if (newValue == DefaultButton.None) return;

            static void throwArgumentException() => throw new ArgumentException("DefaultButton must be included in DialogButton value.");

            switch (DialogButton)

            {

                case DialogButton.OK:

                    if (newValue != DefaultButton.OK) throwArgumentException();

                    break;

                case DialogButton.OKCancel:

                    if (If(IfCT.And, IfCM.Logical, IfComp.NotEqual, newValue, DefaultButton.OK, DefaultButton.Cancel)) throwArgumentException();

                    break;

                case DialogButton.AbortRetryIgnore:

                    if (If(IfCT.And, IfCM.Binary, IfComp.Equal, newValue, DefaultButton.Abort, DefaultButton.Retry, DefaultButton.Ignore)) throwArgumentException();

                    break;

                case DialogButton.YesNoCancel:

                    if (If(IfCT.And, IfCM.Logical, IfComp.NotEqual, newValue, DefaultButton.Yes, DefaultButton.No, DefaultButton.Cancel)) throwArgumentException();

                    break;

                case DialogButton.YesNo:

                    if (If(IfCT.And, IfCM.Logical, IfComp.NotEqual, newValue, DefaultButton.Yes, DefaultButton.No)) throwArgumentException();

                    break;

                case DialogButton.RetryCancel:

                    if (If(IfCT.And, IfCM.Logical, IfComp.NotEqual, newValue, DefaultButton.Retry, DefaultButton.Cancel)) throwArgumentException();

                    break;

                case DialogButton.CancelTryContinue:

                    if (If(IfCT.And, IfCM.Logical, IfComp.NotEqual, newValue, DefaultButton.Cancel, DefaultButton.Retry, DefaultButton.Continue)) throwArgumentException();

                    break;

                case DialogButton.ContinueIgnoreCancel:

                    if (If(IfCT.And, IfCM.Logical, IfComp.NotEqual, newValue, DefaultButton.Continue, DefaultButton.Ignore, DefaultButton.Cancel)) throwArgumentException();

                    break;

                case DialogButton.OKApplyCancel:

                    if (If(IfCT.And, IfCM.Logical, IfComp.NotEqual, newValue, DefaultButton.OK, DefaultButton.Apply, DefaultButton.Cancel)) throwArgumentException();

                    break;

                case DialogButton.RetryIgnoreCancel:

                    if (If(IfCT.And, IfCM.Logical, IfComp.NotEqual, newValue, DefaultButton.Retry, DefaultButton.Ignore, DefaultButton.Cancel)) throwArgumentException();

                    break;

                case DialogButton.IgnoreCancel:

                    if (If(IfCT.And, IfCM.Logical, IfComp.NotEqual, newValue, DefaultButton.Ignore, DefaultButton.Cancel)) throwArgumentException();

                    break;

                case DialogButton.YesToAllNoToAllCancel:

                    if (If(IfCT.And, IfCM.Logical, IfComp.NotEqual, newValue, DefaultButton.YesToAll, DefaultButton.NoToAll, DefaultButton.Cancel)) throwArgumentException();

                    break;

                case DialogButton.YesToAllNoToAll:

                    if (If(IfCT.And, IfCM.Logical, IfComp.NotEqual, newValue, DefaultButton.YesToAll, DefaultButton.NoToAll)) throwArgumentException();

                    break;

            }

        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e) => OnCommandCanExecute(e);

        protected virtual void OnCommandCanExecute(CanExecuteRoutedEventArgs e)
        {

            if (e.Command == DialogCommands.Ok)

                e.CanExecute = DialogButton == DialogButton.OK || Command == null || Command.CanExecute(CommandParameter, CommandTarget);

            else if (e.Command == DialogCommands.Apply)

                e.CanExecute = Command?.CanExecute(CommandParameter, CommandTarget) == true;

            else if (If(IfCT.Or, IfCM.Logical, IfComp.Equal, e.Command, DialogCommands.Yes, DialogCommands.Retry, DialogCommands.Continue))

                e.CanExecute = Command == null || Command.CanExecute(CommandParameter, CommandTarget);

            else if (If(IfCT.Or, IfCM.Logical, IfComp.Equal, e.Command, DialogCommands.No, DialogCommands.YesToAll, DialogCommands.NoToAll, DialogCommands.Cancel, DialogCommands.Abort, DialogCommands.Ignore))

                e.CanExecute = true;

        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e) => OnCommandExecuted(e);

        protected virtual void OnCommandExecuted(ExecutedRoutedEventArgs e)

        {

            if (e.Command == DialogCommands.Ok)

                CloseWindowWithDialogResult(true, MessageBoxResult.OK);

            else if (e.Command == DialogCommands.Apply)

                Command.Execute(CommandParameter, CommandTarget);

            else if (e.Command == DialogCommands.Yes)

                CloseWindowWithDialogResult(true, MessageBoxResult.Yes);

            else if (e.Command == DialogCommands.No)

                CloseWindowWithDialogResult(false, MessageBoxResult.No);

            else if (e.Command == DialogCommands.YesToAll)

                CloseWindowWithDialogResult(true, MessageBoxResult.YesToAll);

            else if (e.Command == DialogCommands.NoToAll)

                CloseWindowWithDialogResult(false, MessageBoxResult.NoToAll);

            else if (e.Command == DialogCommands.Cancel)

                CloseWindowWithDialogResult(false, MessageBoxResult.Cancel);

            else if (e.Command == DialogCommands.Abort)

                CloseWindowWithDialogResult(false, MessageBoxResult.Abort);

            else if (e.Command == DialogCommands.Ignore)

                CloseWindowWithDialogResult(false, MessageBoxResult.Ignore);

            else if (e.Command == DialogCommands.Retry)

                CloseWindowWithDialogResult(true, MessageBoxResult.Retry);

            else if (e.Command == DialogCommands.Continue)

                CloseWindowWithDialogResult(true, MessageBoxResult.Continue);

        }

        protected void CloseWindowWithDialogResult(bool dialogResult, MessageBoxResult messageBoxResult)

        {

            DialogResult = dialogResult;

            MessageBoxResult = messageBoxResult;

            if (dialogResult && Command != null && DialogButton != DialogButton.OK)

                Command.Execute(CommandParameter, CommandTarget);

            Close();

        }

    }
}
