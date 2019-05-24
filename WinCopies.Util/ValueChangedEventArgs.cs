using System;

namespace WinCopies.Util
{
    public delegate void EventHandler<T>(object sender, EventArgs<T> e);

    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    public delegate void ValueChangedEventHandler<T>(object sender, ValueChangedEventArgs<T> e);

    public delegate void ValueChangedEventHandler<TOldValue, TNewValue>(object sender, ValueChangedEventArgs<TOldValue, TNewValue> e);

    public class EventArgs<T> : EventArgs
    {

        public T Value { get; } = default;

        public EventArgs(T value) => Value = value;

    }

    public class ValueChangedEventArgs : EventArgs
    {

        public object OldValue { get; } = null;

        public object NewValue { get; } = null;

        public ValueChangedEventArgs(object oldValue, object newValue)

        {

            OldValue = oldValue;

            NewValue = newValue;

        }

    }

    public class ValueChangedEventArgs<T> : EventArgs
    {

        public T OldValue { get; } = default;

        public T NewValue { get; } = default;

        public ValueChangedEventArgs(T oldValue, T newValue)

        {

            OldValue = oldValue;

            NewValue = newValue;

        }

    }

    public class ValueChangedEventArgs<TOldValue, TNewValue> : EventArgs
    {

        public TOldValue OldValue { get; } = default;

        public TNewValue NewValue { get; } = default;

        public ValueChangedEventArgs(TOldValue oldValue, TNewValue newValue)

        {

            OldValue = oldValue;

            NewValue = newValue;

        }

    }
}
