//using System;
//using System.ComponentModel;
//using System.Reflection;
//using System.Reflection.Emit;
//using System.Windows;
//using System.Windows.Input;
//using System.Windows.Markup;
//using WinCopies.Util;

//namespace EventBinding
//{
//    public class EventBindingExtension : MarkupExtension, INotifyPropertyChanged
//    {
//#pragma warning disable IDE0044 // Ajouter un modificateur readonly
//        private ICommand command = null;
//#pragma warning restore IDE0044 // Ajouter un modificateur readonly

//#pragma warning disable IDE0044 // Ajouter un modificateur readonly
//        private object commandParameter = null;
//#pragma warning restore IDE0044 // Ajouter un modificateur readonly

//#pragma warning disable IDE0044 // Ajouter un modificateur readonly
//        private IInputElement commandTarget = null;
//#pragma warning restore IDE0044 // Ajouter un modificateur readonly

//        public ICommand Command { get => command; set => OnPropertyChanged(nameof(Command), nameof(command), value); }

//        public object CommandParameter { get => commandParameter; set => OnPropertyChanged(nameof(commandParameter), nameof(commandParameter), value); }

//        public IInputElement CommandTarget { get => commandTarget; set => OnPropertyChanged(nameof(CommandTarget), nameof(commandTarget), value); }

//        public EventBindingExtension()

//        {

//            Console.WriteLine("azertysdlfmkxlf;c");

//        } 

//        /// <summary>
//        /// The method that is called to set a value to a property. If succeed, then call the <see cref="OnPropertyChanged(string, object, object)"/> method. See the Remarks section.
//        /// </summary>
//        /// <param name="propertyName">The name of the property to set in a new value</param>
//        /// <param name="fieldName">The name of the field to store the new value. This must the field that is called by the property accessors (get and set).</param>
//        /// <param name="newValue">The value to set in the property</param>
//        /// <param name="declaringType">The declaring type of both the property and its associated field</param>
//        /// <remarks>To use this method, you need to work with the WinCopies Framework Property changed notification pattern. See the website of the WinCopies Framework for more details.</remarks>
//        protected virtual void OnPropertyChanged(string propertyName, string fieldName, object newValue, Type declaringType = null)

//        {

//            var (propertyChanged, oldValue) = ((INotifyPropertyChanged)this).SetProperty(propertyName, fieldName, newValue, declaringType);

//            if (propertyChanged) OnPropertyChanged(propertyName, oldValue, newValue);

//        }

//        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));

//        public override object ProvideValue(IServiceProvider serviceProvider)
//        {
//            // MessageBox.Show("");
//            // Console.WriteLine("azertysdlfmkxlf;c");
//            if (!(serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget targetProvider))

//                throw new InvalidOperationException();

//            if (!(targetProvider.TargetObject is FrameworkElement targetObject))

//                throw new InvalidOperationException();

//            var memberInfo = targetProvider.TargetProperty as MemberInfo;

//            if (memberInfo == null)

//                throw new InvalidOperationException();

//            //if (string.IsNullOrWhiteSpace(Command))
//            //{
//            //    Command = memberInfo.Name.Replace("Add", "");
//            //    if (Command.Contains("Handler"))
//            //    {
//            //        Command = Command.Replace("Handler", "Command");
//            //    }
//            //    else
//            //    {
//            //        Command = Command + "Command";
//            //    }
//            //}

//            return CreateHandler(memberInfo, targetObject.GetType());
//        }

//        private Type GetEventHandlerType(MemberInfo memberInfo)
//        {
//            if (memberInfo is EventInfo)

//                return ((EventInfo)memberInfo).EventHandlerType;

//            else if (memberInfo is MethodInfo)

//                return ((MethodInfo)memberInfo).GetParameters()[1].ParameterType;

//            return null;
//        }

//        private Delegate CreateHandler(MemberInfo memberInfo, Type targetType)
//        {
//            Type eventHandlerType = GetEventHandlerType(memberInfo);

//            if (eventHandlerType == null) return null;

//            var handlerInfo = eventHandlerType.GetMethod("Invoke");

//            var method = new DynamicMethod("", handlerInfo.ReturnType,
//                new Type[]
//                {
//                    handlerInfo.GetParameters()[0].ParameterType,
//                    handlerInfo.GetParameters()[1].ParameterType,
//                });

//            var gen = method.GetILGenerator();
//            gen.Emit(OpCodes.Ldarg, 0);
//            gen.Emit(OpCodes.Ldarg, 1);
//            gen.Emit(OpCodes.Call, getMethod);
//            gen.Emit(OpCodes.Ret);

//            return method.CreateDelegate(eventHandlerType);
//        }

//        static readonly MethodInfo getMethod = typeof(EventBindingExtension).GetMethod("HandlerIntern", new Type[] { typeof(object), typeof(object),typeof(EventBindingExtension) });

//        public event PropertyChangedEventHandler PropertyChanged;

//        static void Handler(object sender, object args,EventBindingExtension instance) 
//        {
//            HandlerIntern(sender, args, instance); 
//        }

//        public static void HandlerIntern(object sender, object args, EventBindingExtension instance)
//        {
//            if (!(sender is FrameworkElement fe))

//                return;

//            if (instance.command != null && instance.Command.CanExecute(instance.commandParameter)) 
//            {
//                instance.command.Execute(instance.commandParameter); 
//            }
//        }

//        //        internal static ICommand GetCommand(FrameworkElement target, string cmdName)
//        //        {
//        //            var vm = FindViewModel(target);
//        //            if (vm == null) return null;

//        //            var vmType = vm.GetType();
//        //            var cmdProp = vmType.GetProperty(cmdName);
//        //            if (cmdProp != null)
//        //            {
//        //                return cmdProp.GetValue(vm) as ICommand;
//        //            }
//        //#if DEBUG
//        //            throw new Exception("EventBinding path error: '" + cmdName + "' property not found on '" + vmType + "' 'DelegateCommand'");
//        //#endif

//        //            return null;
//        //        }

//        //internal static object GetCommandParameter(FrameworkElement target, object args, string commandParameter)
//        //{
//        //    object ret = null;
//        //    var classify = commandParameter.Split('.');
//        //    switch (classify[0])
//        //    {
//        //        case "$e":
//        //            ret = args;
//        //            break;
//        //        case "$this":
//        //            ret = classify.Length > 1 ? FollowPropertyPath(target, commandParameter.Replace("$this.", ""), target.GetType()) : target;
//        //            break;
//        //        default:
//        //            ret = commandParameter;
//        //            break;
//        //    }

//        //    return ret;
//        //}

//        //internal static ViewModelBase FindViewModel(FrameworkElement target)
//        //{
//        //    if (target == null) return null;

//        //    var vm = target.DataContext as ViewModelBase;
//        //    if (vm != null) return vm;

//        //    var parent = target.GetParentObject() as FrameworkElement;

//        //    return FindViewModel(parent);
//        //}

//        internal static object FollowPropertyPath(object target, string path, Type valueType = null)
//        {
//            if (target == null) throw new ArgumentNullException("target null");
//            if (path == null) throw new ArgumentNullException("path null");

//            Type currentType = valueType ?? target.GetType();

//            foreach (string propertyName in path.Split('.'))
//            {
//                PropertyInfo property = currentType.GetProperty(propertyName);
//                if (property == null) throw new NullReferenceException("property null");

//                target = property.GetValue(target);
//                currentType = property.PropertyType;
//            }
//            return target;
//        }
//    }
//}
