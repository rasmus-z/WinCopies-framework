using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Media.Imaging;

using TsudaKageyu;

using WinCopies.Collections;
using WinCopies.Util;
using static WinCopies.Util.Util;
using IDisposable = WinCopies.Util.IDisposable;

namespace WinCopies.IO
{

    public abstract class BrowsableObjectInfo : FileSystemObject, IBrowsableObjectInfo
    {

        /// <summary>
        /// When called from a derived class, initializes a new instance of the <see cref="BrowsableObjectInfo"/> class.
        /// </summary>
        protected BrowsableObjectInfo(string path) : base(path) { }

        public abstract bool NeedsObjectsOrValuesReconstruction { get; }

        /// <summary>
        /// This method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
        /// </summary>
        /// <param name="browsableObjectInfo">The cloned <see cref="BrowsableObjectInfo"/>.</param>
        protected virtual void OnDeepClone(BrowsableObjectInfo browsableObjectInfo)

        {

            // browsableObjectInfo.AreItemsLoaded = false;

            //if (!(ItemsLoader is null))

            //    browsableObjectInfo.ItemsLoader = (IBrowsableObjectInfoLoader)ItemsLoader.DeepClone();

            // browsableObjectInfo.SetItemsProperty();

            //if (Factory.UseRecursively)

            // else

            // browsableObjectInfo._factory = null;

            // browsableObjectInfo._parent = null;

        }

        /// <summary>
        /// When overridden in a derived class, gets a deep clone of this <see cref="BrowsableObjectInfo"/>. The <see cref="OnDeepClone(BrowsableObjectInfo)"/> method already has an implementation for deep cloning from constructor and not from an <see cref="object.MemberwiseClone"/> operation. If you perform a deep cloning operation using an <see cref="object.MemberwiseClone"/> operation in <see cref="DeepCloneOverride()"/>, you'll have to override this method if your class has to reinitialize members.
        /// </summary>
        protected abstract BrowsableObjectInfo DeepCloneOverride();

        /// <summary>
        /// Gets a deep clone of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>A new <see cref="IBrowsableObjectInfo"/> that represents the same item that the current <see cref="BrowsableObjectInfo"/>.</returns>
        public object DeepClone()

        {

            //var callee = new StackFrame(0).GetMethod();

            //var caller = new StackFrame(1).GetMethod();

            //if (callee.DeclaringType.Equals(caller.DeclaringType) || (caller.IsConstructor && caller.DeclaringType.BaseType.Equals(this.GetType())))

            //{

            ((IDisposable)this).ThrowIfDisposingOrDisposed();

            BrowsableObjectInfo browsableObjectInfo = DeepCloneOverride();

            OnDeepClone(browsableObjectInfo);

            return browsableObjectInfo;

            //}

            //    else

            //        throw new InvalidOperationException("The type of the caller of the current constructor is not the same as the type of this constructor.");

        }

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        /// <param name="disposing">Whether to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)

        {

            //if (ItemsLoader != null)

            //{

            //    if (ItemsLoader.IsBusy)

            //        ItemsLoader.Cancel();

            //    // ItemsLoader.Path = null;

            //}

            if (disposing)

                Parent = null;

        }

        internal static Icon TryGetIcon(int iconIndex, string dll, System.Drawing.Size size) => new IconExtractor(IO.Path.GetRealPathFromEnvironmentVariables("%SystemRoot%\\System32\\" + dll)).GetIcon(iconIndex).Split()?.TryGetIcon(size, 32, true, true);

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

        /// <summary>
        /// When overridden in a derived class, gets a value that indicates whether this <see cref="BrowsableObjectInfo"/> is browsable.
        /// </summary>
        public abstract bool IsBrowsable { get; }

        /// <summary>
        /// Gets a value that indicates whether the current object is disposing.
        /// </summary>
        public bool IsDisposing { get; internal set; }

        private IBrowsableObjectInfo _parent = default;

        /// <summary>
        /// Gets the <see cref="IBrowsableObjectInfo"/> parent of this <see cref="BrowsableObjectInfo"/>. Returns <see langword="null"/> if this object is the root object of a hierarchy.
        /// </summary>
        public IBrowsableObjectInfo Parent { 
            
            get { 
                
                if (_parent is null)
                    
                    _parent = GetParent();
                
                return _parent;
            
            } internal set => _parent = value; }

        /// <summary>
        /// When overridden in a derived class, returns the parent of this <see cref="BrowsableObjectInfo"/>.
        /// </summary>
        /// <returns>The parent of this <see cref="BrowsableObjectInfo"/>.</returns>
        protected abstract IBrowsableObjectInfo GetParent();

        /// <summary>
        /// Gets a value that indicates whether the current object is disposed.
        /// </summary>
        public bool IsDisposed { get; internal set; }

        /// <summary>
        /// Disposes the current <see cref="IBrowsableObjectInfo"/> and its parent and items recursively.
        /// </summary>
        public void Dispose()

        {

            IsDisposing = true;

            Dispose(true);

            GC.SuppressFinalize(this);

            IsDisposed = true;

            IsDisposing = false;

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ~BrowsableObjectInfo()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        {

            Dispose(false);

        }

    }

}
