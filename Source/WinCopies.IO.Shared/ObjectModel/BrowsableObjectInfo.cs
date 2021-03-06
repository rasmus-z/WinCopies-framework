﻿/* Copyright © Pierre Sprimont, 2020
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


using Microsoft.WindowsAPICodePack.PortableDevices;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;

using TsudaKageyu;

namespace WinCopies.IO.ObjectModel
{
    public abstract class BrowsableObjectInfo : FileSystemObject, IBrowsableObjectInfo
    {
        #region Consts
        public const ushort SmallIconSize = 16;
        public const ushort MediumIconSize = 48;
        public const ushort LargeIconSize = 128;
        public const ushort ExtraLargeIconSize = 256;
        #endregion

        #region Properties
        /// <summary>
        /// When overridden in a derived class, gets a value that indicates whether this <see cref="BrowsableObjectInfo"/> is browsable.
        /// </summary>
        public abstract bool IsBrowsable { get; }

        ///// <summary>
        ///// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="BrowsableObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
        ///// </summary>
        public abstract IBrowsableObjectInfo Parent { get; }

        #region BitmapSources
        /// <summary>
        /// When overridden in a derived class, gets the small <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract BitmapSource SmallBitmapSource { get; }

        /// <summary>
        /// When overridden in a derived class, gets the medium <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract BitmapSource MediumBitmapSource { get; }

        /// <summary>
        /// When overridden in a derived class, gets the large <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract BitmapSource LargeBitmapSource { get; }

        /// <summary>
        /// When overridden in a derived class, gets the extra large <see cref="BitmapSource"/> of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        public abstract BitmapSource ExtraLargeBitmapSource { get; }
        #endregion

        public abstract string ItemTypeName { get; }

        public abstract string Description { get; }

        /// <summary>
        /// Gets the size for this <see cref="IBrowsableObjectInfo"/>.
        /// </summary>
        public abstract Size? Size { get; }

        public abstract bool IsSpecialItem { get; }

        /// <summary>
        /// Gets a value that indicates whether the current object is disposed.
        /// </summary>
        public bool IsDisposed { get; internal set; }

        public ClientVersion? ClientVersion { get; private set; }
        #endregion

        /// <summary>
        /// When called from a derived class, initializes a new instance of the <see cref="BrowsableObjectInfo"/> class.
        /// </summary>
        protected BrowsableObjectInfo(in string path) : this(path, null) { }

        protected BrowsableObjectInfo(in string path, in ClientVersion? clientVersion) : base(path) => ClientVersion = clientVersion;

        #region Methods
        internal static Icon TryGetIcon(in int iconIndex, in string dll, in System.Drawing.Size size) => new IconExtractor(IO.Path.GetRealPathFromEnvironmentVariables("%SystemRoot%\\System32\\" + dll)).GetIcon(iconIndex).Split()?.TryGetIcon(size, 32, true, true);

        public abstract IEnumerable<IBrowsableObjectInfo> GetItems();

        #region IDisposable
        public void Dispose()
        {
            if (IsDisposed)

                return;

            Dispose(true);

            GC.SuppressFinalize(this);

            IsDisposed = true;
        }

        /// <summary>
        /// Not used in this class.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(in bool disposing)
        {
            //if (ItemsLoader != null)

            //{

            //    if (ItemsLoader.IsBusy)

            //        ItemsLoader.Cancel();

            //    // ItemsLoader.Path = null;

            //}

            //if (disposing)

            //    _parent = null;
        }
        #endregion
        #endregion

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ~BrowsableObjectInfo() => Dispose(false);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
