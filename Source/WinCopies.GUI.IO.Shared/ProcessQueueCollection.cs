/* Copyright © Pierre Sprimont, 2020
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

using System.Collections.Specialized;
using WinCopies.Collections.DotNetFix;
using Size = WinCopies.IO.Size;

namespace WinCopies.GUI.IO
{
    public sealed class ProcessQueueCollection : ReadOnlyObservableQueueCollection<IPathInfo>
    {
        private Size _size;

        public Size Size { get => _size; private set { _size = value; RaisePropertyChangedEvent(nameof(Size)); } }

        public ProcessQueueCollection(ObservableQueueCollection<IPathInfo> queueCollection) : base(queueCollection) { }

        protected override void OnCollectionChanged(SimpleLinkedCollectionChangedEventArgs<IPathInfo> e)
        {
            base.OnCollectionChanged(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    if (!e.Item.IsDirectory)

                        Size += e.Item.Size.Value;

                    break;

                case NotifyCollectionChangedAction.Reset:

                    Size = new Size(0ul);

                    break;
            }
        }

        public void DecrementSize(ulong sizeInBytes) => Size -= sizeInBytes;
    }
}
