///* Copyright © Pierre Sprimont, 2020
// *
// * This file is part of the WinCopies Framework.
// *
// * The WinCopies Framework is free software: you can redistribute it and/or modify
// * it under the terms of the GNU General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// *
// * The WinCopies Framework is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with the WinCopies Framework.  If not, see <https://www.gnu.org/licenses/>. */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WinCopies.Collections;
//using WinCopies.Util;
//using IDisposable = WinCopies.Util.IDisposable;

//namespace WinCopies.IO
//{

//    public interface IBrowsableObjectInfoLoaderFilter
//    {

//        string FileName { get; }

//    }

//    public class BrowsableObjectInfoLoaderFilter : IBrowsableObjectInfoLoaderFilter
//    {

//        public string FileName { get; }

//        public BrowsableObjectInfoLoaderFilter(string fileName) => FileName = fileName;

//    }

//    ///// <summary>
//    ///// Represents a loader that can be used to load <see cref="IBrowsableObjectInfo"/>. Note: An <see cref="InvalidOperationException"/> is thrown when the <see cref="System.IDisposable.Dispose"/> method is called if this <see cref="IBrowsableObjectInfoLoader{T}"/> is busy and does not support cancellation.
//    ///// </summary>
//    public interface IBrowsableObjectInfoLoader<TPath, TItems> /* : IBackgroundWorker, IDeepCloneable, IDisposable*/ where TPath : IBrowsableObjectInfo where TItems : IBrowsableObjectInfo

//    {

//        IFileSystemObjectComparer<TItems> FileSystemObjectComparer { get; set; }

//        IEnumerable<IBrowsableObjectInfoLoaderFilter> Filter { get; set; }

//        TPath Path { get; }

//        //bool CheckFilter(string path);

//        int PreloadItems();

//        void LoadItems();

//    }

//    //public interface IBrowsableObjectInfoLoader<TPath, TItems> : IBrowsableObjectInfoLoader where TPath : IBrowsableObjectInfo where TItems : IBrowsableObjectInfo

//    //{

//    //    IBrowsableObjectTreeNode<TPath, TItems> Path { get; }

//    //}

//}
